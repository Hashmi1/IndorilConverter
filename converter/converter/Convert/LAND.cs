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
using TES5;
using Utility;
using System.IO;

namespace Convert
{
    class LAND
    {
        static LAND()
        {
            if (File.Exists(Config.Paths.tesannwyn_path_ltex))
            {
                File.Delete(Config.Paths.tesannwyn_path_ltex);
            }
        }
        
        public static void add_texture(int index, string editor_id, string path, uint formid)
        {
            TextWriter w = File.AppendText(Config.Paths.tesannwyn_path_ltex);
            w.WriteLine((index + 1) + "," + editor_id + "," + path + "," + Text.toHex(formid));
            w.Flush();
            w.Close();
        }


        public static List<Group> convert(string file)
        {
            if (!LTEX.done)
            {
                Log.error("Must convert LTEX before converting LAND.");
            }
                        
            External.TESAnnwyn tesannwyn = new External.TESAnnwyn();

            tesannwyn.convert(file, Config.Paths.tmp + "tmp.esp");

            ESM esm = ESM.read_from_file(Config.Paths.tmp + "tmp.esp");
            List<Group> grps = esm.getGroups();
            
            new LAND().renumber_formids(grps);
            
            return grps;
        }

        // TESAnnwyn has assumed our given texture formids in tes3ltex.txt to be from skyrim.esm, making skyrim.esm a master 
        // of the converted file.
        // So now we need to renumber the formids in the converted esp, decreasing the mod-index back to 00
        // An alternate is to simply remove the master from the TES4 hedr and let the game engine automatically handle stuff
        // but its cleaner this way

        private void renumber_formids(List<Group> grps)
        {            
            foreach (Group g in grps)
            {
                search_group(g);
            }
        }

        private void search_group(Group g)
        {
            if (g.isType(Group.TYPE.TOP) || g.isType(Group.TYPE.INT_BLCK) || g.isType(Group.TYPE.INT_SUB_BLCK) || g.isType(Group.TYPE.EXT_BLCK) || g.isType(Group.TYPE.EXT_SUB_BLCK))
            {
            }
            else
            {
                UInt32 formid = Binary.toUInt32(g.label);

                if (formid >= 0x1000000)
                {
                    formid -= 0x1000000;
                    g.label = Binary.toBin(formid);
                }
            }

            foreach (Record r in g.records)
            {
                search_record(r);
            }

            foreach (Group gSub in g.subGroups)
            {
                search_group(gSub);
            }

        }

        

        private void search_record(Record r)
        {          
         
            if (r.id >= 0x1000000)
            {
                r.id -= 0x1000000;
            }

            foreach (Field f in r.fields)
            {
                search_field(f);
            }
        }

        
        private void search_field(Field f)
        {
            if (f.isType("BTXT"))
            {
                Log.error("BTXT Record found");
            }
        }
                
    }
}
