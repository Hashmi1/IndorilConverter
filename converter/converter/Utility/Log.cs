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

        public static void infoX(Object obj)
        {
            Console.WriteLine(obj);
            TextWriter tw = File.AppendText("Landscape-Log.txt");
            tw.WriteLine(obj);
            tw.Close();
        }


        public static void error(Object obj)
        {

            Console.WriteLine(obj);
            Console.Write("Can not continue. Press ENTER to exit ...");
            Console.Read();
            System.Environment.Exit(-1);
        }

    }

}
