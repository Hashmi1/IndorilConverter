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
