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
using System.IO;
using Utility;

namespace Convert
{
    class check_cell_tex_count
    {
        

        public static void start()
        {

            TextWriter tx = File.CreateText("alog2.txt");
            TES3.ESM.open("tes3/morrowind.esm");

            while (TES3.ESM.find("LAND"))
            {
                Log.info("Found LAND");
                Record land = new Record();
                land.read();

                int x = 0;
                int y = 0;
                HashSet<short> lst = new HashSet<short>();
                bool vtex_once = true;

                foreach (SubRecord srec in land.subRecords)
                {
                    

                    if ("VTEX".Equals(new string(srec.name)))
                    {

                        if (vtex_once == true)
                        {
                            vtex_once = false;
                        }

                        else
                        {
                            Log.error("NOOOO");
                        }

                        BinaryReader br = srec.getData();

                        for (int i = 0; i < 256;i++)
                        {                                                        
                            short index = br.ReadInt16();
                            lst.Add(index);                            
                            
                        }

                        if (br.BaseStream.Position != br.BaseStream.Length)
                        {
                            Log.error("Incomplete read");
                        }

                        Log.info("Found VTEX");
                    }

                    if (srec.isType("INTV"))
                    {
                        BinaryReader br = srec.getData();
                        x = br.ReadInt32();
                        y = br.ReadInt32();
                    }
                }

                if (lst.Count >= 10)
                {
                    Log.info("Ok now");
                }

                tx.WriteLine(x + "," + y + ":" + lst.Count);
                
            }

            tx.Close();

        }
    }
}
