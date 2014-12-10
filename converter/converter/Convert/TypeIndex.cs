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
