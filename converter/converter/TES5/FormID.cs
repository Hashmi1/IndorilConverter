﻿/*
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
using Utility;

namespace TES5
{
    // Because of the nature of FormID class implementation.
    // A single run of the program should be used to make a single
    // esm 
    // TODO: Fix?

    class FormID
    {
        public static void add_master_file()
        {
            current = current + +0x1000000;

        }

        static Dictionary<string, uint> dict = new Dictionary<string, uint>();
        static Dictionary<string, uint> dict_replacements = new Dictionary<string, uint>();

        static uint current = 0x802;// + 0x1000000;
        
        public static string find_editor_id(UInt32 formid)
        {

            foreach (string key in dict.Keys)
            {
                if (dict[key] == formid)
                {
                    return key;
                }
            }

            Log.error("FormID could not find an editor id for the given formid");
            return null;
        }

        // Only TES5.Record() should call this.
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
                Log.error("Editor ID already assigned: " + editor_id);
            }
            uint form_id = getNew();
            dict.Add(editor_id, form_id);
            return form_id;
        }
        
        // The editor_id should be the original morrowind editor id
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
                if (editor_id.Contains("_zload"))
                {
                  //  Log.error("Oh no");
                }
                return 0;
            }
            return  dict[editor_id];
        }

        public static uint getNew()
        {            
            if (current == 0x10036C9)
            {
                Log.info("Here");
            }
            return (current++);
        }

        public static uint getHEDRObj()
        {
            return (current);
        }
    }


}
