using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES5
{
    class StreamableData
    {
        FileStream base_stream;
        int offset;
        int size;

        public byte[] get_data()
        {
            byte[] ret = new byte[size];
            base_stream.Read(ret, offset, size);
            return ret;
        }

        public static StreamableData read(FileStream s, int size)
        {
            StreamableData sd = new StreamableData();
            sd.base_stream = s;
            sd.offset = (int)s.Position;
            sd.size = size;

            s.Read(new byte[size],0,size);

            return sd;
        }

    }
}
