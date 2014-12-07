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
                TES3.CELL morrowind_cell = new TES3.CELL();
                morrowind_cell.read();

                if (!morrowind_cell.interior)
                {
                    continue;
                }

                if (morrowind_cell.references.Count == 0)
                {
                    continue;
                }
                                
                Log.info(morrowind_cell.cell_name);

                TES5.Record cell5 = new TES5.Record("CELL");
                    
                cell5.addField(new TES5.Field("EDID", Text.editor_id(morrowind_cell.cell_name)));                
                cell5.addField(new TES5.Field("FULL", Text.zstring(shorten_name(morrowind_cell.cell_name))));

                ushort flags = (ushort)0x0001; // interior

                if (morrowind_cell.water)
                {
                   flags = BinaryFlag.set((ushort)flags, (ushort)0x0002);
                   cell5.addField(new TES5.Field("XCLW", Binary.toBin(morrowind_cell.water_height)));
                }

                TES5.CELL.XCLL xcll = new TES5.CELL.XCLL();

                int r = 0;
                int g = 1;
                int b = 2;

                xcll.Ambient[r] = 128;
                xcll.Ambient[g] = 128;
                xcll.Ambient[b] = 128;

                xcll.Directional[r] = morrowind_cell.sun_col[1];
                xcll.Directional[g] = morrowind_cell.sun_col[2];
                xcll.Directional[b] = morrowind_cell.sun_col[3];


                cell5.addField(new TES5.Field("XCLL",xcll.toBin()));
                cell5.addField(new TES5.Field("LTMP", Binary.toBin((UInt32) 0)));

                //morrowind_cell.amb_col[0];

                cell5.addField(new TES5.Field("DATA", Binary.toBin(BinaryFlag.set((ushort)0, (ushort)0x0001))));



                cell_grup.addRecord(cell5);


                TES5.Group cell_references = new TES5.Group(cell5.id, TES5.Group.TYPE.CELL_CHILD);
                TES5.Group temp_references = new TES5.Group(cell5.id, TES5.Group.TYPE.TEMP_REFR);

                cell_references.addSubGroup(temp_references);

                //cell_references.label = Binary.toBin(cell5.id);
                cell_grup.addSubGroup(cell_references);
            }

            TES3.ESM.close();            
            return cell_grup;

        }

        static string lookup_replacement(string name)
        {
            string backup = name;
            TextReader tr = File.OpenText(Config.Paths.cell_name_replace);

            while (tr.Peek() != -1)
            {
                string fle = tr.ReadLine();
                string[] splt = fle.Split('=');
                if (splt[0].Equals(name) && splt.Length > 1)
                {
                    name = splt[1];
                    if (name.Length > 33 || String.IsNullOrEmpty(name))
                    {
                        tr.Close();
                        Log.info("Invalid Replacement suggested for " + backup +". Must be < 33 chars and not empty.");
                        Log.info("Please fix in " + Config.Paths.cell_name_replace + " and press enter.");
                        Console.ReadLine();
                        return backup;
                    }
                    return name;
                }
                
            }

            tr.Close();
            TextWriter tx = File.AppendText(Config.Paths.cell_name_replace);
            tx.WriteLine(name + "=");
            tx.Close();
            Log.info("CELL NAME TOO LONG Please update: " + name + " And then press enter to continue");
            Console.ReadLine();
            return backup;
            
             
        }

        static string shorten_name(string cell_name)
        {
            string backup = cell_name;

            if (cell_name.Length < 33)
            {
                return cell_name;
            }

            string[] tok = cell_name.Split(',');
            string name = (tok[tok.Length - 1].Trim());
            
            while (name.Length > 33)
            {

                name = lookup_replacement(name);
            }
            return name;
        }
    }
}
