using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaskGame.Util
{
    public class ShortId
    {
        /// <summary>
        /// short id generator, need improve
        /// </summary>
        /// <returns></returns>
        public static string NewString()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
        }
    }
}
