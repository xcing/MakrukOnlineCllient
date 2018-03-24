using Assets.Script.Component;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Facebook.Unity;
using LitJson;
using System.Collections.Generic;

namespace Assets.Script
{
    public class SplashScreen : MonoBehaviour
    {
        public Image waitingImage;
        public ReadData readData;
        public CanvasGroup bgFadeCanvasGroup;
        public Image alertBlock;

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
            InvokeRepeating("SpinWaitingImage", 0, 0.05f);
            StartCoroutine(CheckForceUpdateService());
        }

        void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                StartCoroutine(WaitMatch(2, "selectLanguage"));
            }
        }

        public IEnumerator CheckForceUpdateService()
        {
            string param = "checkForceUpdate" + "|" + readData.signatureKey + "|" + Application.version;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                if (www.text == "update")
                {
                    OpenAlertBlock();
                }
                else if (www.text == "none")
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
                    else
                    {
                        StartCoroutine(WaitMatch(2, "selectLanguage"));
                    }
                }
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
                        StartCoroutine(WaitMatch(1, "selectLanguage"));
                        //SceneManager.LoadScene("selectLanguage");
                    }
                    else
                    {
                        StartCoroutine(WaitMatch(1, "selectMode"));
                        //SceneManager.LoadScene("selectMode");
                    }
                }
                else
                {
                    StartCoroutine(WaitMatch(1, "selectLanguage"));
                    //SceneManager.LoadScene("selectLanguage");
                }
            }
        }

        public void FacebookLoginButton()
        {
            List<string> perms = new List<string>() { "public_profile" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
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
                        StartCoroutine(WaitMatch(1, "selectLanguage"));
                    }
                    else
                    {
                        StartCoroutine(WaitMatch(1, "selectMode"));
                    }
                }
                else
                {
                    StartCoroutine(WaitMatch(1, "selectLanguage"));
                }
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
                        StartCoroutine(WaitMatch(1, "selectLanguage"));
                    }
                    else
                    {
                        StartCoroutine(WaitMatch(1, "selectMode"));
                    }
                }
                else
                {
                    StartCoroutine(WaitMatch(1, "selectLanguage"));
                }
            }
        }

        public void SpinWaitingImage()
        {
            waitingImage.transform.eulerAngles = new Vector3(waitingImage.transform.eulerAngles.x, waitingImage.transform.eulerAngles.y, waitingImage.transform.eulerAngles.z + 20);
        }

        public IEnumerator WaitMatch(float seconds, string scene)
        {
            yield return StartCoroutine(Wait(seconds, scene));
        }

        public IEnumerator Wait(float seconds, string scene)
        {
            yield return new WaitForSeconds(seconds);
            SceneManager.LoadScene(scene);
        }

        public void OpenAlertBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            alertBlock.GetComponent<CanvasGroup>().alpha = 1;
            alertBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        /*public void CloseAlertBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            alertBlock.GetComponent<CanvasGroup>().alpha = 0;
            alertBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }*/

        public void UpdateButton()
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.outfit7.mytalkingtomfree&hl=th");
        }
    }
}
