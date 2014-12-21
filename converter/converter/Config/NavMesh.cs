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
using System.IO;
using Utility;

namespace Config
{
    public class Navmesh
    {
        public static string config_path = "config\\navmesh.ini";
       
        public static string[] ignored_objects { get; private set; }
        public static string[] use_records { get; private set; }
        public static float cell_height { get; private set; }
        public static float cell_width { get; private set; }
        public static float agent_height { get; private set; }
        public static float agent_width { get; private set; }
        public static float max_climb { get; private set; }
        public static float max_slope { get; private set; }

        static Navmesh()
        {
            load_config();
        }

        private static void restore_defaults()
        {
            config_path = "config\\navmesh.ini";
            use_records = new string[] { "STAT", "LIGH", "CONT", "FURN" };
            ignored_objects = new string[] { };
            //cell2obj_dll_path = "D:\\niflib-master\\niflib\\obj\\Release - DLL\\niflib.dll";
        }

        private static void save_config()
        {
            TextWriter fout = File.CreateText(config_path);

            fout.WriteLine("# Comma seperated list of record types to consider in the navmesh");
            fout.Write("use_records=");
            foreach (string r in use_records)
            {
                fout.Write(r + ",");
            }
            fout.WriteLine();

            fout.WriteLine("# Comma seperated list of editor ids to ignore in the navmesh. This takes precedence over record types.");
            fout.Write("ignored_objects=");
            foreach (string r in ignored_objects)
            {
                fout.Write(r + ",");
            }
            fout.WriteLine();

            fout.Close();
            
        }

        private static void load_config()
        {
            restore_defaults();

            if (!File.Exists(config_path))
            {
                Log.confirm("NavMesh config not found. Making default config");
                save_config();
                return;
            }

            TextReader fin = File.OpenText(config_path);

            while (fin.Peek() != -1)
            {
                string line = fin.ReadLine().Trim().ToLower();

                string option = line.Split('=').First();
                string[] list = line.Split('=').Last().Split(',');                
            }

            fin.Close();
        }

        private void set_option(string option, string[] lst)
        {

            if (option.Equals("use_records"))
            {
                use_records = lst;
            }

            else if (option.Equals("ignored_objects"))
            {
                ignored_objects = lst;
            }
            else
            {
                Log.confirm("Unknown NavMesh Config option will be ignored: " + option);
            }

        }



    }
}
