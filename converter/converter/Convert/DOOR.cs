using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

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

        public static void start()
        {

            TES3.ESM.open("tes3\\morrowind.esm");

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
            
            }
            
        }
    }
}
