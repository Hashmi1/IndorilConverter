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
using Utility;
using System.IO;

namespace TES5
{
    // TODO: ownership / scale / exterior cell / door /lock etc.
    class REFR : Record
    {
        public class placement
        {
            public float x { get; private set; }
            public float y { get; private set; }
            public float z { get; private set; }
            public float xR { get; private set; }
            public float yR { get; private set; }
            public float zR { get; private set; }
            

            public placement(float x, float y, float z, float xR, float yR, float zR)
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

        class portal_data
        {
            float x;
            float y;
            float z;
            float xR;
            float yR;
            float zR;
            UInt32 destination_formid;

            public portal_data(float x, float y, float z, float xR, float yR, float zR, UInt32 destination_formid)
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

        public placement placement_;

        public void attach_portal(REFR other_end)
        {
            portal_data portal_positioning = new portal_data(other_end.placement_.x, other_end.placement_.y, other_end.placement_.z, other_end.placement_.xR, other_end.placement_.yR, other_end.placement_.zR, other_end.id);
            Field XTEL = new Field("XTEL", portal_positioning.toBin());
            fields.Add(XTEL);
        }

       

        public REFR(uint formid,float x, float y, float z, float xR, float yR, float zR,float scale = 1f) : base("REFR")
        {
            
            placement_ = new placement(x, y, z, xR, yR, zR);
            

            Field NAME = new Field("NAME", Binary.toBin(formid));
            Field DATA = new Field("DATA",placement_.toBin());

            
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
