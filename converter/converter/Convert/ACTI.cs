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
    class ACTI : Generic_Object
    {
        ACTI() : base("ACTI")
        {
        }

        static ACTI instance = null;

        public static ACTI getInstance()
        {
            if (instance == null)
            {
                instance = new ACTI();
            }
            return instance;
        }

        public TES5.Group convert(string file, bool ignoreFurniture=false)
        {
            List<OBJ_STRUCT> lst = get_lst(file);

            TES5.Group grup = new TES5.Group(TYPE);

            foreach (OBJ_STRUCT obj in lst)
            {
                TES5.Record r = new TES5.Record(TYPE, obj.editor_id);

                bool isFurn = false;
                if (!ignoreFurniture)
                {
                    isFurn = FURN.consider(obj.model_path, r);
                }

                r.addField(new TES5.Field("EDID", Text.editor_id(obj.editor_id)));
                if (obj.game_name != null && !isFurn) { r.addField(new TES5.Field("FULL", Text.zstring(obj.game_name))); }
                if (obj.model_path != null) { r.addField(new TES5.Field("MODL", Text.model_path(obj.model_path))); }
                
                grup.addRecord(r);
                ModelConverter.convert(obj.model_path, "stat", true);
                

                
            }

            return grup;

        }

    }
}
