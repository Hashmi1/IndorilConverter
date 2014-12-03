using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Convert
{
    class TEST
    {
        public static void doit()
        {

            TES3.ESM.open(Config.Paths.mw_esm);
            TES5.Group cell_grup = new TES5.Group("CELL");

            while (TES3.ESM.find("CELL"))
            {
                
                TES3.CELL morrowind_cell = new TES3.CELL();
                morrowind_cell.read();

                //Utility.Log.info(morrowind_cell.cell_name);
                foreach (TES3.REFR morrowind_reference in morrowind_cell.references)
                {
                    if (morrowind_reference.door_data.hasDNAM && !morrowind_reference.door_data.hasDODT)
                    {
                        Log.info("Has DNAM but no DODT");
                    }
                    if (!morrowind_reference.door_data.hasDNAM && morrowind_reference.door_data.hasDODT)
                    {
                        Log.info("Has DODT but no DNAM");
                    }

                }

            }
        }
    }
}
