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
using Utility;
using System.IO;

namespace TES5
{
    // TODO: ownership / scale / exterior cell / door /lock etc.
    class REFR : Record
    {
        struct placement
        {
            float x;
            float y;
            float z;
            float xR;
            float yR;
            float zR;
            

            public placement(float x, float y, float z, float xR, float yR, float zR)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.xR = xR;
                this.yR = yR;
                this.zR = zR;
                
            }

            public byte[] toBin()
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter br = new BinaryWriter(mstream);
                br.Write(x);
                br.Write(y);
                br.Write(z);
                br.Write(xR);
                br.Write(yR);
                br.Write(zR);
                return mstream.ToArray();
                
            }

            struct teleport_data
            {
                float x;
                float y;
                float z;
                float xR;
                float yR;
                float zR;

                UInt32 destination_formid;
            }

        }

        public REFR(uint formid,float x, float y, float z, float xR, float yR, float zR,float scale = 1f) : base("REFR")
        {
            //TES3.REFR refr;
            
            Field NAME = new Field("NAME", Binary.toBin(formid));
            Field DATA = new Field("DATA",new placement(x, y, z, xR, yR, zR).toBin());

            
            fields.Add(NAME);
            if (scale != 1f)
            {
                Field XSCL = new Field("XSCL", Binary.toBin(scale));
                fields.Add(XSCL);
            }

            fields.Add(DATA);
        }

    }
}
