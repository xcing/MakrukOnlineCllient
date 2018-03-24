using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

namespace Assets.Script.Component
{
    [Serializable]
    public class Advertise
    {
        public int position;
        public string url;
        public string picture;
    }
}
