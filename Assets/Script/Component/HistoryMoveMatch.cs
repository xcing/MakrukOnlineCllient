using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.chess_thai;

namespace Assets.Script.Component
{
    [Serializable]
    public class HistoryMoveMatch
    {
        public List<History> historys;
        public string opponentName;
        public int opponentScore;
        public int result;
        public bool iAmBlack;
        public string createdDate;
    }
}
