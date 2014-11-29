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
using Utility;

namespace ConvertX
{

    class CONSTANTS
    {
        public static int num_layers = 8;
        public static int num_quads = 4;
    }

    class Layer_Opacity_Map
    {
        float[,] opacity_map;
        public Layer_Opacity_Map()
        {
            opacity_map = new float[17, 17];
        }

        public void blend()
        {
            float weight = 1f;
            float[,] blurred_map = new float[17,17];
            
            for (int x = 1; x < 16; x++)
            {
                for (int y = 1; y < 16; y++)
                {
                    float new_opacity = weight * opacity_map[x, y] + weight * opacity_map[x - 1, y] +
                                        weight * opacity_map[x, y - 1] + weight * opacity_map[x - 1, y - 1] +
                                        weight * opacity_map[x + 1, y] + weight * opacity_map[x, y + 1] +
                                        weight * opacity_map[x + 1, y + 1] + weight * opacity_map[x + 1, y - 1] + weight * opacity_map[x - 1, y + 1];
                    new_opacity = new_opacity / 9f;

                    

                    blurred_map[x, y] = new_opacity;
                }
            }

            // borders
            for (int x = 1; x < 16; x++)
            {
                int y = 0;
                float new_opacity =  opacity_map[x, y] +  opacity_map[x - 1, y] +
                                         +  opacity_map[x + 1, y] +  opacity_map[x, y + 1] +
                                            opacity_map[x + 1, y + 1] +  opacity_map[x - 1, y + 1];
                new_opacity = new_opacity / 6f;
                //blurred_map[x, y] = new_opacity;

                y = 16;
                new_opacity =  opacity_map[x, y] +  opacity_map[x - 1, y] +
                               opacity_map[x, y - 1] +  opacity_map[x - 1, y - 1] +
                               opacity_map[x + 1, y] +  opacity_map[x + 1, y - 1];

                new_opacity = new_opacity / 6f;
                //blurred_map[x, y] = new_opacity;
            }

            for (int y = 1; y < 16; y++)
            {
                int x = 0;
                float new_opacity = opacity_map[x, y]+
                                         opacity_map[x, y - 1]+
                                         opacity_map[x + 1, y] + opacity_map[x, y + 1] +
                                         opacity_map[x + 1, y + 1] + opacity_map[x + 1, y - 1];

                new_opacity = new_opacity / 6f;
                //blurred_map[x, y] = new_opacity;

                x = 16;
                new_opacity = opacity_map[x, y] + opacity_map[x - 1, y] +
                                         opacity_map[x, y - 1] + opacity_map[x - 1, y - 1] +
                                         opacity_map[x, y + 1] +
                                         opacity_map[x - 1, y + 1];
                new_opacity = new_opacity / 6f;
                //blurred_map[x, y] = new_opacity;
            }

            // corners
            float new_opacity_ = opacity_map[0, 0] + opacity_map[1, 0] + opacity_map[0,1] + opacity_map[1, 1];
            new_opacity_ = new_opacity_ / 4f;
            //blurred_map[0, 0] = new_opacity_;

            new_opacity_ = opacity_map[16, 0] + opacity_map[15, 0] + opacity_map[16, 1] + opacity_map[15, 1];
            new_opacity_ = new_opacity_ / 4f;
            //blurred_map[16, 0] = new_opacity_;

            new_opacity_ = opacity_map[0, 16] + opacity_map[1,16] + opacity_map[1, 15] + opacity_map[0, 15];
            new_opacity_ = new_opacity_ / 4f;
            //blurred_map[0, 16] = new_opacity_;

            new_opacity_ = opacity_map[16, 16] + opacity_map[16, 15] + opacity_map[15, 15] + opacity_map[15, 16];
            new_opacity_ = new_opacity_ / 4f;
            //blurred_map[16, 16] = new_opacity_;


            opacity_map = blurred_map;


        }

        public void set(int x, int y,float opacity)
        {
            opacity_map[x, y] = opacity;
        }

        public float get(int x, int y)
        {
            return opacity_map[x, y];
        }

