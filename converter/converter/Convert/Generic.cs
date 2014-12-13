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
    class Generic_Object // use for STAT, ACTI, MISC etc.
    {

        protected List<OBJ_STRUCT> list;

        protected string TYPE;

        protected struct OBJ_STRUCT
        {
            public string editor_id;
            public string model_path;
            public string game_name;
        }

        protected Generic_Object(string type)
        {
            if (type.Length != 4)
            {
                Log.error("ES Object TYPE must be 4 chars");
            }

            TYPE = type;
        }

        protected List<OBJ_STRUCT> get_lst(string file)
        {
            List<OBJ_STRUCT> lst = new List<OBJ_STRUCT>();

            TES3.ESM.open(file);
            TES5.Group grup = new TES5.Group(TYPE);

            while (TES3.ESM.find(TYPE))
            {
                TES3.Record r3 = new TES3.Record();
                r3.read();

                OBJ_STRUCT obj = new OBJ_STRUCT();

                foreach (TES3.SubRecord srec in r3.subRecords)
                {
                    if (srec.isType("NAME"))
                    {
                        obj.editor_id = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    if (srec.isType("FNAM"))
                    {
                        if (srec.size <= 1)
                        {
                            continue; // skip empties
                        }

                        obj.game_name = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    if (srec.isType("MODL"))
                    {
                        obj.model_path = Text.trim(new string(srec.getData().ReadChars(srec.size))).ToLower();
                    }


                }

                lst.Add(obj);

            }

            TES3.ESM.close();

            list = lst;
            return lst;
        }

        protected TES5.Group convert(string file)
        {
            List<OBJ_STRUCT> lst = get_lst(file);

            TES5.Group grup = new TES5.Group(TYPE);

            foreach (OBJ_STRUCT obj in lst)
            {
                TES5.Record r = new TES5.Record(TYPE, obj.editor_id);

                if (obj.editor_id != null)
                {
                    r.addField(new TES5.Field("EDID", Text.editor_id(obj.editor_id)));
                }
                if (obj.game_name != null)
                {
                    r.addField(new TES5.Field("FULL", Text.zstring(obj.game_name)));
                }
                if (obj.model_path != null)
                {
                    r.addField(new TES5.Field("MODL", Text.model_path(obj.model_path)));
                }

                grup.addRecord(r);
            }

            return grup;

        }

    }
}
