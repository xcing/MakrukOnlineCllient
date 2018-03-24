using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.Component;
using UnityEngine.SceneManagement;

public class SelectLanguage : MonoBehaviour
{
    public ReadData readData;
    public int language;
    public int languageAmount = 2;
    public Image[] languageButton;

    void Start()
    {
        if(readData.setting.language != 99){
            SceneManager.LoadScene("login");
        }
    }

    public void SubmitChangeLanguage()
    {
        readData.setting.language = language;
        readData.fileData.SaveSetting(readData.setting);
        SceneManager.LoadScene("login");
    }

    public void SelectLang(int position)
    {
        language = position;
        for (int i = 0; i < languageAmount; i++)
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
}
