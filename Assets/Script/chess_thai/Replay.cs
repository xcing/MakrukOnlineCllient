using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Assets.Script.Component;
using LitJson;
using UnityEngine.SceneManagement;

namespace Assets.Script.chess_thai
{
    public class Replay : MonoBehaviour
    {
        public ReadData readData;
        public CanvasGroup loadingCanvasGroup;
        int ordinal;
        public Image resultBlock;
        public Text header;
        public Text ordinalText;
        public Text ordinalValue;
        public Text dateText;
        public Text dateValue;
        public Image resultIcon;
        public Text opponentName;
        public Text opponentRankValue;
        public Text opponentScoreValue;
        public Text[] controlButtonText;
        public Image controlAutoImage;
        public bool isAuto;
        public Text autoText;
        public Color yellowColor;
        public Color grayColor;

        List<Pieces> pieces;
        GameObject prefab;
        public List<GameObject> entity;
        public List<List<GameObject>> block;
        List<GameObject> blockTemp;
        string borderName;
        string borderName2;
        List<Pieces> piecesTemp;
        bool isBlackTurn;
        GameObject[] borders;
        public bool iAmBlack;
        int borderI;
        int borderJ;
        HistoryMoveMatch datas;
        public Image loadingImage;
        public CanvasGroup bgFadeCanvasGroup;

        Sprite img_turn_default;
        Sprite img_turn_lastest;
        public Image img_board;
        Sprite resultImage;

        void Start()
        {
            InvokeRepeating("SpinWaitingImage", 0, 0.05f);
            img_turn_default = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_default");
            img_turn_lastest = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/img_game_turn_latest");
            img_board.sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/bg_play_game_chess_thai");

            header.text = readData.jsonWordingData["playingRecord"][readData.setting.language].ToString();
            ordinalText.text = readData.jsonWordingData["ordinal"][readData.setting.language].ToString()+":";
            dateText.text = readData.jsonWordingData["date"][readData.setting.language].ToString() + ":";
            controlButtonText[0].text = readData.jsonWordingData["first"][readData.setting.language].ToString();
            controlButtonText[1].text = readData.jsonWordingData["backward"][readData.setting.language].ToString();
            controlButtonText[2].text = readData.jsonWordingData["auto"][readData.setting.language].ToString();
            controlButtonText[3].text = readData.jsonWordingData["forward"][readData.setting.language].ToString();
            controlButtonText[4].text = readData.jsonWordingData["last"][readData.setting.language].ToString();
            resultBlock.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = readData.jsonWordingData["submit"][readData.setting.language].ToString();

            prefab = Resources.Load<GameObject>("games/chess_thai/PiecesPrefab");
            pieces = new List<Pieces>();
            borders = GameObject.FindGameObjectsWithTag("Border");

            StartCoroutine(GetHistoryFromServer());
        }

