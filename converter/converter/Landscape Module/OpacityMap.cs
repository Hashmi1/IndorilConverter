using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Landscape_Module
{

    class Opacity_Map
    {

        public SparseArray opacity_map = new SparseArray();
        public uint texture;

        public Opacity_Map(uint texture)
        {
            this.texture = texture;
        }

        public void blur()
        {
            SparseArray blurred_map = new SparseArray();

            IEnumerable<Tuple<int, int>> points = opacity_map.knownPoints();

            foreach (Tuple<int, int> p in points)
            {

                int x = p.Item1;
                int y = p.Item2;

                float new_opacity = get(x, y) + get(x - 1, y) +
                                     get(x, y - 1) + get(x - 1, y - 1) +
                                     get(x + 1, y) + get(x, y + 1) +
                                     get(x + 1, y + 1) + get(x + 1, y - 1) + get(x - 1, y + 1);
                new_opacity = new_opacity / 9f;
                
                List<uint> other_layers = World.getInstance().textures_at_point(x, y);
                
                //float sum = 0;
                //sum = sum + new_opacity;

                foreach (uint t in other_layers)
                {
                    float old_opacity = World.getInstance().dict[t].get(x, y);
                    World.getInstance().dict[t].set(x, y, old_opacity * (1 - new_opacity));
                    //sum = sum + old_opacity;
                }

                

                blurred_map.put2(x, y, new_opacity);
            }

            this.opacity_map = blurred_map;
        }

        private void blur2()
            
        {

            Log.error("OBSOLETE");

            SparseArray blurred_map = new SparseArray();

            for (int y = 30 * 34; y < 63 * 34; y++)
            {
                for (int x = 64 * 34; x < 87 * 34; x++)
                {

                    float new_opacity = get(x, y) + get(x - 1, y) +
                                         get(x, y - 1) + get(x - 1, y - 1) +
                                         get(x + 1, y) + get(x, y + 1) +
                                         get(x + 1, y + 1) + get(x + 1, y - 1) + get(x - 1, y + 1);
                    new_opacity = new_opacity / 9f;
                    
                    //int opacity = (int)(new_opacity * 255f);
                    //new_opacity = (((float)opacity) / 255f);

                    List<uint> oth_layers = World.getInstance().textures_at_point(x, y);

                    float sum = 0;
                    foreach (uint t in oth_layers)
                    {
                        if (t == this.texture)
                        {
                            continue;
                        }

                        sum = sum + World.getInstance().dict[t].get(x, y);
                    }

                    sum = sum + new_opacity;

                    if (sum != 0)
                    {

                        foreach (uint t in oth_layers)
                        {
                            if (t == this.texture)
                            {
                                continue;
                            }

                            float op_other = World.getInstance().dict[t].get(x, y) / sum;
                            World.getInstance().dict[t].set(x, y, op_other);
                        }

                        new_opacity = new_opacity / sum;
                    }
                    blurred_map.put2(x, y, new_opacity);
                    
                }
            }

            opacity_map = blurred_map;
            
        }

        public void set(int x, int y, float opacity)
        {
            opacity_map.put2(x, y, opacity);
        }

        public float get(int x, int y)
        {
            return opacity_map.get(x, y);
        }

        private float coverage()
        {
            Log.error("NOT IMPLEMENTED coverage");
            float coverage_ = 0f;
            for (int x = 0; x < 17; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    //coverage_ = coverage_ + (float)(opacity_map[x, y])/289f;
                }
            }

            return coverage_;
        }

    }

}
