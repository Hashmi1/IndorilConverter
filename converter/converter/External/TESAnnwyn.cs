using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace External
{
    class TESAnnwyn : External_Program
    {
        // TESAnnwyn.exe -p 2 -b 32 -c -T 3 morrowind.esm
        // TESAnnwyn.exe -i Skyrim -p 2 -b 32 -c -T 3 tesannwyn.bmp

        public TESAnnwyn()
            : base("TESAnnwyn", "external\\TESAnnwyn\\tesannwyn.exe")
        {
        }
        
        public void convert(string filename="Morrowind.esm")
        {
            Environment.CurrentDirectory = "external\\tesannwyn";
            string command_1 = "-p 2 -b 32 -c -T 3 " + filename;
            string command_2 = "-i Skyrim -p 2 -b 32 -c -T 3 tesannwyn.bmp";

            base.run(command_1);
            base.run(command_2);
        }
    }
}
