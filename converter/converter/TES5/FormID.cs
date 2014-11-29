using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace TES5
{
    class FormID
    {
        static Dictionary<string, uint> dict = new Dictionary<string, uint>();
        static Dictionary<string, uint> dict_replacements = new Dictionary<string, uint>();

        static uint current = 0x802 + 0x1000000;
        static uint first = 0x802;

        public static void init_replacements()
        {

        }

        public static uint set(string editor_id)
        {
            editor_id = Text.trim(editor_id);

            if (Convert.REPLACEMENT.IsDefined(editor_id))
            {
                uint formid = Convert.REPLACEMENT.get_formid(editor_id);
                dict.Add(editor_id, formid);
                return formid;
            }

            if (dict.ContainsKey(editor_id))
            {
                Log.error("Editor ID already assigned" + editor_id);
            }
            uint form_id = getNew();
            dict.Add(editor_id, form_id);
            return form_id;
        }
        
        public static uint get(string editor_id)
        {
            editor_id = Text.trim(editor_id);

            if (Convert.REPLACEMENT.IsDefined(editor_id))
            {
                uint formid = Convert.REPLACEMENT.get_formid(editor_id);
                dict.Add(editor_id, formid);
                return formid;
            }

            if (!dict.ContainsKey(editor_id))
            {
                return 0;
            }
            return  dict[editor_id];
        }

        public static uint getNew()
        {                 
            return (current++);
        }

        public static uint getHEDRObj()
        {
            return (first);
        }
    }


}
