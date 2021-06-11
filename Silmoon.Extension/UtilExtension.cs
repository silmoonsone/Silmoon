using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Silmoon.Extension
{
    public static class UtilExtension
    {
        public static Match IsChinaMainlandPhoneNumber(this string s, out bool isMatch)
        {
            if (string.IsNullOrEmpty(s))
            {
                isMatch = false;
                return null;
            }
            Match match = Regex.Match(s, @"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,3,5,6,7,8])|(19[8,9]))\d{8}$");
            isMatch = match.Success;
            return isMatch ? match : null;
        }

        public static bool IsChinaMainlandPhoneNumber(this string s)
        {
            IsChinaMainlandPhoneNumber(s, out bool success);
            return success;
        }
    }
}
