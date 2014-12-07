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

        
        
    }
}
