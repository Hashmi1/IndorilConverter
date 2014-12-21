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

namespace TES5
{
    class NVNM
    {
        /////////////////////////////////////////////////////////////////////
        // Field Data
        /////////////////////////////////////////////////////////////////////
        UInt32 world_formid;
        UInt32 cell_formid;
        short x;
        short y;

        List<Vertex> vertices = new List<Vertex>();
        List<Triangle> triangles = new List<Triangle>();
        List<ExternalConnection> ext_connections = new List<ExternalConnection>();
        List<DoorConnection> doors = new List<DoorConnection>();
        List<UInt16> cover_tris = new List<ushort>();

        UInt32 divisor;
        float maxXD;
        float maxYD;

        float minx;
        float miny;
        float minz;

        float maxx;
        float maxy;
        float maxz;
        
        List<Segment> segments = new List<Segment>();
        //////////////////////////////////////////////////////////////////////////////////////


        public NVNM(Field f)
        {
            read(f);
        }

        public NVNM(UInt32 cell ,List<Vertex> verts, List<Triangle> tris)
        {

            this.world_formid = 0;
            this.cell_formid = cell;
            this.vertices = verts;
            this.triangles = tris;

            divisor = calculate_divisor();
            calculate_bounds();

            segments = create_segments().ToList();

        }
        
        void calculate_bounds()
        {
            float minx_ = vertices[0].x;
            float miny_ = vertices[0].y;
            float minz_ = vertices[0].z;

            float maxx_ = vertices[0].x;
            float maxy_ = vertices[0].y;
            float maxz_ = vertices[0].z;


            foreach (Vertex v in vertices)
            {
                if (v.x > maxx_)
                {
                    maxx_ = v.x;
                }

                if (v.y > maxy_)
                {
                    maxy_ = v.y;
                }

                if (v.z > maxz_)
                {
                    maxz_ = v.z;
                }


                if (v.x < minx_)
                {
                    minx_ = v.x;
                }

                if (v.y < miny_)
                {
                    miny_ = v.y;
                }

                if (v.z < minz_)
                {
                    minz_ = v.z;
                }

            }

            minx = minx_;
            miny = miny_;
            minz = minz_;

            maxx = maxx_;
            maxy = maxy_;
            maxz = maxz_;

            maxXD = Math.Abs(maxx - minx) / divisor;
            maxYD = Math.Abs(maxy - miny) / divisor;
        }

        UInt32 calculate_divisor()
        {
            float div = Math.Min(12,2.6f * triangles.Count + 0.011f);
            return (uint)div;
        }

        public class Segment
        {
            public uint length;
            public List<UInt16> tris = new List<ushort>();
        }

        public class Vertex
        {
            public float x = 0;
            public float y = 0;
            public float z = 0;

            public Vertex(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public void scale(float s)
            {
                x = x * s;
                y = y * s;
                z = z * s;
            }

        }

        public class Triangle
        {
            public ushort vert1;
            public ushort vert2;
            public ushort vert3;

            public ushort neigh1;
            public ushort neigh2;
            public ushort neigh3;

            public ushort cover_marker = 2048;
            public ushort cover_flag = 0;

            
            
        }

        public class ExternalConnection
        {
            public UInt32 unknown;
            public UInt32 formid;
            public UInt16 tri;
        }

        public class DoorConnection
        {
            public UInt16 tri;
            public UInt32 unknown;
            public UInt32 door;
        }

        Segment[] create_segments()
        {
            Segment[] recon_segs = new Segment[divisor*divisor];
            for (int i = 0; i < recon_segs.Length; i++)
            {
                recon_segs[i] = new Segment();
            }

            float x_step = Math.Abs(maxx - minx) / divisor;
            float y_step = Math.Abs(maxy - miny) / divisor;

            int segment = 0;

            for (float y = miny; y < maxy - 0.1; y += y_step)
            {

                for (float x = minx; x < maxx - 0.1; x += x_step)
                {

                    for (int i = 0; i < triangles.Count; i++)
                    {
                        Triangle t = triangles[i];

                        Vertex[] vlst = { vertices[t.vert1], vertices[t.vert2], vertices[t.vert3]};
                        
                        foreach (Vertex v in vlst)
                        {
                            if (v.x <= x+x_step && v.x > x && v.y <= y+y_step && v.y > y)
                            {
                                recon_segs[segment].length++;
                                recon_segs[segment].tris.Add((ushort)i);
                                break;
                            }
                        }
                        

                    }

                    segment++;
                }
            }

            return recon_segs;
        }
             
