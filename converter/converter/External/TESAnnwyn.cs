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
