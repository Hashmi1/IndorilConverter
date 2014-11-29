using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.IO;

namespace Convert
{
    class Locations
    {

        public static List<string> get_ext_locations()
        {

            TES3.ESM.open("tes3/morrowind.esm");

            HashSet<String> cell_list = new HashSet<string>();


            while (TES3.ESM.find("CELL"))
            {


                TES3.CELL r = new TES3.CELL();
                r.read();

                //if (!BinaryFlag.isSet(r.data_flags, (int)TES3.CELL.CELL_FLAGS.Interior))
                {
                    if (!String.IsNullOrEmpty(r.cell_name))
                    {
                        cell_list.Add(r.cell_name);
                    }
                    
                }

            }

            Log.info(cell_list.Count);

            for (int i = 0; i < cell_list.Count; i++)
            {
                Log.info(cell_list.ToList<string>()[i]);

            }

            return cell_list.ToList<string>();
        }

        public static void make(List<string> locs)
        {
            BinaryWriter o_file = new TES5.ESM("loctest.esp",FileMode.Create).getWriter();

            TES5.Group grup = new TES5.Group("LCTN");
            int count = 1;

            for (int i = 0; i < locs.Count; i++)
            {
                string loc = locs[i];
            
                
                TES5.Record rec = new TES5.Record("LCTN");
                TES5.Field edid =  new TES5.Field("EDID",Text.editor_id(loc));
                TES5.Field full = new TES5.Field("FULL", Text.zstring(loc));
                rec.addField(edid);
                rec.addField(full);
                grup.addRecord(rec);
                count++;
                               
            }

            TES5.Record tes4 = TES5.Record.TES4(count);

            tes4.write(o_file);
            grup.write(o_file);
        }

    }
}
