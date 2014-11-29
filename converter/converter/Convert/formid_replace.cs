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
using Utility;

namespace Convert
{
    class formid_replace
    {
        static TextWriter tx = File.CreateText("Alog.txt");

        static void search_record(TES5.Record record)
        {
            int vtxt = 0;
            int atxt = 0;

            //record.id = record.id - 0x01000000;

            foreach (TES5.Field field in record.fields)
            {
                if (field.isType("VTXT"))
                {
                    //fix_field(field);
                    vtxt++;
                }

                if (field.isType("ATXT"))
                {
                    atxt++;        
                }

                if (field.isType("BTXT"))
                {
                    Log.error("NOOOOO!");
                }

            }

            if (atxt > 0 || vtxt > 0)
            { tx.WriteLine("Record found: " + atxt + " | " + vtxt); }
            
        }

        static void fix_field(TES5.Field field)
        {
            UInt32 formid;
            byte quad;
            byte unknown_atxt;
            UInt16 layer;

            BinaryReader reader = new BinaryReader(new MemoryStream());
            field.data.CopyTo(reader.BaseStream);
            reader.BaseStream.Position = 0;

            formid = reader.ReadUInt32();
            quad = reader.ReadByte();
            unknown_atxt = reader.ReadByte();
            layer = reader.ReadUInt16();

            formid = formid + 0x1000000;

            //Log.info(formid.ToString("X"));
            
            field.data.Position = 0;
            BinaryWriter writer = new BinaryWriter(field.data);
            writer.Write(formid);
            writer.Write(quad);
            writer.Write(unknown_atxt);
            writer.Write(layer);
            field.data.Position = 0;

        }
        
        static void search_group(TES5.Group group)
        {
            foreach (TES5.Record record in group.records)
            {
                if (record.isType("LAND"))
                {
                    search_record(record);
                }
            }

            foreach (TES5.Group sub_group in group.subGroups)
            {
                search_group(sub_group);
            }
            
        }

        public static void start()
        {
            TES5.ESM esm = new TES5.ESM("tes5/tesannwyn.esp", FileMode.Open);

            esm.read();
            
            foreach (TES5.Group group in esm.groups)
            {
                search_group(group);
            }

            //LandTextures.start(esm);
            tx.Close();
        }
    }
}
