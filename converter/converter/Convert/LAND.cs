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
using TES5;
using Utility;

namespace Convert
{
    class LAND
    {
        public List<Group> convert(string file)
        {
            External.TESAnnwyn tesannwyn = new External.TESAnnwyn();
             return null;
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
        }
                
    }
}
