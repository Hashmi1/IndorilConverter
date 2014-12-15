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

namespace TES5.NPC
{
    public enum Skill_Index : int
    {
        OneHanded = 0,
        TwoHanded = 1,
        Marksman = 2,
        Block = 3,
        Smithing = 4,
        HeavyArmor = 5,
        LightArmor = 6,
        Pickpocket = 7,
        Lockpicking = 8,
        Sneak = 9,
        Alchemy = 10,
        Speechcraft = 11,
        Alteration = 12,
        Conjuration = 13,
        Destruction = 14,
        Illusion = 15,
        Restoration = 16,
        Enchanting = 17,

    }

    class DNAM : Field
    {

        public byte[] skills = new byte[18];
        public byte[] mod_skills = new byte[18];

        public UInt16 calc_health;
        public UInt16 calc_magicka;
        public UInt16 calc_stamina;
        public UInt16 unknown;
        public float far_away_model_dist;
        public byte geared_weapons;
        public byte[] unknown2 = new byte[3];

        public void pack()
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(mstream);

            bw.Write((byte[])skills);
            bw.Write((byte[])mod_skills);
            bw.Write((UInt16)calc_health);
            bw.Write((UInt16)calc_magicka);
            bw.Write((UInt16)calc_stamina);
            bw.Write((UInt16)unknown);
            bw.Write((float)far_away_model_dist);
            bw.Write((byte)geared_weapons);
            bw.Write((byte[])unknown2);

            byte[] data = mstream.ToArray();

            if (data.Length != 52)
            {
                Log.error("NPC_:DNAM should be 52 bytes");
            }

            this.replaceData(data);

        }


    }
}
