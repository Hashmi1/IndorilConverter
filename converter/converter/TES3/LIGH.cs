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

namespace TES3
{
    class LIGH : Record
    {
            public string editor_id = null;
            public string full_name = null;
            public string icon = null;
            public string model = null;
            public float weight;
            public uint value;
            public uint time;
            public uint radius;
            public byte red;
            public byte green;
            public byte blue;
            
            public uint flags;
                    
            public bool Dynamic;
            public bool Can_Carry;
            public bool Negative;
            public bool Flicker;
            public bool Fire;
            public bool Off_Default;
            public bool Flicker_Slow;
            public bool Pulse;
            public bool Pulse_Slow;

            public LIGH()
            {
            }
            public void read()
            {
                base.read();
                foreach (SubRecord srec in base.subRecords)
                {

                    if (srec.isType("NAME"))
                    {
                        editor_id = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    else if (srec.isType("FNAM"))
                    {
                        full_name = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    else if (srec.isType("ITEX"))
                    {
                        icon = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                    }

                    else if (srec.isType("MODL"))
                    {
                        if (srec.size > 1)
                        {
                            model = Text.trim(new string(srec.getData().ReadChars(srec.size)));
                        }
                        
                    }

                    else if (srec.isType("LHDT"))
                    {
                        BinaryReader br = srec.getData();
                        weight = br.ReadSingle();
                        value = br.ReadUInt32();
                        time = br.ReadUInt32();
                        radius = br.ReadUInt32();
                        red = br.ReadByte();
                        green = br.ReadByte();
                        blue = br.ReadByte();
                        br.ReadByte();
                        flags = br.ReadUInt32();
                        read_flags();
                    }


                }

                TypeIndex.getInstance().put(this.editor_id, TypeIndex.TYPE.LIGH); // Mark self as LIGH
            }

            private void read_flags()
            {
                Dynamic = BinaryFlag.isSet(flags, (ushort)0x0001);
                Can_Carry = BinaryFlag.isSet(flags, (ushort)0x0002);
                Negative = BinaryFlag.isSet(flags, (ushort)0x0004);
                Flicker = BinaryFlag.isSet(flags, (ushort)0x0008);
                Fire = BinaryFlag.isSet(flags, (ushort)0x0010);
                Off_Default = BinaryFlag.isSet(flags, (ushort)0x0020);
                Flicker_Slow = BinaryFlag.isSet(flags, (ushort)0x0040);
                Pulse = BinaryFlag.isSet(flags, (ushort)0x0080);
                Pulse_Slow = BinaryFlag.isSet(flags, (ushort)0x0100);
            }


        }

    
}
