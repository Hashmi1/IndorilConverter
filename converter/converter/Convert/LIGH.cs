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
using Utility;

namespace Convert
{
    
    class LIGH
    {
        public static TES5.Group convert(string file)
        {

            TES5.Group LIGH_GRP = new TES5.Group("LIGH");
            TES3.ESM.open(file);

            while (TES3.ESM.find("LIGH"))
            {
                TES3.LIGH l3 = new TES3.LIGH();
                l3.read();

                TES5.LIGH l = new TES5.LIGH(l3.editor_id);
                TES5.LIGH l_shadow = new TES5.LIGH(l3.editor_id + "_shadow");
                
                TES5.LIGH[] ls = new TES5.LIGH[2]{l,l_shadow};

                for (int i = 0; i < 2; i++)
                {
                    TES5.LIGH l5 = ls[i];
                                    
                    l5.model = l3.model;

                    l5.Radius = l3.radius; // TODO: Better Replacement?

                    l5.Time = (int)l3.time;
                    l5.Value = l3.value;
                    l5.r = l3.red;
                    l5.g = l3.green;
                    l5.b = l3.blue;
                    l5.Weight = l3.weight;

                    // l5.Dynamic = l3.Dynamic; // Not in CK?
                    l5.carried = l3.Can_Carry;

                    l5.Flicker = l3.Flicker;
                    //l5.FlickerSlow = l3.Flicker_Slow; // FlickerSlow not in CK
                    l5.Pulse = l3.Pulse;

                    if (l3.model == null)
                    {
                        // if no model then make it shadow-less
                    }
                    else
                    {
                        //l5.Omnidirectional = true; // if model present make it shadow casting omni-directinal

                        if (i == 1)
                        {
                            l5.Omnidirectional = true;
                        }

                        ModelConverter.convert(l3.model, "stat",true); // and convert model
                    }

                    l5.pack();
                    LIGH_GRP.addRecord(l5);
                    TypeIndex.getInstance().put(l5.editor_id, TypeIndex.TYPE.LIGH); // Mark self as LIGH
                }
            }

            TES3.ESM.close();
            return LIGH_GRP;
        }
    }
}
