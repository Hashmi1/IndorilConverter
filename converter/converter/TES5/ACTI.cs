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

        

        public void make()
        {
            addField(new Field("EDID", Text.editor_id(editor_id)));

            if (full_name != null)
            {
                addField(new Field("FULL", Text.zstring(full_name)));
            }
            if (model != null)
            {
                addField(new Field("MODL", Text.model_path(model)));
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
