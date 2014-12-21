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

namespace NavMesh
{
    class StandAlone
    {
        static void aMain(string[] args)
        {
            
            Log lg = new Log("testy.txt");

            ESM eraw = ESM.read_from_file(Config.Paths.tmp + "dragonborn.esm");

            ESM_Index esm = new ESM_Index();
            esm.make(eraw);

            foreach (Record r in esm.get_record_list("NAVM"))
            {

                Field f = r.try_find_field("NVNM");

                if (f == null)
                {
                    Log.confirm("NAVM with no NVNM");
                    continue;
                }

                NVNM nvnm = new NVNM(f);
                //nvnm.read(f);

               // lg.log(nvnm.vertices.Count + "," + nvnm.triangles.Count + "," + nvnm.maxXD + "," + nvnm.maxYD +"," + nvnm.divisor);
            }

            lg.show();
             
            
        }

        static void Main(string[] args)
        {

            ESM esm_raw1 = ESM.read_from_file("tmp\\skyrim.esm");
            Log.exit("DONE");

                ESM esm_raw = ESM.read_from_file(Config.Paths.skyrim_path + "final.esp");

                Generator generator = new Generator();
                generator.load_file(esm_raw);

                generator.make_navmesh();


                esm_raw.write_to_file("navmeshtest.esp");
                Log.exit("DONE");
            
            
            
        }
    }
}
