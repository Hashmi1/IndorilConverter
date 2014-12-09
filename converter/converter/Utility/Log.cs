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

namespace Utility
{

    class Log
    {
        TextWriter tw;
        string file;

        public Log(string file)
        {
            file = Config.Paths.tmp + file;
            this.file = file;

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            tw = System.IO.File.AppendText(file);
        }

        public void log(Object obj)
        {
            tw.WriteLine(obj);
        }

        public void show()
        {
            tw.Close();
            External.Notepad np = new External.Notepad();
            np.open(file);
        }

        public static void info(Object obj)
        {
           Console.WriteLine(obj);
        }

        public static void info_(params Object[] objs)
        {
            foreach (object obj in objs)
            {
                Console.Write(obj + ",");
            }
            Console.Write('\n');
        }

        public static void confirm(Object obj)
        {
            Console.WriteLine(obj);
            Console.ReadLine();
        }

        public static void error(Object obj)
        {

            Console.WriteLine("\nFatal Error Encountered:");
            Console.WriteLine("-------------------------");
            Console.WriteLine(obj);
            Console.Write("Can not continue. Press ENTER to exit ...");
            Console.Read();
            System.Environment.Exit(-1);
        }

        public static void exit(Object obj)
        {         
            Console.WriteLine(obj);
            Console.Write("Press ENTER to exit ...");
            Console.Read();
            System.Environment.Exit(0);
        }




    }

}
