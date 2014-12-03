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
            if (File.Exists("Landscape-Log.txt"))
            {
                File.Delete("Landscape-Log.txt");
            }
                        
            TES5.Group[] ltex = Convert.LTEX.convert(Config.Paths.mw_esm);
            //TES5.Group door = Convert.DOOR.convert(Config.Paths.mw_esm);
            TES5.Group stat = Convert.STAT.convert(Config.Paths.mw_esm);
            TES5.Group cell = Convert.CELL.convert(Config.Paths.mw_esm);
            List<TES5.Group> wrld = Convert.LAND.convert(Config.Paths.mw_esm);

            List<TES5.Group> cell_grp = new List<TES5.Group>();
            cell_grp.Add(cell);

            Convert.REFERENCE.REFR.add_references(Config.Paths.mw_esm,cell_grp,wrld);
            
            TES5.ESM esm = new TES5.ESM();
            
            esm.add_group(ltex[0]);
            esm.add_group(ltex[1]);
            esm.add_group(stat);
            esm.add_group(cell);
            esm.add_group(wrld);
            
            esm.write_to_file(Config.Paths.skyrim_path + "final.esp");
            
            Log.info("DONE");            
            Console.Read();
        }
    }
}