        public void SetData()
        {
            if (!datas.iAmBlack)
            {
                //markingTurn.rectTransform.anchoredPosition = new Vector2(markingTurn.rectTransform.anchoredPosition.x, playerName1.rectTransform.anchoredPosition.y);
                SwapPositionBlackWhite();
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

            ordinalValue.text = readData.userData.currentMatchOrdinal.ToString();
            dateValue.text = datas.createdDate;
            opponentName.text = datas.opponentName;
            if (datas.result == 1)
            {
                resultImage = Resources.Load<Sprite>("UI/ico_history_status_win");
            }
            else if (datas.result == 2)
            {
                resultImage = Resources.Load<Sprite>("UI/ico_history_status_lost");
            }
            else if (datas.result == 3)
            {
                resultImage = Resources.Load<Sprite>("UI/ico_history_status_draw");
            }
            resultIcon.sprite = resultImage;
            opponentRankValue.text = readData.calculate.GetRankFromScore(datas.opponentScore).ToString();
            opponentScoreValue.text = datas.opponentScore.ToString();

            SetPieces();
            CloseLoading();
        }

        public IEnumerator GetHistoryFromServer()
        {
            string param = "getMoveHistoryMatch" + "|" + readData.signatureKey + "|" + readData.userData.currentMatchId + "|" + readData.userData.statuser[readData.setting.gameId - 1].statuserId;
            //string param = "getListHistoryMatch" + "|" + readData.signatureKey + "|11|"+page;

            string encryptData = readData.encryption.EncryptRJ256(param);
            WWWForm form = new WWWForm();
            form.AddField("data", encryptData);
            WWW www = new WWW(readData.url, form);
            yield return www;

            if (www.error == null)
            {
                datas = JsonMapper.ToObject<HistoryMoveMatch>(www.text);
                SetData();
            }
        }

        public void SetPieces()
        {
            ordinal = 0;
            isBlackTurn = true;

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
        }

        public void Move(int piecesId, int x, int y)
        {
            piecesTemp = pieces.FindAll(i => i.x == x && i.y == y && i.isAlive == true);
            foreach (GameObject border in borders)
            {
                border.GetComponent<Image>().sprite = img_turn_default;
            }

            block[pieces[piecesId - 1].x - 1][pieces[piecesId - 1].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
            pieces[piecesId - 1].x = x;
            pieces[piecesId - 1].y = y;
            block[pieces[piecesId - 1].x - 1][pieces[piecesId - 1].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
            entity[piecesId - 1].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, (block[x - 1][y - 1].GetComponent<Image>().rectTransform.anchoredPosition.y + 41));

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

            isBlackTurn = !isBlackTurn;
            /*if (iAmBlack == isBlackTurn)
            {
                markingTurn.rectTransform.anchoredPosition = new Vector2(markingTurn.rectTransform.anchoredPosition.x, playerName2.rectTransform.anchoredPosition.y);
            }
            else
            {
                markingTurn.rectTransform.anchoredPosition = new Vector2(markingTurn.rectTransform.anchoredPosition.x, playerName1.rectTransform.anchoredPosition.y);
            }*/

            if (piecesTemp.Count > 0)
            {
                if (piecesTemp[0].isAlive)
                {
                    piecesTemp[0].isAlive = false;
                    entity[piecesTemp[0].id - 1].GetComponent<CanvasGroup>().alpha = 0;
                }
            }
            ordinal++;
            readData.PlayAudioMove();
        }

        public void Undo()
        {
            if (ordinal > 0)
            {
                foreach (GameObject border in borders)
                {
                    border.GetComponent<Image>().sprite = img_turn_default;
                }
                if (ordinal > 1)
                {
                    block[datas.historys[ordinal - 2].oldX - 1][datas.historys[ordinal - 2].oldY - 1].GetComponent<Image>().sprite = img_turn_lastest;
                    block[datas.historys[ordinal - 2].x - 1][datas.historys[ordinal - 2].y - 1].GetComponent<Image>().sprite = img_turn_lastest;
                }
                pieces[datas.historys[ordinal - 1].id - 1].x = datas.historys[ordinal - 1].oldX;
                pieces[datas.historys[ordinal - 1].id - 1].y = datas.historys[ordinal - 1].oldY;
                entity[datas.historys[ordinal - 1].id - 1].GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(block[datas.historys[ordinal - 1].oldX - 1][datas.historys[ordinal - 1].oldY - 1].GetComponent<Image>().rectTransform.anchoredPosition.x, (block[datas.historys[ordinal - 1].oldX - 1][datas.historys[ordinal - 1].oldY - 1].GetComponent<Image>().rectTransform.anchoredPosition.y+41));

                if ((datas.historys[ordinal - 1].typeId == 7 && datas.historys[ordinal - 1].oldX == 4) || (datas.historys[ordinal - 1].typeId == 8 && datas.historys[ordinal - 1].oldX == 5))
                {
                    pieces[datas.historys[ordinal - 1].id - 1].typeId = datas.historys[ordinal - 1].typeId;
                    if (pieces[datas.historys[ordinal - 1].id - 1].isBlack)
                    {
                        entity[datas.historys[ordinal - 1].id - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_B_pawn_th");
                    }
                    else
                    {
                        entity[datas.historys[ordinal - 1].id - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("games/chess_thai/" + readData.userData.statuser[readData.setting.gameId - 1].themeEquip + "/character_chess_W_pawn_th");
                    }
                }

                if (datas.historys[ordinal - 1].eatedId != 0)
                {
                    pieces[datas.historys[ordinal - 1].eatedId - 1].isAlive = true;
                    entity[datas.historys[ordinal - 1].eatedId - 1].GetComponent<CanvasGroup>().alpha = 1;
                }
                isBlackTurn = !isBlackTurn;
                ordinal--;

                resultBlock.GetComponent<CanvasGroup>().alpha = 0;
                resultBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
                readData.PlayAudioMove();
            }
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

        public void DestroyAllPieces()
        {
            for (int i = 1; i <= 8;i++)
            {
                foreach (Transform child in GameObject.Find("Row"+i+"Canvas").transform)
                {
                    if (child.name == "PiecesPrefab(Clone)")
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }

        public void FirstButton()
        {
            if (isAuto)
            {
                AutoButton();
            }
            foreach (GameObject border in borders)
            {
                border.GetComponent<Image>().sprite = img_turn_default;
            }
            pieces.Clear();
            entity.Clear();
            DestroyAllPieces();

            SetPieces();
            CloseResult();
            readData.PlayAudioMove();
        }

        public void BackwardButton()
        {
            if (isAuto)
            {
                AutoButton();
            }
            Undo();
        }

        public void AutoButton()
        {
            isAuto = !isAuto;
            if(isAuto){
                autoText.color = yellowColor;
                controlAutoImage.sprite = Resources.Load<Sprite>("UI/btn_history_replay_turn_auto_active");
                InvokeRepeating("Forward", 0, 1.0f);
            }
            else
            {
                autoText.color = grayColor;
                controlAutoImage.sprite = Resources.Load<Sprite>("UI/btn_history_replay_turn_auto");
                CancelInvoke("Forward");
            }
        }

        public void ForwardButton()
        {
            if (isAuto)
            {
                AutoButton();
            }
            Forward();
        }

        public void Forward()
        {
            if (ordinal < datas.historys.Count)
            {
                Move(datas.historys[ordinal].id, datas.historys[ordinal].x, datas.historys[ordinal].y);
                if (ordinal == datas.historys.Count)
                {
                    ShowResult();
                }
            }
        }

        public void LastButton()
        {
            if (isAuto)
            {
                AutoButton();
            }
            for (int i = ordinal; i < datas.historys.Count; i++)
            {
                Move(datas.historys[i].id, datas.historys[i].x, datas.historys[i].y);
            }
            ShowResult();
        }

        public void ShowResult()
        {
            if (datas.result == 1)
            {
                resultBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["win"][readData.setting.language].ToString();
            }
            else if (datas.result == 2)
            {
                resultBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["lose"][readData.setting.language].ToString();
            }
            else if (datas.result == 3)
            {
                resultBlock.GetComponentInChildren<Text>().text = readData.jsonWordingData["draw"][readData.setting.language].ToString();
            }
            bgFadeCanvasGroup.alpha = 1;
            bgFadeCanvasGroup.blocksRaycasts = true;
            resultBlock.GetComponent<CanvasGroup>().alpha = 1;
            resultBlock.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void CloseResult()
        {
            bgFadeCanvasGroup.alpha = 0;
            bgFadeCanvasGroup.blocksRaycasts = false;
            resultBlock.GetComponent<CanvasGroup>().alpha = 0;
            resultBlock.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void BackButton()
        {
            SceneManager.LoadScene("matchHistory");
        }
    }
}
