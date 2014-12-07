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
            data.BaseStream.Position = 0;
            data.BaseStream.CopyTo(mstream);
            mstream.Position = 0;
            data.BaseStream.Position = 0;
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
