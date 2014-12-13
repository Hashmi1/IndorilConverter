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

namespace TES5 
{
    class ESM
    {

        public List<Group> groups { get; private set; }
        private TES4 header = new TES4(1);
        
        public ESM()
        {
            groups = new List<Group>();
            header = new TES4(1);
        }

        public void add_masters(params string[] files)
        {
            header.add_masters(files);
        }

        public void add_group(Group g)
        {
            groups.Add( g);
        }

        public void add_group(List<Group> grps)
        {
            foreach (Group g in grps)
            {
                groups.Add(g);
            }            
        }

        public static ESM read_from_file(string filename)
        {
            if (!File.Exists(filename))
            {
                Log.error("TES5.ESM asked to open non-existing file: " +filename);
            }

            ESM esm = new ESM();

            FileStream fstream = new FileStream(filename,FileMode.Open);
            BinaryReader reader = new BinaryReader(fstream);

            esm.header.read(reader);

            while (fstream.Position < fstream.Length)
            {
                Group g = new Group();
                g.read(reader);
                esm.groups.Add(g);
            }

            return esm;
        }

        public void write_to_file(string filename)
        {
            FileStream fstream = new FileStream(filename,FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fstream);
                                   
            header.write(writer);

            foreach (Group g in groups)
            {
                g.recalculate_size();
                g.write(writer);
            }
        }
        
    }
}
