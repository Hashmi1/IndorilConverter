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
    class TypeIndex
    {
        public enum TYPE
        {
            STAT,
            LIGH,
            DOOR,
            ACTI,
            CELL,
            NOT_KNOWN
        }

        private static TypeIndex indx; 
        public static TypeIndex getInstance()
        {
            if (indx == null)
            {
                indx = new TypeIndex();
            }

            return indx;
        }

        Dictionary<string, TYPE> dict = new Dictionary<string, TYPE>();

        public void put(string editor_id, TYPE type)
        {
            if (dict.ContainsKey(editor_id))
            {
                Log.error("TES3:TypeIndex Type Redefined");
            }

            dict.Add(editor_id, type);
        }

        public TYPE get(string editor_id)
        {
            if (!dict.ContainsKey(editor_id))
            {
                //Log.error("TES3:TypeIndex asked for type of unknown editor_id");
                return TYPE.NOT_KNOWN;
            }

            return dict[editor_id];
        }
    }
}
