using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utility
{

    class Log
    {
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

        public static void infoX(Object obj)
        {
            Console.WriteLine(obj);
            TextWriter tw = File.AppendText("Landscape-Log.txt");
            tw.WriteLine(obj);
            tw.Close();
        }


        public static void error(Object obj)
        {

            Console.WriteLine("Fatal Error Encountered:");
            Console.WriteLine("-------------------------");
            Console.WriteLine(obj);
            Console.Write("Can not continue. Press ENTER to exit ...");
            Console.Read();
            System.Environment.Exit(-1);
        }

    }

}
