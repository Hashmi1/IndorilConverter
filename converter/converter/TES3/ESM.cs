using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES3
{
   
    class ESM
    {
        
        public static BinaryReader input;

        public static void rewind(int bytes)
        {
            input.BaseStream.Position = input.BaseStream.Position - bytes;
        }

        public static void open(string file)
        {
            if (!File.Exists(file))
            {
                Log.error("File Not Found: " + file);
            }
            input = new BinaryReader(new FileStream(file,FileMode.Open));
        }

        public static void close()
        {
            input.BaseStream.Close();
        }

        public static void rewind()
        {
            input.BaseStream.Seek(0,SeekOrigin.Begin);
        }

        public static bool find(string rec)
        {
            Record r = new Record();
            r.read(true);
            while (!rec.Contains(new string(r.Name)) && input.BaseStream.Position < input.BaseStream.Length-1)
            {
             
                r = new Record();
                r.read(true);
                
            }

            if (input.BaseStream.Position >= input.BaseStream.Length - 1)
            {
                Log.info("Reached End of File.");
                rewind();
                return false;
            }

            input.BaseStream.Seek(-r.Size-16, SeekOrigin.Current);
            return true;
        }
    } 



    



}

