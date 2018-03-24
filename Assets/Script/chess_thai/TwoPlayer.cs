using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Assets.Script.Component;
using UnityEngine.SceneManagement;

namespace Assets.Script.chess_thai
{
    public class TwoPlayer : MonoBehaviour
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
        int countTemp;
        List<Pieces> piecesTemp;
        bool prepareMove;
        int piecesIdPrepareMove;
        bool isBlackTurn;
        GameObject[] borders;
        public Image turnMarkingBlack;
        public Image turnMarkingWhite;
        List<History> historys;
        bool isGameOver;
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
        public Image resultBlock;
        public Text[] colBoard;
        public Text[] rowBoard;
        public CanvasGroup bgFadeCanvasGroup;
        public CanvasGroup optionBlock;
        public Text resultText;
        public Text countTurnText;
        public Text countTurn;
        public Text newGameButtonText;
        public Text undoButtonText;
        public Text quitButtonText;

        GoogleMobileAdBanner banner;
        string MY_BANNERS_AD_UNIT_ID = "ca-app-pub-9781457328779143/6673315110";
        string MY_INTERSTISIALS_AD_UNIT_ID = "ca-app-pub-9781457328779143/8150048315";

        public Image advertise;
        int currentAdvertise;
        List<Advertise> adverList;

