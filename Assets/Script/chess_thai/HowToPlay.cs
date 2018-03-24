using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.Component;
using UnityEngine.SceneManagement;

namespace Assets.Script.chess_thai
{
    public class HowToPlay : MonoBehaviour
    {
        public ReadData readData;
        public Text selectGameButtonText;
        public Text header;

        public CanvasGroup footerBarCanvasGroup;
        public CanvasGroup backBarCanvasGroup;
        public CanvasGroup howtoCanvasGroup;
        public CanvasGroup moveCanvasGroup;
        public CanvasGroup winloseCanvasGroup;
        public CanvasGroup drawCanvasGroup;
        public CanvasGroup countPiecesCanvasGroup;
        public CanvasGroup countBoardCanvasGroup;

        public CanvasGroup backBar2CanvasGroup;
        public CanvasGroup kunCanvasGroup;
        public CanvasGroup medCanvasGroup;
        public CanvasGroup koneCanvasGroup;
        public CanvasGroup horseCanvasGroup;
        public CanvasGroup boatCanvasGroup;
        public CanvasGroup biaCanvasGroup;

        public Text howToMove;
        public Text howToWinLose;
        public Text howToDraw;
        public Text howToCountPiece;
        public Text howToCountBoard;

        public Text kun;
        public Text med;
        public Text kone;
        public Text horse;
        public Text boat;
        public Text bia;
        public Text kunHeader;
        public Text medHeader;
        public Text koneHeader;
        public Text horseHeader;
        public Text boatHeader;
        public Text biaHeader;

        public Text howToMoveKun;
        public Text howToMoveMed;
        public Text howToMoveKone;
        public Text howToMoveHorse;
        public Text howToMoveBoat;
        public Text howToMoveBia;

        public Text howToWinLoseHeader;
        public Text howToDrawHeader;
        public Text howToCountPieceHeader;
        public Text howToCountBoardHeader;

        public Text howToWinLoseDetail;
        public Text howToDrawDetail;
        public Text howToCountPieceDetail;
        public Text howToCountBoardDetail;

