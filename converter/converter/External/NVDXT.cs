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

namespace External
{
    class NVDXT
    {


        //nvdxt -file tes5/morrowind/*.dds -n4 -outdir tes5/morrowind/normal
        //tesannwyn -p 2 -b 32 -T 5 morrowind.esm
        //tesannwyn -i Skyrim -p 2 -b 32 tesannwyn.bmp
        
        // TESAnnwyn.exe -p 2 -b 32 -c -T 3 morrowind.esm
        // TESAnnwyn.exe -i Skyrim -p 2 -b 32 -c -T 3 tesannwyn.bmp

        public static NVDXT use()
        {
            return new NVDXT();
        }
        
        private NVDXT()
        {
        }

        public void compute_normal_maps(string path, bool incremental = true)
        {
            if (!Directory.Exists(path))
            {
                Log.error("NVDXT: comoute_normal_maps -> Texture Path not found.");
            }

            string[] files = Directory.GetFiles(path, "*.dds");
            List<string> files_process = new List<string>();

            if (files.Length < 1)
            {
                Log.error("NVDXT: No Texture files (.dds) found in path: " + path);
            }

            foreach (string file in files)
            {
                // ignore old normal maps
                if (file.EndsWith("_n.dds"))
                {
                    continue;
                }

                if (incremental) // if doing incremental ignore files which already have normal maps
                {
                    if (File.Exists(file.Replace(".dds", "_n.dds")))
                    {
                        continue;
                    }
                }

                Log.info(file);
                files_process.Add(file);    
            }

            
            normal_map(files_process);

        }

        private void normal_map(string file_in)
        {
            file_in = "\"" + file_in + "\"";
            string file_out = file_in.Replace(".dds", "_n.dds");
            string command = " -file " + file_in + " -output " + file_out +" -n4 -aclear";
            run(command);
        }
        
        private void normal_map(List<string> file_in_lst)
        {
            foreach (string file in file_in_lst)
            {
                normal_map(file);
            }
        }

        private void run(string command)
        {
            // I've had nvdxt freezing when running in batch mode (*.dds)
            // So I've added some code to restart it if freezing occurs
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "external\\nvdxt.exe";
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();


            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                Log.info(line);
            }

            
            bool success = process.WaitForExit(30000);
            
            int count = 0;
            while (!success)
            {
                Log.info("NVDXT Not responding, restarting...");
                count++;

                if (!process.HasExited)
                {
                    process.Kill();
                }
                process.Start();
                success = process.WaitForExit(30000);
                if (count > 3)
                {
                    break;
                }
            }
        
            if (process.ExitCode != 0 || success == false)
            {
                Log.error("NVDXT returned error or could not complete." + '\n' + command);
            }
            

        }

    }
}
