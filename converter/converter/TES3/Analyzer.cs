using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace TES3
{
    // Used to dump subrecords for the given record and their size, as heu appear in morrowind.esm
    // helpful for learning about records not documented well on the web

    class Analyzer
    {
        public static void analyze(string record)
        {
            
            ESM.open(Config.Paths.mw_esm);
            Log lg = new Log("explore.txt");

            while (ESM.find(record))
            {
                TES3.Record rec = new TES3.Record();
                rec.read();

                lg.log(new string(rec.Name));

                foreach (SubRecord sr in rec.subRecords)
                {
                    lg.log("    " + new string(sr.name) + "       " + sr.size);
                }
                
            }

            lg.show();

        }
    }
}
