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
    class NPC_
    {
        public static Dictionary<string, TES5.Record> body_templates = new Dictionary<string, TES5.Record>();

        /// <summary>
        /// 
        /// Users must define custom replacements for Morrowind faces for the conversion. Default Morrowind.esm
        /// Face Replacements are shipped in the template file "templates\faces.esp" , to define more face replacements
        /// users must open this file in Creation Kit and create an NPC with the desired face, then name(not editor id) it 
        /// with the morrowind head model they intend to replace for example: "b_n_breton_m_head_01".
        /// 
        /// </summary>
        static bool doOnce = false;
        public static void read_templates()
        {
            if (doOnce)
            {
                return;
            }

            doOnce = true;
            TES5.ESM esm = TES5.ESM.read_from_file(Config.Paths.Templates.characters);

            foreach (TES5.Record r in esm.find_TOP_group_OR_FAIL("NPC_").records)
            {
                TES5.Field FULL = r.try_find_field("FULL");
                TES5.Field EDID = r.find_field_OR_FAIL("EDID", "NPC encountered with no editor id, FORMID="+ Text.toHex(r.id));

                if (FULL == null)
                {
                    Log.confirm(EDID.readString() + " : FaceGen Template NPC does not have a NAME. Un-named NPCs are ignored and not used as facegen templates.");
                    continue;
                }

                string name_full = FULL.readString().ToLower();
                body_templates.Add(name_full, r);
            }

        }
        public static void presets()
        {
            string[] races = {"Breton"	,
                "Dark Elf"	,
                "High Elf"	,
                "Imperial"	,
                "Redguard"	,
                "Wood Elf"	,
                "Nord"	,
                "Khajiit"	,
                "Argonian"	,
                "Orc"	
                       };

            string[] sexes = { "Female", "Male" };
                        
            Dictionary<string, Dictionary<string, Queue<string>>> dict = new Dictionary<string, Dictionary<string, Queue<string>>>();

            foreach (string r in races)
            {
                Dictionary<string, Queue<string>> d = new Dictionary<string, Queue<string>>();

                dict.Add(r.Replace(" ",""), d);

                foreach (string s in sexes)
                {
                    d.Add(s, new Queue<string>());    
                }
            }

            TextReader fin = File.OpenText("tmp\\faces.csv");

            while (fin.Peek() != -1)
            {
                string line = fin.ReadLine();
                string[] parsed = line.Split(',');

                string race = parsed[0];
                string sex = parsed[1];
                string model = parsed[2].ToLower();

                dict[race.Replace(" ","")][sex].Enqueue(model);
            }

            BinaryReader bw = new BinaryReader(new FileStream("tmp\\Skyrim.esm",FileMode.Open));
            TES5.Record head = new TES5.Record();

            head.read(bw);

            TES5.Group g = new TES5.Group();
            g.read(bw);

            while (!g.hasLabel("NPC_"))
            {
                Log.info("Reading: ");
                g.read(bw);
            }

            TES5.ESM esm = new TES5.ESM("faces.esp");
            esm.add_masters("Skyrim.esm");

            TES5.Group g_out = new TES5.Group("NPC_");

            foreach (TES5.Record r in g.records)
            {
                if (!r.isType("NPC_"))
                {
                    continue;
                }

                TES5.Field f = r.find_field_OR_FAIL("ACBS", "ACBS not found");

                UInt32 flagers = f.getData().ReadUInt32();

                if (BinaryFlag.isSet(flagers, 0x04))
                {
                    

                    string edid = r.find_field_OR_FAIL("EDID", "").readString();
                    string sex = "";
                    if (edid.Contains("Female"))
                    {
                        sex = "Female";
                    }
                    else if (edid.Contains("Male"))
                    {
                        sex = "Male";
                    }
                    else
                    {
                        Log.error("Can't be");
                    }

                    string race = edid.Split(new string[]{sex},StringSplitOptions.None).First();

                    if (dict[race][sex].Count == 0)
                    {                        
                        continue;
                    }

                    string model = dict[race][sex].Dequeue();

                    Log.info(race);
                    Log.info(sex);
                    Log.info(r.find_field_OR_FAIL("EDID","").readString());

                    TES5.Field full = r.try_find_field("FULL");

                    if (full == null)
                    {
                        full = new TES5.Field("FULL", Text.zstring(model));
                        r.fields.Insert(11, full);
                    }
                    else
                    {
                        full.replaceData(Text.zstring(model));
                    }

                    r.find_field_OR_FAIL("EDID", "").replaceData(Text.zstring(model));

                    TES5.Record new_rec = new TES5.Record("NPC_");
                    new_rec.clone(r,model);
                    new_rec.find_field_OR_FAIL("EDID", "").replaceData(Text.zstring(model));
                    g_out.addRecord(new_rec);
                }
            }


            esm.add_group(g_out);
            esm.write_to_file(Config.Paths.Templates.characters);


        }

        public static TES5.Group convert(string file)
        {            
            read_templates();
            TES3.ESM.open(file);

            TES5.Group npc_grup = new TES5.Group("NPC_");

            while (TES3.ESM.find("NPC_"))
            {
                TES3.NPC_ npc3 = new TES3.NPC_();
                npc3.read();

                string npc3_head_model = npc3.head.ToLower();

                TES5.NPC.NPC_ npc5 = new TES5.NPC.NPC_();

                if (!body_templates.ContainsKey(npc3_head_model))
                {
                    Log.confirm("No Face-Gen data for " + npc3_head_model + " defined in template: " + Config.Paths.Templates.characters);
                    continue;
                }

                npc5.clone(body_templates[npc3_head_model],npc3.editor_id);

                npc5.editor_id = npc3.editor_id; // redundant?
                npc5.game_name = npc3.game_name;

                npc_grup.addRecord(npc5);
            }

            TES3.ESM.close();
            
            return npc_grup;
        }

    }
}
