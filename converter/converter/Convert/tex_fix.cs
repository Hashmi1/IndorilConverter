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
    class tex_fix
    {
        static TextWriter tx = File.CreateText("Alog.txt");

        static void search_record(TES5.Record record)
        {
            // INSIDE LAND RECORD
            //bool needs_processing = false;
            int count = 0;

            int[] quad_texes = new int[4] {0,0,0,0}; 

            foreach (TES5.Field field in record.fields)
            {
                                

                if (field.isType("ATXT"))
                {
                    count++;
                    //needs_processing = true;
                    BinaryReader be = field.getData();
                    be.ReadUInt32();
                    byte quad = be.ReadByte();
                    be.ReadSByte();
                    ushort layer = be.ReadUInt16();

                    quad_texes[quad] = quad_texes[quad] + 1;

                    if (quad > 3)
                    {
                        Log.info("QUAD UNHANDLED");
                        Console.ReadLine();
                    }
                    //Log.info(layer);
                        
                }

                if (field.isType("VTXT"))
                {

                    BinaryReader br =  field.getData();
                    br.ReadInt32();
                    float opacity = br.ReadSingle();

                    if ( opacity != 1f)
                    {
                        Log.error("Wrong again!" +opacity);
                    }
                    
                }

            
                if (field.isType("BTXT"))
                {
                    Log.error("NOOOOO!");
                }

            }

            if (count > 32)
            {
                Log.error("NO");
            }

            for (int i = 0; i < quad_texes.Length; i++ )
            {
                if (quad_texes[i] > 7)
                {
                    Log.info(quad_texes[i] + " in quad " + i);
                }
            }


            
            
        }

        static void fix_field(TES5.Field field)
        {
            Log.error(-1);
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
