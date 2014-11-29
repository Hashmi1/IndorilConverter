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
using TES5;

namespace Landscape_Module
{
    // Obsolete
    class VTXT
    {
        MemoryStream mstream = null;
        BinaryWriter writer = null;


        public VTXT()
        {
            mstream = new MemoryStream();
            writer = new BinaryWriter(mstream);
        }

        public int size()
        {
            return ((int)mstream.Length);
        }

        Dictionary<ushort, float> dict = new Dictionary<ushort, float>();

        public void add_point(ushort position, float opacity)
        {
            dict.Add(position, opacity);

            writer.Write((ushort)position);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((float)opacity);
        }

        public void replace(Field f)
        {
            if (!f.isType("VTXT"))
            {
                Utility.Log.error("Given Field is not VTXT");
            }

            
            byte[] new_data = mstream.ToArray();
            
            f.replaceData(new_data);
        }
    }
}
