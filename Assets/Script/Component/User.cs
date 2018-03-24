using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

namespace Assets.Script.Component
{
    [Serializable]
    public class User
    {
        public int userId;
        public string displayName;
        public string email;
        public string guestId;
        public string facebookId;
        public string displayImage;
        public List<Statuser> statuser;
        public int currentMatchId;
        public int currentMatchOrdinal;
        public List<Advertise> advertise;
        public bool ads;
        public string xsignature;
    }
}
