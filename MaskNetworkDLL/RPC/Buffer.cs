using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaskGame.RPC
{
    public class Buffer
    {
        const int MAX_LENGTH = 1024;
        const int MAX_ALIVE_SECONDS = 15;

        private static readonly Buffer instance = new Buffer();

        private Dictionary<string, Meta> dict = new Dictionary<string, Meta>();

        private object dictLock = new object();

        public static Buffer GetInstance()
        {
            return instance;
        }

        public Meta Get(string k)
        {
            var meta = dict[k];
            dict.Remove(k);
            return meta;
        }

        public void Set(string k, Meta meta)
        {
            dict[k] = meta;
            lock(dictLock)
            {
                if (Length > MAX_LENGTH)
                {
                    var keyCollection = dict.Keys;
                    var removeKeyList = new List<string>();
                    foreach(string key in keyCollection)
                    {
                        var aliveSeconds = Util.Date.UtcNow().Seconds - dict[key].Timestamp;
                        if(aliveSeconds > MAX_ALIVE_SECONDS || aliveSeconds < 0)
                        {
                            removeKeyList.Add(key);
                        }
                    }
                    foreach(string key in removeKeyList)
                    {
                        dict.Remove(key);
                    }
                }
            }
        }

        public int Length
        {
            get
            {
                return dict.Count;
            }
        }
    }
}
