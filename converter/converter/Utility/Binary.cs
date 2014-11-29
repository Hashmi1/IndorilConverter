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
