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

namespace TES5.NPC
{
    
    class ACBS : Field
    {
        enum FLAGS : uint
        {
            Female = 0x01,
            Essential = 0x02,
            Is_CharGen_Face_Preset = 0x04,
            Respawn = 0x08,
            Auto_calc_stats = 0x10,
            Unique = 0x20,
            Doesnt_affect_stealth_meter = 0x40,
            PC_Level_Mult = 0x80,
            Audio_template = 0x100,
            Protected = 0x800,
            Summonable = 0x4000,
            Doesnt_Bleed = 0x10000,
            Owned = 0x40000,
            Opposite_Gender_Anims = 0x80000,
            Simple_Actor = 0x100000,
            looped_script = 0x200000,
            looped_audio = 0x10000000,
            Ghost = 0x20000000,
            Invulnerable = 0x80000000,

        }

        Field base_field;

        public bool isPreset
        {
            get
            {
                return BinaryFlag.isSet(flags, (uint)FLAGS.Is_CharGen_Face_Preset);
            }

            set
            {
                if (value == true)
                {
                    flags = BinaryFlag.set(flags, (uint)FLAGS.Is_CharGen_Face_Preset);
                }
                else
                {
                    flags = BinaryFlag.remove(flags, (uint)FLAGS.Is_CharGen_Face_Preset);
                }
            }
        }

        ////////////////////////////////////////////////////////////
        // Field Data
        ///////////////////////////////////////////////////////////
        public UInt32 flags;
        public UInt16 Magicka_Offset;
        public UInt16 Stamina_Offset;
        public UInt16 Level;
        public UInt16 Calc_min_level;
        public UInt16 Calc_max_level;
        public UInt16 Speed_Multiplier;
        public UInt16 Disposition_Base;
        public UInt16 Template_Data_Flag;
        public UInt16 Health_Offset;
        public UInt16 Bleedout_Override;
        ///////////////////////////////////////////////////////////
       

        public void flush()
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(mstream);
            bw.Write((UInt32)flags);
            bw.Write((UInt16)Magicka_Offset);
            bw.Write((UInt16)Stamina_Offset);
            bw.Write((UInt16)Level);
            bw.Write((UInt16)Calc_min_level);
            bw.Write((UInt16)Calc_max_level);
            bw.Write((UInt16)Speed_Multiplier);
            bw.Write((UInt16)Disposition_Base);
            bw.Write((UInt16)Template_Data_Flag);
            bw.Write((UInt16)Health_Offset);
            bw.Write((UInt16)Bleedout_Override);
            base_field.replaceData(mstream.ToArray());

        }

        public ACBS(Field f)
        {
            base_field = f;

            BinaryReader br = f.getData();
            flags = br.ReadUInt32();
            Magicka_Offset = br.ReadUInt16();
            Stamina_Offset = br.ReadUInt16();
            Level = br.ReadUInt16();
            Calc_min_level = br.ReadUInt16();
            Calc_max_level = br.ReadUInt16();
            Speed_Multiplier = br.ReadUInt16();
            Disposition_Base = br.ReadUInt16();
            Template_Data_Flag = br.ReadUInt16();
            Health_Offset = br.ReadUInt16();
            Bleedout_Override = br.ReadUInt16();
                        
        }

    }
}