        public Field pack()
        {
            
            MemoryStream mstream = new MemoryStream();
            BinaryWriter w = new BinaryWriter(mstream);

            w.Write((UInt32)0xC);
            w.Write((UInt32)2783551548);
            w.Write((UInt32)world_formid);
            w.Write((UInt32)cell_formid);
            
            w.Write((UInt32)vertices.Count);
            foreach (Vertex v in vertices)
            {
                w.Write((float)v.x);
                w.Write((float)v.y);
                w.Write((float)v.z);
            }

            w.Write((UInt32)triangles.Count);
            foreach (Triangle t in triangles)
            {
                w.Write((UInt16)t.vert1);
                w.Write((UInt16)t.vert2);
                w.Write((UInt16)t.vert3);

                w.Write((UInt16)t.neigh1);
                w.Write((UInt16)t.neigh2);
                w.Write((UInt16)t.neigh3);

                w.Write((UInt16)t.cover_marker);
                w.Write((UInt16)t.cover_flag);
            }

            w.Write((UInt32)ext_connections.Count);
            foreach (ExternalConnection e in ext_connections)
            {
                w.Write((UInt32)e.unknown);
                w.Write((UInt32)e.formid);
                w.Write((UInt16)e.tri);
            }

            w.Write((UInt32)doors.Count);
            foreach (DoorConnection d in doors)
            {
                w.Write((UInt16)d.tri);
                w.Write((UInt32)d.unknown);
                w.Write((UInt32)d.door);
            }

            w.Write((UInt32)cover_tris.Count);
            foreach (UInt16 ct in cover_tris)
            {
                w.Write((UInt16)ct);                
            }

            divisor = calculate_divisor();
            calculate_bounds();

            w.Write((UInt32)divisor);
            w.Write((float)maxXD);
            w.Write((float)maxYD);
            w.Write((float)minx);
            w.Write((float)miny);
            w.Write((float)minz);
            w.Write((float)maxx);
            w.Write((float)maxy);
            w.Write((float)maxz);

            segments = create_segments().ToList();

            foreach (Segment s in segments)
            {
                w.Write((UInt32)s.length);

                foreach (UInt16 tindex in s.tris)
                {
                    w.Write((UInt16)tindex);
                }

            }

            return new Field("NVNM", (mstream.ToArray()));
        }
           
        void read(Field f)
        {

            BinaryReader br = f.getData();

            uint unknown = br.ReadUInt32(); // unknown
            
            uint loc_marker = br.ReadUInt32(); // loc-marker
           
            world_formid = br.ReadUInt32();

            if (world_formid != 0)
            {
                x = br.ReadInt16();
                y = br.ReadInt16();
            }
            else
            {
                cell_formid = br.ReadUInt32();
            }

            uint nverts = br.ReadUInt32();

            for (int i = 0; i < nverts; i++)
            {

                Vertex v = new Vertex(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());                
                vertices.Add(v);
            }

            uint ntris = br.ReadUInt32();

            for (int i = 0; i < ntris; i++)
            {

                Triangle t = new Triangle();
                t.vert1 = br.ReadUInt16();
                t.vert2 = br.ReadUInt16();
                t.vert3 = br.ReadUInt16();

                t.neigh1 = br.ReadUInt16();
                t.neigh2 = br.ReadUInt16();
                t.neigh3 = br.ReadUInt16();

                t.cover_marker = br.ReadUInt16();
                t.cover_flag = br.ReadUInt16();
                
                triangles.Add(t);
            }

            uint nconns = br.ReadUInt32();
            for (int i = 0; i < nconns; i++)
            {
                ExternalConnection ec = new ExternalConnection();
                ec.unknown = br.ReadUInt32();
                ec.formid = br.ReadUInt32();
                ec.tri = br.ReadUInt16();
            }

            uint ndoors = br.ReadUInt32();
            for (int i = 0; i < ndoors; i++)
            {
                DoorConnection d = new DoorConnection();

                d.tri = br.ReadUInt16();
                d.unknown = br.ReadUInt32();
                d.door = br.ReadUInt32();
            }
            uint nctris = br.ReadUInt32();
            for (int i = 0; i < nctris; i++)
            {
                cover_tris.Add(br.ReadUInt16());
            }

            divisor = br.ReadUInt32();

            maxXD = br.ReadSingle();
            maxYD = br.ReadSingle();

            minx = br.ReadSingle();
            miny = br.ReadSingle();
            minz = br.ReadSingle();

            maxx = br.ReadSingle();
            maxy = br.ReadSingle();
            maxz = br.ReadSingle();

            uint num_segments = divisor * divisor;

            for (int i = 0; i < num_segments; i++)
            {
                Segment sg = new Segment();
                sg.length = br.ReadUInt32();

                for (int j = 0; j < sg.length; j++)
                {
                    sg.tris.Add(br.ReadUInt16());
                }

                segments.Add(sg);
            }


            int remaining_data = (int)(br.BaseStream.Length - br.BaseStream.Position);
            if (remaining_data != 0)
            {
                Log.error("NVNM ending error.");
            }

            
        }
    }
}
