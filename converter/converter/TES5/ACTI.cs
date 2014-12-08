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

namespace TES5
{
    class ACTI : Record
    {
        public string editor_id = null;
        public string full_name = null;
        public string model = null;
        public uint water = 0x18;
        public string verb = null;

        public bool isWater;

        public ACTI(string editor_id)
            : base("ACTI", editor_id)
        {
            this.editor_id = editor_id;
        }

        static ACTI water_mesh = null;

        public static ACTI get_water_instance()
        {
            if (water_mesh == null)
            {
                water_mesh = new ACTI("mw_water_default");
                water_mesh.isWater = true;
                water_mesh.model = "Water\\Water2048.nif";
                water_mesh.pack();
            }
            return water_mesh;
        }

        public void pack()
        {
            fields = new List<Field>();

            addField(new Field("EDID", Text.editor_id(editor_id)));

            if (full_name != null)
            {
                addField(new Field("FULL", Text.zstring(full_name)));
            }
            if (model != null)
            {
                addField(new Field("MODL", Text.zstring(model))); // TODO: Change to modelpath() for MW
            }
            
            if (isWater)
            {
                addField(new Field("WNAM", Binary.toBin((uint)water)));
            }

            if (verb != null)
            {
                addField(new Field("RNAM", Text.zstring(editor_id)));
            }

        }

    }
}
