using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{

    class Text
    {
        
        public static char[] zstring(string txt)
        {
            txt = trim(txt);

            if (txt[txt.Length - 1] != '\0')
            {
                txt = txt + '\0';
            }

            return txt.ToCharArray();

        }

        public static string trim(string txt)
        {
            txt = txt.Trim();
            while (txt[txt.Length - 1] == '\0')
            {
                txt = txt.Substring(0, txt.Length - 1);
            }
            txt = txt.Trim();
            return txt;
        }

        public static char[] editor_id(string txt)
        {
            txt = trim(txt);
            txt = "mw_" + txt;
            txt = txt.ToLower();
            txt = txt.Replace("'", "");
            txt = txt.Replace(",", "");
            txt = txt.Replace(":", "");
            txt = txt.Replace(" ", "_");
            txt = txt.Replace("-", "");
            return zstring(txt);
        }
    }

}
