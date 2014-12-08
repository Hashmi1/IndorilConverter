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

namespace External
{
    class TESAnnwyn : External_Program
    {
        // TESAnnwyn.exe -p 2 -b 32 -c -T 3 morrowind.esm
        // TESAnnwyn.exe -i Skyrim -p 2 -b 32 -c -T 3 tesannwyn.bmp

        public TESAnnwyn()
            : base("TESAnnwyn", Config.Paths.tesannwyn_path + "tesannwyn.exe", true ,int.MaxValue/* TESAnnwyn can take a long time */)
        {
        }

        private void get_bottom_left(string[] lines, ref int x, ref int y)
        {
            string coordinates_str = null;

            foreach (string line in lines)
            {
                string search_start = "Bottom left of image corresponds to cell (";
                string search_end = "). This could be useful if you want to re-import.";

                int index_start = line.IndexOf(search_start);
                int index_end = line.IndexOf(search_end);

                if (index_start != -1 && index_end != -1)
                {
                    coordinates_str = line.Substring(index_start + search_start.Length, index_end - (index_start+search_start.Length));
                    Log.info(coordinates_str);

                    string[] cords = coordinates_str.Split(',');

                    x = int.Parse(cords[0]);
                    y = int.Parse(cords[1]);

                    return;
                }

                
            }

            if (coordinates_str == null)
            {
                Log.error("TESAnnwyn apparently did not report the bottom left corner");
            }
            
        }

        public void convert(string filename_in, string filename_out)
        {
            if (!(filename_in.EndsWith(".esp") || filename_in.EndsWith(".esm")))
            {
                Utility.Log.error("TESAnnwyn given non esp/esm file.");
            }

            string full_file_path = filename_in;

            filename_in = filename_in.Split('\\').Last();
            
            // remove old file copy if it exists
            if (File.Exists(Config.Paths.tesannwyn_path+filename_in))
            {
                File.Delete(Config.Paths.tesannwyn_path+filename_in);
            }
            
            // create a copy in tesannwyn directory
            Log.info("Copying file \'" + filename_in + "\' please wait");
            File.Copy(full_file_path,Config.Paths.tesannwyn_path+filename_in);

            Log.info("Converting " + filename_in + " in TESAnnwyn.");

            string old_dir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Config.Paths.tesannwyn_path;
            
            string command_1 = "-p 2 -b 32 -c -T 3 " + filename_in;
            
                        
            base.run(command_1);

            int bottom_x = 0;
            int bottom_y = 0;
            get_bottom_left(output.ToArray(), ref bottom_x, ref bottom_y);
            bottom_x *= 2;
            bottom_y *= 2;
            string command_2 = "-i Skyrim -p 2 -b 32 -c -T 3 -x " +bottom_x + " -y " + bottom_y  + " tesannwyn.bmp";
            base.run(command_2);
            Environment.CurrentDirectory = old_dir;

            if (File.Exists(filename_out))
            {
                File.Delete(filename_out);
            }

            

            File.Move(Config.Paths.tesannwyn_path + "tesannwyn.esp", filename_out);

            
        }
    }
}
