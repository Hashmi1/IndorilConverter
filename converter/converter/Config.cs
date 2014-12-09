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


    class Config
    {
        public static HashSet<string> ignored_editor_ids = new HashSet<string>();
        
        public class Prefixes
        {
            public static string converted_meshes = "morrowind\\";
            public static string converted_textures = "textures\\morrowind\\";
            public static string converted_editor_ids = "mw_";
        }

        public class Paths
        {
            public static string tmp = "tmp\\";

            public static string nifconvert_path = "D:\\code\\NifUtilsSuite-master\\NifUtilsSuite-master\\Release\\NifConvert.exe";

            public static string tesannwyn_path = "external\\TESAnnwyn\\";
            public static string tesannwyn_path_ltex = "external\\TESAnnwyn\\tes3ltex.txt";

            public static string cell_name_replace = "config\\cell_name_replacement.txt";
            public static string cell_class = "config\\cell_classification.txt";

            public static string morrowind_path = "D:\\gms\\morrowind\\data\\";
            public static string mw_meshes = morrowind_path + "meshes\\";
            public static string mw_esm = morrowind_path + "morrowind.esm";

            public static string skyrim_path = "D:\\gms\\skyrim\\data\\";
            public static string sk_meshes =  skyrim_path + "meshes\\";

            
            public static string light_templates = "templates\\ltmp.esp";
            
        }

    }

