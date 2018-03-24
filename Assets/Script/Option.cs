using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.Component;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Facebook.Unity;

public class Option : MonoBehaviour
{
    public ReadData readData;
    public Text header;
    public Text selectGameButtonText;

    public Color yellowColor;
    public Color grayColor;
    public CanvasGroup bgFadeCanvasGroup;
    public CanvasGroup bgFade2CanvasGroup;
    public CanvasGroup registerBlock;
    public CanvasGroup changeEmailBlock;
    public CanvasGroup changePasswordBlock;
    public CanvasGroup changeDisplaynameBlock;
    public CanvasGroup connectDeviceBlock;
    public CanvasGroup connectFacebookBlock;
    public CanvasGroup changeLanguageBlock;
    public CanvasGroup changeAvatarBlock;
    public CanvasGroup logoutBlock;
    public CanvasGroup alertBlock;

    public Text emailText;
    public Text email;
    public Text registerEmail;
    public Text registerPassword;
    public Text registerConfirmPassword;
    public Text changeEmail_email;
    public Text changeEmail_password;
    public Button changeEmailButton;
    public Text changeEmail_PlaceHolder_email;
    public Text changeEmail_PlaceHolder_password;
    public Text changeEmail_submitButton;
    public Text changeEmail_cancelButton;
    public Text register_PlaceHolder_email;
    public Text register_PlaceHolder_password;
    public Text register_PlaceHolder_confirmPassword;
    public Text register_submitButton;
    public Text register_cancelButton;

    public Text passwordText;
    public Text password;
    public Text changePassword_oldPassword;
    public Text changePassword_newPassword;
    public Text changePassword_confirmPassword;
    public Button changePasswordButton;
    public Text changePassword_PlaceHolder_oldPassword;
    public Text changePassword_PlaceHolder_newPassword;
    public Text changePassword_PlaceHolder_confirmPassword;
    public Text changePassword_submitButton;
    public Text changePassword_cancelButton;

    public Text displaynameText;
    public Text displayname;
    public Text changeDisplayname_displayname;
    public Button changeDisplaynameButton;
    public Text changeDisplayname_PlaceHolder_displayname;
    public Text changeDisplayname_submitButton;
    public Text changeDisplayname_cancelButton;

    public Text connectDeviceText;
    public Text connectDeviceValue;
    public Button connectDeviceButton;
    public Text messageConnectDevice;
    public Text connectDeviceSubmitButton;
    public Text connectDeviceCancelButton;

    public Text connectFacebookText;
    public Text connectFacebookValue;
    public Button connectFacebookButton;
    public Text messageConnectFacebook;
    public Text connectFacebookSubmitButton;
    public Text connectFacebookCancelButton;
    string facebookDisplayImage;

    public Text languageText;
    public Text languageValue;
    string[] languageArray;
    int language;
    public Image[] languageButton;
    public Text changeLanguageButton;

    public Text soundText;
    public Image soundSwitchImage;
    public Text openText;
    public Text closeText;

    public Text avatarText;
    public Image displayImage;
    public Image displayImageBorder;
    public Text photoFrameText;
    public Text profilePicText;
    public Button[] photoFrame;
    public Button[] profilePic;
    public Image selectBorderPhotoFrame;
    public Image selectBorderProfilePic;
    int selectPhotoFrame;
    int selectProfilePic;
    public Text changeAvatarButton;

    public Text logoutText;
    public Text messageLogout;
    public Text logoutSubmitButton;
    public Text logoutCancelButton;

