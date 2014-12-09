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
using System.IO;

namespace Convert
{
    // TODO: ownership/water/light
    class CELL
    {
        public static TES5.Group convert(string file)
        {            
            TES3.ESM.open(file);
            TES5.Group cell_grup = new TES5.Group("CELL");

            while (TES3.ESM.find("CELL"))
            {
                TES3.CELL cell3 = new TES3.CELL();
                cell3.read();

                if (!cell3.interior)
                {
                    continue;
                }

                if (cell3.references.Count == 0)
                {
                    continue;
                }

                Log.info(cell3.cell_name);

                TES5.CELL cell5 = new TES5.CELL(cell3.cell_name);
                
                cell5.editor_id = cell3.cell_name;
                cell5.full_name = cell3.cell_name;

                cell5.Interior = true;
                if (cell3.HasWater)
                {
                    // Calculate cell bounds
                    foreach (TES3.REFR mw_ref in cell3.references)
                    {
                        cell5.update_bounds(mw_ref.x, mw_ref.y);
                    }
                    // add water planes
                    cell5.addWater(cell3.water_height);
                }

                cell5.add_ambient_light(LGTM.get(CellTYPE.getInstance().get_class(cell3.cell_name)));
                
                cell5.addToGroup(cell_grup);
                cell5.pack();
            }

            TES3.ESM.close();
            return cell_grup;
        }
               
    }
}
