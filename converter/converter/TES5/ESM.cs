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

        private List<Group> groups;

        public List<Group> getGroups()
        {
            if (groups == null)
            {
                Log.error("No Groups in ESM.");
            }

            return groups;
        }
        
        private TES4 header = new TES4(1);
        private string filename_;

        public ESM(string filename)
        {
            this.filename_ = filename;
            groups = new List<Group>();
            header = new TES4(1);
        }

        public Group find_TOP_group_OR_FAIL(string grup, string error="")
        {
            Group g = try_find_TOP_group(grup);

            if (g == null)
            {
                Log.error("Group: " + grup + " was not found ESM:" + filename_ + '\n' + error);
            }

            return g;
        }

        public Group try_find_TOP_group(string grup)
        {
            foreach (Group g in groups)
            {
                if (g.isType(Group.TYPE.TOP) && g.hasLabel(grup))
                {
                    return g;
                }                
                
            }

            return null;
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
                        
            ESM esm = new ESM(filename);
            esm.filename_ = filename;

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
