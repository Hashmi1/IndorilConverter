using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utility
{
    class Binary
    {

        public static byte[] toBin(Int32 a)
        {
            
            MemoryStream mstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mstream);
            br.Write(a);

            return mstream.ToArray();
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
