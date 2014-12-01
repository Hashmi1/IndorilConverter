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

namespace Landscape_Module
{

    class Quad
    {
        int x;
        int y;
        int quad;

        uint base_texture;
        List<uint> texture_layers = new List<uint>();
        static Dictionary<Tuple<int, int, int>, Quad> dict = new Dictionary<Tuple<int, int, int>, Quad>();

        public string desc()
        {
            return "Cell " + x + "," + y + " quad " + quad;
        }

        public static Quad get(int x, int y, int quad)
        {
            Tuple<int, int, int> q = new Tuple<int, int, int>(x, y, quad);

            if (dict.ContainsKey(q))
            {
                return dict[q];
            }

            Quad q_new = new Quad(x, y, quad);
            dict.Add(q,q_new);
            return q_new;
        }

        Quad(int x, int y, int quad)
        {
            this.x = x;
            this.y = y;
            this.quad = quad;
            this.base_texture = UInt32.MaxValue;        
        }

        public void setBaseTexture(uint t)
        {
            this.base_texture = t;
            addTexture(t);
        }

        public uint getBaseTexture()
        {
            if (base_texture == UInt32.MaxValue)
            {
                Utility.Log.error("QUAD asked for base texture, which does not exist");
            }

            return this.base_texture;
        }


        public bool hasBaseTexture()
        {
            return (base_texture != UInt32.MaxValue);
        }

        public bool hasLayer(int layer)
        {
            return (texture_layers.Count > layer);
        }

        public uint getTexture(int layer)
        {
            return texture_layers[layer];
        }

        public bool hasTexture(uint texture)
        {
            return texture_layers.Contains(texture);
        }

        public bool addTexture(uint texture)
        {
            if (texture_layers.Contains(texture))
            {
                return true;
            }

            else if (texture_layers.Count < CONSTANTS.num_layers)
            {
                texture_layers.Add(texture);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
