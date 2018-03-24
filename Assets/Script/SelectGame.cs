using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.Component;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class SelectGame : MonoBehaviour
{
    public ReadData readData;
    public Text header;
    public Text[] gameName;
    public Text[] onlineAmount;
    public CanvasGroup selectGameCanvasGroup;
    public Text[] menuText;

    void Start()
    {
        SetText();
    }

    public void SetText()
    {
        header.text = readData.jsonWordingData["selectGame"][readData.setting.language].ToString();

        gameName[0].text = readData.jsonWordingData["thaiChess"][readData.setting.language].ToString();
        //gameName[1].text = readData.jsonWordingData["thaiChecker"][readData.setting.language].ToString();
        //gameName[2].text = readData.jsonWordingData["chess"][readData.setting.language].ToString();
        onlineAmount[0].text = "12";
        onlineAmount[1].text = "0";
        onlineAmount[2].text = "125252452";
        if(menuText.Length > 1){
            menuText[0].text = readData.jsonWordingData["play"][readData.setting.language].ToString();
            menuText[1].text = readData.jsonWordingData["history"][readData.setting.language].ToString();
            menuText[2].text = readData.jsonWordingData["howtoplay"][readData.setting.language].ToString();
            menuText[3].text = readData.jsonWordingData["theme"][readData.setting.language].ToString();
            menuText[4].text = readData.jsonWordingData["setting"][readData.setting.language].ToString();
        }
    }

    void Update()
    {

    }

    public void SelectGameButton(int position)
    {
        if (readData.setting.gameId == position && SceneManager.GetActiveScene().name != "selectGame")
        {
            CloseSelectGameButton();
        }
        else
        {
            //readData.setting.gameId = position;
            readData.fileData.SaveSetting(readData.setting);
            SceneManager.LoadScene("selectMode");
        }
    }

    public void CloseSelectGameButton()
    {
        selectGameCanvasGroup.alpha = 0;
        selectGameCanvasGroup.blocksRaycasts = false;
    }

    public void OpenSelectGameButton()
    {
        selectGameCanvasGroup.alpha = 1;
        selectGameCanvasGroup.blocksRaycasts = true;
    }
}
