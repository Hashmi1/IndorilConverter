using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace Convert
{
    class REPLACEMENT
    {
        static Dictionary<string, uint> dict = new Dictionary<string, uint>();

        
        public static bool IsDefined(string editor_id)
        {
            return dict.ContainsKey(editor_id);
        }

        public static uint get_formid(string editor_id)
        {
            return dict[editor_id];
        }

        public static void init()
        {
            TextReader fin = File.OpenText("replacements\\a.txt");
            // +_!@#$%^&*()_+

            while (fin.Peek() != -1)
            {
                string line = fin.ReadLine();
                line.Trim();

                if (line.StartsWith("MAST:"))
                {
                    
                }

                string[] splt = line.Split('=');

                if (splt.Length != 2)
                {
                    Log.info("Invalid Replacement Format:\n" + line + "\n");
                    continue;
                }

                string editor_id = splt[0];
                string str_formid = splt[1];

                uint formid = System.Convert.ToUInt32(str_formid, 16);
                if (dict.ContainsKey(editor_id))
                {
                    Log.info("Multiple Replacements found for \'" + editor_id + "\' Will use latest found.");
                }
                dict.Add(str_formid,formid);

            }

        }
    }
}
