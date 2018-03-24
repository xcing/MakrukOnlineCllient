using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Component
{
    [Serializable]
    public class Match
    {
        public string matchId;
        public string oppenentDisplayname;
        public int oppenentScore;
        public int isHost;
    }
}
