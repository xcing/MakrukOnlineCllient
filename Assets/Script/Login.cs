using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using LitJson;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using System.Collections.Generic;

namespace Assets.Script.Component
{
    class Login : MonoBehaviour
    {
        public Text email;
        public Text password;
        public Text registerEmail;
        public Text registerPassword;
        public Text registerConfirmPassword;
        public Text submitText;
        public Text displayName;
        public Image displayImage;
        public CanvasGroup registerBlockCanvasGroup;
        public CanvasGroup setDisplayNameBlockCanvasGroup;
        public CanvasGroup bgFadeCanvasGroup;
        public CanvasGroup bgFade2CanvasGroup;
        public Image alertBlock;

        public Text header;
        public Text placeHolderEmail;
        public Text placeHolderPassword;
        public Text placeHolderRegisterEmail;
        public Text placeHolderRegisterPassword;
        public Text placeHolderRegisterConfirmPassword;
        public Text placeHolderDisplayname;
        public Text assumeDisplayname;
        public Text loginText;
        public Text loginWithDeviceText;
        public Text registerText;
        public Text newRegisterText;
        public Text registerHeaderText;

        public ReadData readData;
        public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        public string facebookId;
        public string facebookDisplayImage;

        void Awake()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        void Start()
        {
            if (readData.setting.loginType == 1 && readData.setting.email != "" && readData.setting.password != "")
            {
                StartCoroutine(LoginService(readData.setting.email, readData.setting.password));
            }
            else if (readData.setting.loginType == 3 && readData.setting.deviceId != "")
            {
                StartCoroutine(GuestLoginService());
            }
            else if (readData.setting.loginType == 2 && readData.setting.facebookId != "")
            {
                FacebookLoginButton();
            }

            header.text = readData.jsonWordingData["loginHeader"][readData.setting.language].ToString();
            placeHolderEmail.text = readData.jsonWordingData["email"][readData.setting.language].ToString();
            placeHolderPassword.text = readData.jsonWordingData["password"][readData.setting.language].ToString();
            placeHolderRegisterEmail.text = readData.jsonWordingData["email"][readData.setting.language].ToString();
            placeHolderRegisterPassword.text = readData.jsonWordingData["password"][readData.setting.language].ToString();
            placeHolderRegisterConfirmPassword.text = readData.jsonWordingData["confirmPassword"][readData.setting.language].ToString();
            placeHolderDisplayname.text = readData.jsonWordingData["displayname"][readData.setting.language].ToString();
            assumeDisplayname.text = readData.jsonWordingData["assumeDisplayname"][readData.setting.language].ToString();
            loginText.text = readData.jsonWordingData["login"][readData.setting.language].ToString();
            loginWithDeviceText.text = readData.jsonWordingData["loginWithDevice"][readData.setting.language].ToString();
            registerText.text = readData.jsonWordingData["register"][readData.setting.language].ToString();
            newRegisterText.text = readData.jsonWordingData["newRegister"][readData.setting.language].ToString();
            registerHeaderText.text = readData.jsonWordingData["newRegister"][readData.setting.language].ToString();
            submitText.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();

            //FacebookInit();
        }

        public void LoginButton()
        {
            if (email.text == "")
            {
                OpenAlertBlock(readData.jsonWordingData["pleaseInputEmail"][readData.setting.language].ToString());
            }
            else if (password.text == "")
            {
                OpenAlertBlock(readData.jsonWordingData["pleaseInputPassword"][readData.setting.language].ToString());
            }
            else if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                OpenAlertBlock(readData.jsonWordingData["internetConnectionLost"][readData.setting.language].ToString());
            }
            else
            {
                StartCoroutine(LoginService(email.text, password.text));
            }
        }

