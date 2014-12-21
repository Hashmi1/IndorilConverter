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
using System.IO;
using TES5;
using Utility;

namespace Landscape_Module
{
    class Texturiser
    {
        public World world = World.getInstance();
        
        public void reconstruct(Record land, int cell_x, int cell_y)
        {            
            Log.info("Re-Assembling cell " + cell_x + "," + cell_y);            
            
            for (int quad = 0; quad < CONSTANTS.num_quads; quad++)
            {
                world.update_quad(cell_x, cell_y, quad);

                Quad quad_ = Quad.get(cell_x, cell_y, quad);
                
                for (int layer = 0; layer < CONSTANTS.num_layers; layer++)
                {                
                    if (!quad_.hasLayer(layer))
                    {
                        continue;
                    }

                    uint texture = quad_.getTexture(layer);

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

                        float opacity = world.get(cell_x, cell_y, quad, layer, position);

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

        public void update(int cell_x, int cell_y, Field BTXT)
        {            

            if (!BTXT.isType("BTXT"))
            {
                Log.error("Texturiser.Update() Given Field is not BTXT");
            }

            BinaryReader btxt_data = BTXT.getData();
            uint texture = btxt_data.ReadUInt32();
            byte quad = btxt_data.ReadByte();

            world.setBaseTexture(texture, cell_x, cell_y, quad);
        }

        public void addBTXT(Record r)
        {
            for (byte quad = 0; quad < 4; quad++)
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter br = new BinaryWriter(mstream);
                
                br.Write((UInt32)0x833);
                br.Write((byte)quad);
                br.Write(new byte[3]);

                Field btxt = new Field("BTXT", mstream.ToArray());
               // r.addField(btxt);
            }
        }

        // Removes all texture related fields from the given land record
        public void clean(Record r)
        {

            List<Field> fields_new = new List<Field>();

            foreach (Field f in r.fields)
            {
                if (f.isType("BTXT") || f.isType("ATXT") || f.isType("VTXT"))
                {
                    continue;
                }

                fields_new.Add(f);
            }

            r.fields = fields_new;

            addBTXT(r);
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
            while (read_data < VTXT.data_size())
            {
                int position = vtxt_data.ReadUInt16(); // position
                vtxt_data.ReadByte(); //unknown
                vtxt_data.ReadByte(); // unknown
                float opacity = vtxt_data.ReadSingle(); //opacity
                read_data = read_data + 8; // update read size

                world.set(texture, layer, cell_x, cell_y, quad, position, opacity);

            }

        }
    }
}
