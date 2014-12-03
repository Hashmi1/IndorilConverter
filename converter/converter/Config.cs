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


    class Config
    {
        public class Prefixes
        {
            public static string morrowind_meshes = "morrowind\\";
            public static string morrowind_textures = "morrowind\\";
        }

        public class Paths
        {
            public static string tmp = "tmp\\";

            public static string tesannwyn_path = "external\\TESAnnwyn\\";
            public static string tesannwyn_path_ltex = "external\\TESAnnwyn\\tes3ltex.txt";

            public static string cell_name_replace = "config\\cell_name_replacements.txt";
            public static string morrowind_path = "tes3\\";
            public static string mw_meshes = "tes3\\data\\meshes\\";
            public static string mw_esm = "tes3\\data\\morrowind.esm";

            public static string skyrim_path = "tes5\\";
            public static string sk_meshes = "tes5\\data\\meshes\\morrowind\\";
            public static string sk_textures = "tes5\\data\\meshes\\morrowind\\";

            public static string nif_batch_file = "misc\\nifbatch.txt"; // Do not change unless mirrored in nifconvert
        }

    }

