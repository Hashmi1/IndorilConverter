﻿/*
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

namespace External
{
    class Notepad : External_Program
    {
        public Notepad() : base("Notepad","notepad.exe")
        {
        }

        public void open(string file)
        {
            string command = file;
            List<string> output = new List<string>();
            base.run(command);
        }

    }
}
