/*
Copyright(c) 2014 Hashmi1. 

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

// TODO: Add Beds and Benches

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.IO;

namespace Convert
{
    class FURN
    {
        static TES5.Group furn = null;

        static TES5.Record chair;
        static TES5.Record bed;
        static TES5.Record bed_double;
        static TES5.Record bench;

        static Dictionary<string /* model */ , Tuple<TES5.Record,TES5.REFR.Placement /* marker placement */>> dict = null;

        private static void read_placement_config()
        {
            if (dict != null)
            {
                return;
            }

            dict = new Dictionary<string, Tuple<TES5.Record, TES5.REFR.Placement>>();

            if (!File.Exists(Config.Paths.furniture_marker_config))
            {
                Log.confirm("Furniture Placement Config File was not found. \n Please generate it through FurnitureWorkshop.");
                return;
            }

            TextReader config = File.OpenText(Config.Paths.furniture_marker_config);

            
            while (config.Peek() != -1)
            {
                string line = config.ReadLine().Trim();

                if (line.StartsWith("#"))
                {
                    continue;
                }

                string[] parsed = line.Split(',');

                string model = parsed[0];
                string editor_id = parsed[1];
                
                float x = float.Parse(parsed[2]);
                float y = float.Parse(parsed[3]);
                float z = float.Parse(parsed[4]);

                float xR = float.Parse(parsed[5]);
                float yR = float.Parse(parsed[6]);
                float zR = float.Parse(parsed[7]);


                TES5.Record associated_marker = furn.find_record(editor_id);
                TES5.REFR.Placement pos = new TES5.REFR.Placement(x, y, z, xR, yR, zR);

                if (associated_marker == null)
                {
                    Log.error(editor_id + " was not found in FURN group. Ensure that conversion has run");
                }

                Tuple<TES5.Record, TES5.REFR.Placement> value = new Tuple<TES5.Record, TES5.REFR.Placement>(associated_marker, pos);

                dict.Add(model, value);
            }
            
        }
                     
        public static TES5.Group convert()
        {
            if (furn != null)
            {
                return furn;
            }

            furn = new TES5.Group("FURN");
            
            TES5.ESM esm = TES5.ESM.read_from_file(Config.Paths.Templates.furniture);
            
            chair = esm.groups[0].find_record("mw_chair_marker");
            bed = esm.groups[0].find_record("mw_bed_marker");
            bench = esm.groups[0].find_record("mw_bench_marker");
            bed_double = esm.groups[0].find_record("mw_doublebed_marker");

            if (chair == null || bed == null || bench == null || bed_double == null)
            {
                Log.error("FURN some Furniture marker were not assigned. Possibly bad furn.esp template"); 
            }

            chair.reset_formid("mw_chair_marker");
            bed.reset_formid("mw_bed_marker");
            bench.reset_formid("mw_bench_marker");
            bed_double.reset_formid("mw_doublebed_marker");

            furn.addRecord(chair);
            furn.addRecord(bed);
            furn.addRecord(bench);
            furn.addRecord(bed_double);

            return furn;
        }

        public static bool consider(string model_path, TES5.Record stat_rec)
        {
            convert();
            read_placement_config();

            bool taken = false;
            
            model_path = Text.model_path_string(model_path);

            if (model_path.Contains("chair") || model_path.Contains("stool") || model_path.Contains("bench") || model_path.Contains("bed") || model_path.Contains("hammock"))
            {
                taken = true;

                if (!dict.ContainsKey(model_path))
                {
                    Log.error("No FURN marker defined for potential furniture model: " +model_path);
                }

                Tuple<TES5.Record, TES5.REFR.Placement> marker_data = dict[model_path];

                TES5.REFR.Placement pos = marker_data.Item2;
                TES5.Record addon_base = marker_data.Item1;

                Convert.REFERENCE.Addon_Rules.add_rule(stat_rec, addon_base.id, /*pos.x*/0, pos.y, pos.z, pos.xR, pos.yR, pos.zR);
            }

            return taken;
                
        }
        
    }
}
