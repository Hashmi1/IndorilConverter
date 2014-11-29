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

        }

        public REFR(uint formid,float x, float y, float z, float xR, float yR, float zR) : base("REFR")
        {
            
            Field NAME = new Field("NAME", Binary.toBin(formid));
            Field DATA = new Field("DATA",new placement(x, y, z, xR, yR, zR).toBin());
            fields.Add(NAME);
            fields.Add(DATA);
        }

    }
}
