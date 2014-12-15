using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Convert
{
    class FACT
    {
        public static TES5.Group convert(string file)
        {
            TES5.Group fact = new TES5.Group("FACT");

            TES3.ESM.open(file);

            while (TES3.ESM.find("FACT"))
            {
                TES3.Record f3 = new TES3.Record();
                f3.read();
                
                string editor_id = f3.find_first("NAME").readString();
                string full_name = f3.find_first("NAME") != null ? f3.find_first("NAME").readString() : null;

                TES5.Record f5 = new TES5.Record("FACT", editor_id);

                if (full_name != null)
                {
                    f5.addField(new TES5.Field("FULL", Text.zstring(full_name)));
                }

                List<TES3.SubRecord> ANAMs = f3.find_all("ANAM", true);
                List<TES3.SubRecord> INTVs = f3.find_all("INTV", true);

                foreach (TES3.SubRecord intv in INTVs)
                {
                    int reaction = intv.getData().ReadInt32();
                    if (reaction <= -3) // make hostile
                    {
                        reaction = 1;
                    }
                    else if (reaction > -3 && reaction <= 1)
                    {
                        reaction = 0;
                    }
                    else if(reaction >= 2)
                    {
                        reaction = 2;
                    }

                }

                uint flags = 0;
                flags = BinaryFlag.set(flags,0x8000);
                f5.addField(new TES5.Field("DATA", Binary.toBin(flags)));
                fact.addRecord(f5);
            }


            TES3.ESM.close();

            return fact;
        }

        public static void add_factions()
        {
        }
    }
}
