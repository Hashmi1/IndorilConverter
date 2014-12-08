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
            External.NifConvert nconvert = new External.NifConvert();
            nconvert.convert(path,type,incremental);
            return false;
        }
    }
}
