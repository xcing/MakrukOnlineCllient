using UnityEngine;
using System.Collections;
using Assets.Script.Component;
using UnityEngine.UI;
using System;
using LitJson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SelectMode : MonoBehaviour
{
    int timeWait = 10;
    public ReadData readData;
    public Text selectGameButtonText;
    public bool isWait;

    public CanvasGroup findMatchCanvasGroup;
    public Image boardBg;
    public Text displayName;
    public Text scoreText;
    public Text rankText;
    public Text scoreValue;
    public Text rankValue;
    public Image rankImage;
    public Text moneyText;
    public Text money;
    public Text gameNameText;
    public Text playOnlineButtonText;
    public Text twoPlayerButtonText;
    public Text winText;
    public Text winValue;
    public Text drawText;
    public Text drawValue;
    public Text loseText;
    public Text loseValue;
    public Image displayImage;
    public Image displayImageBorder;
    public Button selectGameIcon;
    public GameObject noAdsButton;

    public Text findMatchBlockFindingText;
    public Image findMatchBlockDisplayImage;
    public Image findMatchBlockDisplayImageBorder;
    public Text findMatchBlockDisplayName;
    public Text findMatchBlockScoreText;
    public Text findMatchBlockScoreValue;
    public Text findMatchBlockRankValue;
    public Image findMatchBlockRankImage;
    public Image findMatchWaitingImage;
    bool stillFindMatch;

    public CanvasGroup rankingBlock;
    public CanvasGroup loadingCanvasGroup;
    public Image loadingImage;
    public Text rankingButtonText;
    public Text rankingHeader;

    //scrroll rect ranking
    public int amountList;
    public Image boxScrollRect;
    public Image listBox;
    public GameObject listUnitBoxPrefab;
    public GameObject[] listUnitBox;
    public int heightListPanelBox;
    public List<RankingList> datas;

    void OnApplicationPause()
    {
        if (stillFindMatch)
        {
            StopFindMatchButton();
        }
    }

    void Awake()
    {
        BillingManager.init();
    }

    void Start()
    {
        gameNameText.text = readData.getGameName();
        selectGameButtonText.text = readData.jsonWordingData["selectGame"][readData.setting.language].ToString();

        playOnlineButtonText.text = readData.jsonWordingData["playOnline"][readData.setting.language].ToString();
        twoPlayerButtonText.text = readData.jsonWordingData["play2player"][readData.setting.language].ToString();
        noAdsButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["closeAds"][readData.setting.language].ToString()+" (0.99$)";
        findMatchBlockFindingText.text = readData.jsonWordingData["finding"][readData.setting.language].ToString();
        scoreText.text = readData.jsonWordingData["score"][readData.setting.language].ToString();
        rankText.text = readData.jsonWordingData["rank"][readData.setting.language].ToString();
        findMatchBlockScoreText.text = readData.jsonWordingData["score"][readData.setting.language].ToString();
        winText.text = readData.jsonWordingData["win"][readData.setting.language].ToString() + ":";
        drawText.text = readData.jsonWordingData["draw"][readData.setting.language].ToString() + ":";
        loseText.text = readData.jsonWordingData["lose"][readData.setting.language].ToString() + ":";
        moneyText.text = readData.jsonWordingData["coin"][readData.setting.language].ToString();

        boardBg.sprite = readData.GetBgGameMode();
        displayName.text = readData.userData.displayName;
        findMatchBlockDisplayName.text = readData.userData.displayName;
        scoreValue.text = readData.userData.statuser[readData.setting.gameId - 1].score.ToString();
        rankValue.text = readData.calculate.GetRankFromScore(readData.userData.statuser[readData.setting.gameId - 1].score).ToString();
        rankImage.sprite = Resources.Load<Sprite>("UI/" + readData.calculate.GetImageFromScore(readData.userData.statuser[readData.setting.gameId - 1].score));
        findMatchBlockScoreValue.text = readData.userData.statuser[readData.setting.gameId - 1].score.ToString();
        findMatchBlockRankValue.text = readData.calculate.GetRankFromScore(readData.userData.statuser[readData.setting.gameId - 1].score).ToString();
        findMatchBlockRankImage.sprite = Resources.Load<Sprite>("UI/" + readData.calculate.GetImageFromScore(readData.userData.statuser[readData.setting.gameId - 1].score));
        if (readData.userData.statuser[readData.setting.gameId - 1].money > 0)
        {
            money.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString("#,#");
        }
        else
        {
            money.text = "0";
        }
        
        winValue.text = readData.userData.statuser[readData.setting.gameId - 1].win.ToString();
        drawValue.text = readData.userData.statuser[readData.setting.gameId - 1].draw.ToString();
        loseValue.text = readData.userData.statuser[readData.setting.gameId - 1].lose.ToString();

        displayImageBorder.sprite = Resources.Load<Sprite>("photo_frame/" + readData.setting.photoFrame);
        if (readData.userData.displayImage != null)
        {
            StartCoroutine(readData.downloadImg(readData.userData.displayImage, displayImage));
            StartCoroutine(readData.downloadImg(readData.userData.displayImage, findMatchBlockDisplayImage));
        }
        else
        {
            displayImage.sprite = Resources.Load<Sprite>("profile_picture/" + readData.setting.profilePic);
            findMatchBlockDisplayImage.sprite = Resources.Load<Sprite>("profile_picture/" + readData.setting.profilePic);
        }
        findMatchBlockDisplayImageBorder.sprite = Resources.Load<Sprite>("photo_frame/" + readData.setting.photoFrame);

        rankingButtonText.text = readData.jsonWordingData["ranking"][readData.setting.language].ToString();
        rankingHeader.text = readData.jsonWordingData["ranking"][readData.setting.language].ToString();
        if(readData.userData.statuser[readData.setting.gameId-1].ads == 0 || !readData.userData.ads){
            noAdsButton.SetActive(false);
        }
        else{
            noAdsButton.SetActive(true);
        }

        isWait = false;
    }

    public void RankingButton()
    {
        OpenLoading();
        OpenRankingBlock();
        StartCoroutine(GetRankingListFromServer());
    }

    public IEnumerator GetRankingListFromServer()
    {
        string param = "getRankingList" + "|" + readData.signatureKey + "|" + readData.setting.gameId;
        //string param = "getListHistoryMatch" + "|" + readData.signatureKey + "|11|"+page;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            datas = JsonMapper.ToObject<List<RankingList>>(www.text);
            SetDataRankingList();
        }
    }

    public void SetDataRankingList()
    {
        for (int i = listBox.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(listBox.transform.GetChild(i).gameObject);
        }

        amountList = datas.Count;

        listUnitBox = new GameObject[amountList];
        listUnitBoxPrefab = Resources.Load<GameObject>("Prefab/RankingBlock");

        if (amountList > 6)
        {
            heightListPanelBox = (amountList * (Convert.ToInt32(listUnitBoxPrefab.GetComponent<Image>().rectTransform.rect.height) + 9) + 150);
            listBox.rectTransform.anchoredPosition = new Vector3(0, (heightListPanelBox - Convert.ToInt32(boxScrollRect.rectTransform.rect.height)) / 2);
        }
        else
        {
            heightListPanelBox = Convert.ToInt32(boxScrollRect.rectTransform.rect.height);
            listBox.rectTransform.anchoredPosition = new Vector3(0, 0);
        }
        if (heightListPanelBox < boxScrollRect.rectTransform.rect.height)
        {
            heightListPanelBox = Convert.ToInt32(boxScrollRect.rectTransform.rect.height);
            listBox.rectTransform.anchoredPosition = new Vector3(0, 0);
        }
        else
        {
            listBox.rectTransform.anchoredPosition = new Vector3(0, (heightListPanelBox - boxScrollRect.rectTransform.rect.height) / -2);
        }
        listBox.rectTransform.sizeDelta = new Vector2(0, heightListPanelBox);

        for (int i = 0; i < amountList; i++)
        {
            listUnitBox[i] = Instantiate(listUnitBoxPrefab, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
            listUnitBox[i].transform.SetParent(listBox.transform, true);
            listUnitBox[i].transform.localScale = new Vector3(1, 1, 1);
            listUnitBox[i].transform.position = listBox.transform.position;
            listUnitBox[i].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(0, (heightListPanelBox / 2) - 130 - (i * (Convert.ToInt32(listUnitBoxPrefab.GetComponent<Image>().rectTransform.rect.height) + 4)) - (i * 5));
            listUnitBox[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(0, listUnitBox[i].GetComponent<Image>().rectTransform.rect.height);

            listUnitBox[i].GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("profile_picture/" + datas[i].profilePictureId);
            listUnitBox[i].GetComponentsInChildren<Image>()[2].sprite = Resources.Load<Sprite>("photo_frame/" + datas[i].photoFrameId);
            listUnitBox[i].GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["rank"][readData.setting.language].ToString();
            listUnitBox[i].GetComponentsInChildren<Text>()[1].text = (i+1).ToString();
            listUnitBox[i].GetComponentsInChildren<Text>()[2].text = datas[i].displayName;
            listUnitBox[i].GetComponentsInChildren<Text>()[3].text = readData.jsonWordingData["score"][readData.setting.language].ToString();
            listUnitBox[i].GetComponentsInChildren<Text>()[4].text = datas[i].score.ToString("#,#");

            //RankingListTempData tempData = listUnitBox[i].GetComponent<RankingListTempData>();
            //tempData.userId = datas[i].userId;
        }
        CloseLoading();
    }

    public void SpinWaitingImage()
    {
        findMatchWaitingImage.transform.eulerAngles = new Vector3(findMatchWaitingImage.transform.eulerAngles.x, findMatchWaitingImage.transform.eulerAngles.y, findMatchWaitingImage.transform.eulerAngles.z + 20);
    }

    public void SpinWaitingImageRanking()
    {
        loadingImage.transform.eulerAngles = new Vector3(findMatchWaitingImage.transform.eulerAngles.x, findMatchWaitingImage.transform.eulerAngles.y, findMatchWaitingImage.transform.eulerAngles.z + 20);
    }

    public void StopFindMatchButton()
    {
        StartCoroutine(StopFindMatch());
    }

    public IEnumerator FindMatch()
    {
        stillFindMatch = true;
        string param = "findmatch" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + readData.setting.gameId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;
        if (www.error == null)
        {
            string[] result = www.text.Split(',');
            if (result[0] == "success")
            {
                Match match = new Match();
                match.matchId = result[1];
                match.isHost = Convert.ToInt32(result[2]);
                readData.fileData.SaveMatchData(match);
                SceneManager.LoadScene("playOnline");
            }
            else if (result[0] == "wait")
            {
                isWait = true;
                StartCoroutine(WaitMatch(timeWait));
            }
            else
            {
                Debug.Log(result[0]);
            }
        }
    }

    public IEnumerator WaitMatch(float seconds)
    {
        string param = "waitmatch" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + readData.setting.gameId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return StartCoroutine(Wait(seconds));

        if (www.error == null)
        {
            if (stillFindMatch)
            {
                string[] result = www.text.Split(',');
                if (result[0] == "success")
                {
                    Match match = new Match();
                    match.matchId = result[1];
                    match.oppenentDisplayname = result[2];
                    match.oppenentScore = Convert.ToInt32(result[3]);
                    match.isHost = Convert.ToInt32(result[4]);
                    readData.fileData.SaveMatchData(match);
                    SceneManager.LoadScene("playOnline");
                }
                else if (result[0] == "wait")
                {
                    if (isWait)
                    {
                        StartCoroutine(WaitMatch(timeWait));
                    }
                }
                else
                {
                    Debug.Log(result[0]);
                }
            }
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator StopFindMatch()
    {
        stillFindMatch = false;
        string param = "stopfindmatch" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + readData.setting.gameId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if (www.text == "success")
            {
                findMatchCanvasGroup.alpha = 0;
                findMatchCanvasGroup.blocksRaycasts = false;
                selectGameIcon.interactable = true;
                selectGameIcon.GetComponentInChildren<Text>().color = Color.white;
                isWait = false;
                CancelInvoke("SpinWaitingImage");
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public void PlayOnlineButton()
    {
        findMatchCanvasGroup.alpha = 1;
        findMatchCanvasGroup.blocksRaycasts = true;
        selectGameIcon.interactable = false;
        selectGameIcon.GetComponentInChildren<Text>().color = new Color32(88, 88, 88, 255);

        InvokeRepeating("SpinWaitingImage", 0, 0.05f);

        StartCoroutine(FindMatch());
        //SceneManager.LoadScene(3);
    }

    public void TwoPlayerButton()
    {
        SceneManager.LoadScene("twoPlayer");
    }

    public void NoAdsButton()
    {
        BillingManager.purchase(BillingManager.NO_ADS_PRODUCT_ID);
    }

    public void BuyNoAds()
    {
        StartCoroutine(BuyNoAdsService());
    }

    public IEnumerator BuyNoAdsService()
    {
        string param = "buyNoads" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            if ((string)www.text == "success")
            {
                readData.userData.statuser[readData.setting.gameId - 1].ads = 0;
                readData.fileData.SaveUserData(readData.userData);
                noAdsButton.SetActive(false);
            }
            else
            {
                Debug.Log(www.text);
            }
        }
    }

    public void MenuButton(int position)
    {
        switch (position)
        {
            case 0:
                //SceneManager.LoadScene("selectMode");
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
                SceneManager.LoadScene("option");
                break;
        }
    }

    public void OpenLoading()
    {
        loadingCanvasGroup.alpha = 1;
        loadingCanvasGroup.blocksRaycasts = true;
        InvokeRepeating("SpinWaitingImageRanking", 0, 0.05f);
    }

    public void CloseLoading()
    {
        loadingCanvasGroup.alpha = 0;
        loadingCanvasGroup.blocksRaycasts = false;
        CancelInvoke("SpinWaitingImageRanking");
    }

    public void OpenRankingBlock()
    {
        rankingBlock.alpha = 1;
        rankingBlock.blocksRaycasts = true;
    }

    public void CloseRankingBlock()
    {
        rankingBlock.alpha = 0;
        rankingBlock.blocksRaycasts = false;
    }
}
