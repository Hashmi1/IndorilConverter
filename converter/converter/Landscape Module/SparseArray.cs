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
using System.Drawing;
using System.IO;

namespace Landscape_Module
{

    class SparseArray
    {
        Dictionary<Tuple<int, int>, float> array = new Dictionary<Tuple<int, int>, float>();

        public IEnumerable<Tuple<int, int>> knownPoints()
        {
            Tuple<int, int>[] a = array.Keys.ToArray();
            IEnumerable<Tuple<int,int>> outer = a.OrderBy(x => x.Item1).ThenBy( x => x.Item2);
            return outer;
        }

        public void put2(int x, int y, float opacity)
        {
            if (x % 17 == 16 && y % 17 == 16)
            {
                put(x + 1, y , opacity);
                put(x, y + 1, opacity);
            }
            else if (x % 17 == 16)
            {
                put(x + 1, y, opacity);
            }
            else if (y % 17 == 16)
            {
                put(x , y + 1, opacity);
            }


            if (x % 17 == 0 && y % 17 == 0)
            {
                put(x - 1, y, opacity);
                put(x, y - 1, opacity);
            }
            else if (x % 17 == 0)
            {
                put(x - 1, y, opacity);
            }
            else if (y % 17 == 0)
            {
                put(x, y - 1, opacity);
            }


            put(x, y, opacity);

        }

        private void update_neighbours(int x, int y)
        {
            Tuple<int,int>[] n_ = new Tuple<int,int>[]{

            new Tuple<int, int>(x-1, y),
            new Tuple<int, int>(x, y-1),
            new Tuple<int, int>(x, y+1),
            new Tuple<int, int>(x+1, y),
            new Tuple<int, int>(x+1, y+1),
            new Tuple<int, int>(x-1, y-1),
            new Tuple<int, int>(x-1, y+1),
            new Tuple<int, int>(x+1, y-1)

            };

            foreach (Tuple<int, int> t in n_)
            {

                if (!array.ContainsKey(t))
                {
                    array.Add(t, 0);
                }

            }

        }

        private void put(int x, int y, float opacity)
        {
            update_neighbours(x, y);
                        
            if (opacity == 0f)
            {
                if (array.ContainsKey(new Tuple<int, int>(x, y)))
                {
                    //array.Remove(new Tuple<int, int>(x, y));
                }

                //return;
            }
            
            if (array.ContainsKey(new Tuple<int, int>(x, y)))
            {
                                
                if (array[new Tuple<int, int>(x, y)] != opacity)
                {                    
                    array[new Tuple<int, int>(x, y)] = opacity;
                }

                return;
            }

            array.Add(new Tuple<int, int>(x, y), opacity);
        }

        public float get(int x, int y)
        {
            if (!array.ContainsKey(new Tuple<int, int>(x, y)))
            {
                return 0;
            }

            return array[new Tuple<int, int>(x, y)];
        }

        public void toBitmap(string file)
        {
            Bitmap bmp = new Bitmap(3400,3400);

            foreach (Tuple<int, int> k in array.Keys)
            {
                int x = k.Item1;
                int y = k.Item2;

                float opacity = get(x, y);
                
                if (opacity == 0)
                {
                    continue;
                }

                int inten = (int)(opacity * 255f);
                bmp.SetPixel(x,y,Color.FromArgb(0,0,inten));

            }

            bmp.Save(file);
        }
    }

}
