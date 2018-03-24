using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Assets.Script.Component;
using LitJson;
using UnityEngine.SceneManagement;

public class MatchHistory : MonoBehaviour
{
    public ReadData readData;
    public Text selectGameButtonText;

    Sprite resultImage;
    string resultText;
    Color32 resultColor;
    public Color32 winColor;
    public Color32 drawColor;
    public Color32 loseColor;
    public Text header;
    public CanvasGroup loadingCanvasGroup;
    public Image loadingImage;
    public Text rankValue;
    public Text displayName;
    public Text winValue;
    public Text drawValue;
    public Text loseValue;
    public Text scoreValue;

    //scrroll rect match
    public int amountList;
    public Image boxScrollRect;
    public Image listBox;
    public GameObject listUnitBoxPrefab;
    public GameObject[] listUnitBox;
    public int heightListPanelBox;
    public List<HistoryMatchList> datas;

    //scrroll rect all match
    public int all_amountList;
    public Image all_boxScrollRect;
    public Image all_listBox;
    public GameObject all_listUnitBoxPrefab;
    public GameObject[] all_listUnitBox;
    public int all_heightListPanelBox;
    public List<HistoryMatchList> all_datas;

    public CanvasGroup bgFadeCanvasGroup;
    public CanvasGroup viewAllBlockCanvasGroup;

    void Start()
    {
        InvokeRepeating("SpinWaitingImage", 0, 0.05f);
        header.text = readData.jsonWordingData["historys"][readData.setting.language].ToString();
        selectGameButtonText.text = readData.jsonWordingData["selectGame"][readData.setting.language].ToString();

        rankValue.text = readData.calculate.GetRankFromScore(readData.userData.statuser[readData.setting.gameId - 1].score).ToString();
        displayName.text = readData.userData.displayName;
        winValue.text = readData.userData.statuser[readData.setting.gameId - 1].win.ToString();
        drawValue.text = readData.userData.statuser[readData.setting.gameId - 1].draw.ToString();
        loseValue.text = readData.userData.statuser[readData.setting.gameId - 1].lose.ToString();
        scoreValue.text = readData.userData.statuser[readData.setting.gameId - 1].score.ToString();

        StartCoroutine(GetHistoryFromServer(1));
        //SetDataMatchList();
    }

    public IEnumerator GetHistoryFromServer(int page)
    {
        string param = "getListHistoryMatch" + "|" + readData.signatureKey + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId + "|" + page;
        //string param = "getListHistoryMatch" + "|" + readData.signatureKey + "|11|"+page;

        string encryptData = readData.encryption.EncryptRJ256(param);
        WWWForm form = new WWWForm();
        form.AddField("data", encryptData);
        WWW www = new WWW(readData.url, form);
        yield return www;

        if (www.error == null)
        {
            datas = JsonMapper.ToObject<List<HistoryMatchList>>(www.text);
            SetDataMatchList();
        }
    }

    public void SetDataMatchList()
    {
        for (int i = listBox.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(listBox.transform.GetChild(i).gameObject);
        }

        amountList = datas.Count;

        listUnitBox = new GameObject[amountList];
        listUnitBoxPrefab = Resources.Load<GameObject>("Prefab/MatchBlock");

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

            if (datas[i].result == 1)
            {
                resultImage = Resources.Load<Sprite>("UI/ico_history_status_win");
            }
            else if (datas[i].result == 2)
            {
                resultImage = Resources.Load<Sprite>("UI/ico_history_status_lost");
            }
            else if (datas[i].result == 3)
            {
                resultImage = Resources.Load<Sprite>("UI/ico_history_status_draw");
            }
            listUnitBox[i].GetComponentsInChildren<Text>()[0].text = datas[i].ordinal.ToString() + " |";
            listUnitBox[i].GetComponentsInChildren<Text>()[1].rectTransform.anchoredPosition = new Vector3(listUnitBox[i].GetComponentsInChildren<Text>()[1].rectTransform.anchoredPosition.x + readData.calculate.GetWidthPlus((amountList - i), 38), listUnitBox[i].GetComponentsInChildren<Text>()[1].rectTransform.anchoredPosition.y);
            listUnitBox[i].GetComponentsInChildren<Text>()[1].text = datas[i].createdDate;
            listUnitBox[i].GetComponentsInChildren<Text>()[3].text = datas[i].myScore;
            listUnitBox[i].GetComponentsInChildren<Image>()[1].sprite = resultImage;
            listUnitBox[i].GetComponentsInChildren<Text>()[4].text = datas[i].opponentName;
            listUnitBox[i].GetComponentsInChildren<Text>()[6].text = readData.calculate.GetRankFromScore(datas[i].opponentScore).ToString();
            listUnitBox[i].GetComponentsInChildren<Text>()[8].text = datas[i].opponentScore.ToString();
            if (datas[i].haveHistory == 1)
            {
                listUnitBox[i].GetComponentsInChildren<Button>()[1].GetComponent<CanvasGroup>().alpha = 1;
                listUnitBox[i].GetComponentsInChildren<Button>()[1].GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            MatchHistoryTempData tempData = listUnitBox[i].GetComponent<MatchHistoryTempData>();
            tempData.matchId = datas[i].matchId;
            tempData.matchOrdinal = datas[i].ordinal;
            //listUnitBox[i].GetComponentsInChildren<Button>()[1].onClick.AddListener(() => tempData.WatchReplay());

            EventTrigger.Entry entry = new EventTrigger.Entry();
            //entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { tempData.WatchReplay(); });
            listUnitBox[i].GetComponentsInChildren<Button>()[1].GetComponent<EventTrigger>().triggers.Add(entry);
        }
        SetDataAllMatchList(0);
        CloseLoading();
    }

