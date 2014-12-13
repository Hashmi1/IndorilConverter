/*
Copyright(c) 2014 Hashmi1. 

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

namespace Convert.REFERENCE
{
    // This class manages addons for references. Where an Addon in virtually any World Reference 
    // that is placed in the same cell as another reference at a given offset. For example it is used
    // to place invisible Skyrim furniture on top of Morrowind's static furniture so that it can be used
    // another hypothetical use case could be to add cobwebs in defined passages etc.

    class Addon_Rules
    {

        class Addon
        {
            public uint formid;
            public float x { get; private set; }
            public float y { get; private set; }
            public float z { get; private set; }
            public float xR { get; private set; }
            public float yR { get; private set; }
            public float zR { get; private set; }

            public Addon(uint formid, float x, float y, float z, float xR, float yR, float zR)
            {
                this.formid = formid;
                this.x = x;
                this.y = y;
                this.z = z;
                this.xR = xR;
                this.yR = yR;
                this.zR = zR;

            }

        }

        static Dictionary<uint, Addon> dict = new Dictionary<uint, Addon>();

        public static void add_rule(TES5.Record obj, UInt32 addon_id, float x, float y, float z, float xR, float yR, float zR)
        {
            Addon relative_placement = new Addon(addon_id,x, y, z, xR, yR, zR);
            dict.Add(obj.id, relative_placement);
        }

        public static bool addon_defined(uint object_id)
        {
            return dict.ContainsKey(object_id);
        }

        public static TES5.REFR get_addon(TES5.REFR obj)
        {
            Addon addon = dict[obj.base_id];
            uint addon_base_id = addon.formid;

            float x = obj.loc.x + addon.x;
            float y = obj.loc.y + addon.y;
            float z = obj.loc.z + addon.z;

            float xR = obj.loc.xR + addon.xR;
            float yR = obj.loc.yR + addon.yR;
            float zR = obj.loc.zR + addon.zR;

            TES5.REFR addon_ref = new TES5.REFR(addon_base_id,x,y,z,xR,yR,zR);
            
            return addon_ref;
        }
    }
}
