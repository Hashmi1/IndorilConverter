using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Convert.REFERENCE
{
    class REFR
    {
        static Interior_ReferenceGroup_Index int_index = new Interior_ReferenceGroup_Index();
        static Exterior_ReferenceGroup_Index ext_index = new Exterior_ReferenceGroup_Index();

        public static void make_indices(List<TES5.Group> interior_cells, List<TES5.Group> exterior_cells)
        {
            int_index.make(interior_cells);
            ext_index.make(exterior_cells);
        }

        public static void add_references(string filename, List<TES5.Group> interiors,List<TES5.Group> exteriors)
        {
            make_indices(interiors, exteriors);
            
            TES3.ESM.open(filename);
            
            while (TES3.ESM.find("CELL"))
            {
                TES3.CELL cell = new TES3.CELL();
                cell.read();

                foreach (TES3.REFR refr in cell.references)
                {
                    string refr_id = refr.editor_id;
                    uint formid = TES5.FormID.get(refr_id);
                    if (formid == 0)
                    {
                        continue;   // Reference Base not converted, so skip
                    }
                    Log.info("Adding Reference: " + refr_id);

                    TES5.REFR skyrim_reference;

                    if (refr.isPortal)
                    {
                        TES5.REFR exit = make_exit(refr, formid);
                        skyrim_reference = new TES5.REFR(formid, exit.id, refr);
                    }
                    else
                    {
                        skyrim_reference = new TES5.REFR(formid, refr.x, refr.y, refr.z, refr.xR, refr.yR, refr.zR, refr.scale);
                    }

                    if (cell.interior)
                    {
                        add_interior_references(skyrim_reference, Text.editor_id_string(cell.cell_name));
                    }

                    if (!cell.interior)
                    {
                        add_exterior_references(skyrim_reference);
                    }

                }
            }
        }

        private static void add_exterior_references(TES5.REFR refr)
        {            
            int cell_x = (int)(refr.placement_.x / 4096f);
            int cell_y = (int)(refr.placement_.y / 4096f);

            if (refr.placement_.x < 0f)
            {
                cell_x--;
            }

            if (refr.placement_.y < 0f)
            {
                cell_y--;
            }


            TES5.Group reference_group = ext_index.get_reference_group(cell_x, cell_y);

            reference_group.addRecord(refr);

        }

        private static void add_interior_references(TES5.REFR refr, string cell_id)
        {
            
            TES5.Group reference_group = int_index.get_reference_group(cell_id);
            reference_group.addRecord(refr);
                                        
        }

        private static TES5.REFR make_exit(TES3.REFR refr, uint formid)
        {
            // Place the exit marker in the world
            // TODO: reference for generic marker
            TES5.REFR exit_marker = new TES5.REFR(0, refr.portal.x, refr.portal.y, refr.portal.z, refr.portal.xR, refr.portal.yR, refr.portal.zR, 1);

            if (String.IsNullOrEmpty(refr.portal.destination_cell)) // teleports to exterior
            {
                add_exterior_references(exit_marker);
            }

            else // teleports to interior
            {
                add_interior_references(exit_marker, Text.editor_id_string(refr.portal.destination_cell));
            }

            return exit_marker;
                      
            
        }
    }
}
