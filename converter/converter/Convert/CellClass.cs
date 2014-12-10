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
using System.IO;
using Utility;

namespace Convert
{
    class CellTYPE
    {
        private CellTYPE() { }
        static CellTYPE instance;

        public static CellTYPE getInstance()
        {
            if (instance == null)
            {
                instance = new CellTYPE();
                instance.read_config();
            }

            return instance;
        }

        public enum TYPE
        {
            TOMB,
            CAVE,
            MINE,
            DAEDRIC,
            REDORAN,
            TELVANNI,
            HLAALU,
            IMPERIAL,
            VELOTHI,
            DWEMER,
            FORT,
            DEFAULT
        }

        Dictionary<string, TYPE> class_dict = new Dictionary<string, TYPE>() 
        {
            {"tomb", TYPE.TOMB},
            {"cave", TYPE.CAVE},
            {"mine", TYPE.MINE},
            {"daedric", TYPE.DAEDRIC},
            {"redoran", TYPE.REDORAN},
            {"telvanni", TYPE.TELVANNI},
            {"hlaalu", TYPE.HLAALU},
            {"imperial", TYPE.IMPERIAL},
            {"velothi", TYPE.VELOTHI},
            {"dwemer", TYPE.DWEMER},
            {"fort", TYPE.FORT},
            {"default", TYPE.DEFAULT},
            
        };

        Dictionary<string, TYPE> dict = new Dictionary<string, TYPE>();

        private void read_config()
        {
            if (!File.Exists(Config.Paths.cell_class))
            {
                Log.error("Cell Classification Config File not found: " + Config.Paths.cell_class);
            }

            TextReader fin = File.OpenText(Config.Paths.cell_class);

            string mode = null;

            while (fin.Peek() != -1)
            {
                string line;
                line = fin.ReadLine().Trim().ToLower();

                if (String.IsNullOrWhiteSpace(line) || String.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.StartsWith("#"))
                {
                    mode = line.Replace("#", "").Trim();
                    if (!class_dict.ContainsKey(mode))
                    {
                        Log.error("Class " + mode + " was not defined in code");
                    }
                }

                else
                {
                    if (mode == null)
                    {
                        Log.error("CellClass: No Mode Defined");

                    }

                    string cell = line;

                    if (dict.ContainsKey(line))
                    {
                        Log.error("CellClass cell " + line + " redefined");
                    }

                    

                    dict.Add(line, class_dict[mode]);
                }
            }


            

        }

        public TYPE get_class(string name)
        {
            name = name.ToLower();

            if (name.Contains(",")) // for city children (nested names) consider the first part only
            {
                name = name.Split(',').First();
            }

            if (!dict.ContainsKey(name))
            {
                Log.non_fatal_error("Cell Class not known " + name + " assigning default");
                return TYPE.DEFAULT;
            }

            return dict[name];
            
        }
    }
}
