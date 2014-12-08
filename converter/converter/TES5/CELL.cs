using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES5
{
    class CELL : Record
    {
        public CELL(string editor_id)
            : base("CELL", editor_id)
        {
            this.editor_id = editor_id;

            cell_references = new TES5.Group(id, TES5.Group.TYPE.CELL_CHILD);
            temp_references = new TES5.Group(id, TES5.Group.TYPE.TEMP_REFR);
            cell_references.addSubGroup(temp_references);
        }
        
        
        public enum FLAGS :ushort
        {
            Interior = 0x0001,
            HasWater = 0x0002
        }
        
        ushort data_flags;

        public class XCLL
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
                //Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0020);
                //Inheritflags_minuscontrols = (uint)BinaryFlag.set(Inheritflags_minuscontrols, 0x0040);
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
        public bool HasWater;

        public string editor_id;
        public string full_name;

        public TES5.Group cell_references;
        public TES5.Group temp_references;
        
        public List<TES5.REFR> references;
        public XCLL lighting;

        private void pack_flags()
        {
            data_flags = 0;
            if (Interior) { data_flags = BinaryFlag.set((ushort)data_flags, (ushort)FLAGS.Interior); }
            if (HasWater) { data_flags = BinaryFlag.set((ushort)data_flags, (ushort)FLAGS.HasWater); }
        }
        
        public void addWater(float height)
        {
            HasWater = true;
            addField(new Field("XCLW", Binary.toBin((float)0)));

            TES5.REFR water_mesh = new REFR(0, 0, 0, height, 0, 0, 0, 10);

        }

        public void get_bounds()
        {
            float min_x = float.NaN;
            float min_y = float.NaN;

            float max_x = float.NaN;
            float max_y = float.NaN;


            foreach (TES5.REFR refr in references)
            {
                # region initialize
                if (min_x == float.NaN)
                {
                    min_x = refr.placement_.x;
                }

                if (max_x == float.NaN)
                {
                    max_x = refr.placement_.x;
                }
                
                if (min_y == float.NaN)
                {
                    min_y = refr.placement_.y;
                }

                if (max_y == float.NaN)
                {
                    max_y = refr.placement_.y;
                }
                # endregion

                # region assign min/max
                if (refr.placement_.x < min_x)
                {
                    min_x = refr.placement_.x;
                }

                if (refr.placement_.x > max_x)
                {
                    max_x = refr.placement_.x;
                }

                if (refr.placement_.y < min_y)
                {
                    min_y = refr.placement_.y;
                }

                if (refr.placement_.y > max_y)
                {
                    max_y = refr.placement_.y;
                }
                # endregion

            }

            min_x = min_x - 1024;
            min_y = min_y - 1024;

            max_x = max_x + 1024;
            max_y = max_y + 1024;
        }

        public void finalize()
        {
            addField(new TES5.Field("EDID", Text.editor_id(editor_id)));
            addField(new TES5.Field("FULL", Text.zstring(shorten_name(full_name))));
            addField(new TES5.Field("XCLL", lighting.toBin()));
            addField(new TES5.Field("LTMP", Binary.toBin((UInt32)0)));
            addField(new TES5.Field("DATA", Binary.toBin(data_flags)));
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
