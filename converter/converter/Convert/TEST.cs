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
                    
                }

            }
        }
    }
}
