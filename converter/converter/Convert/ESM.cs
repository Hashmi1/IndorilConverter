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
            TES5.Group stat_grup = STAT.start();
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
