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

        protected void run(string command)
        {
            
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
                Log.info(line);
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


        }
    }
}
