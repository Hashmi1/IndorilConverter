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

                TES5.LIGH l5 = new TES5.LIGH(l3.editor_id);

                l5.editor_id = l3.editor_id;
                l5.model = l3.model;
                //l5.icon = l3.icon; // ditch the icons

                l5.Radius = l3.radius/2;

                l5.Time = (int)l3.time;
                l5.Value = l3.value;
                l5.r = l3.red;
                l5.g = l3.green;
                l5.b = l3.blue;
                l5.Weight = l3.weight;

                l5.Dynamic = l3.Dynamic;
                l5.carried = l3.Can_Carry;
                
                l5.Flicker = l3.Flicker;
                //l5.FlickerSlow = l3.Flicker_Slow;
                l5.Pulse = l3.Pulse;

                if (l3.model == null)
                {
                    // if no model then make it shadow-less
                }
                else
                {
                    l5.Omnidirectional = true; // if model present make it shadow casting omni-directinal
                    ModelConverter.convert(l3.model, "stat",true); // and convert model
                }

                l5.pack();
                
                LIGH_GRP.addRecord(l5);
            }

            TES3.ESM.close();

            return LIGH_GRP;
        }
    }
}
