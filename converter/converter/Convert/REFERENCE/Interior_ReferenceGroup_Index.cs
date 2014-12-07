/*
Copyright(c) 2014 Hashmi1. All Rights Reserved

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
using System.IO;
using Utility;

namespace Convert.REFERENCE
{
    class Interior_ReferenceGroup_Index
    {
        Dictionary<string, TES5.Group> dict = new Dictionary<string, TES5.Group>();
        Dictionary<uint, string> formid_index = new Dictionary<uint, string>();

        bool made = false;

        private void put(string cell_id,TES5.Group group)
        {
            if (!cell_id.StartsWith(Config.Prefixes.converted_editor_ids))
            {
                Log.error("Given Editor ID is not in skyrim format. Potential Logical Bug");
            }
            cell_id = Text.trim(cell_id);
            dict.Add(cell_id, group);
        }

        public TES5.Group get_reference_group(string cell_id)
        {
            if (!cell_id.StartsWith(Config.Prefixes.converted_editor_ids))
            {
                Log.error("Given Editor ID is not in skyrim format. Potential Logical Bug");
            }

            if (!made)
            {
                Log.error("ReferenceGroup Index has not been made.");
            }

            string key = Text.trim(cell_id);

            if (!dict.ContainsKey(key))
            {
                Log.error("Cell-Index not found: " + key);
                return null;
            }

            return dict[key];
        }

        public void make(List<TES5.Group> groups)
        {
            made = true;
                                    
            foreach (TES5.Group g in groups)
            {
                search_group(g);                
            }
                        
        }
                
        private void search_group(TES5.Group grup)
        {
            if (grup.isType(TES5.Group.TYPE.TEMP_REFR))
            {
                UInt32 cell_form_id = Binary.toUInt32(grup.label);

                if (!formid_index.ContainsKey(cell_form_id))
                {
                    Log.error("Cell with formid: " + cell_form_id + " was not encountered");
                }

                string cell_id = formid_index[cell_form_id];
                                
                put(cell_id, grup);

                return;
            }
          
            Queue<TES5.Group> grps = new Queue<TES5.Group>(grup.subGroups);
            Queue<TES5.Record> recs = new Queue<TES5.Record>(grup.records);

            foreach (int t in grup.turn )
            {
                
                if (t == 1)
                {
                    
                    TES5.Record r = recs.Dequeue();

                    #region if (r.isType("CELL"))

                    if (r.isType("CELL"))
                    {
                        TES5.Field DATA = r.find_field("DATA");

                        if (DATA == null)
                        {
                            continue;
                        }

                        if (DATA.dataSize < 2)
                        {
                            continue;
                        }
                        
                        UInt16 flags = DATA.getData().ReadUInt16();

                        if (!BinaryFlag.isSet((UInt16)flags, (UInt16)0x0001))
                        {
                            continue;
                        }

                        TES5.Field EDID = r.find_field("EDID");

                        if (EDID != null)
                        {
                            BinaryReader reader = EDID.getData();
                            string cell_id = Text.trim(new string (reader.ReadChars(EDID.dataSize)));                            
                            formid_index.Add(r.id, cell_id);
                        }
                    }
                    #endregion
                }

                if (t == 2)
                {
                    TES5.Group g = grps.Dequeue();
                    search_group(g);
                }

            }
        }
       
    }

}
