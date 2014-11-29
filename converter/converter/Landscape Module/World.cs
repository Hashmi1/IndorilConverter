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
using Utility;

namespace Landscape_Module
{

    class World
    {
        private World()
        {
        }

        static World instance = new World();

        public static World getInstance()
        {
            if (instance == null)
            {
                instance = new World();
            }
            return instance;
        }
             
        public  List<uint> textures_at_point(int x, int y)
        {
            List<uint> all_textures = dict.Keys.ToList<uint>();
            List<uint> output = new List<uint>();

            foreach (uint t in all_textures)
            {
                if (dict[t].get(x, y) > 0f)
                {
                    output.Add(t);
                }
            }

            return output;
        }

        public void blend()
        {
            Opacity_Map[] maps = dict.Values.ToArray();

            int count = 0;
            foreach (Opacity_Map map in maps)
            {
                Log.info("PHASE 1: Blending World texture map: " + ++count + "//" + maps.Length);
                map.blur();
                map.blur();
                map.opacity_map.toBitmap("bmp/" + map.texture + ".bmp");
            }

        }

        public Dictionary<uint, Opacity_Map> dict = new Dictionary<uint, Opacity_Map>();                

        public int toPosX(int cell_x, int quad, int position)
        {
            

            int x = 0;


            int x_ = position / 17;


            if (quad == 0)
            {
                x = (cell_x * (17 + 17)) + x_;

            }

            else if (quad == 2)
            {
                x = (cell_x * (17 + 17)) + 17 + x_;

            }

            else if (quad == 1)
            {
                x = (cell_x * (17 + 17)) + x_;

            }

            else if (quad == 3)
            {
                x = (cell_x * (17 + 17)) + 17 + x_;

            }

            return x;
        }

        public int toPosY(int cell_y, int quad, int position)
        {
            

            int y = 0;

            int y_ = position % 17;

            if (quad == 0)
            {

                y = (cell_y * (17 + 17)) + y_;
            }

            else if (quad == 2)
            {

                y = (cell_y * (17 + 17)) + y_;
            }

            else if (quad == 1)
            {

                y = (cell_y * (17 + 17)) + 17 + y_;
            }

            else if (quad == 3)
            {

                y = (cell_y * (17 + 17)) + 17 + y_;
            }

            return y;
        }

        public void update_quad(int cell_x, int cell_y, int quad)
        {
            
            // get list of textures used here
            Quad quad_ = Quad.get(cell_x,cell_y,quad);
            uint[] textures = dict.Keys.ToArray();
            
            foreach (uint texture in textures)
            {
                Opacity_Map opmap = dict[texture];
                
                for (int pos = 0; pos < 289; pos++)
                {
                    int x = toPosX(cell_y, quad, pos);
                    int y = toPosY(cell_x, quad, pos);

                    float opacity = opmap.get(x, y);

                    if (opacity > 0f && !quad_.hasTexture(texture))
                    {
                        bool added = quad_.addTexture(texture);

                        if (added)
                        {
                            Log.infoX("Added texture to " +quad_.desc());
                        }

                        if (!added)
                        {
                            Log.infoX("NO more room in " + quad_.desc());
                        }
                    }

                }
                
            }

            
        }

        public float get(int cell_x, int cell_y, int quad, int layer, int position)
        {
            uint texture = Quad.get(cell_x,cell_y,quad).getTexture(layer);
            
            int x = toPosX(cell_y,quad,position);
            int y = toPosY(cell_x, quad, position);
            
            return dict[texture].get(x, y);
        }

        public void set(uint texture, int layer, int cell_x, int cell_y, int quad, int position, float opacity)
        {
            if (!(opacity > 0f))
            {                
                return;
            }
                        
            bool added = Quad.get(cell_x, cell_y, quad).addTexture(texture);
            if (!added)
            {                
                Log.error("No more layers available");
            }
            
            int x = toPosX(cell_y, quad, position);
            int y = toPosY(cell_x, quad, position);

            // If base texture exists clear it from this point
            if (Quad.get(cell_x, cell_y, quad).hasBaseTexture())
            {
                uint base_txt = Quad.get(cell_x, cell_y, quad).getBaseTexture();
                set(base_txt, x, y, 0);
            }
            
            set(texture, x, y, opacity);
        }

        public void setBaseTexture(uint texture, int cell_x, int cell_y, int quad)
        {            
            Quad.get(cell_x, cell_y, quad).setBaseTexture(texture);

            for (int pos = 0; pos < 289; pos++)
            {
                int x = toPosX(cell_y, quad, pos);
                int y = toPosY(cell_x, quad, pos);

                set(texture, x, y, 1);
            }

        }

        private void set(uint texture, int x, int y, float opacity)
        {
            if (!dict.ContainsKey(texture))
            {
                dict.Add(texture, new Opacity_Map(texture));
            }

            dict[texture].set(x, y, opacity);
        }
        

    }

}
