using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Convert
{
    class FURN
    {
        static TES5.Group furn = new TES5.Group();

        public static TES5.Group convert()
        {
            if (furn.records.Count == 0)
            {
                Log.error("No furniture was found. Please make sure that STAT has been converted");
            }

            return furn;
        }

        public static bool isFurn(STAT.STRUCT_STAT r)
        {
            string editor_id = r.editor_id;
            bool taken = false;
            editor_id = editor_id.ToLower();

            if (editor_id.Contains("charir") || editor_id.Contains("stool") || editor_id.Contains("bench"))
            {
                taken = true;
                // is sitable
            }

            if (editor_id.Contains("bed") || editor_id.Contains("bunk"))
            {
                taken = true;
                // is sitable
            }


            return taken;

        }

        private TES5.Record convert(STAT.STRUCT_STAT f)
        {
            TES5.Record r = new TES5.Record("FURN", f.editor_id);
            r.addField(

            return null;
        }

    }
}
