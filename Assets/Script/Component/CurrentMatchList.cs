using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Component
{
    [Serializable]
    public class CurrentMatchList
    {
        public int matchId;
        public int score1;
        public int score2;
        public string name1;
        public string name2;
    }
}
