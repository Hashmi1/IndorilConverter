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

namespace External
{
    class NifConvert : External_Program
    {
        public NifConvert()
            : base("NifConvert", Config.Paths.nifconvert_path)
        {
        }

        public void convert(string model_path, string mode, bool incremental)
        {
            string model_path_in = Config.Paths.mw_meshes + model_path;
            string model_path_out = Config.Paths.sk_meshes + Text.model_path_string(model_path);

            if (!File.Exists(model_path_in))
            {
                Utility.Log.error("Nifconvert is being given a non-existent file for conversion.");
            }

            if (mode.Equals("door_load"))
            {
                model_path_out = model_path_out.ToLower().Replace(".nif", "_zload.nif");
            }
                        
            if (incremental && File.Exists(model_path_out))
            {
                return;
            }

            model_path_in = '"' + model_path_in + '"';
            model_path_out = '"' + model_path_out + '"';

            string command = "-in " + model_path_in + " -out " + model_path_out + " -" + mode + " -txtr_prefix " +Config.Prefixes.converted_textures;
            run(command);
        }
    }
}

