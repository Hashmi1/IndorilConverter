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

namespace TES5.NPC
{
    class NPC_ : Record
    {
        Field EDID;
        Field FULL;
        ACBS ACBS;

        public string editor_id
        {
            get
            {                
                return EDID.readString();                
            }

            set
            {
                EDID.replaceData(Text.editor_id(value));
            }
        }

        public string game_name
        {
            get
            {
                if (FULL == null)
                {
                    return null;
                }
                return FULL.readString();
            }

            set
            {
                if (FULL == null)
                {
                    Log.error(editor_id + " has no full name. ");
                }
                FULL.replaceData(Text.zstring(value));
            }
        }

        public NPC_() : base("NPC_")
        {
        }

        public new void clone(Record r, string editor_id)
        {
            base.clone(r,editor_id);
            
            EDID = try_find_field("EDID");
            FULL = try_find_field("FULL");
            ACBS = new ACBS(find_field_OR_FAIL("ACBS","NPC does not have ACBS."));
            ACBS.isPreset = false;
            ACBS.flush();

            if (EDID == null || FULL == null)
            {
                Log.error("Failed to clone NPC. " + r.id);
            }
        }
    }
}
