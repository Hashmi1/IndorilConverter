using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES5;
using System.IO;

namespace Landscape_Module
{
    // obsolete
    class ATXT
    {
        public uint texture {get;private set;}
        public byte quad { get; private set; }
        public ushort layer { get; private set; }

        private ATXT()
        {
        }

        public static ATXT fromField(Field f)
        {
            ATXT me = new ATXT();
            BinaryReader br = f.getData();
            me.texture = br.ReadUInt32();
            me.quad = br.ReadByte();
            br.ReadByte();// unknown
            me.layer = br.ReadUInt16();

            return me;

        }
    }
}
