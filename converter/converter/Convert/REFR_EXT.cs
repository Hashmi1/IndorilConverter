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
using Utility;
using System.IO;

namespace Convert
{
    // We use TESAnnwyn to get deal with the messy landscape data for us. Now we need to place objects within it

    class REFR_EXT
    {    
        static ReferenceGroup_Index ref_index = new ReferenceGroup_Index();

        public static void add_references(List<TES5.Group> grps_, string file_mw)
        {
            Log.info("Adding Exterior CELL References");

            ref_index.make(grps_);

            TES3.ESM.open(file_mw);
            int count = 0;
            while (TES3.ESM.find("CELL"))
            {
                
                TES3.CELL morrowind_cell = new TES3.CELL();
                morrowind_cell.read();

                if (morrowind_cell.interior)
                {
                    continue; 
                }

                Log.info(morrowind_cell.cell_name);

                count++;

                foreach (TES3.REFR morrowind_reference in morrowind_cell.references)
                {
                    string refr_id = morrowind_reference.editor_id;
                    uint formid = TES5.FormID.get(refr_id);
                    if (formid == 0)
                    {
                        continue;   // Reference Base not converted, so skip
                    }

                    Log.info("Adding Reference: " + refr_id);

                    TES5.REFR skyrim_reference = new TES5.REFR(formid, morrowind_reference.x, morrowind_reference.y, morrowind_reference.z, morrowind_reference.xR, morrowind_reference.yR, morrowind_reference.zR,morrowind_reference.scale);
                                        
                    int cell_x = (int)(morrowind_reference.x / 4096f);
                    int cell_y = (int)(morrowind_reference.y / 4096f);

                    if (morrowind_reference.x < 0f)
                    {
                        cell_x--;
                    }

                    if (morrowind_reference.y < 0f)
                    {
                        cell_y--;
                    }


                    TES5.Group reference_group = ref_index.get_reference_group(cell_x,cell_y);
                
                    reference_group.addRecord(skyrim_reference);

                }
            }

            //Log.info(count + " Exterior Cells found");
            TES3.ESM.close();
        }
            
      
    }

    class ReferenceGroup_Index
    {
        Dictionary<Tuple<int, int>, TES5.Group> dict = new Dictionary<Tuple<int, int>, TES5.Group>();
        Dictionary<uint, Tuple<int, int>> formid_index = new Dictionary<uint, Tuple<int, int>>();

        bool made = false;

        private void put(int x, int y,TES5.Group group)
        {
            dict.Add(new Tuple<int, int>(x, y), group);
        }

        public TES5.Group get_reference_group(int x, int y)
        {
            if (!made)
            {
                Log.error("ReferenceGroup Index has not been made.");
            }

            Tuple<int, int> key = new Tuple<int, int>(x, y);

            if (!dict.ContainsKey(key))
            {
                Log.error("Cell-Index not found: " + x + "," + y);
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

                Tuple<int, int> cell_grid_info = formid_index[cell_form_id];

                int x = cell_grid_info.Item1;
                int y = cell_grid_info.Item2;

                put(x, y, grup);

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
                        TES5.Field xclc = r.find_field("XCLC");

                        if (xclc != null)
                        {

                            BinaryReader reader = xclc.getData();
                            int x = reader.ReadInt32();
                            int y = reader.ReadInt32();

                            formid_index.Add(r.id, new Tuple<int, int>(x, y));

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
