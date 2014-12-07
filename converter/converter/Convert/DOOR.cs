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
            //public string open_sound;
            //public string close_sound;
        }

        static public UInt32 marker_id{get; private set;}

        static public TES5.Group convert(string file)
        {
            return make_tes5_doors(get_tes3_doors(file));
        }

        static List<door_> get_tes3_doors(string file)
        {

            TES3.ESM.open(file);
            
            List<door_> door_list = new List<door_>();

            while (TES3.ESM.find("DOOR"))
            {
                

                TES3.Record door = new TES3.Record();
                door.read();

                if (!new string(door.Name).Equals("DOOR"))
                {
                    Log.error("Read non-door record");
                }

                door_ d = new door_();

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
                    
                    
                }

                if (String.IsNullOrEmpty(d.full_name))
                {
                    d.full_name = "Door";
                }

                door_list.Add(d);
            
            }

            TES3.ESM.close();
            return door_list;
        }
        
        static TES5.Group make_tes5_doors(List<door_> lst)
        {
            
            TES5.Group grup = new TES5.Group("DOOR");

            TES5.Record marker = new TES5.Record("DOOR");
            marker.addField(new TES5.Field("EDID", Text.editor_id("teleport_marker")));

            marker_id = marker.id;
            grup.addRecord(marker);

            foreach (door_ d in lst)
            {
                // Make Normal version
                TES5.Record r = new TES5.Record("DOOR",d.id);
                r.addField(new TES5.Field("EDID",Text.editor_id(d.id)));
                r.addField(new TES5.Field("FULL",Text.zstring(d.full_name)));
                r.addField(new TES5.Field("MODL",Text.model_path(d.model_path)));

                // Make Portal version
                TES5.Record r_load = new TES5.Record("DOOR",d.id+"_zload");
                r_load.addField(new TES5.Field("EDID", Text.editor_id(d.id+"_zload")));
                r_load.addField(new TES5.Field("FULL", Text.zstring(d.full_name)));
                r_load.addField(new TES5.Field("MODL", Text.model_path(d.model_path.ToLower().Replace(".nif", "_zload.nif"))));
                
                grup.addRecord(r);
                grup.addRecord(r_load);

                ModelConverter.convert(d.model_path, "door_load");
                ModelConverter.convert(d.model_path, "door_anim");

            }

            return grup;

        }

        

        
    }
}
