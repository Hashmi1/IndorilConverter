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
using Utility;

namespace TES3
{

    class Record
    {
        Dictionary<string, List<SubRecord>> subrecord_index = new Dictionary<string, List<SubRecord>>();

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

                    string srec_name = Text.trim(new string(subrec.name));
                    if (!subrecord_index.ContainsKey(srec_name))
                    {
                        subrecord_index.Add(Text.trim(new string(subrec.name)), new List<SubRecord>());
                    }
                    subrecord_index[srec_name].Add(subrec);
                    

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

        public SubRecord find_first(string srec_name, bool acceptNULL=false)
        {            
            if (!subrecord_index.ContainsKey(srec_name) || subrecord_index[srec_name].Count == 0)
            {
                if (acceptNULL)
                {
                    return null;
                }
                Log.error("Subrecord " + srec_name + " not found in " +new string(this.Name));
            }

            return subrecord_index[srec_name][0];
        }

        public List<SubRecord> find_all(string srec_name, bool AcceptNULL=false)
        {
            if (!subrecord_index.ContainsKey(srec_name) || subrecord_index[srec_name].Count == 0)
            {
                if (AcceptNULL == true)
                {
                    return new List<SubRecord>();
                }
                Log.error("Subrecord " + srec_name + " not found in " + new string(this.Name));
            }

            return subrecord_index[srec_name];
        }
       

    }

}
