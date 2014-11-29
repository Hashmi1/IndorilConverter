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
