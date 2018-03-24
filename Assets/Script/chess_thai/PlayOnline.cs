using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using LitJson;
using Assets.Script.Component;
using UnityEngine.SceneManagement;

namespace Assets.Script.chess_thai
{
    public class PlayOnline : MonoBehaviour
    {
        Sprite img_turn_active;
        Sprite img_turn_default;
        Sprite img_turn_now;
        Sprite img_turn_lastest;
        Sprite img_turn_check;
        public Image img_board;

        public ReadData readData;
        List<Pieces> pieces;
        GameObject prefab;
        public List<GameObject> entity;
        public List<List<GameObject>> block;
        List<GameObject> blockTemp;
        string borderName;
        string borderName2;
        int borderI;
        int borderJ;
        int countTemp;
        List<Pieces> piecesTemp;
        Pieces piecesTemp2;
        bool prepareMove;
        int piecesIdPrepareMove;
        public bool isBlackTurn;
        GameObject[] borders;
        public Text countTurnText;
        public Text countTurn;
        List<History> historys;
        public bool isGameOver;
        bool isChecked;
        List<Pieces> piecesIsChecked;
        int turnCountPieces;
        int maxTurnCountPieces;
        int turnCountBoard;
        //int maxTurnCountBoard;
        bool isCountTurnPieces;
        bool isCountTurnBoard;
        bool isBlackCountTurnPieces;
        bool isBlackCountTurnBoard;
        public Button countTurnBoardButton;
        public bool iAmBlack;

        int lastMovePieceId;
        int lastMoveX;
        int lastMoveY;
        int ordinal = 1;
        //int eatedId = 0;
        int useSec = 0;
        bool isWon = false;

        public Player lastMove;
        public Text playerName1;
        public Text playerName2;
        public Text playerTime1;
        public Text playerTime2;
        public Text playerTimeTurn;
        public int playerMin1;
        int playerSec1;
        public int playerMin2;
        int playerSec2;
        int playerTurnSec;
        public int playerDisconnectTime1;
        public int playerDisconnectTime2;
        bool countdown1;
        //bool countdown2;
        bool playerDisconnectIsBlack;

        public Text closeReceiveEmoticonButtonText;
        public Text drawButtonText;
        public Text concedeButtonText;
        public Image resultBlock;
        public Image drawBlock;
        public Image askDrawBlock;
        public Image concedeBlock;
        public Image countBoardBlock;
        public Image cannotAskForDrawBlock;
        public Image textAskForDrawBlock;
        public Image internetProblemBlock;
        public Image optionBlock;
        public Image opponentInternetProblemBlock;
        public CanvasGroup bgFadeCanvasGroup;
        public CanvasGroup bgFade2CanvasGroup;
        public CanvasGroup loadingCanvasGroup;
        public int countTurnAskForDraw;

        public Text[] colBoard;
        public Text[] rowBoard;
        //public Image displayNameBg1;
        //public Image displayNameBg2;

        GoogleMobileAdBanner banner;
        string MY_BANNERS_AD_UNIT_ID = "ca-app-pub-9781457328779143/6673315110";
        string MY_INTERSTISIALS_AD_UNIT_ID = "ca-app-pub-9781457328779143/8150048315";

        public Image advertise;
        int currentAdvertise;
        List<Advertise> adverList;

        public CanvasGroup emoticonBlockCanvasGroup;
        public Image emoticonBorder;
        public Image[] emoticon;
        int amountSendOnTurn;
        bool isCloseReceiveEmoticon;
        public CanvasGroup emoticonDisplayCanvasGroup1;
        public CanvasGroup emoticonDisplayCanvasGroup2;
        public Image emoticonDisplay1;
        public Image emoticonDisplay2;
        float countDownEmoticon1;
        float countDownEmoticon2;

        public CanvasGroup highlightPlayerNameCanvasGroup1;
        public CanvasGroup highlightPlayerNameCanvasGroup2;

        public Image displayImage;
        public Image displayImageBorder;
        public Image resultImage;
        public Text currentScore;
        public Text resultScore;
        public Text dailyMission;
        public Text currentCoin;
        public Text resultCoin;
        int score1;
        int score2;

        void Awake()
        {
            img_turn_active = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_active");
            img_turn_default = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_default");
            img_turn_now = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_now");
            img_turn_lastest = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_latest");
            img_turn_check = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_check");
            img_board.sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/bg_play_game_chess_thai");

            closeReceiveEmoticonButtonText.text = readData.jsonWordingData["closeReceiveEmoticon"][readData.setting.language].ToString();
            drawButtonText.text = readData.jsonWordingData["draw"][readData.setting.language].ToString();
            concedeButtonText.text = readData.jsonWordingData["concede"][readData.setting.language].ToString();
            countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
            drawBlock.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            drawBlock.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
            askDrawBlock.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            askDrawBlock.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
            concedeBlock.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            concedeBlock.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = readData.jsonWordingData["cancel"][readData.setting.language].ToString();
            cannotAskForDrawBlock.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            textAskForDrawBlock.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            drawBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["opponentAskForDraw"][readData.setting.language].ToString();
            askDrawBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["askForDraw"][readData.setting.language].ToString();
            concedeBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["confirmConcede"][readData.setting.language].ToString();
            cannotAskForDrawBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["afterOpponentRefuse"][readData.setting.language].ToString();
            textAskForDrawBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["textAskForDraw"][readData.setting.language].ToString();
            internetProblemBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["internetConnectionLost"][readData.setting.language].ToString();
            opponentInternetProblemBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["opponentInternetConnectionLost"][readData.setting.language].ToString();
            colBoard[0].text = readData.jsonWordingData["a"][readData.setting.language].ToString();
            colBoard[1].text = readData.jsonWordingData["b"][readData.setting.language].ToString();
            colBoard[2].text = readData.jsonWordingData["c"][readData.setting.language].ToString();
            colBoard[3].text = readData.jsonWordingData["d"][readData.setting.language].ToString();
            colBoard[4].text = readData.jsonWordingData["e"][readData.setting.language].ToString();
            colBoard[5].text = readData.jsonWordingData["f"][readData.setting.language].ToString();
            colBoard[6].text = readData.jsonWordingData["g"][readData.setting.language].ToString();
            colBoard[7].text = readData.jsonWordingData["h"][readData.setting.language].ToString();
            rowBoard[0].text = readData.jsonWordingData["1"][readData.setting.language].ToString();
            rowBoard[1].text = readData.jsonWordingData["2"][readData.setting.language].ToString();
            rowBoard[2].text = readData.jsonWordingData["3"][readData.setting.language].ToString();
            rowBoard[3].text = readData.jsonWordingData["4"][readData.setting.language].ToString();
            rowBoard[4].text = readData.jsonWordingData["5"][readData.setting.language].ToString();
            rowBoard[5].text = readData.jsonWordingData["6"][readData.setting.language].ToString();
            rowBoard[6].text = readData.jsonWordingData["7"][readData.setting.language].ToString();
            rowBoard[7].text = readData.jsonWordingData["8"][readData.setting.language].ToString();

            for (int i = 0; i < emoticon.Length; i++)
            {
                emoticon[i].sprite = Resources.Load<Sprite>("emoticon/" + readData.setting.language + "/default/0" + (i + 1));
            }

            displayImageBorder.sprite = Resources.Load<Sprite>("photo_frame/" + readData.setting.photoFrame);
            if (readData.userData.displayImage != null)
            {
                StartCoroutine(readData.downloadImg(readData.userData.displayImage, displayImage));
            }
            else
            {
                displayImage.sprite = Resources.Load<Sprite>("profile_picture/" + readData.setting.profilePic);
            }

            prefab = Resources.Load<GameObject>("games/chess_thai/PiecesPrefab");
            pieces = new List<Pieces>();
            borders = GameObject.FindGameObjectsWithTag("Border");
            historys = new List<History>();
            piecesIsChecked = new List<Pieces>();

            ordinal = 1;
            //eatedId = 0;
            useSec = 0;
            //lastMove = new Player();
            lastMove.ordinal = 1;
            isGameOver = true;

            amountSendOnTurn = 0;
            isCloseReceiveEmoticon = false;

            playerMin1 = 45;
            playerSec1 = 0;
            playerMin2 = 45;
            playerSec2 = 0;
            playerTurnSec = 90;
            playerDisconnectTime1 = 120;
            playerDisconnectTime2 = 120;
            countTurnAskForDraw = 0;

            if (readData.userData.ads && readData.userData.statuser[readData.setting.gameId - 1].ads == 1)
            {
                adverList = readData.userData.advertise.FindAll(x => x.position == 1);
                if (adverList.Count == 0)
                {
                    AndroidAdMobController.Instance.Init(MY_BANNERS_AD_UNIT_ID, MY_INTERSTISIALS_AD_UNIT_ID);
                    banner = AndroidAdMobController.Instance.CreateAdBanner(0, 0, GADBannerSize.SMART_BANNER);

                    AndroidAdMobController.Instance.OnInterstitialLoaded += OnInterstisialsLoaded;
                    AndroidAdMobController.Instance.OnInterstitialOpened += OnInterstisialsOpen;
                    AndroidAdMobController.Instance.OnInterstitialClosed += OnInterstisialsClosed;
                }
                else
                {
                    currentAdvertise = (adverList.Count - 1);
                    if (adverList.Count > 1)
                    {
                        InvokeRepeating("ChangeAdvertise", 0, 30.0f);
                    }
                    else
                    {
                        ChangeAdvertise();
                    }
                    advertise.GetComponent<CanvasGroup>().alpha = 1;
                    advertise.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }

            //GameStart(true);
        }

        public void ChangeAdvertise()
        {
            if (currentAdvertise == (adverList.Count - 1))
            {
                currentAdvertise = 0;
            }
            else
            {
                currentAdvertise++;
            }
            StartCoroutine(readData.downloadImg(adverList[currentAdvertise].picture, advertise));
        }

        public void ViewAdvertise()
        {
            Application.OpenURL(readData.userData.advertise[currentAdvertise].url);
        }

        private void OnInterstisialsLoaded()
        {
            //ad loaded, strting ad

        }

        private void OnInterstisialsOpen()
        {
            //pausing the game
        }

        private void OnInterstisialsClosed()
        {
            //un-pausing the game
        }

        void Update()
        {
            if (playerName2.text == "")
            {
                SetTwoPlayer();
            }

            if (emoticonDisplayCanvasGroup1.alpha == 1)
            {
                countDownEmoticon1 -= Time.deltaTime;
                if (countDownEmoticon1 <= 0.0f)
                {
                    emoticonDisplayCanvasGroup1.alpha = 0;
                }
            }
            if (emoticonDisplayCanvasGroup2.alpha == 1)
            {
                countDownEmoticon2 -= Time.deltaTime;
                if (countDownEmoticon2 <= 0.0f)
                {
                    emoticonDisplayCanvasGroup2.alpha = 0;
                }
            }
        }

        public void StartCountdownTime(bool isBlack)
        {
            if (isBlack)
            {
                InvokeRepeating("DecreaseTimeRemaining1", 0, 1.0f);
            }
            else
            {
                InvokeRepeating("DecreaseTimeRemaining2", 0, 1.0f);
            }
            //InvokeRepeating("SetRPCMove", 0, 1.0f);
        }

        public void ChangeCountdownTime()
        {
            if (countdown1)
            {
                CancelInvoke("DecreaseTimeRemaining1");
                InvokeRepeating("DecreaseTimeRemaining2", 0, 1.0f);
            }
            else
            {
                CancelInvoke("DecreaseTimeRemaining2");
                InvokeRepeating("DecreaseTimeRemaining1", 0, 1.0f);
            }
        }

        public void StopCountdownTime()
        {
            if (iAmBlack)
            {
                playerDisconnectIsBlack = isBlackTurn;
            }
            else
            {
                playerDisconnectIsBlack = !isBlackTurn;
            }

            CancelInvoke("DecreaseTimeRemaining1");
            CancelInvoke("DecreaseTimeRemaining2");
        }

        public void StartCountdownDisconnectTime(bool isBlack)
        {
            StopCountdownTime();
            if (isBlack)
            {
                InvokeRepeating("DecreseTimeDisconnectRemaining2", 0, 1.0f);
            }
            else
            {
                InvokeRepeating("DecreseTimeDisconnectRemaining1", 0, 1.0f);
            }
        }

        public void DecreseTimeDisconnectRemaining1()
        {
            playerDisconnectTime1--;
            opponentInternetProblemBlock.GetComponentsInChildren<Text>()[1].text = playerDisconnectTime1.ToString();
            if (playerDisconnectTime1 == 0)
            {
                SendResultTimeout(2);
            }
        }

        public void DecreseTimeDisconnectRemaining2()
        {
            playerDisconnectTime2--;
            opponentInternetProblemBlock.GetComponentsInChildren<Text>()[1].text = playerDisconnectTime2.ToString();
            if (playerDisconnectTime2 == 0)
            {
                SendResultTimeout(1);
            }
        }

        public void StartCountdownTimeReconnected()
        {
            StopCountdownDisconnectTime();
            StartCountdownTime(playerDisconnectIsBlack);
        }

        public void StopCountdownDisconnectTime()
        {
            CancelInvoke("DecreseTimeDisconnectRemaining1");
            CancelInvoke("DecreseTimeDisconnectRemaining2");
        }

        public void DecreaseTimeRemaining1()
        {
            countdown1 = true;
            //countdown2 = false;
            if (playerSec1 > 0)
            {
                playerSec1--;
            }
            else
            {
                if (playerMin1 == 0)
                {
                    SendResultTimeout(2);
                }
                playerSec1 = 59;
                playerMin1--;
            }
            playerTime1.text = playerMin1.ToString() + ":" + playerSec1.ToString("00");

            playerTurnSec--;
            playerTimeTurn.text = playerTurnSec.ToString();

            if (playerTurnSec == 0)
            {
                SendResultTimeout(2);
            }
        }

        public void DecreaseTimeRemaining2()
        {
            //countdown2 = true;
            countdown1 = false;
            if (playerSec2 > 0)
            {
                playerSec2--;
            }
            else
            {
                if (playerMin2 == 0)
                {
                    SendResultTimeout(1);
                }
                playerSec2 = 59;
                playerMin2--;
            }
            playerTime2.text = playerMin2.ToString() + ":" + playerSec2.ToString("00");

            playerTurnSec--;
            playerTimeTurn.text = playerTurnSec.ToString();

            if (playerTurnSec == 0)
            {
                SendResultTimeout(1);
            }
        }

        public IEnumerator SetOnePlayer(string name, int score, bool isBlack)
        {
            if (isBlack)
            {
                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, name, score);
                playerName1.text = GameObject.Find("PlayerBlack(Clone)").GetComponent<Player>().playerName;
            }
            else
            {
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, name, score);
                playerName1.text = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().playerName;
            }
            score1 = score;
            yield return null;
        }

