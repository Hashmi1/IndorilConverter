/*
Copyright(c) 2014 Hashmi1

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{

    class Text
    {
        public static string toHex(uint id)
        {
            string st = id.ToString("X");

            while (st.Length < 8)
            {
                st = "0" + st;
            }

            return st;
        }
        
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
