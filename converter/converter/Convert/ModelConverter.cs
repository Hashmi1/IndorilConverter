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

namespace Convert
{
    class ModelConverter
    {        
        public static bool convert(string path, string type,bool incremental)
        {
            if (Config.Ignored.ignored_models.Contains(path.ToLower()) || Config.Ignored.ignored_model_folders.Contains(path.Split('\\').First()))
            {
                return false;
            }

            External.NifConvert nconvert = new External.NifConvert();
            nconvert.convert(path,type,incremental);
            return false;
        }
    }
}
