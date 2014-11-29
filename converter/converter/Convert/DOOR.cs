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
using Utility;
using converter;

namespace Convert
{
    class DOOR
    {
        struct door_
        {
            public string id;
            public string full_name;
            public string model_path;
            public string open_sound;
            public string close_sound;
        }

        public static List<door_> get_tes3_doors()
        {

            TES3.ESM.open("tes3\\morrowind.esm");
            
            List<door_> door_list = new List<door_>();

            while (TES3.ESM.find("DOOR"))
            {
                door_ d = new door_();

                TES3.Record door = new TES3.Record();
                door.read();

                foreach (TES3.SubRecord srec in door.subRecords)
                {

                    if (srec.isType("NAME"))
                    {
                        d.id = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }


                    if (srec.isType("FNAM"))
                    {
                        d.full_name = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    if (srec.isType("MODL"))
                    {
                        d.model_path = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    if (srec.isType("NAME"))
                    {
                        d.id = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    
                }

                door_list.Add(d);
            
            }

            return dor_list;
        }

        public static TES5.Group make_tes5_doors(List<door_> lst)
        {
            
            TES5.Group grup = new TES5.Group("DOOR");

            foreach (door_ d in lst)
            {

                TES5.Record r = new TES5.Record("DOOR");
                r.addField(new TES5.Field("EDID",Text.zstring(d.id)));
                r.addField(new TES5.Field("FULL",Text.zstring(d.full_name)));
                r.addField(new TES5.Field("MODL",Text.zstring(d.model_path)));

                grup.addRecord(r);
            }

            return grup;

        }

        public static void convert_models(List<door_> lst, string input_path, string output_path)
        {
            //Config.Paths.mw_meshes + 
        }


    }
}
