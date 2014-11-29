using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TES3
{

    class REGN : Record
    {
        public string editor_id;
        public string region_name;

        public void read()
        {
            base.read();

            for (int i = 0; i < subRecords.Count; i++)
            {
                SubRecord srec = subRecords[i];
                BinaryReader srec_data = srec.getData();
                switch (new string(srec.name))
                {
                    case ("NAME"):
                        editor_id = new string(srec_data.ReadChars(srec.size));
                        break;
                    case ("FNAM"):
                        region_name = new string(srec_data.ReadChars(srec.size));
                        break;
                }
            }
        }
    }

}
