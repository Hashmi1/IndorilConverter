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

namespace Utility
{
    class Binary
    {
        public static byte[] toBin(float a)
        {

            MemoryStream mstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mstream);
            br.Write(a);

            return mstream.ToArray();
        }

        public static byte[] toBin(Int32 a)
        {
            
            MemoryStream mstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mstream);
            br.Write(a);

            return mstream.ToArray();
        }

        public static UInt32 toUInt32(byte[] data)
        {
            if (data.Length != 4)
            {
                Log.error("Utility.Binary.toUInt32, not given 4 bytes");
            }

            MemoryStream mstream = new MemoryStream();
            mstream.Write(data, 0, data.Length);
            mstream.Position = 0;

            return (new BinaryReader(mstream)).ReadUInt32();


        }

        public static byte[] toBin(UInt32 a)
        {

            MemoryStream mstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mstream);
            br.Write(a);

            return mstream.ToArray();
        }

        public static byte[] toBin(Int16 a)
        {

            MemoryStream mstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mstream);
            br.Write(a);

            return mstream.ToArray();
        }

        public static byte[] toBin(UInt16 a)
        {

            MemoryStream mstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mstream);
            br.Write(a);

            return mstream.ToArray();
        }

                
    }
}