        public IEnumerator LoginService(string tempEmail, string tempPassword)
        {
            string param = "login" + "|" + readData.signatureKey + "|" + tempEmail + "|" + tempPassword;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                JsonData jsonData = JsonMapper.ToObject(www.text);
                if (jsonData["result"].ToString() == "success")
                {
                    readData.userData = JsonMapper.ToObject<User>(www.text);
                    readData.userData.xsignature = readData.encryption.DecryptRJ256(readData.userData.xsignature);
                    readData.userData.displayImage = null;
                    readData.fileData.SaveUserData(readData.userData);
                    readData.setting.loginType = 1;
                    readData.setting.email = tempEmail;
                    readData.setting.password = tempPassword;
                    readData.fileData.SaveSetting(readData.setting);
                    if (readData.userData.displayName == null)
                    {
                        OpenSetDisplayNameBlock();
                    }
                    else
                    {
                        /*if (readData.setting.gameId == 0)
                        {
                            SceneManager.LoadScene("selectGame");
                        }
                        else
                        {*/
                        SceneManager.LoadScene("selectMode");
                        //}
                    }
                }
                else if (jsonData["result"].ToString() == "passwordWrong")
                {
                    OpenAlertBlock(readData.jsonWordingData["passwordIsNotCorrect"][readData.setting.language].ToString());
                }
                else if (jsonData["result"].ToString() == "emailNotFound")
                {
                    OpenAlertBlock(readData.jsonWordingData["emailNotFound"][readData.setting.language].ToString());
                }
                else if (jsonData["result"].ToString() == "banUser")
                {
                    OpenAlertBlock(readData.jsonWordingData["banUser"][readData.setting.language].ToString());
                }
                else
                {
                    Debug.Log(jsonData["result"].ToString());
                }
            }
        }

        public void RegisterButton()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            registerBlockCanvasGroup.alpha = 1;
            registerBlockCanvasGroup.blocksRaycasts = true;
        }

        public void FacebookLoginButton()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                OpenAlertBlock(readData.jsonWordingData["internetConnectionLost"][readData.setting.language].ToString());
            }
            else
            {
                List<string> perms = new List<string>() { "public_profile" };
                FB.LogInWithReadPermissions(perms, AuthCallback);
            }
        }

        private void AuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                //Debug.Log(aToken.UserId);
                facebookId = aToken.UserId;
                facebookDisplayImage = aToken.TokenString;
                StartCoroutine(FacebookLoginService());
                // Print current access token's granted permissions
                /*foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }*/
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        }

        public IEnumerator FacebookLoginService()
        {
            string param = "facebooklogin" + "|" + readData.signatureKey + "|" + facebookId;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                JsonData jsonData = JsonMapper.ToObject(www.text);
                if (jsonData["result"].ToString() == "success")
                {
                    readData.userData = JsonMapper.ToObject<User>(www.text);
                    readData.userData.xsignature = readData.encryption.DecryptRJ256(readData.userData.xsignature);
                    readData.userData.displayImage = "https" + "://graph.facebook.com/" + facebookId + "/picture?access_token=" + facebookDisplayImage;
                    readData.fileData.SaveUserData(readData.userData);
                    readData.setting.loginType = 2;
                    readData.setting.facebookId = facebookId;
                    readData.fileData.SaveSetting(readData.setting);
                    if (readData.userData.displayName == null)
                    {
                        OpenSetDisplayNameBlock();
                    }
                    else
                    {
                        /*if (readData.setting.gameId == 0)
                        {
                            SceneManager.LoadScene("selectGame");
                        }
                        else
                        {*/
                        SceneManager.LoadScene("selectMode");
                        //}
                    }
                }
                else if (jsonData["result"].ToString() == "banUser")
                {
                    OpenAlertBlock(readData.jsonWordingData["banUser"][readData.setting.language].ToString());
                }
                else
                {
                    Debug.Log(jsonData["result"].ToString());
                }
            }
        }

        /*private void FacebookInit()
        {
            SPFacebook.OnInitCompleteAction += OnInit;
            SPFacebook.OnFocusChangedAction += OnFocusChanged;
            OnInit();
        }

        private void OnInit()
        {
            if (SPFacebook.Instance.IsLoggedIn)
            {
                Debug.Log("Already Facebook Login");
            }
            SPFacebook.Instance.Init();
        }

        private void OnFocusChanged(bool focus)
        {
            if (!focus)
            {
                // pause the game - we will need to hide                                             
                Time.timeScale = 0;
            }
            else
            {
                // start the game back up - we're getting focus again                                
                Time.timeScale = 1;
            }
        }

        public void FacebookLoginButton()
        {
            SPFacebook.OnAuthCompleteAction += OnAuth;
            SPFacebook.Instance.Login();
        }

        private void OnAuth(FB_Result result)
        {
            if (SPFacebook.Instance.IsLoggedIn)
            {
                //IsAuntificated = true;
                Debug.Log("FacebookLogin success");
                SA_StatusBar.text = "user Login -> true";
            }
            else
            {
                Debug.Log("Failed to log in");
            }
        }*/

        public void GuestLoginButton()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                OpenAlertBlock(readData.jsonWordingData["internetConnectionLost"][readData.setting.language].ToString());
            }
            else
            {
                StartCoroutine(GuestLoginService());
            }
        }

        public IEnumerator GuestLoginService()
        {
            string param = "guestlogin" + "|" + readData.signatureKey + "|" + SystemInfo.deviceUniqueIdentifier;
            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;
            if (www.error == null)
            {
                JsonData jsonData = JsonMapper.ToObject(www.text);
                if (jsonData["result"].ToString() == "success")
                {
                    readData.userData = JsonMapper.ToObject<User>(www.text);
                    readData.userData.xsignature = readData.encryption.DecryptRJ256(readData.userData.xsignature);
                    readData.userData.displayImage = null;
                    readData.fileData.SaveUserData(readData.userData);
                    readData.setting.loginType = 3;
                    readData.setting.deviceId = SystemInfo.deviceUniqueIdentifier;
                    readData.fileData.SaveSetting(readData.setting);
                    if (readData.userData.displayName == null)
                    {
                        OpenSetDisplayNameBlock();
                    }
                    else
                    {
                        /*if (readData.setting.gameId == 0)
                        {
                            SceneManager.LoadScene("selectGame");
                        }
                        else
                        {*/
                        SceneManager.LoadScene("selectMode");
                        //}
                    }
                }
                else if (jsonData["result"].ToString() == "banUser")
                {
                    OpenAlertBlock(readData.jsonWordingData["banUser"][readData.setting.language].ToString());
                }
                else
                {
                    Debug.Log(jsonData["result"].ToString());
                }
            }
        }

        public void RegisterSubmitButton()
        {
            if (registerEmail.text == "")
            {
                OpenAlertBlock(readData.jsonWordingData["pleaseInputEmail"][readData.setting.language].ToString());
            }
            else if (!Regex.IsMatch(registerEmail.text, MatchEmailPattern))
            {
                OpenAlertBlock(readData.jsonWordingData["wrongFormatEmail"][readData.setting.language].ToString());
            }
            else if (registerPassword.text == "")
            {
                OpenAlertBlock(readData.jsonWordingData["pleaseInputPassword"][readData.setting.language].ToString());
            }
            else if (registerConfirmPassword.text == "")
            {
                OpenAlertBlock(readData.jsonWordingData["pleaseInputConfirmPassword"][readData.setting.language].ToString());
            }
            else if (registerPassword.text != registerConfirmPassword.text)
            {
                OpenAlertBlock(readData.jsonWordingData["passwordNotMatch"][readData.setting.language].ToString());
            }
            /*else if (registerDisplayName.text == "")
            {
                Debug.Log(readData.jsonWordingData["pleaseInputDisplayName"][readData.setting.language].ToString());
            }*/
            else
            {
                StartCoroutine(Register());
            }
        }

        public IEnumerator Register()
        {
            string param = "register" + "|" + readData.signatureKey + "|" + registerEmail.text + "|" + registerPassword.text;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                if (www.text == "success")
                {
                    OpenAlertBlock(readData.jsonWordingData["registerDone"][readData.setting.language].ToString());
                }
                else if (www.text == "already")
                {
                    OpenAlertBlock(readData.jsonWordingData["alreadyEmail"][readData.setting.language].ToString());
                }
                else
                {
                    Debug.Log(www.text);
                }
            }
        }

        public void SubmitDisplayNameButton()
        {
            if (displayName.text == "")
            {
                OpenAlertBlock(readData.jsonWordingData["pleaseInputDisplayName"][readData.setting.language].ToString());
            }
            else
            {
                StartCoroutine(SetDisplayName());
            }
        }

        public IEnumerator SetDisplayName()
        {
            WWWForm form = new WWWForm();
            form.AddField("userId", readData.userData.userId);
            form.AddField("displayName", displayName.text);
            WWW www = new WWW("http://checkmate.whitenightsoft.com/index.php?r=service/setdisplayname", form);
            yield return www;

            if (www.error == null)
            {
                if (www.text == "success")
                {
                    readData.userData.displayName = displayName.text;
                    readData.fileData.SaveUserData(readData.userData);
                    //SceneManager.LoadScene("selectGame");
                    SceneManager.LoadScene("selectMode");
                }
                else if (www.text == "already")
                {
                    OpenAlertBlock(readData.jsonWordingData["alreadyDisplayName"][readData.setting.language].ToString());
                }
                else
                {
                    Debug.Log(www.text);
                }
            }
        }

        public void CloseButton()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            registerBlockCanvasGroup.alpha = 0;
            registerBlockCanvasGroup.blocksRaycasts = false;
        }

        public void OpenSetDisplayNameBlock()
        {
            if (readData.userData.displayImage != null)
            {
                StartCoroutine(readData.downloadImg(readData.userData.displayImage, displayImage));
            }
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            setDisplayNameBlockCanvasGroup.alpha = 1;
            setDisplayNameBlockCanvasGroup.blocksRaycasts = true;
        }

        public void CloseSetDisplayNameBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            setDisplayNameBlockCanvasGroup.alpha = 0;
            setDisplayNameBlockCanvasGroup.blocksRaycasts = false;
        }

        public void OpenAlertBlock(string message)
        {
            alertBlock.GetComponentInChildren<Text>().text = message;
            bgFade2CanvasGroup.alpha = 1;
            bgFade2CanvasGroup.blocksRaycasts = true;
            alertBlock.GetComponent<CanvasGroup>().alpha = 1;
            alertBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void CloseAlertBlock()
        {
            bgFade2CanvasGroup.alpha = 0;
            bgFade2CanvasGroup.blocksRaycasts = false;
            alertBlock.GetComponent<CanvasGroup>().alpha = 0;
            alertBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
}
