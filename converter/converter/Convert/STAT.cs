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
using System.IO;
using Utility;


namespace Convert
{
    class STAT
    {
        
        struct STRUCT_STAT
        {
            public string editor_id;
            public string model_path;
        }

        public static TES5.Group convert(string file_path)
        {
            int count = 0;
            TES3.ESM.open(file_path);
            
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
                        //stat_.model_path = stat_.model_path.Split('\\')[stat_.model_path.Split('\\').Length - 1];
                        //stat_.model_path = "morrowind\\" + stat_.model_path;
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
                stat_tes5.addField(new TES5.Field("MODL", Text.model_path(stat.model_path)));
                
                        MemoryStream mstream = new MemoryStream();
                        BinaryWriter bw = new BinaryWriter(mstream);
                        bw.Write(90f);
                        bw.Write(0);

                stat_tes5.addField(new TES5.Field("DNAM", mstream.ToArray()));


                stat_grup.addRecord(stat_tes5);
                count = +1;

               // ModelConverter.convert(stat.model_path,"stat",true);
            }

            
            return stat_grup;

        }


    }
}
