/*
Copyright 2014 Hashmi1

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
using System.IO;
using Utility;

namespace Convert
{
    class ESM
    {
        public static void start()
        {
            TES5.Group stat_grup = STAT.mw_statics(Config.Paths.mw_esm);
            TES5.Group cell_grup = CELL.start();

            TES5.ESM esm = new TES5.ESM("interiors.esp", FileMode.Create);
            BinaryWriter file_out = esm.getWriter();

            int count = stat_grup.records.Count + cell_grup.records.Count + cell_grup.subGroups.Count;

            TES5.Record.TES4(count).write(file_out);
            stat_grup.write(file_out);
            cell_grup.write(file_out);

            Log.info(count + " counts");

            esm.close();
        }
    }
}
