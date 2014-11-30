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

            List<TES5.Group> grps = Convert.LAND.convert(Config.Paths.mw_esm);

            TES5.ESM esm = new TES5.ESM(grps);

            esm.write_to_file("tes5\\data\\converted.esp");
            
            



            //TESAnnwyn conv = new TESAnnwyn();
            //conv.convert("tes3\\data\\morrowind.esm", "tes5\\data\\conv.esp");

            
            /*
            TES5.ESM esm = new TES5.ESM("tes5\\data\\conv.esp",FileMode.Open);
            TES5.Group stat_ = Convert.STAT.mw_statics(Config.Paths.mw_esm);
            esm.add_Top_Group(stat_);
            Convert.Exterior_CELL.add_references(esm,"ecells.esp");
            */


            Log.info("DONE");            
            Console.Read();
        }
    }
}