        public void SetTwoPlayer()
        {
            if (iAmBlack && GameObject.Find("PlayerWhite(Clone)") != null)
            {
                playerName2.text = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().playerName;
                score2 = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().score;
            }
            else if (!iAmBlack && GameObject.Find("PlayerBlack(Clone)") != null)
            {
                playerName2.text = GameObject.Find("PlayerBlack(Clone)").GetComponent<Player>().playerName;
                score2 = GameObject.Find("PlayerBlack(Clone)").GetComponent<Player>().score;
            }
        }

        public IEnumerator SetPlayer(string name, int score, string oppenentName, int oppenentScore, bool isBlack)
        {
            if (isBlack)
            {
                /*GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, name, score);
                playerName2.text = GameObject.Find("PlayerBlack(Clone)").GetComponent<Player>().playerName;

                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, oppenentName, oppenentScore);
                playerName1.text = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().playerName;*/

                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, name, score);
                playerName1.text = GameObject.Find("PlayerBlack(Clone)").GetComponent<Player>().playerName;
                score1 = score;

                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, oppenentName, oppenentScore);
                playerName2.text = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().playerName;
                score2 = oppenentScore;
            }
            else
            {
                /*GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, name, score);
                playerName2.text = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().playerName;

                playerName1.text = oppenentName;*/

                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetPlayer", RPCMode.All, name, score);
                playerName1.text = GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().playerName;
                score1 = score;

                playerName2.text = oppenentName;
                score2 = oppenentScore;
            }
            yield return null;
        }

        public void GameStart(bool iAmBlack)
        {
            this.iAmBlack = iAmBlack;
            if (!iAmBlack)
            {
                //markingTurn.rectTransform.anchoredPosition = new Vector2(markingTurn.rectTransform.anchoredPosition.x, playerName1.rectTransform.anchoredPosition.y);
                SwapPositionBlackWhite();
                //displayNameBg1.sprite = Resources.Load<Sprite>("UI/img_player_active");
                //displayNameBg2.sprite = Resources.Load<Sprite>("UI/img_player_wait");
                highlightPlayerNameCanvasGroup1.alpha = 0;
                highlightPlayerNameCanvasGroup2.alpha = 1;
            }
            else
            {
                //displayNameBg1.sprite = Resources.Load<Sprite>("UI/img_player_active");
                //displayNameBg2.sprite = Resources.Load<Sprite>("UI/img_player_wait");
                highlightPlayerNameCanvasGroup1.alpha = 1;
                highlightPlayerNameCanvasGroup2.alpha = 0;
            }

            block = new List<List<GameObject>>();
            for (int i = 0; i < 8; i++)
            {
                blockTemp = new List<GameObject>();
                for (int j = 1; j <= 8; j++)
                {
                    borderName = "Border" + getColName(i) + j;
                    blockTemp.Add(GameObject.Find(borderName));
                }
                block.Add(blockTemp);
            }

            prepareMove = false;
            isBlackTurn = true;
            isChecked = false;
            isCountTurnPieces = false;
            isCountTurnBoard = false;
            turnCountPieces = 0;
            maxTurnCountPieces = 64;
            turnCountBoard = 0;
            //maxTurnCountBoard = 64;
            countTurnText.text = "";
            countTurn.text = "";
            foreach (GameObject border in borders)
            {
                border.GetComponent<Image>().sprite = img_turn_default;
            }
            pieces.Clear();
            entity.Clear();
            DestroyAllPieces();

            pieces.Add(SetDataPiece(1, true, 8, 1, 6));
            pieces.Add(SetDataPiece(2, true, 8, 2, 5));
            pieces.Add(SetDataPiece(3, true, 8, 3, 3));
            pieces.Add(SetDataPiece(4, true, 8, 4, 1));
            pieces.Add(SetDataPiece(5, true, 8, 5, 2));
            pieces.Add(SetDataPiece(6, true, 8, 6, 3));
            pieces.Add(SetDataPiece(7, true, 8, 7, 5));
            pieces.Add(SetDataPiece(8, true, 8, 8, 6));
            pieces.Add(SetDataPiece(9, true, 6, 1, 7));
            pieces.Add(SetDataPiece(10, true, 6, 2, 7));
            pieces.Add(SetDataPiece(11, true, 6, 3, 7));
            pieces.Add(SetDataPiece(12, true, 6, 4, 7));
            pieces.Add(SetDataPiece(13, true, 6, 5, 7));
            pieces.Add(SetDataPiece(14, true, 6, 6, 7));
            pieces.Add(SetDataPiece(15, true, 6, 7, 7));
            pieces.Add(SetDataPiece(16, true, 6, 8, 7));

            pieces.Add(SetDataPiece(17, false, 1, 1, 6));
            pieces.Add(SetDataPiece(18, false, 1, 2, 5));
            pieces.Add(SetDataPiece(19, false, 1, 3, 4));
            pieces.Add(SetDataPiece(20, false, 1, 4, 2));
            pieces.Add(SetDataPiece(21, false, 1, 5, 1));
            pieces.Add(SetDataPiece(22, false, 1, 6, 4));
            pieces.Add(SetDataPiece(23, false, 1, 7, 5));
            pieces.Add(SetDataPiece(24, false, 1, 8, 6));
            pieces.Add(SetDataPiece(25, false, 3, 1, 8));
            pieces.Add(SetDataPiece(26, false, 3, 2, 8));
            pieces.Add(SetDataPiece(27, false, 3, 3, 8));
            pieces.Add(SetDataPiece(28, false, 3, 4, 8));
            pieces.Add(SetDataPiece(29, false, 3, 5, 8));
            pieces.Add(SetDataPiece(30, false, 3, 6, 8));
            pieces.Add(SetDataPiece(31, false, 3, 7, 8));
            pieces.Add(SetDataPiece(32, false, 3, 8, 8));

            //StartCoroutine(GetHistoryMoveFromServer());
        }

        public void TabForMove(Pieces currentPieces)
        {
            if (iAmBlack == isBlackTurn && isBlackTurn == currentPieces.isBlack && !isGameOver)
            //if (isBlackTurn == currentPieces.isBlack && !isGameOver)
            {
                CancelMove();
                block[currentPieces.x - 1][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_active;
                switch (currentPieces.typeId)
                {
                    case 6:
                        //boat
                        for (int j = currentPieces.x; j < 8; j++)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (j + 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, j + 1, currentPieces.y) && !MoveIsCancelCheck(j + 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[j][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                            else if (piecesTemp.Count == 1)
                            {
                                break;
                            }
                            piecesTemp = pieces.FindAll(i => i.x == (j + 1) && i.y == currentPieces.y && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, j + 1, currentPieces.y) && !MoveIsCancelCheck(j + 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[j][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                                break;
                            }
                        }
                        for (int j = currentPieces.x; j > 1; j--)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (j - 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, j - 1, currentPieces.y) && !MoveIsCancelCheck(j - 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[j - 2][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                            else if (piecesTemp.Count == 1)
                            {
                                break;
                            }
                            piecesTemp = pieces.FindAll(i => i.x == (j - 1) && i.y == currentPieces.y && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, j - 1, currentPieces.y) && !MoveIsCancelCheck(j - 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[j - 2][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                                break;
                            }
                        }
                        for (int j = currentPieces.y; j > 1; j--)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x, j - 1) && !MoveIsCancelCheck(currentPieces.x, j - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 1][j - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                            else if (piecesTemp.Count == 1)
                            {
                                break;
                            }
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j - 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x, j - 1) && !MoveIsCancelCheck(currentPieces.x, j - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 1][j - 2].GetComponent<Image>().sprite = img_turn_now;
                                break;
                            }
                        }
                        for (int j = currentPieces.y; j < 8; j++)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x, j + 1) && !MoveIsCancelCheck(currentPieces.x, j + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 1][j].GetComponent<Image>().sprite = img_turn_now;
                            }
                            else if (piecesTemp.Count == 1)
                            {
                                break;
                            }
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j + 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x, j + 1) && !MoveIsCancelCheck(currentPieces.x, j + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 1][j].GetComponent<Image>().sprite = img_turn_now;
                                break;
                            }
                        }
                        break;
                    case 5:
                        //horse
                        if (currentPieces.x > 2 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 2) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 2, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 2, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 3][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 2)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 2) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 2))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 3].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 2 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 2) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 2, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 2, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 3][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y < 7)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 2) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 2))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y + 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 7 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 2) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 2, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 2, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x + 1][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 2)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 2) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 2))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 3].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 7 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 2) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 2, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 2, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x + 1][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 7)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 2) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 2))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y + 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        break;
                    case 3:
                        //black kone
                        if (currentPieces.x > 1) //move top
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y < 8) //move top right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1) // move top left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8) //move bottom right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1) //move bottom left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        break;
                    case 4:
                        //white kone
                        if (currentPieces.x < 8) //move bottom
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y < 8) //move top right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1) // move top left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8) //move bottom right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1) //move bottom left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        break;
                    case 2:
                        //med
                        if (currentPieces.x > 1 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        break;
                    case 1:
                        //kun
                        if (currentPieces.x < 8) //move bottom
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x + 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;

                            }
                        }
                        if (currentPieces.x > 1) //move top
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x - 1, currentPieces.y))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.y > 1) //move left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 1][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.y < 8) //move right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 1][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }

                        if (currentPieces.x > 1 && currentPieces.y < 8) //move top right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1) // move top left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x - 2][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8) //move bottom right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1) //move bottom left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                prepareMove = true;
                                piecesIdPrepareMove = currentPieces.id;
                                block[currentPieces.x][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                            }
                        }
                        break;
                    case 7:
                        //bia black
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                        {
                            prepareMove = true;
                            piecesIdPrepareMove = currentPieces.id;
                            block[currentPieces.x - 2][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                        {
                            prepareMove = true;
                            piecesIdPrepareMove = currentPieces.id;
                            block[currentPieces.x - 2][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y) && i.isAlive == true);
                        if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y))
                        {
                            prepareMove = true;
                            piecesIdPrepareMove = currentPieces.id;
                            block[currentPieces.x - 2][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                        }
                        break;
                    case 8:
                        //bia white
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                        {
                            prepareMove = true;
                            piecesIdPrepareMove = currentPieces.id;
                            block[currentPieces.x][currentPieces.y - 2].GetComponent<Image>().sprite = img_turn_now;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                        {
                            prepareMove = true;
                            piecesIdPrepareMove = currentPieces.id;
                            block[currentPieces.x][currentPieces.y].GetComponent<Image>().sprite = img_turn_now;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y) && i.isAlive == true);
                        if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y))
                        {
                            prepareMove = true;
                            piecesIdPrepareMove = currentPieces.id;
                            block[currentPieces.x][currentPieces.y - 1].GetComponent<Image>().sprite = img_turn_now;
                        }
                        break;
                }
            }
        }

        public void ClickBlock(string position)
        {
            if (!isGameOver)
            {
                string[] positions = position.Split(',');
                int positionX = Convert.ToInt32(positions[0]);
                int positionY = Convert.ToInt32(positions[1]);
                piecesTemp = pieces.FindAll(i => i.x == positionX && i.y == positionY && i.isAlive == true);

                if (prepareMove)
                {
                    if (block[positionX - 1][positionY - 1].GetComponent<Image>().sprite == img_turn_now)
                    {
                        CancelMove();
                        Move(piecesIdPrepareMove, positionX, positionY, true);
                    }
                    else
                    {
                        if (piecesTemp.Count > 0)
                        {
                            TabForMove(piecesTemp[0]);
                        }
                        else
                        {
                            CancelMove();
                        }
                    }
                }
                else
                {
                    if (piecesTemp.Count > 0)
                    {
                        TabForMove(piecesTemp[0]);
                    }
                    else
                    {
                        CancelMove();
                    }
                }
            }
        }

        public void SetRPCBlackMove()
        {
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetId", RPCMode.All, lastMovePieceId);
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetX", RPCMode.All, lastMoveX);
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetY", RPCMode.All, lastMoveY);
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetIsBlackTurn", RPCMode.All, true);
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetOrdinal", RPCMode.All, ordinal);
        }

        public void SetRPCWhiteMove()
        {
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetId", RPCMode.All, lastMovePieceId);
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetX", RPCMode.All, lastMoveX);
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetY", RPCMode.All, lastMoveY);
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetIsBlackTurn", RPCMode.All, false);
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetOrdinal", RPCMode.All, ordinal);
        }

        public void Move(int piecesId, int x, int y, bool saveToServer)
        {
            piecesTemp2 = pieces.Find(k => k.id == piecesId);
            if (piecesTemp2.x == x && piecesTemp2.y == y)
            {
                return;
            }

            piecesTemp = pieces.FindAll(i => i.x == x && i.y == y && i.isAlive == true);
            foreach (GameObject border in borders)
            {
                border.GetComponent<Image>().sprite = img_turn_default;
            }

            History history = new History();
            history.id = piecesId;
            history.x = x;
            history.y = y;
            history.oldX = pieces[piecesId - 1].x;
            history.oldY = pieces[piecesId - 1].y;
            history.typeId = pieces[piecesId - 1].typeId;
            history.eatedId = 0;

            block[pieces[piecesId - 1].x - 1][pieces[piecesId - 1].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
            pieces[piecesId - 1].x = x;
            pieces[piecesId - 1].y = y;
            block[pieces[piecesId - 1].x - 1][pieces[piecesId - 1].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
            entity[piecesId - 1].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, (block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.y + 41));
            entity[piecesId - 1].transform.SetParent(GameObject.Find("Row" + x + "Canvas").GetComponent<Transform>(), true);

            if ((pieces[piecesId - 1].typeId == 7 && x == 3) || (pieces[piecesId - 1].typeId == 8 && x == 6))
            {
                pieces[piecesId - 1].typeId = 2;
                if (pieces[piecesId - 1].isBlack)
                {
                    entity[piecesId - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_pawn_promote_th");
                }
                else
                {
                    entity[piecesId - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_pawn_promote_th");
                }
            }
            ordinal++;
            if (saveToServer)
            {
                lastMovePieceId = piecesId;
                lastMoveX = x;
                lastMoveY = y;
                if (isBlackTurn)
                {
                    CancelInvoke("SetRPCWhiteMove");
                    InvokeRepeating("SetRPCBlackMove", 0, 1.0f);
                }
                else
                {
                    CancelInvoke("SetRPCBlackMove");
                    InvokeRepeating("SetRPCWhiteMove", 0, 1.0f);
                }
            }

            isBlackTurn = !isBlackTurn;
            if (highlightPlayerNameCanvasGroup1.alpha == 0)
            {
                highlightPlayerNameCanvasGroup1.alpha = 1;
                highlightPlayerNameCanvasGroup2.alpha = 0;
            }
            else
            {
                highlightPlayerNameCanvasGroup1.alpha = 0;
                highlightPlayerNameCanvasGroup2.alpha = 1;
            }
            /*if (iAmBlack == isBlackTurn)
            {
                displayNameBg1.sprite = Resources.Load<Sprite>("UI/img_player_active");
                displayNameBg2.sprite = Resources.Load<Sprite>("UI/img_player_wait");
            }
            else
            {
                displayNameBg1.sprite = Resources.Load<Sprite>("UI/img_player_wait");
                displayNameBg2.sprite = Resources.Load<Sprite>("UI/img_player_active");
            }*/

            if (piecesTemp.Count > 0)
            {
                if (piecesTemp[0].isAlive)
                {
                    piecesTemp[0].isAlive = false;
                    entity[piecesTemp[0].id - 1].GetComponent<CanvasGroup>().alpha = 0;
                    history.eatedId = piecesTemp[0].id;
                    if (piecesTemp[0].id == 4)
                    {
                        SendResultCheckmate();
                    }
                    else if (piecesTemp[0].id == 21)
                    {
                        SendResultCheckmate();
                    }
                }
            }
            historys.Add(history);

            CheckCountTurnPieces();
            if (isCountTurnPieces && isBlackTurn != isBlackCountTurnPieces)
            {
                turnCountPieces--;
                countTurn.text = turnCountPieces.ToString();
                if (turnCountPieces <= 0)
                {
                    SendResultDraw();
                }
            }
            CheckCountTurnBoard();
            if (!isCountTurnPieces && isCountTurnBoard && isBlackTurn != isBlackCountTurnBoard)
            {
                turnCountBoard--;
                countTurn.text = turnCountBoard.ToString();
                if (turnCountBoard <= 0)
                {
                    SendResultDraw();
                }
            }

            piecesIsChecked.Clear();
            isChecked = MoveToCheck(piecesId);
            if (isChecked)
            {
                Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);
                block[myKun.x - 1][myKun.y - 1].GetComponent<Image>().sprite = img_turn_check;
            }
            else
            {
                List<Pieces> myKun = pieces.FindAll(k => k.typeId == 1);
                block[myKun[0].x - 1][myKun[0].y - 1].GetComponent<Image>().sprite = img_turn_default;
                block[myKun[1].x - 1][myKun[1].y - 1].GetComponent<Image>().sprite = img_turn_default;
            }
            if (!CheckCanMove())
            {
                if (isChecked)
                {
                    SendResultCheckmate();
                }
                else
                {
                    SendResultDraw();
                }
            }

            playerTurnSec = 90;
            playerTimeTurn.text = playerTurnSec.ToString();
            if (countTurnAskForDraw > 0)
            {
                countTurnAskForDraw--;
            }

            ChangeCountdownTime();
            if (saveToServer) // temporary close
            {
                //StartCoroutine(SendRequestMove(history));
            }
            amountSendOnTurn = 0;

            readData.PlayAudioMove();
        }

        public IEnumerator SendRequestMove(History history)
        {
            string param = "move" + "|" + readData.signatureKey + "|" + readData.match.matchId + "|" + piecesIdPrepareMove + "|" + history.x + "|" + history.y + "|" + history.eatedId + "|" + history.oldX + "|" + history.oldY + "|" + ordinal + "|" + useSec;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                if ((string)www.text == "success")
                {
                    ordinal++;
                }
            }
        }

        public IEnumerator SendResult(int result)
        {
            isGameOver = true;
            StopCountdownTime();
            StopCountdownDisconnectTime();
            internetProblemBlock.GetComponent<CanvasGroup>().alpha = 0;
            internetProblemBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
            opponentInternetProblemBlock.GetComponent<CanvasGroup>().alpha = 0;
            opponentInternetProblemBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;

            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;

            JsonData jsonHistory = JsonMapper.ToJson(historys);
            //Debug.Log(jsonHistory);

            string param = "matchresult" + "|" + readData.signatureKey + "|" + readData.match.matchId + "|" + result + "|" + jsonHistory + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                int scoreDiff;
                if ((string)www.text == "fail2")
                {
                    Debug.Log(www.text);
                }
                else if (result == 3)
                {
                    resultImage.sprite = Resources.Load<Sprite>("UI/" + readData.setting.language + "/img_result_text_draw");
                    currentScore.text = readData.jsonWordingData["score"][readData.setting.language].ToString() + " : " + (readData.userData.statuser[readData.setting.gameId - 1].score).ToString();
                    resultScore.text = "";
                    dailyMission.text = readData.jsonWordingData["dailyMission"][readData.setting.language].ToString() + " (" + www.text + "/2)";
                    currentCoin.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString();
                    resultCoin.text = "";
                }
                else if (Convert.ToInt32(www.text.ToString()) == 2)
                {
                    if (isWon)
                    {
                        resultImage.sprite = Resources.Load<Sprite>("UI/" + readData.setting.language + "/img_result_text_win");
                        scoreDiff = (int)readData.calculate.getScoreWinAndLose(score1, score2, true);
                        currentScore.text = readData.jsonWordingData["score"][readData.setting.language].ToString() + " : " + (readData.userData.statuser[readData.setting.gameId - 1].score + scoreDiff).ToString();
                        resultScore.text = "(+" + scoreDiff.ToString() + ")";
                        dailyMission.text = readData.jsonWordingData["dailyMission"][readData.setting.language].ToString() + " (" + www.text + "/2)";
                        currentCoin.text = (readData.userData.statuser[readData.setting.gameId - 1].money+60).ToString();
                        resultCoin.text = " (+60)";

                        readData.userData.statuser[readData.setting.gameId - 1].score += scoreDiff;
                        readData.userData.statuser[readData.setting.gameId - 1].money += 60;
                        readData.fileData.SaveUserData(readData.userData);
                    }
                    else
                    {
                        resultImage.sprite = Resources.Load<Sprite>("UI/" + readData.setting.language + "/img_result_text_lose");
                        scoreDiff = (int)readData.calculate.getScoreWinAndLose(score1, score2, false);
                        currentScore.text = readData.jsonWordingData["score"][readData.setting.language].ToString() + " : " + (readData.userData.statuser[readData.setting.gameId - 1].score - scoreDiff).ToString();
                        resultScore.text = "(-" + scoreDiff.ToString() + ")";
                        dailyMission.text = readData.jsonWordingData["dailyMission"][readData.setting.language].ToString() + " (" + www.text + "/2)";
                        currentCoin.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString();
                        resultCoin.text = "";

                        readData.userData.statuser[readData.setting.gameId - 1].score -= scoreDiff;
                        readData.fileData.SaveUserData(readData.userData);
                    }
                }
                else if (Convert.ToInt32(www.text.ToString()) != 2)
                {
                    if (isWon)
                    {
                        resultImage.sprite = Resources.Load<Sprite>("UI/" + readData.setting.language + "/img_result_text_win");
                        scoreDiff = (int)readData.calculate.getScoreWinAndLose(score1, score2, true);
                        currentScore.text = readData.jsonWordingData["score"][readData.setting.language].ToString() + " : " + (readData.userData.statuser[readData.setting.gameId - 1].score + scoreDiff).ToString();
                        resultScore.text = "(+" + scoreDiff.ToString() + ")";
                        dailyMission.text = readData.jsonWordingData["dailyMission"][readData.setting.language].ToString() + " (" + www.text + "/2)";
                        currentCoin.text = (readData.userData.statuser[readData.setting.gameId - 1].money+10).ToString();
                        resultCoin.text = " (+10)";

                        readData.userData.statuser[readData.setting.gameId - 1].score += scoreDiff;
                        readData.userData.statuser[readData.setting.gameId - 1].money += 10;
                        readData.fileData.SaveUserData(readData.userData);
                    }
                    else
                    {
                        resultImage.sprite = Resources.Load<Sprite>("UI/" + readData.setting.language + "/img_result_text_lose");
                        scoreDiff = (int)readData.calculate.getScoreWinAndLose(score1, score2, false);
                        currentScore.text = readData.jsonWordingData["score"][readData.setting.language].ToString() + " : " + (readData.userData.statuser[readData.setting.gameId - 1].score - scoreDiff).ToString();
                        resultScore.text = "(-" + scoreDiff.ToString() + ")";
                        dailyMission.text = readData.jsonWordingData["dailyMission"][readData.setting.language].ToString() + " (" + www.text + "/2)";
                        currentCoin.text = readData.userData.statuser[readData.setting.gameId - 1].money.ToString();
                        resultCoin.text = "";

                        readData.userData.statuser[readData.setting.gameId - 1].score -= scoreDiff;
                        readData.fileData.SaveUserData(readData.userData);
                    }
                }
            }
            resultBlock.GetComponent<CanvasGroup>().alpha = 1;
            resultBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
            GameObject.Find("NetworkManagement").GetComponent<NetworkManagement>().QuitNetwork();
        }

        public void SendResultTimeout(int side)
        {
            int result;
            if (iAmBlack)
            {
                if (side == 1)
                {
                    result = 2;
                    //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["win"][readData.setting.language].ToString();
                    isWon = true;
                }
                else
                {
                    result = 1;
                    //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["lose"][readData.setting.language].ToString();
                    isWon = false;
                }
            }
            else
            {
                if (side == 1)
                {
                    result = 1;
                    //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["win"][readData.setting.language].ToString();
                    isWon = true;
                }
                else
                {
                    result = 2;
                    //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["lose"][readData.setting.language].ToString();
                    isWon = false;
                }
            }

            StartCoroutine(SendResult(result));
        }

        public void SendResultDraw()
        {
            //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["draw"][readData.setting.language].ToString();
            StartCoroutine(SendResult(3));
        }

        public void SendResultCheckmate()
        {
            int result;
            if (iAmBlack)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }
            if (isBlackTurn == iAmBlack)
            {
                //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["lose"][readData.setting.language].ToString();
                isWon = false;
            }
            else
            {
                //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["win"][readData.setting.language].ToString();
                isWon = true;
            }

            StartCoroutine(SendResult(result));
        }

        public void SendResultConcede()
        {
            CancelConcedeButton();
            int result;
            if (iAmBlack)
            {
                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetSideConcede", RPCMode.All, 1);
                result = 1;
            }
            else
            {
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideConcede", RPCMode.All, 2);
                result = 2;
            }
            //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["lose"][readData.setting.language].ToString();
            isWon = false;

            StartCoroutine(SendResult(result));
        }

        public void SendResultEnemyConcede()
        {
            int result;
            if (iAmBlack)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }
            //resultBlock.GetComponentsInChildren<Text>()[0].text = readData.jsonWordingData["win"][readData.setting.language].ToString();
            isWon = true;

            StartCoroutine(SendResult(result));
        }

        public IEnumerator GetHistoryMoveFromServer() // unuse
        {
            string param = "gethistory" + "|" + readData.signatureKey + "|" + readData.match.matchId;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            HistoryFromServer historyFromServer;

            if (www.error == null)
            {
                historyFromServer = JsonMapper.ToObject<HistoryFromServer>(www.text);
                for (int i = 0; i < historyFromServer.historyFromServer.Count; i++)
                {
                    //ordinal++;
                    //Move(historyFromServer.historyFromServer[i].id, historyFromServer.historyFromServer[i].x, historyFromServer.historyFromServer[i].y, false);
                }
            }
        }

        public void Undo()
        {
            if (historys.Count > 0 && iAmBlack != isBlackTurn)
            {
                foreach (GameObject border in borders)
                {
                    border.GetComponent<Image>().sprite = img_turn_default;
                }
                if (historys.Count > 1)
                {
                    block[historys[historys.Count - 2].oldX - 1][historys[historys.Count - 2].oldY - 1].GetComponent<Image>().sprite = img_turn_lastest;
                    block[historys[historys.Count - 2].x - 1][historys[historys.Count - 2].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
                }
                pieces[historys[historys.Count - 1].id - 1].x = historys[historys.Count - 1].oldX;
                pieces[historys[historys.Count - 1].id - 1].y = historys[historys.Count - 1].oldY;
                entity[historys[historys.Count - 1].id - 1].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(block[historys[historys.Count - 1].oldX - 1][historys[historys.Count - 1].oldY - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, (block[historys[historys.Count - 1].oldX - 1][historys[historys.Count - 1].oldY - 1].GetComponent<Image>().rectTransform.anchoredPosition.y + 41));
                entity[historys[historys.Count - 1].id - 1].transform.SetParent(GameObject.Find("Row" + historys[historys.Count - 1].oldX + "Canvas").GetComponent<Transform>(), true);

                if ((historys[historys.Count - 1].typeId == 7 && historys[historys.Count - 1].oldX == 4) || (historys[historys.Count - 1].typeId == 8 && historys[historys.Count - 1].oldX == 5))
                {
                    pieces[historys[historys.Count - 1].id - 1].typeId = historys[historys.Count - 1].typeId;
                    if (pieces[historys[historys.Count - 1].id - 1].isBlack)
                    {
                        entity[historys[historys.Count - 1].id - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_pawn_th");
                    }
                    else
                    {
                        entity[historys[historys.Count - 1].id - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_pawn_th");
                    }
                }

                if (historys[historys.Count - 1].eatedId != 0)
                {
                    pieces[historys[historys.Count - 1].eatedId - 1].isAlive = true;
                    entity[historys[historys.Count - 1].eatedId - 1].GetComponent<CanvasGroup>().alpha = 1;
                }
                isBlackTurn = !isBlackTurn;
                if (historys.Count > 1)
                {
                    isChecked = MoveToCheck(historys[historys.Count - 2].id);
                    if (isChecked)
                    {
                        Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);
                        block[myKun.x - 1][myKun.y - 1].GetComponent<Image>().sprite = img_turn_check;
                    }
                    else
                    {
                        List<Pieces> myKun = pieces.FindAll(k => k.typeId == 1);
                        block[myKun[0].x - 1][myKun[0].y - 1].GetComponent<Image>().sprite = img_turn_default;
                        block[myKun[1].x - 1][myKun[1].y - 1].GetComponent<Image>().sprite = img_turn_default;
                    }
                }
                historys.RemoveAt(historys.Count - 1);
                if (isGameOver)
                {
                    isGameOver = false;
                }
                if (iAmBlack == isBlackTurn)
                {
                    //markingTurn.rectTransform.anchoredPosition = new Vector2(markingTurn.rectTransform.anchoredPosition.x, playerName2.rectTransform.anchoredPosition.y);
                }
                else
                {
                    //markingTurn.rectTransform.anchoredPosition = new Vector2(markingTurn.rectTransform.anchoredPosition.x, playerName1.rectTransform.anchoredPosition.y);
                }
                if (isCountTurnPieces)
                {
                    CheckCountTurnPiecesWhenUndo();
                }
                if (isCountTurnBoard)
                {
                    CheckCountTurnBoardWhenUndo();
                }
                CancelMove();
            }
        }

        public bool MoveIsCancelCheck(int x, int y)
        {
            if (isChecked)
            {
                bool[] cancelCheck = new bool[piecesIsChecked.Count];
                Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);

                for (int i = 0; i < piecesIsChecked.Count; i++)
                {
                    cancelCheck[i] = false;
                    if (x == piecesIsChecked[i].x && y == piecesIsChecked[i].y)
                    {
                        cancelCheck[i] = true;
                    }
                    else if (piecesIsChecked[i].typeId == 6)
                    {
                        if (piecesIsChecked[i].y == myKun.y)
                        {
                            if (piecesIsChecked[i].x < myKun.x)
                            {
                                if (x < myKun.x && x > piecesIsChecked[i].x && y == myKun.y)
                                {
                                    cancelCheck[i] = true;
                                }
                            }
                            else if (piecesIsChecked[i].x > myKun.x)
                            {
                                if (x > myKun.x && x < piecesIsChecked[i].x && y == myKun.y)
                                {
                                    cancelCheck[i] = true;
                                }
                            }
                        }
                        if (piecesIsChecked[i].x == myKun.x)
                        {
                            if (piecesIsChecked[i].y > myKun.y)
                            {
                                if (y > myKun.y && y < piecesIsChecked[i].y && x == myKun.x)
                                {
                                    cancelCheck[i] = true;
                                }
                            }
                            else if (piecesIsChecked[i].y < myKun.y)
                            {
                                if (y < myKun.y && y > piecesIsChecked[i].y && x == myKun.x)
                                {
                                    cancelCheck[i] = true;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < piecesIsChecked.Count; i++)
                {
                    if (!cancelCheck[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MoveOutOffLineBoatCheck(Pieces myKun)
        {
            List<Pieces> enemyBoats = pieces.FindAll(k => k.typeId == 6 && k.isBlack != isBlackTurn && k.isAlive == true);
            foreach (Pieces enemyBoat in enemyBoats)
            {
                if (enemyBoat.y == myKun.y)
                {
                    if (enemyBoat.x < myKun.x)
                    {
                        piecesTemp = pieces.FindAll(i => i.x > enemyBoat.x && i.x < myKun.x && i.y == enemyBoat.y && i.isAlive == true);
                        if (piecesTemp.Count == 0)
                        {
                            piecesIsChecked.Add(enemyBoat);
                            return true;
                        }
                    }
                    else if (enemyBoat.x > myKun.x)
                    {
                        piecesTemp = pieces.FindAll(i => i.x < enemyBoat.x && i.x > myKun.x && i.y == enemyBoat.y && i.isAlive == true);
                        if (piecesTemp.Count == 0)
                        {
                            piecesIsChecked.Add(enemyBoat);
                            return true;
                        }
                    }
                }
                if (enemyBoat.x == myKun.x)
                {
                    if (enemyBoat.y > myKun.y)
                    {
                        piecesTemp = pieces.FindAll(i => i.x == enemyBoat.x && i.y < enemyBoat.y && i.y > myKun.y && i.isAlive == true);
                        if (piecesTemp.Count == 0)
                        {
                            piecesIsChecked.Add(enemyBoat);
                            return true;
                        }
                    }
                    else if (enemyBoat.y < myKun.y)
                    {
                        piecesTemp = pieces.FindAll(i => i.x == enemyBoat.x && i.y > enemyBoat.y && i.y < myKun.y && i.isAlive == true);
                        if (piecesTemp.Count == 0)
                        {
                            piecesIsChecked.Add(enemyBoat);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool MoveToCheck(int id)
        {
            Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);
            bool checkedTemp = false;
            if (MoveOutOffLineBoatCheck(myKun))
            {
                checkedTemp = true;
            }
            Pieces currentPieces = pieces.Find(k => k.id == id);
            switch (currentPieces.typeId)
            {
                case 6:
                    if (currentPieces.y == myKun.y)
                    {
                        if (currentPieces.x < myKun.x)
                        {
                            piecesTemp = pieces.FindAll(i => i.x > currentPieces.x && i.x < myKun.x && i.y == currentPieces.y && i.isAlive == true);
                            if (piecesTemp.Count == 0)
                            {
                                piecesIsChecked.Add(currentPieces);
                                return true;
                            }
                        }
                        else if (currentPieces.x > myKun.x)
                        {
                            piecesTemp = pieces.FindAll(i => i.x < currentPieces.x && i.x > myKun.x && i.y == currentPieces.y && i.isAlive == true);
                            if (piecesTemp.Count == 0)
                            {
                                piecesIsChecked.Add(currentPieces);
                                return true;
                            }
                        }

                    }
                    if (currentPieces.x == myKun.x)
                    {
                        if (currentPieces.y < myKun.y)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y > currentPieces.y && i.y < myKun.y && i.isAlive == true);
                            if (piecesTemp.Count == 0)
                            {
                                piecesIsChecked.Add(currentPieces);
                                return true;
                            }
                        }
                        else if (currentPieces.y > myKun.y)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y < currentPieces.y && i.y > myKun.y && i.isAlive == true);
                            if (piecesTemp.Count == 0)
                            {
                                piecesIsChecked.Add(currentPieces);
                                return true;
                            }
                        }
                    }
                    break;
                case 5:
                    if (myKun.x == currentPieces.x - 2 && myKun.y == currentPieces.y - 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y - 2)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x - 2 && myKun.y == currentPieces.y + 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y + 2)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 2 && myKun.y == currentPieces.y - 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y - 2)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 2 && myKun.y == currentPieces.y + 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y + 2)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    break;
                case 2:
                case 3:
                case 4:
                    if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y + 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y - 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y + 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y - 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (currentPieces.typeId == 3)
                    {
                        if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y)
                        {
                            piecesIsChecked.Add(currentPieces);
                            return true;
                        }
                    }
                    if (currentPieces.typeId == 4)
                    {
                        if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y)
                        {
                            piecesIsChecked.Add(currentPieces);
                            return true;
                        }
                    }
                    break;
                case 7:
                    if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y + 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x - 1 && myKun.y == currentPieces.y - 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    break;
                case 8:
                    if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y + 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    if (myKun.x == currentPieces.x + 1 && myKun.y == currentPieces.y - 1)
                    {
                        piecesIsChecked.Add(currentPieces);
                        return true;
                    }
                    break;
            }
            if (checkedTemp)
            {
                return true;
            }
            return false;
        }

        public bool MoveCheckBoat(Pieces currentPieces, int x, int y)
        {
            List<Pieces> enemyBoats = pieces.FindAll(k => k.typeId == 6 && k.isBlack != isBlackTurn && k.isAlive == true);
            Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);
            foreach (Pieces enemyBoat in enemyBoats)
            {
                if (enemyBoat.y == myKun.y)
                {
                    piecesTemp = pieces.FindAll(i => i.x > enemyBoat.x && i.x < myKun.x && i.y == enemyBoat.y && i.isAlive == true);
                    if (piecesTemp.Count == 1 && currentPieces.id == piecesTemp[0].id && y != myKun.y)
                    {
                        return true;
                    }
                    piecesTemp = pieces.FindAll(i => i.x < enemyBoat.x && i.x > myKun.x && i.y == enemyBoat.y && i.isAlive == true);
                    if (piecesTemp.Count == 1 && currentPieces.id == piecesTemp[0].id && y != myKun.y)
                    {
                        return true;
                    }
                }
                if (enemyBoat.x == myKun.x)
                {
                    piecesTemp = pieces.FindAll(i => i.x == enemyBoat.x && i.y < enemyBoat.y && i.y > myKun.y && i.isAlive == true);
                    if (piecesTemp.Count == 1 && currentPieces.id == piecesTemp[0].id && x != myKun.x)
                    {
                        return true;
                    }
                    piecesTemp = pieces.FindAll(i => i.x == enemyBoat.x && i.y > enemyBoat.y && i.y < myKun.y && i.isAlive == true);
                    if (piecesTemp.Count == 1 && currentPieces.id == piecesTemp[0].id && x != myKun.x)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool KunMoveCheck(int x, int y)
        {
            List<Pieces> enemyPieces = pieces.FindAll(k => k.isBlack != isBlackTurn && k.isAlive == true);
            Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);
            foreach (Pieces currentPieces in enemyPieces)
            {
                switch (currentPieces.typeId)
                {
                    case 6:
                        for (int j = currentPieces.x; j < 8; j++)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (j + 1) && i.y == currentPieces.y && i.isAlive == true && i.id != myKun.id);
                            if (piecesTemp.Count != 0)
                            {
                                if (x == (j + 1) && y == currentPieces.y)
                                {
                                    return true;
                                }
                                break;
                            }
                            if (x == (j + 1) && y == currentPieces.y)
                            {
                                return true;
                            }
                        }
                        for (int j = currentPieces.x; j > 1; j--)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (j - 1) && i.y == currentPieces.y && i.isAlive == true && i.id != myKun.id);
                            if (piecesTemp.Count != 0)
                            {
                                if (x == (j - 1) && y == currentPieces.y)
                                {
                                    return true;
                                }
                                break;
                            }
                            if (x == (j - 1) && y == currentPieces.y)
                            {
                                return true;
                            }
                        }
                        for (int j = currentPieces.y; j > 1; j--)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j - 1) && i.isAlive == true && i.id != myKun.id);
                            if (piecesTemp.Count != 0)
                            {
                                if (x == currentPieces.x && y == (j - 1))
                                {
                                    return true;
                                }
                                break;
                            }
                            if (x == currentPieces.x && y == (j - 1))
                            {
                                return true;
                            }
                        }
                        for (int j = currentPieces.y; j < 8; j++)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j + 1) && i.isAlive == true && i.id != myKun.id);
                            if (piecesTemp.Count != 0)
                            {
                                if (x == currentPieces.x && y == (j + 1))
                                {
                                    return true;
                                }
                                break;
                            }
                            if (x == currentPieces.x && y == (j + 1))
                            {
                                return true;
                            }
                        }
                        break;
                    case 5:
                        if (x == currentPieces.x - 2 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 1 && y == currentPieces.y - 2)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 2 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 1 && y == currentPieces.y + 2)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 2 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y - 2)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 2 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y + 2)
                        {
                            return true;
                        }
                        break;
                    case 1:
                        if (x == currentPieces.x - 1 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 1 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 1 && y == currentPieces.y)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y)
                        {
                            return true;
                        }
                        if (x == currentPieces.x && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        break;
                    case 2:
                    case 3:
                    case 4:
                        if (x == currentPieces.x - 1 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 1 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        if (currentPieces.typeId == 3)
                        {
                            if (x == currentPieces.x - 1 && y == currentPieces.y)
                            {
                                return true;
                            }
                        }
                        if (currentPieces.typeId == 4)
                        {
                            if (x == currentPieces.x + 1 && y == currentPieces.y)
                            {
                                return true;
                            }
                        }
                        break;
                    case 7:
                        if (x == currentPieces.x - 1 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x - 1 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        break;
                    case 8:
                        if (x == currentPieces.x + 1 && y == currentPieces.y + 1)
                        {
                            return true;
                        }
                        if (x == currentPieces.x + 1 && y == currentPieces.y - 1)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        public void SwapPositionBlackWhite()
        {

            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    borderI = 7;
                }
                else if (i == 1)
                {
                    borderI = 6;
                }
                else if (i == 2)
                {
                    borderI = 5;
                }
                else if (i == 3)
                {
                    borderI = 4;
                }
                for (int j = 1; j <= 8; j++)
                {
                    if (j == 1)
                    {
                        borderJ = 8;
                    }
                    else if (j == 2)
                    {
                        borderJ = 7;
                    }
                    else if (j == 3)
                    {
                        borderJ = 6;
                    }
                    else if (j == 4)
                    {
                        borderJ = 5;
                    }
                    else if (j == 5)
                    {
                        borderJ = 4;
                    }
                    else if (j == 6)
                    {
                        borderJ = 3;
                    }
                    else if (j == 7)
                    {
                        borderJ = 2;
                    }
                    else if (j == 8)
                    {
                        borderJ = 1;
                    }
                    borderName = "Border" + getColName(i) + j;
                    borderName2 = "Border" + getColName(borderI) + borderJ;

                    Vector3 vectorTemp = GameObject.Find(borderName).GetComponent<Image>().rectTransform.anchoredPosition;
                    GameObject.Find(borderName).GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(GameObject.Find(borderName2).GetComponent<Image>().rectTransform.anchoredPosition.x, GameObject.Find(borderName2).GetComponent<Image>().rectTransform.anchoredPosition.y);
                    GameObject.Find(borderName2).GetComponent<Image>().rectTransform.anchoredPosition = vectorTemp;
                }
            }
        }

        public void CheckCountTurnBoard()
        {
            if (!isCountTurnPieces && !isCountTurnBoard)
            {
                List<Pieces> biaPieces = pieces.FindAll(k => k.isAlive == true && (k.typeId == 7 || k.typeId == 8));
                if (biaPieces.Count == 0 && iAmBlack == isBlackTurn)
                {
                    countTurnBoardButton.interactable = true;
                }
                else
                {
                    countTurnBoardButton.interactable = false;
                }
            }
        }

        public void StartCountTurnBoard()
        {
            if (!isCountTurnBoard)
            {
                if (iAmBlack)
                {
                    GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetSideCountBoard", RPCMode.All, 1);
                }
                else
                {
                    GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideCountBoard", RPCMode.All, 2);
                }
                CountTurnBoard();
            }
            else
            {
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideCountBoard", RPCMode.All, 0);
                CancelCountTurnBoard();
            }
        }

        public void CountTurnBoard()
        {
            //List<Pieces> biaPieces = pieces.FindAll(k => k.isAlive == true && (k.typeId == 7 || k.typeId == 8));
            //if (biaPieces.Count == 0)
            //{
            isCountTurnBoard = true;
            turnCountBoard = 64;
            isBlackCountTurnBoard = isBlackTurn;
            countTurnText.text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
            countTurn.text = turnCountBoard.ToString();
            countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["cancelCountBoard"][readData.setting.language].ToString();
            //}

            CancelCountBoardButton();
        }

        public void CancelCountTurnBoard()
        {
            isCountTurnBoard = false;
            turnCountBoard = 64;
            countTurnText.text = "";
            countTurn.text = "";
            countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
            CancelCountBoardButton();
        }

        public void CheckCountTurnBoardWhenUndo()
        {
            if (!isCountTurnPieces)
            {
                List<Pieces> biaPieces = pieces.FindAll(k => k.isAlive == true && (k.typeId == 7 || k.typeId == 8));
                if (biaPieces.Count == 0)
                {
                    if (isBlackTurn == isBlackCountTurnBoard && countTurnText.text != "")
                    {
                        turnCountBoard--;
                    }
                    countTurnText.text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
                    countTurn.text = turnCountBoard.ToString();
                    countTurnBoardButton.interactable = true;
                }
                else
                {
                    isCountTurnBoard = false;
                    turnCountBoard = 0;
                    countTurnText.text = "";
                    countTurn.text = "";
                    countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
                    countTurnBoardButton.interactable = false;
                }
            }
        }

        public void CheckCountTurnPiecesWhenUndo()
        {
            List<Pieces> myPieces = pieces.FindAll(k => k.isBlack == isBlackCountTurnPieces && k.isAlive == true && (k.typeId == 6 || k.typeId == 5 || k.typeId == 4 || k.typeId == 3 || k.typeId == 2 || k.typeId == 7 || k.typeId == 8));
            List<Pieces> enemyPieces = pieces.FindAll(k => k.isBlack != isBlackCountTurnPieces && k.isAlive == true && (k.typeId == 7 || k.typeId == 8));

            if (myPieces.Count == 0 && enemyPieces.Count == 0) // นับศักดิ์หมาก
            {
                if (isBlackTurn == isBlackCountTurnPieces)
                {
                    turnCountPieces--;
                }
                countTurnText.text = readData.jsonWordingData["countPieces"][readData.setting.language].ToString();
                countTurn.text = turnCountPieces.ToString();
            }
            else
            {
                isCountTurnPieces = false;
                turnCountPieces = 0;
                countTurnText.text = "";
                countTurn.text = "";
            }
        }

        public void CheckCountTurnPieces()
        {
            if (!isCountTurnPieces)
            {
                List<Pieces> alivePieces;
                List<Pieces> myPieces = pieces.FindAll(k => k.isBlack == isBlackTurn && k.isAlive == true && (k.typeId == 6 || k.typeId == 5 || k.typeId == 4 || k.typeId == 3 || k.typeId == 2 || k.typeId == 7 || k.typeId == 8));
                List<Pieces> enemyPieces = pieces.FindAll(k => k.isBlack != isBlackTurn && k.isAlive == true && (k.typeId == 7 || k.typeId == 8));

                if (myPieces.Count == 0 && enemyPieces.Count == 0) // นับศักดิ์หมาก
                {
                    isCountTurnPieces = true;
                    isBlackCountTurnPieces = isBlackTurn;
                    countTurnBoardButton.interactable = false;

                    enemyPieces = pieces.FindAll(k => k.isBlack != isBlackTurn && k.isAlive == true && k.typeId == 6);
                    if (enemyPieces.Count == 2)
                    {
                        maxTurnCountPieces = 8;
                    }
                    else if (enemyPieces.Count == 1)
                    {
                        maxTurnCountPieces = 16;
                    }
                    else if (enemyPieces.Count == 0)
                    {
                        enemyPieces = pieces.FindAll(k => k.isBlack != isBlackTurn && k.isAlive == true && (k.typeId == 3 || k.typeId == 4));
                        if (enemyPieces.Count == 2)
                        {
                            maxTurnCountPieces = 22;
                        }
                        else if (enemyPieces.Count == 1)
                        {
                            enemyPieces = pieces.FindAll(k => k.isBlack != isBlackTurn && k.isAlive == true && k.typeId == 5);
                            if (enemyPieces.Count == 2)
                            {
                                maxTurnCountPieces = 32;
                            }
                            else
                            {
                                maxTurnCountPieces = 44;
                            }
                        }
                        else if (enemyPieces.Count == 0)
                        {
                            enemyPieces = pieces.FindAll(k => k.isBlack != isBlackTurn && k.isAlive == true && k.typeId == 5);
                            if (enemyPieces.Count == 2)
                            {
                                maxTurnCountPieces = 32;
                            }
                            else
                            {
                                maxTurnCountPieces = 64;
                            }
                        }
                    }
                    alivePieces = pieces.FindAll(k => k.isAlive == true);
                    turnCountPieces = maxTurnCountPieces - alivePieces.Count;
                    countTurnText.text = readData.jsonWordingData["countPieces"][readData.setting.language].ToString();
                    countTurn.text = turnCountPieces.ToString();
                }
                /*else
                {
                    alivePieces = pieces.FindAll(k => k.isAlive == true && k.typeId == 7 && k.typeId == 8);
                    if (alivePieces.Count == 0) //count turn board
                    {

                    }
                }*/
            }
        }

        public bool CheckCanMove()
        {
            List<Pieces> alivePieces = pieces.FindAll(k => k.isBlack == isBlackTurn && k.isAlive == true);
            foreach (Pieces currentPieces in alivePieces)
            {
                switch (currentPieces.typeId)
                {
                    case 6:
                        //boat
                        for (int j = currentPieces.x; j < 8; j++)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (j + 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, j + 1, currentPieces.y) && !MoveIsCancelCheck(j + 1, currentPieces.y))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int j = currentPieces.x; j > 1; j--)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (j - 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, j - 1, currentPieces.y) && !MoveIsCancelCheck(j - 1, currentPieces.y))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int j = currentPieces.y; j > 1; j--)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x, j - 1) && !MoveIsCancelCheck(currentPieces.x, j - 1))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int j = currentPieces.y; j < 8; j++)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x, j + 1) && !MoveIsCancelCheck(currentPieces.x, j + 1))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                    case 5:
                        //horse
                        if (currentPieces.x > 2 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 2) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 2, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 2, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 2)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 2) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 2))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 2 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 2) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 2, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 2, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y < 7)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 2) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 2))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 7 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 2) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 2, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 2, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 2)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 2) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 2))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 7 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 2) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 2, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 2, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 7)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 2) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 2) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 2))
                            {
                                return true;
                            }
                        }
                        break;
                    case 3:
                        //black kone
                        if (currentPieces.x > 1) //move top
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y < 8) //move top right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1) // move top left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8) //move bottom right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1) //move bottom left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        break;
                    case 4:
                        //white kone
                        if (currentPieces.x < 8) //move bottom
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y < 8) //move top right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1) // move top left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8) //move bottom right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1) //move bottom left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        break;
                    case 2:
                        //med
                        if (currentPieces.x > 1 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1)
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        break;
                    case 1:
                        //kun
                        if (currentPieces.x < 8) //move bottom
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x + 1, currentPieces.y))
                            {
                                return true;

                            }
                        }
                        if (currentPieces.x > 1) //move top
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == currentPieces.y && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x - 1, currentPieces.y))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.y > 1) //move left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.y < 8) //move right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }

                        if (currentPieces.x > 1 && currentPieces.y < 8) //move top right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x - 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x > 1 && currentPieces.y > 1) // move top left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x - 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y < 8) //move bottom right
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x + 1, currentPieces.y + 1))
                            {
                                return true;
                            }
                        }
                        if (currentPieces.x < 8 && currentPieces.y > 1) //move bottom left
                        {
                            piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack == currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 0 && !KunMoveCheck(currentPieces.x + 1, currentPieces.y - 1))
                            {
                                return true;
                            }
                        }
                        break;
                    case 7:
                        //bia black
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y - 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y - 1))
                        {
                            return true;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y + 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y + 1))
                        {
                            return true;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x - 1) && i.y == (currentPieces.y) && i.isAlive == true);
                        if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x - 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x - 1, currentPieces.y))
                        {
                            return true;
                        }
                        break;
                    case 8:
                        //bia white
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y - 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y - 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y - 1))
                        {
                            return true;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y + 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                        if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y + 1) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y + 1))
                        {
                            return true;
                        }
                        piecesTemp = pieces.FindAll(i => i.x == (currentPieces.x + 1) && i.y == (currentPieces.y) && i.isAlive == true);
                        if (piecesTemp.Count == 0 && !MoveCheckBoat(currentPieces, currentPieces.x + 1, currentPieces.y) && !MoveIsCancelCheck(currentPieces.x + 1, currentPieces.y))
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        public void QuitButton()
        {
            //GameObject.Find("NetworkManagement").GetComponent<NetworkManagement>().QuitNetwork();
            if (readData.userData.ads && readData.userData.statuser[readData.setting.gameId - 1].ads == 1 && readData.userData.advertise[0].position == 0)
            {
                AndroidAdMobController.Instance.DestroyBanner(banner.id);
            }
            SceneManager.LoadScene("selectMode");
        }

        public void PlayInterstitialAd()
        {
            if (readData.userData.ads && readData.userData.statuser[readData.setting.gameId - 1].ads == 1)
            {
                AndroidAdMobController.Instance.StartInterstitialAd();
                AndroidAdMobController.Instance.ShowInterstitialAd();
            }
        }

        private void DestroyBanner()
        {
            //un-pausing the game
        }

        public void ConcedeButton()
        {
            if (!isGameOver)
            {
                CloseOptionBlock();
                bgFadeCanvasGroup.alpha = 1;
                bgFadeCanvasGroup.blocksRaycasts = true;
                concedeBlock.GetComponent<CanvasGroup>().alpha = 1;
                concedeBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        public void SubmitConcedeButton()
        {
            CancelConcedeButton();
            SendResultConcede();
        }

        public void CancelConcedeButton()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            concedeBlock.GetComponent<CanvasGroup>().alpha = 0;
            concedeBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void AskForDrawButton()
        {
            CancelAskDrawButton();
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            countTurnAskForDraw = 20;
            textAskForDrawBlock.GetComponent<CanvasGroup>().alpha = 1;
            textAskForDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
            askDrawBlock.GetComponent<CanvasGroup>().alpha = 0;
            askDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (iAmBlack)
            {
                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetSideAskForDraw", RPCMode.All, 1);
            }
            else
            {
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideAskForDraw", RPCMode.All, 2);
            }

        }

        public void CloseCannotAskForDrawBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            cannotAskForDrawBlock.GetComponent<CanvasGroup>().alpha = 0;
            cannotAskForDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void CloseTextAskForDraw()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            textAskForDrawBlock.GetComponent<CanvasGroup>().alpha = 0;
            textAskForDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OpenDrawBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            drawBlock.GetComponent<CanvasGroup>().alpha = 1;
            drawBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OpenAskDrawBlock()
        {
            if (!isGameOver)
            {
                CloseOptionBlock();
                bgFadeCanvasGroup.alpha = 1;
                bgFadeCanvasGroup.blocksRaycasts = true;
                if (countTurnAskForDraw == 0)
                {
                    askDrawBlock.GetComponent<CanvasGroup>().alpha = 1;
                    askDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    cannotAskForDrawBlock.GetComponent<CanvasGroup>().alpha = 1;
                    cannotAskForDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }

            }
        }

        public void CancelAskDrawButton()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            askDrawBlock.GetComponent<CanvasGroup>().alpha = 0;
            askDrawBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void SubmitDrawButton()
        {
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetSideAskForDraw", RPCMode.All, 0);
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideAskForDraw", RPCMode.All, 0);
            drawBlock.GetComponent<CanvasGroup>().alpha = 0;
            drawBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (iAmBlack)
            {
                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetIsDraw", RPCMode.All, true);
            }
            else
            {
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetIsDraw", RPCMode.All, true);
            }
            SendResultDraw();
        }

        public void CancelDrawButton()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            drawBlock.GetComponent<CanvasGroup>().alpha = 0;
            drawBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
            GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetSideAskForDraw", RPCMode.All, 0);
            GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideAskForDraw", RPCMode.All, 0);
        }

        public void OpenCountBoardBlock()
        {
            CloseOptionBlock();
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            countBoardBlock.GetComponent<CanvasGroup>().alpha = 1;
            countBoardBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void CancelCountBoardButton()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            countBoardBlock.GetComponent<CanvasGroup>().alpha = 0;
            countBoardBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OpenInternetProblemBlock()
        {
            bgFade2CanvasGroup.alpha = 1;
            bgFade2CanvasGroup.blocksRaycasts = true;
            internetProblemBlock.GetComponent<CanvasGroup>().alpha = 1;
            internetProblemBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void CloseInternetProblemBlock()
        {
            bgFade2CanvasGroup.alpha = 0;
            bgFade2CanvasGroup.blocksRaycasts = false;
            internetProblemBlock.GetComponent<CanvasGroup>().alpha = 0;
            internetProblemBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OpenOptionBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            optionBlock.GetComponent<CanvasGroup>().alpha = 1;
            optionBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void CloseOptionBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            optionBlock.GetComponent<CanvasGroup>().alpha = 0;
            optionBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void CloseReceivedEmoticonButton()
        {
            if (isCloseReceiveEmoticon)
            {
                isCloseReceiveEmoticon = false;
                closeReceiveEmoticonButtonText.text = readData.jsonWordingData["closeReceiveEmoticon"][readData.setting.language].ToString();
            }
            else
            {
                isCloseReceiveEmoticon = true;
                closeReceiveEmoticonButtonText.text = readData.jsonWordingData["openReceiveEmoticon"][readData.setting.language].ToString();
            }
            CloseOptionBlock();
        }

        public void OpenEmoticonBlock()
        {
            if (amountSendOnTurn < 3)
            {
                emoticonBlockCanvasGroup.alpha = 1;
                emoticonBlockCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                // play sound tood tood
            }

        }

        public void CloseEmoticonBlock()
        {
            emoticonBlockCanvasGroup.alpha = 0;
            emoticonBlockCanvasGroup.blocksRaycasts = false;
        }

        public void EmoticonPointerDown(int position)
        {
            emoticonBorder.rectTransform.anchoredPosition = new Vector3(emoticon[position - 1].rectTransform.anchoredPosition.x, emoticon[position - 1].rectTransform.anchoredPosition.y);
            emoticonBorder.GetComponent<CanvasGroup>().alpha = 1;
        }

        public void EmoticonPointerUp(int position)
        {
            emoticonBorder.GetComponent<CanvasGroup>().alpha = 0;
            amountSendOnTurn++;
            emoticonDisplay1.sprite = Resources.Load<Sprite>("emoticon/" + readData.setting.language + "/default/0" + position);
            emoticonDisplayCanvasGroup1.alpha = 1;
            countDownEmoticon1 = 3;
            if (iAmBlack)
            {
                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetSideSendEmoticon", RPCMode.All, 1);
                GameObject.Find("PlayerBlack(Clone)").GetComponent<NetworkView>().RPC("SetEmoticonIdFromOpponent", RPCMode.All, position);
            }
            else
            {
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetSideSendEmoticon", RPCMode.All, 2);
                GameObject.Find("PlayerWhite(Clone)").GetComponent<NetworkView>().RPC("SetEmoticonIdFromOpponent", RPCMode.All, position);
            }

            CloseEmoticonBlock();
        }

        public void OpponentSendEmoticon(int position)
        {
            if (!isCloseReceiveEmoticon)
            {
                emoticonDisplay2.sprite = Resources.Load<Sprite>("emoticon/" + readData.setting.language + "/default/0" + position);
                emoticonDisplayCanvasGroup2.alpha = 1;
                countDownEmoticon2 = 3;
            }
        }

        public void OpenOpponentInternetProblemBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            if (internetProblemBlock.GetComponent<CanvasGroup>().alpha == 0)
            {
                opponentInternetProblemBlock.GetComponent<CanvasGroup>().alpha = 1;
                opponentInternetProblemBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        public void CloseOpponentInternetProblemBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            opponentInternetProblemBlock.GetComponent<CanvasGroup>().alpha = 0;
            opponentInternetProblemBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void CloseLoading()
        {
            loadingCanvasGroup.alpha = 0;
            loadingCanvasGroup.blocksRaycasts = false;
        }

        public void CancelMove()
        {
            foreach (GameObject border in borders)
            {
                if (border.GetComponent<Image>().sprite != img_turn_lastest && border.GetComponent<Image>().sprite != img_turn_check)
                {
                    border.GetComponent<Image>().sprite = img_turn_default;
                }
            }
            prepareMove = false;
        }

        public string getColName(int i)
        {
            switch (i)
            {
                case 0:
                    return "A";
                case 1:
                    return "B";
                case 2:
                    return "C";
                case 3:
                    return "D";
                case 4:
                    return "E";
                case 5:
                    return "F";
                case 6:
                    return "G";
                case 7:
                    return "H";
                default:
                    return "A";
            }
        }

        public Pieces SetDataPiece(int id, bool isBlack, int x, int y, int typeId)
        {
            //Pieces pieces = new Pieces();
            Pieces pieces = gameObject.AddComponent<Pieces>();
            pieces.id = id;
            pieces.isBlack = isBlack;
            pieces.x = x;
            pieces.y = y;
            pieces.isAlive = true;
            pieces.typeId = typeId;
            entity.Add(Instantiate(prefab, new Vector3(1, 1, 1), Quaternion.identity) as GameObject);
            entity[id - 1].transform.SetParent(GameObject.Find("Row" + x + "Canvas").GetComponent<Transform>(), true);
            entity[id - 1].transform.localScale = new Vector3(1, 1, 1);
            entity[id - 1].transform.position = GameObject.Find("Row" + x + "Canvas").GetComponent<Transform>().position;
            entity[id - 1].GetComponent<Image>().sprite = GetImagePieces(pieces.isBlack, pieces.typeId);
            entity[id - 1].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, (block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.y + 41));
            entity[id - 1].GetComponent<Button>().onClick.AddListener(() => pieces.TabForMove());
            entity[id - 1].GetComponent<CanvasGroup>().alpha = 1;
            return pieces;
        }

        public Sprite GetImagePieces(bool isBlack, int typeId)
        {
            switch (typeId)
            {
                case 6:
                    if (isBlack)
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_rook_th");
                    else
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_rook_th");
                case 5:
                    if (isBlack)
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_knight_th");
                    else
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_knight_th");
                case 3:
                case 4:
                    if (isBlack)
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_queen_th");
                    else
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_queen_th");
                case 2:
                    if (isBlack)
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_bishop_th");
                    else
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_bishop_th");
                case 1:
                    if (isBlack)
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_king_th");
                    else
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_king_th");
                case 7:
                    return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_pawn_th");
                case 8:
                    return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_pawn_th");
                default:
                    if (isBlack)
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_pawn_th");
                    else
                        return Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_pawn_th");
            }
        }

        public void DestroyAllPieces()
        {
            for (int i = 1; i <= 8; i++)
            {
                foreach (Transform child in GameObject.Find("Row" + i + "Canvas").transform)
                {
                    if (child.name == "PiecesPrefab(Clone)")
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }
    }
}
