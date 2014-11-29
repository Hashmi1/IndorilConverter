using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TES5 
{
    class ESM
    {
        public Record tes4 = new Record();
        public List<Group> groups = new List<Group>();
        public FileStream fstream;

        public ESM(string file,FileMode mode)
        {
            fstream = new FileStream(file, mode);
        }
        
        public BinaryReader getReader()
        {
            return new BinaryReader(fstream);
        }

        public BinaryWriter getWriter()
        {
            return new BinaryWriter(fstream);
        }

        public void close()
        {
            fstream.Flush();
            fstream.Close();
        }


        public void read()
        {
            long size = fstream.Length;
            
            BinaryReader input = getReader();

            tes4 = new Record();
            tes4.read(input);

            while (fstream.Position < fstream.Length)
            {
                Group grup = new Group();
                grup.read(input);
                groups.Add(grup);
            }

        }


              

    } 
 
}
