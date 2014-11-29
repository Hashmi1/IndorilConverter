using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES5
{
    class LAND : Record
    {
        class ATXT : Field
        {
            UInt32 formid;
            byte quad;
            byte unknown_atxt;
            UInt16 layer;

            public void open()
            {
                
                


                if (this.dataSize != 8)
                {
                    Log.error("ATXT field of incorrect size");
                }

                BinaryReader reader = new BinaryReader(new MemoryStream());
                data.CopyTo(reader.BaseStream);
                reader.BaseStream.Position = 0;

                formid = reader.ReadUInt32();
                quad = reader.ReadByte();
                unknown_atxt = reader.ReadByte();
                layer = reader.ReadUInt16();

            }

            public void save()
            {
                data.Position = 0;
                BinaryWriter writer = new BinaryWriter(data);
                writer.Write(formid);
                writer.Write(quad);
                writer.Write(unknown_atxt);
                writer.Write(layer);
                data.Position = 0;
            }

        }

        List<ATXT> textures = new List<ATXT>(); 

        void interpret(Record record)
        {
            foreach (Field field in fields)
            {
                if ("ATXT".Equals(new string(field.type)))
                {
                    //textures.Add(field);
                }
            }
        }
 
    }
}
