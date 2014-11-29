using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace Convert
{
    class STAT
    {
        public static void get_unique_models()
        {
            
            string mesh_path_mor = "d:\\data\\meshes\\";
            string mesh_path_sky = "d:\\data\\morrowind";

            HashSet<string> model_list = new HashSet<string>();
            TextWriter tx = File.CreateText("stats.txt");
            
            int count = 0;
            TES3.ESM.open("tes3/morrowind.esm");
            

            while (TES3.ESM.find("STAT"))
            {
                count++;
                TES3.Record stat = new TES3.Record();
                stat.read();


                foreach (TES3.SubRecord srec in stat.subRecords)
                {
                    if (srec.isType("MODL"))
                    {                        
                        string model = new string(srec.getData().ReadChars(srec.size));
                        model_list.Add(model);                        
                    }
                }


            }

            List<string> modl_lst = model_list.ToList<string>();
            foreach (string modl in modl_lst)
            {
                tx.WriteLine(mesh_path_mor+modl);
                tx.WriteLine(mesh_path_sky + "\\" + modl.Split('\\')[modl.Split('\\').Length-1]);
                
            }

            tx.Close();
            Log.info(count + " STAT found");
        }

        struct STRUCT_STAT
        {
            public string editor_id;
            public string model_path;
        }

        public static TES5.Group start()
        {
            int count = 0;
            TES3.ESM.open("tes3/morrowind.esm");
            
            List<STRUCT_STAT> list_stat = new List<STRUCT_STAT>();

            while (TES3.ESM.find("STAT"))
            {
                TES3.Record stat = new TES3.Record();
                stat.read();

                STRUCT_STAT stat_ = new STRUCT_STAT();

                foreach (TES3.SubRecord srec in stat.subRecords)
                {
                    if (srec.isType("MODL"))
                    {
                        stat_.model_path = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                        stat_.model_path = stat_.model_path.Split('\\')[stat_.model_path.Split('\\').Length - 1];
                        stat_.model_path = "morrowind\\" + stat_.model_path;
                        Log.info(stat_.model_path);
                    }

                    if (srec.isType("NAME"))
                    {
                        stat_.editor_id = Text.trim(new string(srec.getData().ReadChars(srec.size)));                        
                    }

                    
                }

                list_stat.Add(stat_);

            }

            TES3.ESM.close();

            // Make TES5

            count += 1;
            TES5.Group stat_grup = new TES5.Group("STAT");

            foreach (STRUCT_STAT stat in list_stat)
            {
                TES5.Record stat_tes5 = new TES5.Record("STAT", stat.editor_id ,0);
                stat_tes5.addField(new TES5.Field("EDID",Text.editor_id(stat.editor_id)));
                //stat_tes5.addField(new TES5.Field("OBND", new byte[12]));
                stat_tes5.addField(new TES5.Field("MODL", Text.zstring(stat.model_path)));
                
                        MemoryStream mstream = new MemoryStream();
                        BinaryWriter bw = new BinaryWriter(mstream);
                        bw.Write(90f);
                        bw.Write(0);
                stat_tes5.addField(new TES5.Field("DNAM", mstream.ToArray()));


                stat_grup.addRecord(stat_tes5);
                count = +1;
            }

            
            return stat_grup;

        }


    }
}
