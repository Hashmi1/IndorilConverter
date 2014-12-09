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
        private Record tes4;


        public ESM()
        {
            groups = new List<Group>();
            tes4 = new Record();
        }

        public ESM(List<Group> grps_)
        {
            groups = grps_;
            tes4 = new Record();
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

            esm.tes4.read(reader);

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

            tes4 = get_tes4_template(1);
            tes4.write(writer);

            foreach (Group g in groups)
            {
                g.recalculate_size();
                g.write(writer);
            }
        }

        private Record get_tes4_template(int num_records)
        {
         
            Record tes4;
            ESM file = ESM.read_from_file("tes5\\template01.esp");
            tes4 = file.tes4;

            Field hedr = tes4.fields[0];
            hedr.data = new MemoryStream();
            BinaryWriter b_writer = new BinaryWriter(hedr.data);

            b_writer.Write(1.7f);
            b_writer.Write(num_records);
            b_writer.Write(FormID.getHEDRObj());

            hedr.data.Position = 0;
            
            return tes4;
                    
        }

    }
}
