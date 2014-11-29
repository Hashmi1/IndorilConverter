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
