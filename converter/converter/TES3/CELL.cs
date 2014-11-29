using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES3
{

    class CELL : Record
    {

        public enum CELL_FLAGS : int
        {
            Interior = 0x01,
            Water = 0x02,
            NoSleep = 0x04,
            likeexterior = 0x80

        }

        public bool interior = false;

        public int data_flags;
        public int grid_x;
        public int grid_y;

        public string cell_name = "";

        public List<REFR> references = new List<REFR>();

        public new void read(bool skip = false)
        {

            Name = ESM.input.ReadChars(4);
            Size = ESM.input.ReadInt32();
            Header1 = ESM.input.ReadInt32();
            Flags = ESM.input.ReadInt32();

            if (!"CELL".Equals(new string(Name)))
            {
                Log.error("Error reading CELL, mismatch name");
            }

            int read_size = 0;
            while (read_size < Size)
            {

                SubRecord subrec = new SubRecord();
                subrec.read();
                read_size = read_size + subrec.size + 8;

                // Skipping over cell reference data for now
                if (subrec.isType("FRMR"))
                {
                    //Log.info("FRMR found");
                    REFR refr = new REFR();
                    read_size = read_size + refr.read(Size - read_size);
                    references.Add(refr);

                    continue;

                }

                subRecords.Add(subrec);
                

            }


            for (int i = 0; i < subRecords.Count; i++)
            {
                SubRecord srec = subRecords[i];
                BinaryReader srec_data = srec.getData();
                switch (new string(srec.name))
                {
                    case ("NAME"):
                        if (srec.size > 1)
                        {
                            cell_name = new string(srec_data.ReadChars(srec.size));
                            cell_name = Text.trim(cell_name);
                        }
                        break;
                    case ("RGNN"):
                        if (String.IsNullOrWhiteSpace(cell_name) || String.IsNullOrEmpty(cell_name.Trim()))
                        {
                            cell_name = new string(srec_data.ReadChars(srec.size));
                        }
                        break;
                    case ("DATA"):
                        data_flags = srec_data.ReadInt32();
                        grid_x = srec_data.ReadInt32();
                        grid_y = srec_data.ReadInt32();

                        interior = BinaryFlag.isSet(data_flags,(int)CELL_FLAGS.Interior);
                        break;

                }
            }

        }



    }

}
