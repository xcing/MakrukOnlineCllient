using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.Component;
using UnityEngine.SceneManagement;
using LitJson;

namespace Assets.Script.chess_thai
{
    public class Theme : MonoBehaviour
    {
        public ReadData readData;
        public Text header;
        public Text buycoinText;
        public CanvasGroup equipedCanvasGroup;
        public CanvasGroup buyButtonCanvasGroup;
        public CanvasGroup equipCanvasGroup;
        public Image themePreview;
        public Text themeName;
        public Text themePrice;
        public Image moneyIcon;
        public Text moneyValue;
        public Text equip;
        public Text equiped;
        public CanvasGroup bgFadeCanvasGroup;
        public CanvasGroup confirmCanvasGroup;
        public CanvasGroup buyCoinCanvasGroup;
        public CanvasGroup alertCanvasGroup;
        public Text alertText;
        public Text confirmText;
        public Text ConfirmBlockSubmitText;
        public Text ConfirmBlockCancelText;
        public Text AlertBlockSubmitText;
        public Text buyCoinHeader;

        Sprite[] themeFilenameArray;
        string[] themeNameArray;
        int[] themePriceArray;
        int currentThemeShow;
        int amountTheme;

        void Awake()
        {
            BillingManager.init();
        }

        void Start()
        {
            header.text = readData.jsonWordingData["theme"][readData.setting.language].ToString();
            buycoinText.text = readData.jsonWordingData["buyCoin"][readData.setting.language].ToString();
            equip.text = readData.jsonWordingData["equip"][readData.setting.language].ToString();
            equiped.text = readData.jsonWordingData["equiped"][readData.setting.language].ToString();
            ConfirmBlockSubmitText.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            ConfirmBlockCancelText.text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
            AlertBlockSubmitText.text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            buyCoinHeader.text = readData.jsonWordingData["buyCoin"][readData.setting.language].ToString();

            currentThemeShow = readData.userData.statuser[readData.setting.gameId-1].themeEquip-1;
            amountTheme = 5;
            themeFilenameArray = new Sprite[amountTheme];
            themeNameArray = new string[amountTheme];
            themePriceArray = new int[amountTheme];

            themeNameArray[0] = "Default";
            themePriceArray[0] = 0;
            themeNameArray[1] = "black & white";
            themePriceArray[1] = 0;
            themeNameArray[2] = "White marble";
            themePriceArray[2] = 15000;
            themeNameArray[3] = "Black marble";
            themePriceArray[3] = 15000;
            themeNameArray[4] = "Metallic";
            themePriceArray[4] = 25000;
            for (int i = 1; i <= amountTheme; i++)
            {
                themeFilenameArray[i-1] = Resources.Load<Sprite>("games/chess_thai/" + i + "/preview");
            }

            themePreview.sprite = themeFilenameArray[currentThemeShow];
            themeName.text = themeNameArray[currentThemeShow];
            themePrice.text = themePriceArray[currentThemeShow].ToString("#,#");
            if (readData.userData.statuser[readData.setting.gameId - 1].money != 0)
            {
                moneyValue.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString("#,#");
            }
            else
            {
                moneyValue.text = "0";
            }
            //moneyIcon.rectTransform.anchoredPosition = new Vector3(moneyIcon.rectTransform.anchoredPosition.x - readData.calculate.GetWidthPlus(readData.userData.statuser[readData.setting.gameId-1].money, 45), moneyIcon.rectTransform.anchoredPosition.y);
        }

        public void EquipButton()
        {
            StartCoroutine(EquipService());
        }

        public IEnumerator EquipService()
        {
            string param = "equipTheme" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + (currentThemeShow + 1);

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                if ((string)www.text == "success")
                {
                    readData.userData.statuser[readData.setting.gameId - 1].themeEquip = (currentThemeShow + 1);
                    readData.fileData.SaveUserData(readData.userData);
                    ChangeAction(0);
                    alertText.text = readData.jsonWordingData["equipThemeDone"][readData.setting.language].ToString();
                    OpenAlertBlock();
                }
                else if ((string)www.text == "nomoney")
                {
                    CloseConfirmBlock();
                    alertText.text = readData.jsonWordingData["notEnoughCoin"][readData.setting.language].ToString();
                    OpenAlertBlock();
                }
                else
                {
                    Debug.Log(www.text);
                }
            }
        }

        public void BuyButton()
        {
            if (readData.userData.statuser[readData.setting.gameId - 1].money < themePriceArray[currentThemeShow])
            {
                alertText.text = readData.jsonWordingData["notEnoughCoin"][readData.setting.language].ToString();
                OpenAlertBlock();
            }
            else
            {
                confirmText.text = readData.jsonWordingData["confirmBuyTheme"][readData.setting.language].ToString();
                OpenConfirmBlock();
            }
        }

        public void ConfirmBuyButton()
        {
            StartCoroutine(BuyService());
        }

