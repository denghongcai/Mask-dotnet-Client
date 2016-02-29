using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaskGame.Util
{
    public class Date
    {
        public TimeSpan time;

        public UInt32 Seconds
        {
            get {
                return (UInt32)time.TotalSeconds;
            }
        }

        public UInt64 Milliseconds
        {
            get
            {
                return (UInt64)time.TotalMilliseconds;
            }
        }

        public static Date UtcNow()
        {
            var date = new Date();
            date.time = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1));
            return date;
        }
    }
}
