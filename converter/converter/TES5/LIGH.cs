using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.IO;


# pragma warning disable 0649
namespace TES5
{
    class LIGH : Record
    {

        public Int32 Time;
        public UInt32 Radius;
        public byte r;
        public byte g;
        public byte b;
        public UInt32 Flags;
        public float Falloff_Exponent;
        public float FOV;
        public float Near_Clip;
        public float Period;
        public float Intensity_Amplitude;
        public float Movement_Amplitude;
        public UInt32 Value;
        public float Weight;

        public bool	Dynamic	;
        public bool carried;
        public bool Flicker;
        public bool Off;
        public bool FlickerSlow;
        public bool Pulse;
        public bool Spotlight;
        public bool Hemisphere;
        public bool Omnidirectional;
        
        public string editor_id = null;
        public string model = null;
        public string full_name = null;
        public string icon = null;

        public LIGH(string editor_id)
            : base("LIGH",editor_id)
        {
        }

        public void pack()
        {

            addField(new Field("EDID", Text.editor_id(editor_id)));

            if (full_name != null)
            {
                addField(new Field("FULL", Text.zstring(full_name)));
            }

            if (model != null)
            {
                addField(new Field("MODL", Text.model_path(model)));
            }

            if (icon != null)
            {
                addField(new Field("ICON", Text.zstring(icon)));
            }

            // FIELD -> DATA
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);

                bw.Write((UInt32)Time);
                bw.Write((UInt32)Radius);

                bw.Write((byte)r);
                bw.Write((byte)g);
                bw.Write((byte)b);
                bw.Write((byte)0);

                UInt32 flags = 0;

                if (Dynamic) { flags = BinaryFlag.set(flags, (UInt32)0x0001); }
                if (carried) { flags = BinaryFlag.set(flags, (UInt32)0x0002); }
                if (Flicker) { flags = BinaryFlag.set(flags, (UInt32)0x0008); }
                if (Off) { flags = BinaryFlag.set(flags, (UInt32)0x0020); }
                if (FlickerSlow) { flags = BinaryFlag.set(flags, (UInt32)0x0040); }
                if (Pulse) { flags = BinaryFlag.set(flags, (UInt32)0x0080); }
                if (Spotlight) { flags = BinaryFlag.set(flags, (UInt32)0x0400); }
                if (Hemisphere) { flags = BinaryFlag.set(flags, (UInt32)0x0800); }
                if (Omnidirectional) { flags = BinaryFlag.set(flags, (UInt32)0x1000); }

                bw.Write((UInt32)flags);
                bw.Write((float)Falloff_Exponent);
                bw.Write((float)FOV);
                bw.Write((float)Near_Clip);
                bw.Write((float)Period);
                bw.Write((float)Intensity_Amplitude);
                bw.Write((float)Movement_Amplitude);
                bw.Write((UInt32)Value);
                bw.Write((float)Weight);

                if (ms.Length != 48)
                {
                    Log.error("LIGH:DATA must be 48 bytes");
                }

                addField(new Field("DATA", ms.ToArray()));
            }
        }

    }
}
