using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES3;
using Utility;
using System.IO;

using External;

namespace Program
{
    class Program
    {
        
        static void Main(string[] args)
        {
            if (File.Exists("Landscape-Log.txt"))
            {
                File.Delete("Landscape-Log.txt");
            }

            //TESAnnwyn tn = new TESAnnwyn();
            //tn.convert();

            //Landscape_Module.fixer.start();

            Landscape_Module.Main.start();
            //Landscape_Module.comparer.main();
            Log.info("DONE");            
            Console.Read();
        }
    }
}
