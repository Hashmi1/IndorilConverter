using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TES3
{
    class SubRecord
    {
        public char[] name { get; protected set; }
        public int size { get; protected set; }
        //public byte[] data { get; protected set; }
        BinaryReader data;

        public void read()
        {
            name = ESM.input.ReadChars(4);
            size = ESM.input.ReadInt32();
            data = new BinaryReader(new MemoryStream(ESM.input.ReadBytes(size)));

        }

        public void dump()
        {

            Console.WriteLine(name);
            Console.WriteLine(size);
            Console.WriteLine(data.ToString());



        }

        public BinaryReader getData()
        {
            MemoryStream mstream = new MemoryStream();
            data.BaseStream.CopyTo(mstream);
            mstream.Position = 0;
            return new BinaryReader(mstream);
        }

        public bool isType(string type_)
        {
            if (type_.Equals(new string(this.name)))
            {
                return true;
            }

            return false;
        }


    }

}