        public IEnumerator BuyService()
        {
            string param = "buyTheme" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + (currentThemeShow + 1);

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;
            Debug.Log(www.text);
            if (www.error == null)
            {
                if ((string)www.text == "success")
                {
                    readData.userData.statuser[readData.setting.gameId - 1].themeHave.Add(currentThemeShow + 1);
                    readData.userData.statuser[readData.setting.gameId - 1].themeEquip = (currentThemeShow + 1);
                    readData.userData.statuser[readData.setting.gameId - 1].money -= themePriceArray[currentThemeShow];
                    readData.fileData.SaveUserData(readData.userData);
                    moneyValue.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString("#,#");
                    ChangeAction(0);
                    CloseConfirmBlock();
                    alertText.text = readData.jsonWordingData["buyThemeDone"][readData.setting.language].ToString();
                    OpenAlertBlock();
                }
                else if ((string)www.text == "nomoney")
                {
                    CloseConfirmBlock();
                    alertText.text = readData.jsonWordingData["notEnoughCoin"][readData.setting.language].ToString();
                    OpenAlertBlock();
                }
                else
                {
                    Debug.Log(www.text);
                }
            }
        }

        public void MoveTheme(bool isRight)
        {
            if (isRight)
            {
                currentThemeShow++;
                if(currentThemeShow == amountTheme){
                    currentThemeShow = 0;
                }
            }
            else
            {
                currentThemeShow--;
                if (currentThemeShow == -1)
                {
                    currentThemeShow = (amountTheme-1);
                }
            }
            themePreview.sprite = themeFilenameArray[currentThemeShow];
            themeName.text = themeNameArray[currentThemeShow];
            themePrice.text = themePriceArray[currentThemeShow].ToString("#,#");
            if(readData.userData.statuser[readData.setting.gameId-1].themeEquip == (currentThemeShow+1)){
                ChangeAction(0);
            }
            else if (readData.userData.statuser[readData.setting.gameId - 1].themeHave.Find(x => x == (currentThemeShow+1)) != 0)
            {
                ChangeAction(2);
            }
            else{
                ChangeAction(1);
            }
            
        }

        public void ChangeAction(int position)
        {
            switch (position)
            {
                case 0:
                    equipedCanvasGroup.alpha = 1;
                    equipedCanvasGroup.blocksRaycasts = true;
                    buyButtonCanvasGroup.alpha = 0;
                    buyButtonCanvasGroup.blocksRaycasts = false;
                    equipCanvasGroup.alpha = 0;
                    equipCanvasGroup.blocksRaycasts = false;
                    break;
                case 1:
                    equipedCanvasGroup.alpha = 0;
                    equipedCanvasGroup.blocksRaycasts = false;
                    buyButtonCanvasGroup.alpha = 1;
                    buyButtonCanvasGroup.blocksRaycasts = true;
                    equipCanvasGroup.alpha = 0;
                    equipCanvasGroup.blocksRaycasts = false;
                    break;
                case 2:
                    equipedCanvasGroup.alpha = 0;
                    equipedCanvasGroup.blocksRaycasts = false;
                    buyButtonCanvasGroup.alpha = 0;
                    buyButtonCanvasGroup.blocksRaycasts = false;
                    equipCanvasGroup.alpha = 1;
                    equipCanvasGroup.blocksRaycasts = true;
                    break;
            }
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
                    //SceneManager.LoadScene("theme");
                    break;
                case 4:
                    SceneManager.LoadScene("option");
                    break;
            }
        }

        public void CloseAlertBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            alertCanvasGroup.alpha = 0;
            alertCanvasGroup.blocksRaycasts = false;
        }

        public void OpenAlertBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            alertCanvasGroup.alpha = 1;
            alertCanvasGroup.blocksRaycasts = true;
        }

        public void CloseConfirmBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            confirmCanvasGroup.alpha = 0;
            confirmCanvasGroup.blocksRaycasts = false;
        }

        public void OpenConfirmBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            confirmCanvasGroup.alpha = 1;
            confirmCanvasGroup.blocksRaycasts = true;
        }

        public void CloseBuyCoinBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            buyCoinCanvasGroup.alpha = 0;
            buyCoinCanvasGroup.blocksRaycasts = false;
        }

        public void OpenBuyCoinBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            buyCoinCanvasGroup.alpha = 1;
            buyCoinCanvasGroup.blocksRaycasts = true;
        }

        public void BuyCoinButton(int position)
        {
            switch (position)
            {
                case 1:
                    BillingManager.purchase(BillingManager.ONE_DOLLAR_PRODUCT_ID);
                    break;
                case 2:
                    BillingManager.purchase(BillingManager.FIVE_DOLLAR_PRODUCT_ID);
                    break;
                case 3:
                    BillingManager.purchase(BillingManager.TEN_DOLLAR_PRODUCT_ID);
                    break;
            }
        }

        public void BuyCoin(int amount)
        {
            StartCoroutine(BuyCoinService(amount));
        }

        public IEnumerator BuyCoinService(int amount)
        {
            string param = "buyCoin" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + amount;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                if ((string)www.text == "success")
                {
                    readData.userData.statuser[readData.setting.gameId - 1].money += amount;
                    readData.fileData.SaveUserData(readData.userData);
                    if (readData.userData.statuser[readData.setting.gameId - 1].money != 0)
                    {
                        moneyValue.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString("#,#");
                    }
                    else
                    {
                        moneyValue.text = "0";
                    }
                    CloseBuyCoinBlock();
                }
                else
                {
                    Debug.Log(www.text);
                }
            }
        }
    }
}
