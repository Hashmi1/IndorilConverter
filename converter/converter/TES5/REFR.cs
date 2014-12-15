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

namespace TES5
{
    // TODO: ownership / scale / exterior cell / door /lock etc.
    class REFR : Record
    {
        public class Placement
        {
            public float x { get; private set; }
            public float y { get; private set; }
            public float z { get; private set; }
            public float xR { get; private set; }
            public float yR { get; private set; }
            public float zR { get; private set; }
            

            public Placement(float x, float y, float z, float xR, float yR, float zR)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.xR = xR;
                this.yR = yR;
                this.zR = zR;
                
            }

            public byte[] toBin()
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter br = new BinaryWriter(mstream);
                br.Write(x);
                br.Write(y);
                br.Write(z);
                br.Write(xR);
                br.Write(yR);
                br.Write(zR);
                return mstream.ToArray();
                
            }           
            
            
        }

        class Portal_Data
        {
            float x;
            float y;
            float z;
            float xR;
            float yR;
            float zR;
            UInt32 destination_formid;

            public Portal_Data(float x, float y, float z, float xR, float yR, float zR, UInt32 destination_formid)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.xR = xR;
                this.yR = yR;
                this.zR = zR;
                this.destination_formid = destination_formid;

            }

            public byte[] toBin()
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter br = new BinaryWriter(mstream);
                br.Write(destination_formid);
                br.Write(x);
                br.Write(y);
                br.Write(z);
                br.Write(xR);
                br.Write(yR);
                br.Write(zR);
                br.Write((UInt32)0);
                return mstream.ToArray();

            }


        }

        public void make_persistent()
        {
            this.flags = BinaryFlag.set(this.flags, 0x400);
        }

        public void add_owner(uint owner_id)
        {
            addField(new Field("XOWN", Binary.toBin((uint)(owner_id))));
        }

        public Placement loc;

        public void attach_portal(REFR other_end)
        {
            Portal_Data portal_positioning = new Portal_Data(other_end.loc.x, other_end.loc.y, other_end.loc.z, other_end.loc.xR, other_end.loc.yR, other_end.loc.zR, other_end.id);
            Field XTEL = new Field("XTEL", portal_positioning.toBin());
            fields.Add(XTEL);
        }
        
        public void configure_light()
        {
            this.setFlag(0x00010000); // make never fades to avoid pop-in /pop-out artifacts

            MemoryStream mstream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(mstream);

            float  FOV_Offset = 0f;
            float  Fade_Offset = 0f;
            float  End_Distance_Cap = 0;
            float Shadow_Depth_Bias = 50f; // increase shadow depth bias to avoid striping artifacts
            uint flags_light = 0;

            bw.Write((float)FOV_Offset);            
            bw.Write((float)Fade_Offset);
            bw.Write((float)End_Distance_Cap);
            bw.Write((float)Shadow_Depth_Bias);
            bw.Write((uint)flags_light);

            addField(new Field("XLIG",mstream.ToArray()));
        }

        public uint base_id { get; private set; }
        
        public REFR(Record r)
        {
            if (!r.isType("REFR"))
            {
                Log.error("TES5.REFR.interpret() not given REFR type record");
            }

            this.compressed = r.isCompressed();

            this.type = r.type;
            this.dataSize = r.dataSize;
            this.flags = r.flags;
            this.id = r.id;
            this.revision = r.revision;
            this.version = r.version;
            this.unknown = r.unknown;
            this.fields = r.fields;

            Field data = r.find_field_OR_FAIL("DATA", "TES5.REFR.interpret() No Placement info found in given record");
            Field name = r.find_field_OR_FAIL("NAME","REFR has no base type?");

            BinaryReader br = data.getData();
            loc = new Placement(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            this.base_id = name.getData().ReadUInt32();
        }

        public REFR(uint formid,float x, float y, float z, float xR, float yR, float zR,float scale = 1f) : base("REFR")
        {

            if ((id == 0x10036C9 ||
id == 0x10036CA ||
id == 0x10036CB ||
id == 0x10036CC ||
id == 0x1006596 ||
id == 0x1006597 ||
id == 0x1006598 || 
id == 0x1006599 ||
id == 0x100659A ||
id == 0x100659B))
            {
                Log.info(id);
            }

            base_id = formid;

            loc = new Placement(x, y, z, xR, yR, zR);
            

            Field NAME = new Field("NAME", Binary.toBin(formid));
            Field DATA = new Field("DATA",loc.toBin());

            
            fields.Add(NAME);
            if (scale != 1f)
            {
                Field XSCL = new Field("XSCL", Binary.toBin(scale));
                fields.Add(XSCL);
            }

            fields.Add(DATA);
        }

    }
}
