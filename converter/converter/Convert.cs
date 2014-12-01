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

namespace converter
{
    class cell_name_script
    {
        public static void exterior_locations()
        {
            TES3.ESM.open("tes3/morrowind.esm");
            HashSet<String> cell_list= new HashSet<string>();

            
            while (TES3.ESM.find("CELL"))
            {


                CELL r = new CELL();
                r.read();

                if (!BinaryFlag.isSet(r.data_flags, (int)CELL.CELL_FLAGS.Interior))
                {
                    cell_list.Add(r.cell_name);        
                }

                

            }

            Log.info(cell_list.Count);

            for (int i = 0; i < cell_list.Count; i++)
            {
                Log.info(cell_list.ToList<string>()[i]);

            }

            Log.info("DOne");
        }

        public static void make()
        {

            string[,] name_array = new string[50,50];

            
            TES3.ESM.open("tes3/morrowind.esm");

            int c = 0;
            while (TES3.ESM.find("CELL"))
            {


                CELL r = new CELL();
                r.read();

                if (!BinaryFlag.isSet(r.data_flags, (int)CELL.CELL_FLAGS.Interior))
                {
                    c++;
                    Console.WriteLine("ss: " + (r.cell_name));
                    name_array[r.grid_x, r.grid_y] = r.cell_name;
                }

            }

        }
    }
}
