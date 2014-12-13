using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES5
{
    class TES4 : Record
    {

        Field HEDR;
        Field INTV;

        List<Field> MAST_ = new List<Field>();
        List <Field> DATA_ = new List<Field>();

        public TES4(int num_records) : base("TES4")
        {
            this.id = 0;
            make_hedr(num_records);
            INTV = new Field("INTV", Binary.toBin((UInt32)0));
        }

        public void add_masters(params string[] files)
        {
            //Log.confirm("WARNING: Adding master file to TES4: Record. It is your responsibility to adjust form_ids with the mod_index");
            
            MAST_ = new List<Field>();
            DATA_ = new List<Field>();

            foreach (string file in files)
            {
                FormID.add_master_file();
                MAST_.Add(new Field("MAST", Text.zstring(file)));
                DATA_.Add(new Field("DATA", new byte[8]));
            }
        }
        
        private void  make_hedr(int num_records)
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter b_writer = new BinaryWriter(mstream);

            b_writer.Write(1.7f);
            b_writer.Write(num_records);
            b_writer.Write(FormID.getHEDRObj());

            HEDR = new Field("HEDR", mstream.ToArray());
        }

        public new void write(BinaryWriter br)
        {
            
            fields = new List<Field>();
            fields.Add(HEDR);

            for (int i = 0; i < MAST_.Count; i++)
            {
                fields.Add(MAST_[i]);
                fields.Add(DATA_[i]);
            }

            fields.Add(INTV);

            base.write(br);
        }

        public new void read(BinaryReader br, bool unpack = true)
        {
            base.read(br);

            foreach (Field f in fields)
            {

                if (f.isType("MAST"))
                {
                    MAST_.Add(f);
                }

                else if (f.isType("DATA"))
                {
                    DATA_.Add(f);
                }

                else if (f.isType("INTV"))
                {
                    INTV = f;
                }

                else if (f.isType("HEDR"))
                {
                    HEDR = f;
                }


            }
            

            
        }
        
    }
}