        public float coverage()
        {
            float coverage_ = 0f;
            for (int x = 0; x < 17; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    coverage_ = coverage_ + (float)(opacity_map[x, y])/289f;
                }
            }

            return coverage_;
        }

    }

    class Quad_Opacity_Map
    {
        Layer_Opacity_Map[] opacity_map;

        public Quad_Opacity_Map()
        {
            opacity_map = new Layer_Opacity_Map[CONSTANTS.num_layers];
            
            for (int i = 0; i < CONSTANTS.num_layers; i++)
            {
                opacity_map[i] = new Layer_Opacity_Map();
            }
            
        }

        public void set(int layer, int x, int y, float opacity)
        {
            
            opacity_map[layer].set(x, y, opacity);
        }

        public float get(int layer, int x, int y)
        {
            return opacity_map[layer].get(x, y);
        }

        public void blend()
        {
            for (int i = 0; i < CONSTANTS.num_layers; i++)
            {
                opacity_map[i].blend();
            }
        }

    }

    class Cell_Opacity_Map
    {
        public uint[,] texture { get; private set; }
        public bool[,] layer_present { get; private set; }
        Quad_Opacity_Map[] opacity_map;

        public Cell_Opacity_Map()
        {
            opacity_map = new Quad_Opacity_Map[CONSTANTS.num_quads];

            for (int i = 0; i < CONSTANTS.num_quads; i++)
            {
                opacity_map[i] = new Quad_Opacity_Map();
            }

            layer_present = new bool[CONSTANTS.num_quads, CONSTANTS.num_layers];
            texture = new uint[CONSTANTS.num_quads, CONSTANTS.num_layers];
        }

        public void set(uint formid, int quad, int layer, int x, int y, float opacity)
        {
            if (opacity > 0f)
            {
                layer_present[quad, layer] = true;
            }

            texture[quad, layer] = formid;
            opacity_map[quad].set(layer, x, y, opacity);
        }

        public float get(int quad, int layer, int x, int y)
        {            
            return opacity_map[quad].get(layer, x, y);
        }

        public void blend()
        {
            for (int i = 0; i < CONSTANTS.num_quads; i++)
            {
                opacity_map[i].blend();
            }
        }
    }

    class VTEX_HELPER
    {

        Cell_Opacity_Map cmap = new Cell_Opacity_Map();

        public void blend()
        {
            cmap.blend();
        }

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

        public void reconstruct(Record land)
        {

            Log.info("Fixing LAND REC");

            for (int quad = 0; quad < CONSTANTS.num_quads; quad++)
            {
                for (int layer = 0; layer < CONSTANTS.num_layers; layer++)
                {
                    if (cmap.layer_present[quad, layer])
                    {
                        uint formid = cmap.texture[quad, layer];// +0x1000000;

                        MemoryStream atxt_mstream = new MemoryStream();
                        BinaryWriter atxt_writer = new BinaryWriter(atxt_mstream);

                        atxt_writer.Write((uint)formid);
                        atxt_writer.Write((byte)quad);
                        atxt_writer.Write((byte)0);
                        atxt_writer.Write((ushort)layer);


                        MemoryStream vtxt_mstream = new MemoryStream();
                        BinaryWriter vtxt_writer = new BinaryWriter(vtxt_mstream);

                        for (int x = 0; x < 17; x++)
                        {
                            for (int y = 0; y < 17; y++)
                            {
                                ushort position = (ushort)(x * 17 + y);
                                byte unknown = 0;
                                float opacity = cmap.get(quad, layer, x, y);

                                vtxt_writer.Write(position);
                                vtxt_writer.Write(unknown);
                                vtxt_writer.Write(unknown);
                                vtxt_writer.Write(opacity);

                            }
                        }
                        
                        Field ATXT = new Field("ATXT", atxt_mstream.ToArray());
                        Field VTXT = new Field("VTXT", vtxt_mstream.ToArray());

                        land.addField(ATXT);
                        land.addField(VTXT);                        
            
                    }
                }
            }

            //land.recalculate_size();
        }

        public void update(Field ATXT, Field VTXT)
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

                cmap.set(texture,quad, layer, x, y, opacity); // update opacity map
            }
            
        }
    }
}
