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

namespace Config
{
    public class GeneralSettings
    {
        public static bool verbose = false; // Shows more details about what the program is doing
        public static bool paged = true;    // Offloads some data to the disk, allows loading slightly bigger files at a performance cost
    }

     
}
