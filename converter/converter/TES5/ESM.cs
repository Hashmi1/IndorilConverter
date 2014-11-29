/*
Copyright 2014 Hashmi1

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

namespace TES5 
{
    class ESM
    {
        
        public long length
        {
            get { return fstream.Length; }
            //private set;
        }

        public long pointer
        {
            get { return fstream.Position; }
            //private set;
        }

        private string file_name;

        private Record tes4 = new Record();
        private bool read_ = false;
        private FileStream fstream;
        private List<Group> groups = new List<Group>();

        private BinaryReader reader = null;
        private BinaryWriter writer = null;
                
        private FileMode my_mode;

        public ESM(string file,FileMode mode)
        {
            this.my_mode = mode;
            this.file_name = file;
            fstream = new FileStream(file, mode);
        }

        private int get_num_records()
        {
            return 0;
        }
        
        public BinaryReader getReader()
        {
            if (my_mode != FileMode.Open)
            {
                Utility.Log.error("Can not read from this file, was opened for writing.");
            }

            if (reader == null)
            {
                reader = new BinaryReader(fstream);
            }
            return reader;
        }

        public BinaryWriter getWriter()
        {
            if (my_mode != FileMode.Create)
            {
                Utility.Log.error("Can not write to this file, was opened for writing.");
            }

            if (writer == null)
            {
                writer = new BinaryWriter(fstream);
            }

            return writer;
        }

        public void close()
        {
            fstream.Flush();
            fstream.Close();
        }

        public void add_Top_Group(Group g)
        {
            // TODO: Order matters
            groups.Add(g);
        }

        public void write()
        {
            if (groups.Count == 0)
            {
                Utility.Log.error("ESM has no data.");
            }
                        
            fstream = new FileStream(file_name, FileMode.Create);
            my_mode = FileMode.Create;

            BinaryWriter bw = getWriter();

            tes4.write(bw);

            foreach (Group g in groups)
            {
                g.write(bw);
            }


        }

        public List<Group> read()
        {
            if (my_mode != FileMode.Open)
            {
                Utility.Log.error("ESM can not be read, is opened in write mode");
            }

            if (read_)
            {
                return groups;
            }
            
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

            read_ = true;
            return (groups);
        }


              

    } 
 
}
