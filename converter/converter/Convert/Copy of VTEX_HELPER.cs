using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES5;
using System.IO;
using Utility;
/*
namespace Convert
{
    
    
    class VTEX_HELPER2
    {

    
        public void clean(Record land)
        {
            

            List<Field> fields_new = new List<Field>();

            foreach (Field field in land.fields)
            {
                if (!field.isType("ATXT") || !field.isType("VTXT"))
                {
                    fields_new.Add(field);
                }
            }

            land.fields = fields_new;
            
        }

        public void blend()
        {
            Log.info("Blending CELL");
            World.blend();
        }

        

        public void reconstruct(Record land, int cell_x, int cell_y)
        {
            TextWriter tx = File.AppendText("loger.txt");
            tx.WriteLine("Remaking Cell: " + cell_x + " " + cell_y);
            tx.Close();
            Log.info("Remaking Cell: " + cell_x + " " + cell_y);

            for (int quad = 0; quad < CONSTANTS.num_quads; quad++)
            {
                for (int layer = 0; layer < CONSTANTS.num_layers; layer++)
                {
                    if (!World.has_layer[cell_x, cell_y, quad, layer])
                    {
                        continue;
                    }

                    uint texture = World.texture_index[cell_x, cell_y, quad, layer];

                    MemoryStream atxt_mstream = new MemoryStream();
                    BinaryWriter atxt_writer = new BinaryWriter(atxt_mstream);

                    atxt_writer.Write((uint)texture);
                    atxt_writer.Write((byte)quad);
                    atxt_writer.Write((byte)0);
                    atxt_writer.Write((ushort)layer);

                    MemoryStream vtxt_mstream = new MemoryStream();
                    BinaryWriter vtxt_writer = new BinaryWriter(vtxt_mstream);


                    for (int position = 0; position < 289; position++)
                    {

                        float opacity = World.get(cell_x, cell_y, quad, layer, position);

                        if (opacity == 0f)
                        {
                            continue;
                        }

                        if (opacity > 1 || opacity < 0)
                        {
                            Log.error("Opacity out of range");
                        }
                                        
                        vtxt_writer.Write((ushort)position);
                        vtxt_writer.Write((byte)0);
                        vtxt_writer.Write((byte)0);
                        vtxt_writer.Write((float)opacity);
                                                
                    }


                    Field ATXT = new Field("ATXT", atxt_mstream.ToArray());
                    Field VTXT = new Field("VTXT", vtxt_mstream.ToArray());

                    land.addField(ATXT);
                    land.addField(VTXT);                

                }
            }
        }


        public void update(int cell_x, int cell_y, Field ATXT, Field VTXT)
        {
            
            BinaryReader atxt_data = ATXT.getData();
            uint texture = atxt_data.ReadUInt32(); // formid
            int quad = atxt_data.ReadByte(); // quad
            atxt_data.ReadByte(); // unknown
            int layer = atxt_data.ReadUInt16(); // layer

            BinaryReader vtxt_data = VTXT.getData();
            
            int read_data = 0;
            while (read_data < VTXT.dataSize)
            {
                int position = vtxt_data.ReadUInt16(); // position
                vtxt_data.ReadByte(); //unknown
                vtxt_data.ReadByte(); // unknown
                float opacity = vtxt_data.ReadSingle(); //opacity
                read_data = read_data + 8; // update read size

                int x = position / 17; // calculate grid position
                int y = position % 17;

                World.set(texture, layer,cell_x, cell_y, quad, position, opacity);
                
            }
            
        }
    }
     
}
     
*/