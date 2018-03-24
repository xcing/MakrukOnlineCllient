using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Script.chess_thai
{
    public class Pieces : MonoBehaviour
    {
        public int id;
        public bool isBlack;
        public int x;
        public int y;
        public bool isAlive;
        public int typeId; // 1 = kun, 2 = med, 3 = black kone, 4 = white kone, 5 = horse, 6 = boat, 7 = black bia, 8 = white bia

        public void TabForMove()
        {
            GameObject.Find("TwoPlayerScript").GetComponent<TwoPlayer>().TabForMove(this);
        }
    }
}
