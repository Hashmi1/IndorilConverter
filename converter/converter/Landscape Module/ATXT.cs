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
