using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace Landscape_Module
{
    class Main
    {

        static Texturiser helper = new Texturiser();
        static int mode = 1;

        static void search_record(TES5.Record record)
        {
            if (mode == 1)
            {
                TES5.Field ATXT = null;

                foreach (TES5.Field field in record.fields)
                {
                    if (field.isType("BTXT"))
                    {                        
                       helper.update(x, y, field);
                    }

                    if (field.isType("ATXT"))
                    {
                        
                        ATXT = field;
                    }

                    if (field.isType("VTXT"))
                    {
                        
                        helper.update(x, y, ATXT, field);
                        ATXT = null;
                    }

                }
            }

            if (mode == 2)
            {


                helper.clean(record);
                helper.reconstruct(record, x, y);

            }
            
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
                                Log.info(x + "," + y);
                            }
                        }
                    }

                    if (r.isType("LAND"))
                    {
                        search_record(r);
                        x = 0;
                        y = 0;
                    }
                }
            }


        }

        public static void start()
        {
            TES5.ESM esm = new TES5.ESM("tes5/land2.esp", FileMode.Open);

            esm.read();

            mode = 1;

            foreach (TES5.Group group in esm.groups)
            {
                search_group(group);
            }

            helper.world.blend();

            

            mode = 2;

            foreach (TES5.Group group in esm.groups)
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

            foreach (TES5.Group group in esm.groups)
            {
                group.recalculate_size();
                group.write(bw);
            }

            esm2.close();


        }

    }
}
