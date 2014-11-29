﻿/*
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
using Utility;
using System.IO;

namespace Convert
{
    class LandTextures
    {
        static void search(TES5.Group group)
        {
            foreach (TES5.Record record in group.records)
            {
                if (record.isType("LAND"))
                {
                    foreach (TES5.Field field in record.fields)
                    {
                        if (field.isType("ATXT"))
                        {

                        }
                    }
                }
            }
        }

        public static void test()
        {

            TES5.ESM esm = new TES5.ESM("tes5/tesannwyn.esp", FileMode.Open);
            esm.read();
            
            foreach (TES5.Group grp in esm.groups)
            {
                //foreach (TES5.Group in grp
            }
            
        }


        public static void grab_files()
        {
            string tes3_path = "tes3/data/textures/";
            string tes5_path = "tes5/morrowind/";

            foreach (string texture in Stored.land_texture_list)
            {
                string f_source = tes3_path + texture;
                string f_destin = tes5_path + texture;

                if (!File.Exists(f_source))
                {
                    Log.info("Not found: " +f_source);
                    continue;
                }

                File.Copy(f_source, f_destin,true);
            }

            //Stored.land_texture_list
            //File.Copy();
        }


        public static TES5.Group[] start()
        {   
            TES5.Group grup_txst = new TES5.Group("TXST");
            TES5.Group grup_ltex = new TES5.Group("LTEX");
                        
            foreach (string texture in Stored.land_texture_list)
            {

                TES5.Record txst = make(texture);
                TES5.Record ltex = make_ltex(txst.id,texture);

                grup_txst.addRecord(txst);
                grup_ltex.addRecord(ltex);
                
            }

            TES5.Group[] grps = new TES5.Group[2];
            grps[0] = grup_txst;
            grps[1] = grup_ltex;

            return grps;
                        
        }

        static TES5.Record make_ltex(uint formid,string texture)
        {
            TES5.Record ltex = new TES5.Record("LTEX");
            ltex.addField(new TES5.Field("EDID", Text.editor_id("LAND_" + texture.Replace(".dds", ""))));
            ltex.addField(new TES5.Field("TNAM", Binary.toBin(formid)));
            ltex.addField(new TES5.Field("HNAM", new byte[2]{30,30}));
            ltex.addField(new TES5.Field("SNAM", new byte[1] { 30 }));
            
            return ltex; 
        }

        static TES5.Record make(string texture)
        {
            string path = "morrowind/";

            TES5.Record txset = new TES5.Record("TXST");
            txset.addField(new TES5.Field("EDID", Text.editor_id(texture.Replace(".dds",""))));
            txset.addField(new TES5.Field("OBND", new byte[12]));
            txset.addField(new TES5.Field("TX00", Text.zstring(path+texture)));
            txset.addField(new TES5.Field("TX01", Text.zstring(path + "/normal/" +texture.Replace(".dds","_.dds"))));

            ushort flag = (ushort)BinaryFlag.set(0, 0x01);
            txset.addField(new TES5.Field("DNAM", Binary.toBin(flag)));

            return txset;
        }


    }
}