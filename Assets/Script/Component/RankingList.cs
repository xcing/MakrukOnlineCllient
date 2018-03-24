using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Component
{
    [Serializable]
    public class RankingList
    {
        public int userId;
        public string displayName;
        public int score;
        public int photoFrameId;
        public int profilePictureId;
    }
}