        void Start()
        {
            img_turn_active = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_active");
            img_turn_default = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_default");
            img_turn_now = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_now");
            img_turn_lastest = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_latest");
            img_turn_check = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_check");
            img_board.sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/bg_play_game_chess_thai");

            resultBlock.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();
            newGameButtonText.text = readData.jsonWordingData["newGame"][readData.setting.language].ToString();
            undoButtonText.text = readData.jsonWordingData["undo"][readData.setting.language].ToString();
            quitButtonText.text = readData.jsonWordingData["quit"][readData.setting.language].ToString();
            countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
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

            prefab = Resources.Load<GameObject>("games/chess_thai/PiecesPrefab");
            pieces = new List<Pieces>();
            borders = GameObject.FindGameObjectsWithTag("Border");
            historys = new List<History>();
            piecesIsChecked = new List<Pieces>();

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

            GameStart();
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

        public void GameStart()
        {
            isGameOver = false;
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
                //border.GetComponentsInChildren<Image>()[1].color = new Color32(133, 96, 49, 255);
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

            turnMarkingBlack.GetComponent<CanvasRenderer>().SetAlpha(1);
            turnMarkingWhite.GetComponent<CanvasRenderer>().SetAlpha(0);
        }

        public void TabForMove(Pieces currentPieces)
        {
            if (isBlackTurn == currentPieces.isBlack && !isGameOver)
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
            string[] positions = position.Split(',');
            int positionX = Convert.ToInt32(positions[0]);
            int positionY = Convert.ToInt32(positions[1]);
            piecesTemp = pieces.FindAll(i => i.x == positionX && i.y == positionY && i.isAlive == true);

            if (prepareMove)
            {
                if (block[positionX - 1][positionY - 1].GetComponent<Image>().sprite == img_turn_now)
                {
                    CancelMove();
                    Move(piecesTemp, positionX, positionY);
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

        public void Move(List<Pieces> piecesTemp, int x, int y)
        {
            foreach (GameObject border in borders)
            {
                //border.GetComponentsInChildren<Image>()[1].color = new Color32(133, 96, 49, 255);
                border.GetComponent<Image>().sprite = img_turn_default;
            }

            History history = new History();
            history.id = piecesIdPrepareMove;
            history.x = x;
            history.y = y;
            history.oldX = pieces[piecesIdPrepareMove - 1].x;
            history.oldY = pieces[piecesIdPrepareMove - 1].y;
            history.typeId = pieces[piecesIdPrepareMove - 1].typeId;
            history.eatedId = 0;

            block[pieces[piecesIdPrepareMove - 1].x - 1][pieces[piecesIdPrepareMove - 1].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
            pieces[piecesIdPrepareMove - 1].x = x;
            pieces[piecesIdPrepareMove - 1].y = y;
            block[pieces[piecesIdPrepareMove - 1].x - 1][pieces[piecesIdPrepareMove - 1].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
            entity[piecesIdPrepareMove - 1].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, (block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.y + 41));
            entity[piecesIdPrepareMove - 1].transform.SetParent(GameObject.Find("Row" + x + "Canvas").GetComponent<Transform>(), true);

            if ((pieces[piecesIdPrepareMove - 1].typeId == 7 && x == 3) || (pieces[piecesIdPrepareMove - 1].typeId == 8 && x == 6))
            {
                pieces[piecesIdPrepareMove - 1].typeId = 2;
                if (pieces[piecesIdPrepareMove - 1].isBlack)
                {
                    entity[piecesIdPrepareMove - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_pawn_promote_th");
                }
                else
                {
                    entity[piecesIdPrepareMove - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_pawn_promote_th");
                }
            }

            isBlackTurn = !isBlackTurn;
            if (isBlackTurn)
            {
                turnMarkingBlack.GetComponent<CanvasRenderer>().SetAlpha(1);
                turnMarkingWhite.GetComponent<CanvasRenderer>().SetAlpha(0);
            }
            else
            {
                turnMarkingBlack.GetComponent<CanvasRenderer>().SetAlpha(0);
                turnMarkingWhite.GetComponent<CanvasRenderer>().SetAlpha(1);
            }

            if (piecesTemp.Count > 0)
            {
                if (piecesTemp[0].isAlive)
                {
                    piecesTemp[0].isAlive = false;
                    entity[piecesTemp[0].id - 1].GetComponent<CanvasGroup>().alpha = 0;
                    history.eatedId = piecesTemp[0].id;
                    if (piecesTemp[0].id == 4)
                    {
                        resultText.text = readData.jsonWordingData["whiteWon"][readData.setting.language].ToString();
                        OpenResultBlock();
                        isGameOver = true;
                    }
                    else if (piecesTemp[0].id == 21)
                    {
                        resultText.text = readData.jsonWordingData["blackWon"][readData.setting.language].ToString();
                        OpenResultBlock();
                        isGameOver = true;
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
                    isGameOver = true;
                    resultText.text = readData.jsonWordingData["draw"][readData.setting.language].ToString();
                    OpenResultBlock();
                }
            }
            CheckCountTurnBoard();
            if (!isCountTurnPieces && isCountTurnBoard && isBlackTurn != isBlackCountTurnBoard)
            {
                turnCountBoard--;
                countTurn.text = turnCountBoard.ToString();
                if (turnCountBoard <= 0)
                {
                    isGameOver = true;
                    resultText.text = readData.jsonWordingData["draw"][readData.setting.language].ToString();
                    OpenResultBlock();
                }
            }

            piecesIsChecked.Clear();
            isChecked = MoveToCheck(piecesIdPrepareMove);
            if (isChecked)
            {
                Pieces myKun = pieces.Find(k => k.typeId == 1 && k.isBlack == isBlackTurn);
                //entity[myKun.id - 1].GetComponent<Image>().color = Color.red;
                block[myKun.x - 1][myKun.y - 1].GetComponent<Image>().sprite = img_turn_check;
            }
            else
            {
                List<Pieces> myKun = pieces.FindAll(k => k.typeId == 1);
                //entity[myKun[0].id - 1].GetComponent<Image>().color = Color.white;
                //entity[myKun[1].id - 1].GetComponent<Image>().color = Color.white;
                block[myKun[0].x - 1][myKun[0].y - 1].GetComponent<Image>().sprite = img_turn_default;
                block[myKun[1].x - 1][myKun[1].y - 1].GetComponent<Image>().sprite = img_turn_default;
            }
            if (!CheckCanMove())
            {
                isGameOver = true;
                if (isChecked)
                {
                    if (isBlackTurn)
                    {
                        resultText.text = readData.jsonWordingData["whiteWon"][readData.setting.language].ToString();
                    }
                    else
                    {
                        resultText.text = readData.jsonWordingData["blackWon"][readData.setting.language].ToString();
                    }
                }
                else
                {
                    resultText.text = readData.jsonWordingData["draw"][readData.setting.language].ToString();
                }
                OpenResultBlock();
            }
            readData.PlayAudioMove();
        }

        public void Undo()
        {
            if (historys.Count > 0)
            {
                foreach (GameObject border in borders)
                {
                    //border.GetComponentsInChildren<Image>()[1].color = new Color32(133, 96, 49, 255);
                    //border.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
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
                        //entity[myKun.id - 1].GetComponent<Image>().color = Color.red;
                        block[myKun.x - 1][myKun.y - 1].GetComponent<Image>().sprite = img_turn_check;
                    }
                    else
                    {
                        List<Pieces> myKun = pieces.FindAll(k => k.typeId == 1);
                        //entity[myKun[0].id - 1].GetComponent<Image>().color = Color.white;
                        //entity[myKun[1].id - 1].GetComponent<Image>().color = Color.white;
                        block[myKun[0].x - 1][myKun[0].y - 1].GetComponent<Image>().sprite = img_turn_default;
                        block[myKun[1].x - 1][myKun[1].y - 1].GetComponent<Image>().sprite = img_turn_default;
                    }
                }
                historys.RemoveAt(historys.Count - 1);
                if (isGameOver)
                {
                    isGameOver = false;
                }
                if (isBlackTurn)
                {
                    turnMarkingBlack.GetComponent<CanvasRenderer>().SetAlpha(1);
                    turnMarkingWhite.GetComponent<CanvasRenderer>().SetAlpha(0);
                }
                else
                {
                    turnMarkingBlack.GetComponent<CanvasRenderer>().SetAlpha(0);
                    turnMarkingWhite.GetComponent<CanvasRenderer>().SetAlpha(1);
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
            CloseOptionBlock();
            readData.PlayAudioMove();
        }

        public void CheckCountTurnBoard()
        {
            if (!isCountTurnPieces && !isCountTurnBoard)
            {
                List<Pieces> biaPieces = pieces.FindAll(k => k.isAlive == true && (k.typeId == 7 || k.typeId == 8));
                if (biaPieces.Count == 0)
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
                isCountTurnBoard = true;
                turnCountBoard = 64;
                isBlackCountTurnBoard = isBlackTurn;
                countTurnText.text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
                countTurn.text = turnCountBoard.ToString();
                countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["cancelCountBoard"][readData.setting.language].ToString();
            }
            else
            {
                isCountTurnBoard = false;
                turnCountBoard = 64;
                countTurnText.text = "";
                countTurn.text = "";
                countTurnBoardButton.GetComponentInChildren<Text>().text = readData.jsonWordingData["countBoard"][readData.setting.language].ToString();
            }
            CloseOptionBlock();
        }

        public void CheckCountTurnBoardWhenUndo()
        {
            if (!isCountTurnPieces)
            {
                List<Pieces> biaPieces = pieces.FindAll(k => k.isAlive == true && (k.typeId == 7 || k.typeId == 8));
                if (biaPieces.Count == 0)
                {
                    if (isBlackTurn == isBlackCountTurnBoard && isCountTurnBoard)
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
                            /*piecesTemp = pieces.FindAll(i => i.x == (j + 1) && i.y == currentPieces.y && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, j + 1, currentPieces.y) && !MoveIsCancelCheck(j + 1, currentPieces.y))
                            {
                                return true;
                            }*/
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
                            /*piecesTemp = pieces.FindAll(i => i.x == (j - 1) && i.y == currentPieces.y && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, j - 1, currentPieces.y) && !MoveIsCancelCheck(j - 1, currentPieces.y))
                            {
                                return true;
                            }*/
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
                            /*piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j - 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x, j - 1) && !MoveIsCancelCheck(currentPieces.x, j - 1))
                            {
                                return true;
                            }*/
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
                            /*piecesTemp = pieces.FindAll(i => i.x == currentPieces.x && i.y == (j + 1) && i.isBlack != currentPieces.isBlack && i.isAlive == true);
                            if (piecesTemp.Count == 1 && !MoveCheckBoat(currentPieces, currentPieces.x, j + 1) && !MoveIsCancelCheck(currentPieces.x, j + 1))
                            {
                                return true;
                            }*/
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

        public void NewGame()
        {
            GameStart();
            CloseOptionBlock();
        }

        public void PlayInterstitialAd()
        {
            if (readData.userData.ads && readData.userData.statuser[readData.setting.gameId - 1].ads == 1)
            {
                AndroidAdMobController.Instance.StartInterstitialAd();
                AndroidAdMobController.Instance.ShowInterstitialAd();
            }
        }

        public void Quit()
        {
            if (readData.userData.ads && readData.userData.statuser[readData.setting.gameId - 1].ads == 1 && readData.userData.advertise[0].position == 0)
            {
                AndroidAdMobController.Instance.DestroyBanner(banner.id);
            }
            SceneManager.LoadScene("selectMode");
        }

        private void DestroyBanner()
        {
            //un-pausing the game
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

        public void OpenResultBlock()
        {
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            resultBlock.GetComponent<CanvasGroup>().alpha = 1;
            resultBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;

            //PlayInterstitialAd();
        }

        public void CloseResultBlock()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            resultBlock.GetComponent<CanvasGroup>().alpha = 0;
            resultBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
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

        void Update()
        {

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
