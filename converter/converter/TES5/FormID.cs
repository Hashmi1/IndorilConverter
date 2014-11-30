/*
Copyright 2014 Hashmi1

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
using Utility;

namespace TES5
{
    class FormID
    {
        static Dictionary<string, uint> dict = new Dictionary<string, uint>();
        static Dictionary<string, uint> dict_replacements = new Dictionary<string, uint>();

        static uint current = 0x802;// + 0x1000000;
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
