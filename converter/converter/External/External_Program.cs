/*
Copyright 2014 Hashmi1

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

namespace External
{
    abstract class External_Program
    {
        string exec_path;
        string name;

        protected External_Program(string name, string exec_path)
        {
            this.exec_path = exec_path;
            this.name = name;
        }

        protected string[] run(string command)
        {

            List<string> output = new List<string>();

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = exec_path;
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();


            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                output.Add(line);
                Log.infoX(line);
            }


            bool success = process.WaitForExit(300000000);

            int count = 0;
            while (!success)
            {
                Log.info(name +" not responding, restarting...");
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
                Log.error(name + " returned error or could not complete." + '\n' + command);
            }

            return output.ToArray();
        }
    }
}
