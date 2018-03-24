using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Component
{
    [Serializable]
    public class HistoryMatchList
    {
        public int matchId;
        public int ordinal;
        public string myScore;
        public int result;
        public string opponentName;
        public int opponentScore;
        public string createdDate;
        public int haveHistory;
    }
}
