using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace converter
{
    class Config
    {
        public class Paths
        {
            public static string morrowind_path = "tes3\\";
            public static string mw_meshes = "tes3\\data\\meshes\\";

            public static string skyrim_path = "tes5\\";
            public static string sk_meshes = "tes5\\data\\meshes\\morrowind\\";

            public static string nif_batch_file = "misc\\nifbatch.txt"; // Do not change unless mirrored in nifconvert
        }

    }
}
