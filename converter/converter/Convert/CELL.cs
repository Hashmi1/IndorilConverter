using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.IO;

namespace Convert
{
    // TODO: ownership / scale / exterior cell / door /lock etc.
    class CELL
    {
        
        public static TES5.Group start()
        {
            int count = 2; // TES4 header and CELL TOP Group

            TES3.ESM.open("tes3/morrowind.esm");
            TES5.Group cell_grup = new TES5.Group("CELL");

            while (TES3.ESM.find("CELL"))
            {
                //Log.info("CELL found");
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

                count++;

                
                Log.info(morrowind_cell.cell_name);

                TES5.Record cell5 = new TES5.Record("CELL");
                    
                cell5.addField(new TES5.Field("EDID", Text.editor_id(morrowind_cell.cell_name)));                
                cell5.addField(new TES5.Field("FULL", Text.zstring(shorten_name(morrowind_cell.cell_name))));
                cell5.addField(new TES5.Field("DATA", Binary.toBin(BinaryFlag.set((ushort)0, (ushort)0x0001))));
                cell_grup.addRecord(cell5);

                TES5.Group cell_references = new TES5.Group(cell5.id, TES5.Group.TYPE.TEMP_REFR);

                foreach (TES3.REFR morrowind_reference in morrowind_cell.references)
                {
                    
                    string refr_id = morrowind_reference.editor_id;
                    uint formid = TES5.FormID.get(refr_id);
                    if (formid == 0)
                    {
                        continue;   // Reference Base not converted, so skip
                    }

                    TES5.REFR skyrim_reference = new TES5.REFR(formid, morrowind_reference.x, morrowind_reference.y, morrowind_reference.z, morrowind_reference.xR, morrowind_reference.yR, morrowind_reference.zR);
                    cell_references.addRecord(skyrim_reference);    
                }

                cell_grup.addSubGroup(cell_references);
            }

            TES3.ESM.close();
            Log.info(count);
            return cell_grup;

        }

        static string lookup_replacement(string name)
        {
            string backup = name;
            TextReader tr = File.OpenText("name_replacement.txt");

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
                        Log.info("Please fix name_replacement.txt and press enter.");
                        Console.ReadLine();
                        return backup;
                    }
                    return name;
                }
                
            }

            tr.Close();
            TextWriter tx = File.AppendText("name_replacement.txt");
            tx.WriteLine(name + "=");
            tx.Close();
            Log.info("CELL NAME TOO LONG Please update: " + name + " And then press enter to continue");
            Console.ReadLine();
            return backup;
            
             
        }

        public static string shorten_name(string cell_name)
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
