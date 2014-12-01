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
using Utility;
using System.IO;

namespace Convert
{
    class LTEX
    {
        public static bool done = false;

        private struct mw_ltex
        {
            public string editor_id;
            public int index;
            public string texture_path;
        }

        private static List<mw_ltex> get_mw_land_textures(string file)
        {
            List<mw_ltex> lst = new List<mw_ltex>();

            TES3.ESM.open(file);
            
            while (TES3.ESM.find("LTEX"))
            {

                TES3.Record ltex = new TES3.Record();
                ltex.read();

                mw_ltex mw_ltex_ = new mw_ltex();


                foreach (TES3.SubRecord sr in ltex.subRecords)
                {
                    if (sr.isType("NAME"))
                    {
                        mw_ltex_.editor_id = Text.trim(new string(sr.getData().ReadChars(sr.size)));
                    }

                    else if (sr.isType("DATA"))
                    {
                        mw_ltex_.texture_path = Text.trim(new string (sr.getData().ReadChars(sr.size)));
                    }

                    else if (sr.isType("INTV"))
                    {
                        mw_ltex_.index = sr.getData().ReadInt32();
                    }
                }

                lst.Add(mw_ltex_);

            }

            TES3.ESM.close();
            return lst;
        }

        
        public static TES5.Group[] convert(string file)
        {
            List<mw_ltex> mw_textures = get_mw_land_textures(file);

            TES5.Group grup_txst = new TES5.Group("TXST");
            TES5.Group grup_ltex = new TES5.Group("LTEX");
                        
            foreach (mw_ltex t in mw_textures)
            {
                
                TES5.Record txst = make_txst(t.editor_id,t.texture_path);
                TES5.Record ltex = make_ltex(txst.id,t.editor_id);

                LAND.add_texture(t.index, t.editor_id, t.texture_path, ltex.id);

                grup_txst.addRecord(txst);
                grup_ltex.addRecord(ltex);
                
            }

            TES5.Group[] grps = new TES5.Group[2];
            grps[0] = grup_txst;
            grps[1] = grup_ltex;

            done = true;

            return grps;
                        
        }

        static TES5.Record make_ltex(uint formid,string editor_id)
        {
            TES5.Record ltex = new TES5.Record("LTEX");
            ltex.addField(new TES5.Field("EDID", Text.editor_id("LAND_" + editor_id)));
            ltex.addField(new TES5.Field("TNAM", Binary.toBin(formid)));
            ltex.addField(new TES5.Field("HNAM", new byte[2]{2,0}));
            ltex.addField(new TES5.Field("SNAM", new byte[1] { 30 }));
            
            return ltex; 
        }

        static TES5.Record make_txst(string editor_id, string texture)
        {
            texture = texture.Replace(".tga", ".dds");

            string path = "morrowind\\";

            TES5.Record txset = new TES5.Record("TXST");
            txset.addField(new TES5.Field("EDID", Text.editor_id(editor_id)));
            txset.addField(new TES5.Field("OBND", new byte[12]));
            txset.addField(new TES5.Field("TX00", Text.zstring(path+texture)));
            txset.addField(new TES5.Field("TX01", Text.zstring(path + texture.Replace(".dds","_n.dds"))));

            ushort flag = (ushort)BinaryFlag.set(0, 0x01);
            txset.addField(new TES5.Field("DNAM", Binary.toBin(flag)));

            return txset;
        }


    }
}