    public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public string facebookId;

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
        SetText();
    }

    public void SetText()
    {
        header.text = readData.jsonWordingData["setting"][readData.setting.language].ToString();
        selectGameButtonText.text = readData.jsonWordingData["selectGame"][readData.setting.language].ToString();

        emailText.text = readData.jsonWordingData["email"][readData.setting.language].ToString();
        changeEmail_PlaceHolder_email.text = readData.jsonWordingData["email"][readData.setting.language].ToString();
        changeEmail_PlaceHolder_password.text = readData.jsonWordingData["password"][readData.setting.language].ToString();
        changeEmail_submitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        changeEmail_cancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
        register_PlaceHolder_email.text = readData.jsonWordingData["email"][readData.setting.language].ToString();
        register_PlaceHolder_password.text = readData.jsonWordingData["password"][readData.setting.language].ToString();
        register_PlaceHolder_confirmPassword.text = readData.jsonWordingData["confirmPassword"][readData.setting.language].ToString();
        register_submitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        register_cancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();

        passwordText.text = readData.jsonWordingData["password"][readData.setting.language].ToString();
        changePassword_PlaceHolder_oldPassword.text = readData.jsonWordingData["oldPassword"][readData.setting.language].ToString();
        changePassword_PlaceHolder_newPassword.text = readData.jsonWordingData["newPassword"][readData.setting.language].ToString();
        changePassword_PlaceHolder_confirmPassword.text = readData.jsonWordingData["confirmPassword"][readData.setting.language].ToString();
        changePassword_submitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        changePassword_cancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
        changePasswordButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["edit"][readData.setting.language].ToString();

        displayname.text = readData.userData.displayName;
        displaynameText.text = readData.jsonWordingData["displayname"][readData.setting.language].ToString();
        changeDisplayname_PlaceHolder_displayname.text = readData.jsonWordingData["displayname"][readData.setting.language].ToString();
        changeDisplayname_submitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        changeDisplayname_cancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
        changeDisplaynameButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["edit"][readData.setting.language].ToString();

        connectDeviceText.text = readData.jsonWordingData["loginWithDevice"][readData.setting.language].ToString();
        connectDeviceSubmitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        connectDeviceCancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();

        connectFacebookText.text = readData.jsonWordingData["loginWithFacebook"][readData.setting.language].ToString();
        connectFacebookSubmitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        connectFacebookCancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();

        languageText.text = readData.jsonWordingData["displayLanguage"][readData.setting.language].ToString();
        languageArray = new string[2];
        languageArray[0] = "English";
        languageArray[1] = "ไทย";
        changeLanguageButton.text = readData.jsonWordingData["edit"][readData.setting.language].ToString();

        soundText.text = readData.jsonWordingData["sound"][readData.setting.language].ToString();
        openText.text = readData.jsonWordingData["on"][readData.setting.language].ToString();
        closeText.text = readData.jsonWordingData["off"][readData.setting.language].ToString();

        avatarText.text = readData.jsonWordingData["avatar"][readData.setting.language].ToString();
        changeAvatarButton.text = readData.jsonWordingData["edit"][readData.setting.language].ToString();
        photoFrameText.text = readData.jsonWordingData["photoFrame"][readData.setting.language].ToString();
        profilePicText.text = readData.jsonWordingData["profilePicture"][readData.setting.language].ToString();

        logoutText.text = readData.jsonWordingData["logout"][readData.setting.language].ToString();
        messageLogout.text = readData.jsonWordingData["sureToLogout"][readData.setting.language].ToString();
        logoutSubmitButton.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
        logoutCancelButton.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();

        ChangeEmailData();
        ChangeConnectDeviceData();
        ChangeConnectFacebookData();
        ChangeLanguageData();
        ChangeSoundData();
        ChangeAvatarData();
    }

    void Update()
    {

    }

    public void MenuButton(int position)
    {
        switch (position)
        {
            case 0:
                SceneManager.LoadScene("selectMode");
                break;
            case 1:
                SceneManager.LoadScene("matchHistory");
                break;
            case 2:
                SceneManager.LoadScene("howToPlay");
                break;
            case 3:
                SceneManager.LoadScene("theme");
                break;
            case 4:
                //SceneManager.LoadScene("option");
                break;
        }
    }

    public void SettingBlock1Button()
    {
        if (readData.userData.email == null)
        {
            OpenRegisterEmailBlock();
        }
        else
        {
            OpenChangeEmailBlock();
        }
    }

    public void ChangeEmailData()
    {
        if (readData.userData.email == null)
        {
            email.text = "(" + readData.jsonWordingData["notConnect"][readData.setting.language].ToString() + ")";
            changeEmailButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["connect"][readData.setting.language].ToString();
            changeEmailButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_settings_green");
            password.text = "(" + readData.jsonWordingData["notConnect"][readData.setting.language].ToString() + ")";
            changePasswordButton.interactable = false;
        }
        else
        {
            email.text = readData.userData.email;
            changeEmailButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["edit"][readData.setting.language].ToString();
            changeEmailButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_settings_simple");
            password.text = "********";
            changePasswordButton.interactable = true;
        }
    }

    public void ChangeConnectDeviceData()
    {
        if (readData.userData.guestId == null)
        {
            connectDeviceValue.text = "(" + readData.jsonWordingData["notConnect"][readData.setting.language].ToString() + ")";
            connectDeviceButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_settings_green");
            connectDeviceButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["connect"][readData.setting.language].ToString();
            messageConnectDevice.text = readData.jsonWordingData["sureToConnectDevice"][readData.setting.language].ToString();
        }
        else
        {
            connectDeviceValue.text = "(" + readData.jsonWordingData["connect"][readData.setting.language].ToString() + ")";
            connectDeviceButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_settings_red");
            connectDeviceButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["remove"][readData.setting.language].ToString();
            messageConnectDevice.text = readData.jsonWordingData["sureToRemoveConnectDevice"][readData.setting.language].ToString();
        }
    }

    public void ChangeConnectFacebookData()
    {
        if (readData.userData.facebookId == null)
        {
            connectFacebookValue.text = "(" + readData.jsonWordingData["notConnect"][readData.setting.language].ToString() + ")";
            connectFacebookButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_settings_blue");
            connectFacebookButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["connect"][readData.setting.language].ToString();
            messageConnectFacebook.text = readData.jsonWordingData["sureToConnectFacebook"][readData.setting.language].ToString();
        }
        else
        {
            connectFacebookValue.text = "(" + readData.jsonWordingData["connect"][readData.setting.language].ToString() + ")";
            connectFacebookButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_settings_red");
            connectFacebookButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["remove"][readData.setting.language].ToString();
            messageConnectFacebook.text = readData.jsonWordingData["sureToRemoveConnectFacebook"][readData.setting.language].ToString();
        }
    }

    public void ChangeLanguageData()
    {
        languageValue.text = "(" + languageArray[readData.setting.language] + ")";
        SelectLanguage(readData.setting.language);
    }

    public void ChangeSoundData()
    {
        if (readData.setting.soundOn)
        {
            soundSwitchImage.sprite = Resources.Load<Sprite>("UI/btn_switch_on_simple");
        }
        else
        {
            soundSwitchImage.sprite = Resources.Load<Sprite>("UI/btn_switch_off_simple");
        }
    }

    public void ChangeAvatarData()
    {
        displayImageBorder.sprite = Resources.Load<Sprite>("photo_frame/" + readData.setting.photoFrame);
        if (readData.userData.displayImage != null)
        {
            StartCoroutine(readData.downloadImg(readData.userData.displayImage, displayImage));
        }
        else
        {
            displayImage.sprite = Resources.Load<Sprite>("profile_picture/" + readData.setting.profilePic);
        }
        SelectPhotoFrame(readData.setting.photoFrame);
        SelectProfilePicture(readData.setting.profilePic);
    }

    public void SubmitRegisterButton()
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
        else
        {
            StartCoroutine(Register());
        }
    }

    public IEnumerator Register()
    {
        string param = "registerAfterConnectDevice" + "|" + readData.signatureKey + "|" + registerEmail.text + "|" + registerPassword.text + "|" + readData.userData.guestId + "|" + readData.userData.facebookId;

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
                readData.userData.email = registerEmail.text;
                readData.fileData.SaveUserData(readData.userData);
                ChangeEmailData();
                CloseRegisterEmailBlock();
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

    public void OpenRegisterEmailBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        registerBlock.alpha = 1;
        registerBlock.blocksRaycasts = true;
    }

    public void CloseRegisterEmailBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        registerBlock.alpha = 0;
        registerBlock.blocksRaycasts = false;
    }

    public void SubmitChangeEmailButton()
    {
        if (changeEmail_email.text == "")
        {
            OpenAlertBlock(readData.jsonWordingData["pleaseInputEmail"][readData.setting.language].ToString());
        }
        else if (!Regex.IsMatch(changeEmail_email.text, MatchEmailPattern))
        {
            OpenAlertBlock(readData.jsonWordingData["wrongFormatEmail"][readData.setting.language].ToString());
        }
        else if (changeEmail_password.text == "")
        {
            OpenAlertBlock(readData.jsonWordingData["pleaseInputPassword"][readData.setting.language].ToString());
        }
        else
        {
            StartCoroutine(ChangeEmailService());
        }
    }

    public IEnumerator ChangeEmailService()
    {
        string param = "changeEmail" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|" + changeEmail_email.text + "|" + changeEmail_password.text;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                OpenAlertBlock(readData.jsonWordingData["changeEmailDone"][readData.setting.language].ToString());
                readData.userData.email = changeEmail_email.text;
                readData.fileData.SaveUserData(readData.userData);
                email.text = changeEmail_email.text;
                CloseChangeEmailBlock();
            }
            else if (www.text == "incorrectPassword")
            {
                OpenAlertBlock(readData.jsonWordingData["incorrectPassword"][readData.setting.language].ToString());
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

    public void OpenChangeEmailBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        changeEmailBlock.alpha = 1;
        changeEmailBlock.blocksRaycasts = true;
    }

    public void CloseChangeEmailBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        changeEmailBlock.alpha = 0;
        changeEmailBlock.blocksRaycasts = false;
    }

    public void SubmitChangePasswordButton()
    {
        if (changePassword_oldPassword.text == "")
        {
            OpenAlertBlock(readData.jsonWordingData["pleaseInputOldPassword"][readData.setting.language].ToString());
        }
        else if (changePassword_newPassword.text == "")
        {
            OpenAlertBlock(readData.jsonWordingData["pleaseInputPassword"][readData.setting.language].ToString());
        }
        else if (changePassword_confirmPassword.text == "")
        {
            OpenAlertBlock(readData.jsonWordingData["pleaseInputConfirmPassword"][readData.setting.language].ToString());
        }
        else if (changePassword_newPassword.text != changePassword_confirmPassword.text)
        {
            OpenAlertBlock(readData.jsonWordingData["passwordNotMatch"][readData.setting.language].ToString());
        }
        else
        {
            StartCoroutine(ChangePasswordService());
        }
    }

    public IEnumerator ChangePasswordService()
    {
        string param = "changePassword" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|" + changePassword_oldPassword.text + "|" + changePassword_newPassword.text;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                OpenAlertBlock(readData.jsonWordingData["changePasswordDone"][readData.setting.language].ToString());
                CloseChangePasswordBlock();
            }
            else if (www.text == "incorrectPassword")
            {
                OpenAlertBlock(readData.jsonWordingData["incorrectPassword"][readData.setting.language].ToString());
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public void OpenChangePasswordBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        changePasswordBlock.alpha = 1;
        changePasswordBlock.blocksRaycasts = true;
    }

    public void CloseChangePasswordBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        changePasswordBlock.alpha = 0;
        changePasswordBlock.blocksRaycasts = false;
    }

    public void SubmitChangeDisplaynameButton()
    {
        if (changeDisplayname_displayname.text == "")
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
        string param = "setdisplayname" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|" + changeDisplayname_displayname.text;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                OpenAlertBlock(readData.jsonWordingData["changeDisplayNameDone"][readData.setting.language].ToString());
                readData.userData.displayName = changeDisplayname_displayname.text;
                readData.fileData.SaveUserData(readData.userData);
                displayname.text = changeDisplayname_displayname.text;
                CloseChangeDisplaynameBlock();
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

    public void OpenChangeDisplaynameBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        changeDisplaynameBlock.alpha = 1;
        changeDisplaynameBlock.blocksRaycasts = true;
    }

    public void CloseChangeDisplaynameBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        changeDisplaynameBlock.alpha = 0;
        changeDisplaynameBlock.blocksRaycasts = false;
    }

    public void ConnectDeviceLoginButton()
    {
        CloseConnectDeviceBlock();
        if (readData.userData.guestId == null)
        {
            StartCoroutine(ConnectDeviceService());
        }
        else
        {
            if (readData.userData.email != null || readData.userData.facebookId != null)
            {
                StartCoroutine(RemoveConnectDeviceService());
            }
            else
            {
                OpenAlertBlock(readData.jsonWordingData["registerBeforeRemoveConnectDevice"][readData.setting.language].ToString());
            }
        }
    }

    public IEnumerator ConnectDeviceService()
    {
        string param = "connectDevice" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|" + SystemInfo.deviceUniqueIdentifier;
        //string param = "connectDevice" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|dadsdadas132eqw";

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                readData.userData.guestId = SystemInfo.deviceUniqueIdentifier;
                //readData.userData.guestId = "dadsdadas132eqw";
                readData.fileData.SaveUserData(readData.userData);
                OpenAlertBlock(readData.jsonWordingData["connectDeviceDone"][readData.setting.language].ToString());
                ChangeConnectDeviceData();
            }
            else if (www.text == "already")
            {
                OpenAlertBlock(readData.jsonWordingData["alreadyConnectDevice"][readData.setting.language].ToString());
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public IEnumerator RemoveConnectDeviceService()
    {
        string param = "removeConnectDevice" + "|" + readData.signatureKey + "|" + readData.userData.userId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                readData.userData.guestId = null;
                readData.fileData.SaveUserData(readData.userData);
                OpenAlertBlock(readData.jsonWordingData["removeConnectDeviceDone"][readData.setting.language].ToString());
                ChangeConnectDeviceData();
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public void OpenConnectDeviceBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        connectDeviceBlock.GetComponent<CanvasGroup>().alpha = 1;
        connectDeviceBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseConnectDeviceBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        connectDeviceBlock.GetComponent<CanvasGroup>().alpha = 0;
        connectDeviceBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ConnectFacebookLoginButton()
    {
        CloseConnectFacebookBlock();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OpenAlertBlock(readData.jsonWordingData["internetConnectionLost"][readData.setting.language].ToString());
        }
        else if (readData.userData.facebookId == null)
        {
            List<string> perms = new List<string>() { "public_profile" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        else
        {
            StartCoroutine(RemoveConnectFacebookService());
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
            StartCoroutine(ConnectFacebookService());
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

    public IEnumerator ConnectFacebookService()
    {
        string param = "connectFacebook" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|" + facebookId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                readData.userData.facebookId = facebookId;
                readData.userData.displayImage = "https" + "://graph.facebook.com/" + facebookId + "/picture?access_token=" + facebookDisplayImage;
                readData.fileData.SaveUserData(readData.userData);
                OpenAlertBlock(readData.jsonWordingData["connectFacebookDone"][readData.setting.language].ToString());
                ChangeConnectFacebookData();
                ChangeAvatarData();
            }
            else if (www.text == "already")
            {
                OpenAlertBlock(readData.jsonWordingData["alreadyConnectFacebook"][readData.setting.language].ToString());
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public IEnumerator RemoveConnectFacebookService()
    {
        string param = "removeConnectFacebook" + "|" + readData.signatureKey + "|" + readData.userData.userId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                readData.userData.facebookId = null;
                readData.userData.displayImage = null;
                readData.fileData.SaveUserData(readData.userData);
                OpenAlertBlock(readData.jsonWordingData["removeConnectFacebookDone"][readData.setting.language].ToString());
                ChangeConnectFacebookData();
                ChangeAvatarData();
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public void OpenConnectFacebookBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        connectFacebookBlock.GetComponent<CanvasGroup>().alpha = 1;
        connectFacebookBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseConnectFacebookBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        connectFacebookBlock.GetComponent<CanvasGroup>().alpha = 0;
        connectFacebookBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OpenChangeLanguageBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        changeLanguageBlock.GetComponent<CanvasGroup>().alpha = 1;
        changeLanguageBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseChangeLanguageBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        changeLanguageBlock.GetComponent<CanvasGroup>().alpha = 0;
        changeLanguageBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void SelectLanguage(int position)
    {
        language = position;
        for (int i = 0; i < languageArray.Length; i++)
        {
            if (i == position)
            {
                languageButton[i].sprite = Resources.Load<Sprite>("UI/btn_check_box_active");
            }
            else
            {
                languageButton[i].sprite = Resources.Load<Sprite>("UI/btn_check_box");
            }
        }
    }

    public void SubmitChangeLanguage()
    {
        readData.setting.language = language;
        readData.fileData.SaveSetting(readData.setting);
        SetText();
        GameObject.Find("SelectGameScript").GetComponent<SelectGame>().SetText();
        CloseChangeLanguageBlock();
    }

    public void ChangeSoundOn()
    {
        readData.setting.soundOn = !(readData.setting.soundOn);
        readData.fileData.SaveSetting(readData.setting);
        ChangeSoundData();
    }

    public void OpenChangeAvatarBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        changeAvatarBlock.GetComponent<CanvasGroup>().alpha = 1;
        changeAvatarBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseChangeAvatarBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        changeAvatarBlock.GetComponent<CanvasGroup>().alpha = 0;
        changeAvatarBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void SubmitChangeAvatar()
    {
        StartCoroutine(SetAvatar());
    }

    public IEnumerator SetAvatar()
    {
        string param = "setavatar" + "|" + readData.signatureKey + "|" + readData.userData.userId + "|" + selectPhotoFrame + "|" + selectProfilePic;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                readData.setting.photoFrame = selectPhotoFrame;
                readData.setting.profilePic = selectProfilePic;
                readData.fileData.SaveSetting(readData.setting);
                ChangeAvatarData();
                CloseChangeAvatarBlock();
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public void SelectPhotoFrame(int position)
    {
        selectPhotoFrame = position;
        selectBorderPhotoFrame.rectTransform.anchoredPosition = new Vector3(photoFrame[position - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, photoFrame[position - 1].GetComponent<Image>().rectTransform.anchoredPosition.y);
    }

    public void SelectProfilePicture(int position)
    {
        selectProfilePic = position;
        selectBorderProfilePic.rectTransform.anchoredPosition = new Vector3(profilePic[position - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, profilePic[position - 1].GetComponent<Image>().rectTransform.anchoredPosition.y);
    }

    public void OpenLogoutBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        logoutBlock.GetComponent<CanvasGroup>().alpha = 1;
        logoutBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseLogoutBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        logoutBlock.GetComponent<CanvasGroup>().alpha = 0;
        logoutBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void SubmitLogout()
    {
        Setting data = new Setting();
        readData.fileData.SaveSetting(data);
        SceneManager.LoadScene(0);
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
