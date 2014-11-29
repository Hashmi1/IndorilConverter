using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TES3
{

    class Record
    {

        public char[] Name;
        public int Size;
        public int Header1;
        public int Flags;
        public List<SubRecord> subRecords = new List<SubRecord>();

        public void read(bool skip = false)
        {

            Name = ESM.input.ReadChars(4);
            Size = ESM.input.ReadInt32();
            Header1 = ESM.input.ReadInt32();
            Flags = ESM.input.ReadInt32();

            if (skip)
            {
                ESM.input.BaseStream.Seek(Size, SeekOrigin.Current);
            }

            else
            {

                int read_size = 0;
                while (read_size < Size)
                {

                    SubRecord subrec = new SubRecord();
                    subrec.read();
                    subRecords.Add(subrec);
                    read_size = read_size + subrec.size + 8;

                }
            }

        }

        public void dump()
        {
            Console.WriteLine(Name);
            Console.WriteLine(Size);
            Console.WriteLine(Header1);
            Console.WriteLine(Flags);

        }


    }

}
