using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

namespace Assets.Script.Component
{
    [Serializable]
    public class Setting
    {
        public bool soundOn;
        public int photoFrame;
        public int profilePic;
        public int language;
        public int gameId;

        public string email;
        public string password;
        public string facebookId;
        public string deviceId;
        public int loginType; // 0 = never login, 1 = normal, 2 = facebook, 3 = device

        public Setting()
        {
            soundOn = true;
            photoFrame = 1;
            profilePic = 1;
            language = 99;
            gameId = 1;

            email = "";
            password = "";
            facebookId = "";
            deviceId = "";
            loginType = 0;
        }
    }
}
