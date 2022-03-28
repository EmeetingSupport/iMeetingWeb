﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Core
{
    public static class UnixTimestamp
    {
        public static long ToUnixTimestamp(this DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = System.Convert.ToInt64((target - date).TotalSeconds);

            return unixTimestamp;
        }

       
        public static DateTime ToDateTime(this DateTime target, long timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);

            return dateTime.AddSeconds(timestamp);
        }

        public static string ToBase64Encryption(this string text)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(text);
            string sBase64 = System.Convert.ToBase64String(bytes);
            return sBase64;
        }

    }
}