        void Start()
        {
            header.text = readData.jsonWordingData["howtoplay"][readData.setting.language].ToString();
            selectGameButtonText.text = readData.jsonWordingData["selectGame"][readData.setting.language].ToString();

            howToMove.text = readData.jsonWordingData["howToMove"][readData.setting.language].ToString();
            howToWinLose.text = readData.jsonWordingData["howToWinLose"][readData.setting.language].ToString();
            howToDraw.text = readData.jsonWordingData["howToDraw"][readData.setting.language].ToString();
            howToCountPiece.text = readData.jsonWordingData["howToCountPiece"][readData.setting.language].ToString();
            howToCountBoard.text = readData.jsonWordingData["howToCountBoard"][readData.setting.language].ToString();

            kun.text = readData.jsonWordingData["kun"][readData.setting.language].ToString();
            med.text = readData.jsonWordingData["med"][readData.setting.language].ToString();
            kone.text = readData.jsonWordingData["kone"][readData.setting.language].ToString();
            horse.text = readData.jsonWordingData["horse"][readData.setting.language].ToString();
            boat.text = readData.jsonWordingData["boat"][readData.setting.language].ToString();
            bia.text = readData.jsonWordingData["bia"][readData.setting.language].ToString();
            kunHeader.text = readData.jsonWordingData["kun"][readData.setting.language].ToString();
            medHeader.text = readData.jsonWordingData["med"][readData.setting.language].ToString();
            koneHeader.text = readData.jsonWordingData["kone"][readData.setting.language].ToString();
            horseHeader.text = readData.jsonWordingData["horse"][readData.setting.language].ToString();
            boatHeader.text = readData.jsonWordingData["boat"][readData.setting.language].ToString();
            biaHeader.text = readData.jsonWordingData["bia"][readData.setting.language].ToString();

            howToMoveKun.text = readData.jsonWordingData["howToMoveKun"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToMoveKun2"][readData.setting.language].ToString();
            howToMoveMed.text = readData.jsonWordingData["howToMoveMed"][readData.setting.language].ToString();
            howToMoveKone.text = readData.jsonWordingData["howToMoveKone"][readData.setting.language].ToString();
            howToMoveHorse.text = readData.jsonWordingData["howToMoveHorse"][readData.setting.language].ToString();
            howToMoveBoat.text = readData.jsonWordingData["howToMoveBoat"][readData.setting.language].ToString();
            howToMoveBia.text = readData.jsonWordingData["howToMoveBia"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToMoveBia2"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToMoveBia3"][readData.setting.language].ToString();

            howToWinLoseHeader.text = readData.jsonWordingData["howToWinLose"][readData.setting.language].ToString();
            howToDrawHeader.text = readData.jsonWordingData["howToDraw"][readData.setting.language].ToString();
            howToCountPieceHeader.text = readData.jsonWordingData["howToCountPiece"][readData.setting.language].ToString();
            howToCountBoardHeader.text = readData.jsonWordingData["howToCountBoard"][readData.setting.language].ToString();

            howToWinLoseDetail.text = readData.jsonWordingData["howToWinLoseDetail"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToWinLoseDetail2"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToWinLoseDetail3"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToWinLoseDetail4"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToWinLoseDetail5"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToWinLoseDetail6"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToWinLoseDetail7"][readData.setting.language].ToString();
            howToDrawDetail.text = readData.jsonWordingData["howToDrawDetail"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToDrawDetail2"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToDrawDetail3"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToDrawDetail4"][readData.setting.language].ToString();
            howToCountPieceDetail.text = readData.jsonWordingData["howToCountPieceDetail"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToCountPieceDetail2"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail3"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToCountPieceDetail4"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail5"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToCountPieceDetail6"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail7"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail8"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToCountPieceDetail9"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail10"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail11"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToCountPieceDetail12"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail13"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail14"][readData.setting.language].ToString() + "\n\n" + readData.jsonWordingData["howToCountPieceDetail15"][readData.setting.language].ToString() + "\n" + readData.jsonWordingData["howToCountPieceDetail16"][readData.setting.language].ToString();
            howToCountBoardDetail.text = readData.jsonWordingData["howToCountBoardDetail"][readData.setting.language].ToString();
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
                    //SceneManager.LoadScene("howToPlay");
                    break;
                case 3:
                    SceneManager.LoadScene("theme");
                    break;
                case 4:
                    SceneManager.LoadScene("option");
                    break;
            }
        }

        public void MoveButton()
        {
            HowtoCanvasGroup(false);
            MoveCanvasGroup(true);
        }
        public void WinLoseButton()
        {
            HowtoCanvasGroup(false);
            WinLoseCanvasGroup(true);
        }
        public void DrawButton()
        {
            HowtoCanvasGroup(false);
            DrawCanvasGroup(true);
        }
        public void CountPiecesButton()
        {
            HowtoCanvasGroup(false);
            CountPiecesCanvasGroup(true);
        }
        public void CountBoardButton()
        {
            HowtoCanvasGroup(false);
            CountBoardCanvasGroup(true);
        }
        public void BackToHowto()
        {
            HowtoCanvasGroup(true);
            MoveCanvasGroup(false);
            WinLoseCanvasGroup(false);
            DrawCanvasGroup(false);
            CountPiecesCanvasGroup(false);
            CountBoardCanvasGroup(false);
        }
        public void KunButton()
        {
            MoveCanvasGroup(false);
            KunCanvasGroup(true);
        }
        public void MedButton()
        {
            MoveCanvasGroup(false);
            MedCanvasGroup(true);
        }
        public void KoneButton()
        {
            MoveCanvasGroup(false);
            KoneCanvasGroup(true);
        }
        public void HorseButton()
        {
            MoveCanvasGroup(false);
            HorseCanvasGroup(true);
        }
        public void BoatButton()
        {
            MoveCanvasGroup(false);
            BoatCanvasGroup(true);
        }
        public void BiaButton()
        {
            MoveCanvasGroup(false);
            BiaCanvasGroup(true);
        }
        public void BackToMove()
        {
            MoveCanvasGroup(true);
            KunCanvasGroup(false);
            MedCanvasGroup(false);
            KoneCanvasGroup(false);
            HorseCanvasGroup(false);
            BoatCanvasGroup(false);
            BiaCanvasGroup(false);
        }

        public void FooterBarCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                footerBarCanvasGroup.alpha = 1;
                footerBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                footerBarCanvasGroup.alpha = 0;
                footerBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void BackBarCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                backBarCanvasGroup.alpha = 1;
                backBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                backBarCanvasGroup.alpha = 0;
                backBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void HowtoCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                howtoCanvasGroup.alpha = 1;
                howtoCanvasGroup.blocksRaycasts = true;
                footerBarCanvasGroup.alpha = 1;
                footerBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                howtoCanvasGroup.alpha = 0;
                howtoCanvasGroup.blocksRaycasts = false;
                footerBarCanvasGroup.alpha = 0;
                footerBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void MoveCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                moveCanvasGroup.alpha = 1;
                moveCanvasGroup.blocksRaycasts = true;
                backBarCanvasGroup.alpha = 1;
                backBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                moveCanvasGroup.alpha = 0;
                moveCanvasGroup.blocksRaycasts = false;
                backBarCanvasGroup.alpha = 0;
                backBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void WinLoseCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                winloseCanvasGroup.alpha = 1;
                winloseCanvasGroup.blocksRaycasts = true;
                backBarCanvasGroup.alpha = 1;
                backBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                winloseCanvasGroup.alpha = 0;
                winloseCanvasGroup.blocksRaycasts = false;
                backBarCanvasGroup.alpha = 0;
                backBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void DrawCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                drawCanvasGroup.alpha = 1;
                drawCanvasGroup.blocksRaycasts = true;
                backBarCanvasGroup.alpha = 1;
                backBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                drawCanvasGroup.alpha = 0;
                drawCanvasGroup.blocksRaycasts = false;
                backBarCanvasGroup.alpha = 0;
                backBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void CountPiecesCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                countPiecesCanvasGroup.alpha = 1;
                countPiecesCanvasGroup.blocksRaycasts = true;
                backBarCanvasGroup.alpha = 1;
                backBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                countPiecesCanvasGroup.alpha = 0;
                countPiecesCanvasGroup.blocksRaycasts = false;
                backBarCanvasGroup.alpha = 0;
                backBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void CountBoardCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                countBoardCanvasGroup.alpha = 1;
                countBoardCanvasGroup.blocksRaycasts = true;
                backBarCanvasGroup.alpha = 1;
                backBarCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                countBoardCanvasGroup.alpha = 0;
                countBoardCanvasGroup.blocksRaycasts = false;
                backBarCanvasGroup.alpha = 0;
                backBarCanvasGroup.blocksRaycasts = false;
            }
        }
        public void KunCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                kunCanvasGroup.alpha = 1;
                kunCanvasGroup.blocksRaycasts = true;
                backBar2CanvasGroup.alpha = 1;
                backBar2CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                kunCanvasGroup.alpha = 0;
                kunCanvasGroup.blocksRaycasts = false;
                backBar2CanvasGroup.alpha = 0;
                backBar2CanvasGroup.blocksRaycasts = false;
            }
        }
        public void MedCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                medCanvasGroup.alpha = 1;
                medCanvasGroup.blocksRaycasts = true;
                backBar2CanvasGroup.alpha = 1;
                backBar2CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                medCanvasGroup.alpha = 0;
                medCanvasGroup.blocksRaycasts = false;
                backBar2CanvasGroup.alpha = 0;
                backBar2CanvasGroup.blocksRaycasts = false;
            }
        }
        public void KoneCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                koneCanvasGroup.alpha = 1;
                koneCanvasGroup.blocksRaycasts = true;
                backBar2CanvasGroup.alpha = 1;
                backBar2CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                koneCanvasGroup.alpha = 0;
                koneCanvasGroup.blocksRaycasts = false;
                backBar2CanvasGroup.alpha = 0;
                backBar2CanvasGroup.blocksRaycasts = false;
            }
        }
        public void HorseCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                horseCanvasGroup.alpha = 1;
                horseCanvasGroup.blocksRaycasts = true;
                backBar2CanvasGroup.alpha = 1;
                backBar2CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                horseCanvasGroup.alpha = 0;
                horseCanvasGroup.blocksRaycasts = false;
                backBar2CanvasGroup.alpha = 0;
                backBar2CanvasGroup.blocksRaycasts = false;
            }
        }
        public void BoatCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                boatCanvasGroup.alpha = 1;
                boatCanvasGroup.blocksRaycasts = true;
                backBar2CanvasGroup.alpha = 1;
                backBar2CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                boatCanvasGroup.alpha = 0;
                boatCanvasGroup.blocksRaycasts = false;
                backBar2CanvasGroup.alpha = 0;
                backBar2CanvasGroup.blocksRaycasts = false;
            }
        }
        public void BiaCanvasGroup(bool isOpen)
        {
            if (isOpen)
            {
                biaCanvasGroup.alpha = 1;
                biaCanvasGroup.blocksRaycasts = true;
                backBar2CanvasGroup.alpha = 1;
                backBar2CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                biaCanvasGroup.alpha = 0;
                biaCanvasGroup.blocksRaycasts = false;
                backBar2CanvasGroup.alpha = 0;
                backBar2CanvasGroup.blocksRaycasts = false;
            }
        }
    }
}
