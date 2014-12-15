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
using TES3;
using Utility;
using System.IO;

using External;

namespace Program
{
    class Program
    {
        
        static void Main(string[] args)
        {

            //Convert.NPC_.presets();
            //Log.exit("DONE");

           
            
            TES5.ESM esm = new TES5.ESM("final.esp");
            esm.add_masters("Skyrim.esm");
            
            TES5.Group[] ltex = Convert.LTEX.convert(Config.Paths.mw_esm);
            //TES5.Group door = Convert.DOOR.convert(Config.Paths.mw_esm);
            TES5.Group furn = Convert.FURN.convert();
            TES5.Group acti = Convert.ACTI.getInstance().convert(Config.Paths.mw_esm);
            TES5.Group npc_ = Convert.NPC_.convert(Config.Paths.mw_esm);
            TES5.Group stat = Convert.STAT.convert(Config.Paths.mw_esm);

            TES5.Group lgtm = Convert.LGTM.convert();
                        
            acti.addRecord(TES5.ACTI.get_water_instance());

            TES5.Group light = Convert.LIGH.convert(Config.Paths.mw_esm);
            
            TES5.Group cell = Convert.CELL.convert(Config.Paths.mw_esm);
            //List<TES5.Group> wrld = Convert.LAND.convert(Config.Paths.mw_esm);

            List<TES5.Group> cell_grp = new List<TES5.Group>();
            cell_grp.Add(cell);

            Convert.REFERENCE.REFR.add_references(Config.Paths.mw_esm,cell_grp,null);
            
            
            
            esm.add_group(ltex[0]);
            esm.add_group(ltex[1]);
            esm.add_group(stat);
            esm.add_group(npc_);
            esm.add_group(furn);
            esm.add_group(lgtm);
            esm.add_group(acti);
            esm.add_group(light);

            //esm.add_group(door);
            esm.add_group(cell);
            //esm.add_group(wrld);
            
            esm.write_to_file(Config.Paths.skyrim_path + "final.esp");
            
            Log.exit("DONE");            
            
        }
    }
}
