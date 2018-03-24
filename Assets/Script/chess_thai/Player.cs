using UnityEngine;
using System.Collections;

namespace Assets.Script.chess_thai
{
    public class Player : MonoBehaviour
    {
        public bool iAmBlack;

        public string playerName;
        public int score;
        public int id;
        public int x;
        public int y;
        public bool isBlackTurn;
        public int ordinal;
        public int oldOrdinal;
        public int sideAskForDraw;
        public int sideConcede;
        public int sideCountBoard;
        public bool isDraw;
        public bool isDisconnect;
        public int sideSendEmoticon;
        public int emoticonIdFromOpponent;

        void Start()
        {
            oldOrdinal = 1;
            ordinal = 1;
            sideConcede = 0;
            sideCountBoard = 3;
            isBlackTurn = true;
            isDisconnect = false;
            sideSendEmoticon = 0;
            emoticonIdFromOpponent = 0;
        }

        /*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            if (stream.isWriting)
            {
                id = GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().lastMove.id;
                x = GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().lastMove.x;
                y = GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().lastMove.y;
                isBlackTurn = GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().lastMove.isBlackTurn;
                ordinal = GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().lastMove.ordinal;

                stream.Serialize(ref id);
                stream.Serialize(ref x);
                stream.Serialize(ref y);
                stream.Serialize(ref isBlackTurn);
                stream.Serialize(ref ordinal);
            }
            else
            {
                stream.Serialize(ref id);
                stream.Serialize(ref x);
                stream.Serialize(ref y);
                stream.Serialize(ref isBlackTurn);
                stream.Serialize(ref ordinal);
            }
        }*/

        void Update()
        {
            /*if (GameObject.Find("NetworkManagement").GetComponent<NetworkManagement>().gameIsStart)
            {
                Debug.Log(GameObject.Find("PlayerBlack(Clone)").GetComponent<Player>().ordinal + " " + GameObject.Find("PlayerWhite(Clone)").GetComponent<Player>().ordinal);
            }*/

            if (iAmBlack == isBlackTurn && ordinal > oldOrdinal)
            {
                oldOrdinal = ordinal;
                isBlackTurn = !isBlackTurn;
                SyncedMovement();
            }
            if (name != "" && GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().playerName1.text == "")
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().SetTwoPlayer();
            }
            if (sideAskForDraw != 0)
            {
                if (sideAskForDraw == 1 && !GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack)
                {
                    sideAskForDraw = 0;
                    GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpenDrawBlock();
                }
                else if (sideAskForDraw == 2 && GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack)
                {
                    sideAskForDraw = 0;
                    GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpenDrawBlock();
                }
            }
            if (sideConcede != 0)
            {
                if (sideConcede == 1 && !GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack)
                {
                    sideConcede = 0;
                    GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().SendResultEnemyConcede();
                }
                else if (sideConcede == 2 && GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack)
                {
                    sideConcede = 0;
                    GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().SendResultEnemyConcede();
                }
            }
            if (sideCountBoard == 1 || sideCountBoard == 2)
            {
                sideCountBoard = 3;
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CountTurnBoard();
            }
            if (sideCountBoard == 0)
            {
                sideCountBoard = 3;
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CancelCountTurnBoard();
            }
            if (isDraw)
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().SendResultDraw();
            }

            if (sideSendEmoticon != 0)
            {
                if (sideSendEmoticon == 1 && !GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack)
                {
                    sideSendEmoticon = 0;
                    GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpponentSendEmoticon(emoticonIdFromOpponent);
                    emoticonIdFromOpponent = 0;
                }
                else if (sideSendEmoticon == 2 && GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack)
                {
                    sideSendEmoticon = 0;
                    GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpponentSendEmoticon(emoticonIdFromOpponent);
                    emoticonIdFromOpponent = 0;
                }
            }
        }

        private void SyncedMovement()
        {
            GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().Move(id, x, y, false);
        }

        [RPC]
        public void SetPlayer(string name, int score)
        {
            this.playerName = name;
            this.score = score;
        }

        [RPC]
        public void SetId(int id)
        {
            this.id = id;
        }
        [RPC]
        public void SetX(int x)
        {
            this.x = x;
        }
        [RPC]
        public void SetY(int y)
        {
            this.y = y;
        }
        [RPC]
        public void SetOrdinal(int ordinal)
        {
            this.ordinal = ordinal;
        }
        [RPC]
        public void SetIsBlackTurn(bool isBlackTurn)
        {
            this.isBlackTurn = isBlackTurn;
        }
        [RPC]
        public void SetSideAskForDraw(int sideAskForDraw)
        {
            this.sideAskForDraw = sideAskForDraw;
        }
        [RPC]
        public void SetSideConcede(int sideConcede)
        {
            this.sideConcede = sideConcede;
        }
        [RPC]
        public void SetSideCountBoard(int sideCountBoard)
        {
            this.sideCountBoard = sideCountBoard;
        }
        [RPC]
        public void SetIsDraw(bool isDraw)
        {
            this.isDraw = isDraw;
        }
        [RPC]
        public void SetIsDisconnect(bool isDisconnect)
        {
            this.isDisconnect = isDisconnect;
        }
        [RPC]
        public void SetSideSendEmoticon(int sideSendEmoticon)
        {
            this.sideSendEmoticon = sideSendEmoticon;
        }
        [RPC]
        public void SetEmoticonIdFromOpponent(int emoticonIdFromOpponent)
        {
            this.emoticonIdFromOpponent = emoticonIdFromOpponent;
        }
    }

}