    public void SetDataAllMatchList(int result)
    {
        for (int i = all_listBox.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(all_listBox.transform.GetChild(i).gameObject);
        }
        if (result == 1)
        {
            all_datas = datas.FindAll(x => x.result == 1);
        }
        else if (result == 2)
        {
            all_datas = datas.FindAll(x => x.result == 2);
        }
        else if (result == 3)
        {
            all_datas = datas.FindAll(x => x.result == 3);
        }
        else if (result == 0)
        {
            all_datas = datas;
        }

        all_amountList = all_datas.Count;

        all_listUnitBox = new GameObject[all_amountList];
        all_listUnitBoxPrefab = Resources.Load<GameObject>("Prefab/OrdinalMatch");

        if (all_amountList > 28)
        {
            if (all_amountList % 4 == 0)
            {
                all_heightListPanelBox = ((all_amountList / 4) * (Convert.ToInt32(all_listUnitBoxPrefab.GetComponent<Image>().rectTransform.rect.height) + 100) + 50);
            }
            else
            {
                all_heightListPanelBox = (((all_amountList / 4) + 1) * (Convert.ToInt32(all_listUnitBoxPrefab.GetComponent<Image>().rectTransform.rect.height) + 100) + 50);
            }

            all_listBox.rectTransform.anchoredPosition = new Vector3(0, (all_heightListPanelBox - Convert.ToInt32(all_boxScrollRect.rectTransform.rect.height)) / 2);
        }
        else
        {
            all_heightListPanelBox = Convert.ToInt32(all_boxScrollRect.rectTransform.rect.height);
            all_listBox.rectTransform.anchoredPosition = new Vector3(0, 0);
        }
        if (all_heightListPanelBox < all_boxScrollRect.rectTransform.rect.height)
        {
            all_heightListPanelBox = Convert.ToInt32(all_boxScrollRect.rectTransform.rect.height);
            all_listBox.rectTransform.anchoredPosition = new Vector3(0, 0);
        }
        else
        {
            all_listBox.rectTransform.anchoredPosition = new Vector3(0, all_heightListPanelBox / -2);
        }
        all_listBox.rectTransform.sizeDelta = new Vector2(0, all_heightListPanelBox);

        for (int i = 0; i < all_amountList; i++)
        {
            all_listUnitBox[i] = Instantiate(all_listUnitBoxPrefab, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
            all_listUnitBox[i].transform.SetParent(all_listBox.transform, true);
            all_listUnitBox[i].transform.localScale = new Vector3(1, 1, 1);
            all_listUnitBox[i].transform.position = all_listBox.transform.position;
            all_listUnitBox[i].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(((i % 4) * all_listUnitBox[i].GetComponent<Image>().rectTransform.rect.width) - 500, (all_heightListPanelBox / 2) - 130 - ((i / 4) * (Convert.ToInt32(all_listUnitBoxPrefab.GetComponent<Image>().rectTransform.rect.height) + 100)));
            all_listUnitBox[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(0, all_listUnitBox[i].GetComponent<Image>().rectTransform.rect.height);

            if (all_datas[i].result == 1)
            {
                resultText = "W";
                resultColor = winColor;
            }
            else if (all_datas[i].result == 2)
            {
                resultText = "L";
                resultColor = loseColor;
            }
            else if (all_datas[i].result == 3)
            {
                resultText = "D";
                resultColor = drawColor;
            }

            all_listUnitBox[i].GetComponentsInChildren<Text>()[0].text = all_datas[i].ordinal.ToString();
            all_listUnitBox[i].GetComponentsInChildren<Text>()[1].text = resultText;
            all_listUnitBox[i].GetComponentsInChildren<Text>()[1].color = resultColor;
        }
    }

    public void SetCurrentMatchId(int matchId, int matchOrdinal)
    {
        readData.userData.currentMatchId = matchId;
        readData.userData.currentMatchOrdinal = matchOrdinal;
        readData.fileData.SaveUserData(readData.userData);

        SceneManager.LoadScene("replay");
    }

    public void MenuButton(int position)
    {
        switch (position)
        {
            case 0:
                SceneManager.LoadScene("selectMode");
                break;
            case 1:
                //SceneManager.LoadScene("matchHistory");
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

    public void CloseLoading()
    {
        loadingCanvasGroup.alpha = 0;
        loadingCanvasGroup.blocksRaycasts = false;
        CancelInvoke("SpinWaitingImage");
    }

    public void SpinWaitingImage()
    {
        loadingImage.transform.eulerAngles = new Vector3(loadingImage.transform.eulerAngles.x, loadingImage.transform.eulerAngles.y, loadingImage.transform.eulerAngles.z + 20);
    }

    public void OpenViewAllBlock()
    {
        bgFadeCanvasGroup.alpha = 1;
        bgFadeCanvasGroup.blocksRaycasts = true;
        viewAllBlockCanvasGroup.alpha = 1;
        viewAllBlockCanvasGroup.blocksRaycasts = true;
    }

    public void CloseViewAllBlock()
    {
        bgFadeCanvasGroup.alpha = 0;
        bgFadeCanvasGroup.blocksRaycasts = false;
        viewAllBlockCanvasGroup.alpha = 0;
        viewAllBlockCanvasGroup.blocksRaycasts = false;
    }

    public void AllButton()
    {
        SetDataAllMatchList(0);
    }

    public void WinButton()
    {
        SetDataAllMatchList(1);
    }

    public void DrawButton()
    {
        SetDataAllMatchList(3);
    }

    public void LoseButton()
    {
        SetDataAllMatchList(2);
    }

    void Update()
    {

    }
}
