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
