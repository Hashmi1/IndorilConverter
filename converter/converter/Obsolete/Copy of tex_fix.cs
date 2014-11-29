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
using System.IO;
using Utility;

namespace Convert
{
    class ph
    {
        //static VTEX_HELPER2 helper = new VTEX_HELPER2();
        static int mode = 1;

        static void search_record(TES5.Record record,int x, int y)
        {
            if (mode == 1)
            {
                
                TES5.Field ATXT = null;

                foreach (TES5.Field field in record.fields)
                {

                    if (field.isType("ATXT"))
                    {
                        ATXT = field;
                    }

                    if (field.isType("VTXT"))
                    {
                       // helper.update(x, y, ATXT, field);
                    }

                }
            }

            if (mode == 2)
            {
                
               // helper.clean(record);
               // helper.reconstruct(record, x, y);

            }
            //helper.blend();
            //helper.clean(record);
            //helper.reconstruct(record);
            
        }

        static int x = 0;
        static int y = 0;
        
        static void search_group(TES5.Group group)
        {
            
            Queue<TES5.Record> q_rec = new Queue<TES5.Record>(group.records);
            Queue<TES5.Group> q_sgrp = new Queue<TES5.Group>(group.subGroups);

            foreach (int t in group.turn)
            {
                if (t == 2)
                {

                    search_group(q_sgrp.Dequeue());

                }

                if (t == 1)
                {
                    TES5.Record r = q_rec.Dequeue();
                    if (r.isType("CELL"))
                    {
                        foreach (TES5.Field fld in r.fields)
                        {
                            if (fld.isType("XCLC"))
                            {
                                BinaryReader br = fld.getData();
                                x = br.ReadInt32();
                                y = br.ReadInt32();

                                Log.info(x + ", " + y);
                                TextWriter tx = File.AppendText("Zloger.txt");
                                tx.WriteLine("Remaking Cell: " + x + " " + y);
                                tx.Close();
                            }
                        }
                    }

                    if (r.isType("LAND"))
                    {
                        search_record(r, x, y);
                    }
                }
            }


        }

        public static void start()
        {
            TES5.ESM esm = new TES5.ESM("tes5/land2.esp", FileMode.Open);

            
            mode = 1;

            List<TES5.Group> groups = esm.read();

            foreach (TES5.Group group in groups)
            {
                search_group(group);
            }

            //World.blend();

            mode = 2;

            
            foreach (TES5.Group group in groups)
            {
                search_group(group);
            }
            

            esm.close();

            TES5.ESM esm2 = new TES5.ESM("tes5/experiment.esp", FileMode.Create);
            BinaryWriter bw = esm2.getWriter();


           // TES5.Group[] grps = Convert.LandTextures.start();
            
            
            TES5.Record.TES4(1).write(bw);
            
            //grps[0].write(bw);
            //grps[1].write(bw);
            
            foreach (TES5.Group group in groups)
            {
                group.recalculate_size();
                group.write(bw);
            }

        }
    }
}
