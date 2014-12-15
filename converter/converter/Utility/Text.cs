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
        public static string toString(char[] zstring)
        {
            return trim(new string(zstring));
        }

        public static string toHex(uint id)
        {
            string st = id.ToString("X");

            while (st.Length < 8)
            {
                st = "0" + st;
            }

            return st;
        }

        public static string flatten_path(string txt)
        {
            txt = trim(txt);
            txt = txt.Split('\\')[txt.Split('\\').Length - 1];
            return txt;
        }

        public static string model_path_string(string txt)
        {
            //Encoding.ASCII.GetString(
            return Text.trim(Encoding.ASCII.GetString(model_path(txt)));
        }

        public static byte[] model_path(string txt)
        {
            txt = txt.Split('\\')[txt.Split('\\').Length - 1];
            txt = trim(txt);
            txt = txt.ToLower();
            txt.Replace(" ", "_");
            txt = Config.Prefixes.converted_meshes + txt;
            return zstring(txt);
        }

        public static byte[] zstring(string txt)
        {
            txt = trim(txt);

            if (txt[txt.Length - 1] != '\0')
            {
                txt = txt + '\0';
            }

            

            return Encoding.ASCII.GetBytes(txt.ToCharArray());
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

        public static string editor_id_string(string txt)
        {
            return Text.trim(Encoding.ASCII.GetString(editor_id(txt)));
        }

        public static byte[] editor_id(string txt)
        {
            txt = txt.Replace("_", "__"); // DOUBLE: Prevent editor_id conflicts when converting for example x_y and x.y 
         
            txt = txt.Replace(" ", "_");
            txt = txt.Replace("`", "_");
            txt = txt.Replace("~", "_");
            txt = txt.Replace("!", "_");
            txt = txt.Replace("@", "_");
            txt = txt.Replace("#", "_");
            txt = txt.Replace("$", "_");
            txt = txt.Replace("%", "_");
            txt = txt.Replace("^", "_");
            txt = txt.Replace("&", "_");
            txt = txt.Replace("*", "_");
            txt = txt.Replace("(", "_");
            txt = txt.Replace(")", "_");
            txt = txt.Replace("-", "_");            
            txt = txt.Replace("+", "_");
            txt = txt.Replace("=", "_");
            txt = txt.Replace("[", "_");
            txt = txt.Replace("{", "_");
            txt = txt.Replace("}", "_");
            txt = txt.Replace("]", "_");
            txt = txt.Replace("\\", "_");
            txt = txt.Replace("|", "_");
            txt = txt.Replace(":", "_");
            txt = txt.Replace(";", "_");
            txt = txt.Replace("\"", "_");
            txt = txt.Replace("'", "_");
            txt = txt.Replace(",", "_");
            txt = txt.Replace("<", "_");
            txt = txt.Replace(">", "_");
            txt = txt.Replace(".", "_");
            txt = txt.Replace("?", "_");
            txt = txt.Replace("/", "_");

            txt = trim(txt);
            txt = Config.Prefixes.converted_editor_ids + txt;
            txt = txt.ToLower();

            return zstring(txt);
        }
    }

}
