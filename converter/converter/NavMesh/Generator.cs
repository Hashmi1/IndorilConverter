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
using System.Runtime.InteropServices;
using TES5;
using Utility;
using System.IO;

namespace NavMesh
{
    class Generator
    {

        ///////////////////////////////////////////////////////////////////////////////////
        // C++ DLLs
        //////////////////////////////////////////////////////////////////////////////////

        [DllImport("D:\\niflib-master\\niflib\\obj\\Release - DLL\\niflib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool start_session(string output_file);

        [DllImport("D:\\niflib-master\\niflib\\obj\\Release - DLL\\niflib.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool place_nif(string model, float x, float y, float z, float xR, float yR, float zR, float scale);

        [DllImport("D:\\niflib-master\\niflib\\obj\\Release - DLL\\niflib.dll")]
        static extern bool export_obj();

        [DllImport("recastgen.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool generate(string level, string output_file);

        //////////////////////////////////////////////////////////////////////////////////

        ESM_Index esm;
        
        private bool canUse(Record r)
        {
            if (r.try_find_field("EDID") == null)
            {
                //Log.error("This can not be, can it?"); // maybe, consider landscape, existing navmeshes etc.
                return false;
            }

            string editor_id = r.try_find_field("EDID").readString().ToLower();

            if (Config.Navmesh.ignored_objects.Contains(editor_id))
            {
                return false;
            }
            
            foreach (string t in Config.Navmesh.use_records)
            {
                if (r.isType(t))
                {
                    return true;
                }
            }

            return false;
        }

        public TES5.Record recast_to_skyrim(string file, UInt32 cell_id)
        {
            try
            {

                List<TES5.NVNM.Vertex> vertices = new List<TES5.NVNM.Vertex>();
                List<TES5.NVNM.Triangle> triangles = new List<TES5.NVNM.Triangle>();

                uint nverts = 0;
                uint ntris = 0;

                # region parsing recastnavigation output
                TextReader fin = File.OpenText(file);

                while (fin.Peek() != -1)
                {
                    string line = fin.ReadLine().Trim();

                    if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] parsed = line.Split(' ');

                    if (line.StartsWith("#nverts"))
                    {
                        nverts = uint.Parse(parsed[1]);
                    }

                    else if (line.StartsWith("#ntris"))
                    {
                        ntris = uint.Parse(parsed[1]);
                    }

                    else if (parsed[0].Equals("v"))
                    {
                        float[] vtx = new float[3] { float.Parse(parsed[1]), float.Parse(parsed[2]), float.Parse(parsed[3]) };

                        vtx = Matlib.scale(vtx, 50);
                        vtx = Matlib.rotateX(vtx);

                        vertices.Add(new TES5.NVNM.Vertex(vtx[0], vtx[1], vtx[2]));
                    }

                    else if (parsed[0].Equals("f"))
                    {
                        TES5.NVNM.Triangle tri = new TES5.NVNM.Triangle();
                        tri.vert1 = (ushort)(ushort.Parse(parsed[1]) - 1);
                        tri.vert2 = (ushort)(ushort.Parse(parsed[2]) - 1);
                        tri.vert3 = (ushort)(ushort.Parse(parsed[3]) - 1);

                        string line2 = fin.ReadLine().Trim();

                        string[] parsed_line2 = line2.Split(' ');

                        if (!parsed_line2[0].Equals("#neighbours"))
                        {
                            Log.error("Neighbour data not found for generated navmesh");
                        }

                        tri.neigh1 = ushort.Parse(parsed_line2[1]);
                        tri.neigh2 = ushort.Parse(parsed_line2[2]);
                        tri.neigh3 = ushort.Parse(parsed_line2[3]);

                        triangles.Add(tri);

                    }


                }
                # endregion

                if (triangles.Count != ntris || vertices.Count != nverts)
                {
                    Log.error("Navmesh generation error. Not all vertices or faces imported.");
                }

                TES5.NVNM navmesh = new TES5.NVNM(cell_id, vertices, triangles);
                TES5.Record navm = new TES5.Record("NAVM", true);

                navm.addField(navmesh.pack());

                return navm;
            }

            catch (IOException e)
            {
                Log.error("Error reading recast output file: " + file + " consider regenerating.\n" + e.Message);
                return null;
            }


        }

        public void make_navmesh(TES5.Record c)
        {
            #region overwrite
            if (esm.get_references(c).find_record_type("NAVM").Count() != 0)
            {
                Field EDID = c.try_find_field("EDID");
                Field XCLC = c.try_find_field("XCLC");

                string identifier = "";

                if (EDID != null)
                {
                    identifier = EDID.readString();
                }

                if (XCLC != null)
                {
                    BinaryReader br = XCLC.getData();
                    int x = br.ReadInt32();
                    int y = br.ReadInt32();

                    identifier = identifier + " Grid: " + x + "," + y;
                }

                bool overwrite = Log.ask("Cell " + identifier + " already has a NavMesh, overwrite?");

                if (!overwrite)
                {
                    Log.non_fatal_error("Skipping: " + identifier);
                    return;
                }

                Log.confirm("Overwriting has not been implemented yet. Remove the Navmesh in the CS and try again.");
                Log.non_fatal_error("Skipping: " + identifier);
                return;
            }
            #endregion

            string navmesh_disk_path = "navmeshes\\" + c.find_field_OR_FAIL("EDID", "").readString() + ".obj";

            if (File.Exists(navmesh_disk_path))
            {
                Log.info_colored("Found Pre-Generated NavMesh for " + c.find_field_OR_FAIL("EDID", "").readString(),ConsoleColor.Magenta);
                TES5.Record loaded_navmesh = recast_to_skyrim(navmesh_disk_path, c.id);
                esm.add_reference(c, loaded_navmesh);
                return;
            }
            
            start_session(Config.Paths.Temporary.cell_obj);
     
            Group refr_group = esm.get_references(c);

            foreach (Record refr_rec in refr_group.records)
            {

                REFR refr = new REFR(refr_rec);

                Record refr_base = esm.get_record(refr.base_id);

                if (!canUse(refr_base))
                {
                    continue;
                }


                Field MODL = refr_base.try_find_field("MODL");
                if (MODL == null)
                {
                    continue;
                }

                string model_path = Config.Paths.sk_meshes + MODL.readString();

                if (model_path.ToLower().Contains("_marker.nif"))
                {
                    continue;
                }

                place_nif(model_path, refr.loc.x, refr.loc.y, refr.loc.z, refr.loc.xR, refr.loc.yR, refr.loc.zR, refr.scale);
                
                
            }

            export_obj();
            generate(Config.Paths.Temporary.cell_obj, navmesh_disk_path);
            TES5.Record r = recast_to_skyrim(navmesh_disk_path, c.id);
            esm.add_reference(c,r);

        }

        public void make_navmesh(List<Record> lst_cells)
        {
            int count = 0;
            foreach (Record c in lst_cells)
            {
                Log.info(c.find_field_OR_FAIL("EDID","").readString() + ": " + count++ + "/" + lst_cells.Count);
                
                make_navmesh(c);
            }
            
        }

        public void load_file(ESM esm_non_indexed)
        {
            esm = new ESM_Index();
            esm.make(esm_non_indexed);
        }

        public void make_navmesh()
        {

            make_navmesh(esm.get_record_list("CELL"));



        }


        public void make_navmesh(string editor_id)
        {
            Record c = esm.get_record(editor_id);
            make_navmesh(c);
        }

        
    }
}
