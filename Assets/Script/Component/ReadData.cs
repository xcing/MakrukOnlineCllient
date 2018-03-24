using UnityEngine;
using System;
using System.IO;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Assets.Script.Component
{
    public class ReadData : MonoBehaviour
    {
        public string userId;
        //public int languageId;
        public FileData fileData;
        public Encryption encryption;
        public string signatureKey;
        public string url;
        public JsonData jsonWordingData;
        public User userData;
        public Match match;
        public Setting setting;
        public int themeId;
        public Calculate calculate;
        AudioClip moveSound;

        void Awake()
        {
            moveSound = Resources.Load<AudioClip>("sound/moveSound");
            //languageId = 0;
            themeId = 1;
            signatureKey = "XD1c3@ftheendc1an";
            encryption = new Encryption();
            fileData = new FileData();
            calculate = new Calculate();
            url = "http://checkmate.whitenightsoft.com/index.php?r=service/update";
            //url = "http://checkmate.gurugames.in.th/index.php?r=service/update";
            jsonWordingData = GetDataOnFile("wording");
            if (fileData.LoadUserData() != null)
            {
                userData = fileData.LoadUserData();
            }
            if (fileData.LoadMatchData() != null)
            {
                match = fileData.LoadMatchData();
            }
            if (fileData.LoadSetting() != null)
            {
                setting = fileData.LoadSetting();
            }
        }

        public JsonData GetDataOnFile(string filename)
        {
            TextAsset ta = Resources.Load<TextAsset>("File json/" + filename);
            JsonData jsonData = JsonMapper.ToObject(ta.text);
            return jsonData;
        }

        public string getGameName()
        {
            switch (setting.gameId)
            {
                case 1:
                    return jsonWordingData["thaiChess"][setting.language].ToString();
                case 2:
                    return jsonWordingData["thaiChecker"][setting.language].ToString();
                case 3:
                    return jsonWordingData["chess"][setting.language].ToString();
                default:
                    return jsonWordingData["thaiChess"][setting.language].ToString();
            }
        }

        public Sprite GetBgGameMode()
        {
            switch (setting.gameId)
            {
                case 1:
                    return Resources.Load<Sprite>("games/chess_thai/bg_game_mode");
                case 2:
                    return Resources.Load<Sprite>("games/checkers_thai/bg_game_mode");
                case 3:
                    return Resources.Load<Sprite>("games/chess_inter/bg_game_mode");
                default:
                    return Resources.Load<Sprite>("games/chess_thai/bg_game_mode");
            }
        }

        public void PlayAudioMove()
        {
            if (setting.soundOn)
            {
                GetComponent<AudioSource>().PlayOneShot(moveSound);
            }
        }

        public IEnumerator downloadImg(string url, Image displayImage)
        {
            Texture2D texture = new Texture2D(1, 1);
            WWW www = new WWW(url);
            yield return www;
            www.LoadImageIntoTexture(texture);

            Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            displayImage.sprite = image;
        }
    }
}
