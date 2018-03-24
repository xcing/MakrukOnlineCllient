using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

namespace Assets.Script.Component
{
    [Serializable]
    public class Statuser
    {
        public int statuserId;
        public int score;
        public int win;
        public int lose;
        public int money;
        public int draw;
        public int abandon;
        public List<int> themeHave;
        public int themeEquip;
        public int ads;
    }
}
