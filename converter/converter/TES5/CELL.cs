﻿/*
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

// TODO: Make initializations in constructor

namespace TES5
{
    class CELL : Record
    {
        public CELL(string editor_id)
            : base("CELL", editor_id)
        {
            this.editor_id = editor_id;

            Interior = true;

            cell_references = new Group(id, Group.TYPE.CELL_CHILD);
            temp_references = new Group(id, Group.TYPE.TEMP_REFR);
            persistent_references = new Group(id, Group.TYPE.PERSIS_REFR);
            cell_references.addSubGroup(persistent_references);
            cell_references.addSubGroup(temp_references);

        }
        
        
        private enum FLAGS_CELL :ushort
        {
            Interior = 0x0001,
            HasWater = 0x0002
        }
        
        ushort data_flags;

        private class XCLL
        {
            public byte[] Ambient;
            public byte[] Directional;
            public byte[] FogNear;
            public float FogNear2 = 0;
            public float FogFar = 0;
            public int RotationXY = 0;
            public int RotationZ = 0;
            public float DirectionalFade = 0;
            public float FogClipDist = 0;
            public float FogPow = 0;
            public byte[] AmbientX_plus;
            public byte[] AmbientX_minus;
            public byte[] AmbientY_plus;
            public byte[] AmbientY_minus;
            public byte[] AmbientZ_plus;
            public byte[] AmbientZ_minus;
            public byte[] SpecularColor;
            public float FresnelPower = 0;
            public byte[] FogFar2;
            public float FogMax = 0;
            public float LightFadeDistancesStart = 0;
            public float LightFadeDistancesEnd = 0;
            public UInt32 Inheritflags_minuscontrols= new UInt32();

            public XCLL()
            {
                Ambient = new byte[4];
                Directional = new byte[4];
                FogNear = new byte[4];
                AmbientX_plus = new byte[4]{255,255,255,0};
                AmbientX_minus = new byte[4] { 255, 255, 255, 0 };
                AmbientY_plus = new byte[4] { 255, 255, 255, 0 };
                AmbientY_minus = new byte[4] { 255, 255, 255, 0 };
                AmbientZ_plus = new byte[4] { 255, 255, 255, 0 };
                AmbientZ_minus = new byte[4] { 255, 255, 255, 0 };
                SpecularColor = new byte[4];                
                FogFar2 = new byte[4];

                Inheritflags_minuscontrols = (uint)BinaryFlag.set(0, 0x0001);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0002);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0004);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0008);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0010);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0020);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0040);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0080);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0100);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0200);
                Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0400);

                FogPow = 1.0f;
                FresnelPower = 1.0f;
                FogMax = 1.0f;
            }

            public byte[] toBin()
            {

                MemoryStream mstream = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mstream);

                bw.Write((byte[])Ambient);
                bw.Write((byte[])Directional);
                bw.Write((byte[])FogNear);
                bw.Write((float)FogNear2);
                bw.Write((float)FogFar);
                bw.Write((int)RotationXY);
                bw.Write((int)RotationZ);
                bw.Write((float)DirectionalFade);
                bw.Write((float)FogClipDist);
                bw.Write((float)FogPow);
                bw.Write((byte[])AmbientX_plus);
                bw.Write((byte[])AmbientX_minus);
                bw.Write((byte[])AmbientY_plus);
                bw.Write((byte[])AmbientY_minus);
                bw.Write((byte[])AmbientZ_plus);
                bw.Write((byte[])AmbientZ_minus);
                bw.Write((byte[])SpecularColor);
                bw.Write((float)FresnelPower);
                bw.Write((byte[])FogFar2);
                bw.Write((float)FogMax);
                bw.Write((float)LightFadeDistancesStart);
                bw.Write((float)LightFadeDistancesEnd);
                bw.Write((UInt32)Inheritflags_minuscontrols);

                if (bw.BaseStream.Length != 92)
                {
                    Log.error("XCLL should be 92 bytes");
                }

                return mstream.ToArray();

            }

        }

        public bool Interior;
        private bool HasWater;

        public string editor_id;
        public string full_name;

        public Group cell_references;
        public Group temp_references;
        public Group persistent_references;

        private Record LTMP = null;

        private XCLL lighting = new XCLL();

        private void pack_flags()
        {
            data_flags = 0;
            if (Interior) { data_flags = BinaryFlag.set((ushort)data_flags, (ushort)FLAGS_CELL.Interior); }
            if (HasWater) { data_flags = BinaryFlag.set((ushort)data_flags, (ushort)FLAGS_CELL.HasWater); }
        }
        
        public void addWater(float height)
        {
            HasWater = true;

            float padding = 1024;

            max_x = max_x + padding;
            min_x = min_x - padding;

            max_y += padding;
            min_y -= padding;

            float size_x = Math.Abs(max_x - min_x);
            float size_y = Math.Abs(max_y - min_y);
            float size = Math.Max(size_x, size_y);

            
            if (size / 2048f <= 10.0f)
            {
                REFR water_mesh = new REFR(ACTI.get_water_instance().id, (max_x+min_x)/2f, (max_y+min_y)/2f, height, 0, 0, 0, size / 2048f);
                persistent_references.addRecord(water_mesh);
                water_mesh.make_persistent();
            }

            else
            {

                for (float x = min_x; x <= max_x; x = x + 20480)
                {
                    for (float y = min_y; y <= max_y; y = y + 20480)
                    {
                        Log.confirm("TES5.CELL addwater() FULL-SIZE: This functionality is untested. Are you sure you want to continue?");
                        REFR water_mesh = new REFR(ACTI.get_water_instance().id, x, y, height, 0, 0, 0, 10);
                        persistent_references.addRecord(water_mesh);
                    }
                }
            }
        }

        public void add_ambient_light(Record ltmp)
        {
            if (ltmp == null)
            {
                Log.error("NULL ltmp assigned to TES5:CELL");
            }
            this.LTMP = ltmp;
        }

        float min_x = float.NaN;
        float min_y = float.NaN;

        float max_x = float.NaN;
        float max_y = float.NaN;
                
        public void update_bounds(float x, float y)
        {
            
                # region initialize
                if (float.IsNaN(min_x))
                {
                    min_x = x;
                }

                if (float.IsNaN(max_x))
                {
                    max_x = x;
                }
                
                if (float.IsNaN(min_y))
                {
                    min_y = y;
                }

                if (float.IsNaN(max_y))
                {
                    max_y = y;
                }
                # endregion

                # region assign min/max
                if (x < min_x)
                {
                    min_x = x;
                }

                if (x > max_x)
                {
                    max_x = x;
                }

                if (y < min_y)
                {
                    min_y = y;
                }

                if (y > max_y)
                {
                    max_y = y;
                }
                # endregion
        
        }
        public void pack()
        {
            if (LTMP == null)
            {
                Log.error("TES5:CELL No lighting template specified");
            }

            pack_flags();
            addField(new Field("EDID", Text.editor_id(editor_id)));
            addField(new Field("FULL", Text.zstring(shorten_name(full_name))));
            addField(new Field("DATA", Binary.toBin(data_flags)));
            addField(new Field("XCLL", lighting.toBin()));
            addField(new Field("LTMP", Binary.toBin((UInt32)LTMP.id)));
            addField(new Field("XCLW", Binary.toBin((float)0)));
        }
        
        public void addToGroup(Group g)
        {
            g.addRecord(this);
            g.addSubGroup(this.cell_references);
        }

        private static string lookup_replacement(string name)
        {
            string backup = name;
            TextReader tr = File.OpenText(Config.Paths.cell_name_replace);

            while (tr.Peek() != -1)
            {
                string fle = tr.ReadLine();
                string[] splt = fle.Split('=');
                if (splt[0].Equals(name) && splt.Length > 1)
                {
                    name = splt[1];
                    if (name.Length > 33 || String.IsNullOrEmpty(name))
                    {
                        tr.Close();
                        Log.info("Invalid Replacement suggested for " + backup + ". Must be < 33 chars and not empty.");
                        Log.info("Please fix in " + Config.Paths.cell_name_replace + " and press enter.");
                        Console.ReadLine();
                        return backup;
                    }
                    return name;
                }

            }

            tr.Close();
            TextWriter tx = File.AppendText(Config.Paths.cell_name_replace);
            tx.WriteLine(name + "=");
            tx.Close();
            Log.info("CELL NAME TOO LONG Please update: " + name + " And then press enter to continue");
            Console.ReadLine();
            return backup;


        }

        private static string shorten_name(string cell_name)
        {
            string backup = cell_name;

            if (cell_name.Length < 33)
            {
                return cell_name;
            }

            string[] tok = cell_name.Split(',');
            string name = (tok[tok.Length - 1].Trim());

            while (name.Length > 33)
            {

                name = lookup_replacement(name);
            }
            return name;
        }

        
    }
}
