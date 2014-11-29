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
