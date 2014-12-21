/*
Copyright(c) 2014 Hashmi1. 

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

namespace Convert.REFERENCE
{
    class REFR
    {
        static Interior_ReferenceGroup_Index int_index = new Interior_ReferenceGroup_Index();
        static Exterior_ReferenceGroup_Index ext_index = new Exterior_ReferenceGroup_Index();

        private static void make_indices(List<TES5.Group> interior_cells, List<TES5.Group> exterior_cells)
        {
            int_index.make(interior_cells);
            //ext_index.make(exterior_cells); // TODO: Remove comment after testing
        }

        private static void handle_owner(string owner, TES5.REFR refr)
        {
            if (owner == null)
            {
                return;
            }

            uint owner_id = TES5.FormID.get(owner);
            if (owner_id != 0)
            {                
                refr.add_owner(owner_id);
                return;
            }

            //Log.confirm("OWNER: '" + owner + "' has not been converted. Will not be assigned to owned references.");

        }

        public static void add_references(string filename, List<TES5.Group> interiors,List<TES5.Group> exteriors)
        {
            make_indices(interiors, exteriors);
            
            TES3.ESM.open(filename);
            
            while (TES3.ESM.find("CELL"))
            {
                TES3.CELL cell = new TES3.CELL();
                cell.read();

                // TODO: Remove after testing
                if (!cell.interior)
                {
                    continue;
                }

                Lighting_Module.Shadow shadower = new Lighting_Module.Shadow();
                shadower.mark_shadow_lights(cell.references);
                
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
                        TES5.REFR exit = make_exit(refr);
                        skyrim_reference = new TES5.REFR(formid, refr.x, refr.y, refr.z, refr.xR, refr.yR, refr.zR, refr.scale);
                        
                        exit.attach_portal(skyrim_reference);
                        skyrim_reference.attach_portal(exit);
                    }
                    else
                    {
                        skyrim_reference = new TES5.REFR(formid, refr.x, refr.y, refr.z, refr.xR, refr.yR, refr.zR, refr.scale);
                        
                        if (TypeIndex.getInstance().get(refr.editor_id) == TypeIndex.TYPE.LIGH)
                        {
                            skyrim_reference.configure_light();
                        }
                    }
                    
                    handle_owner(refr.owner, skyrim_reference);
                    
                    if (cell.interior)
                    {

                        if (Addon_Rules.addon_defined(skyrim_reference.base_id))
                        {
                            TES5.REFR addon = Addon_Rules.get_addon(skyrim_reference);
                            add_interior_references(addon, Text.editor_id_string(cell.cell_name));
                        }

                        add_interior_references(skyrim_reference, Text.editor_id_string(cell.cell_name));
                    }

                    if (!cell.interior)
                    {
                        add_exterior_references(skyrim_reference);

                        if (Addon_Rules.addon_defined(skyrim_reference.base_id))
                        {
                            TES5.REFR addon = Addon_Rules.get_addon(skyrim_reference);
                            add_exterior_references(addon);
                        }
                    }

                }
            }
        }

        private static void add_exterior_references(TES5.REFR refr)
        {            
            int cell_x = (int)(refr.loc.x / 4096f);
            int cell_y = (int)(refr.loc.y / 4096f);

            if (refr.loc.x < 0f)
            {
                cell_x--;
            }

            if (refr.loc.y < 0f)
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

        private static TES5.REFR make_exit(TES3.REFR refr)
        {
            // Place the exit marker in the world
            TES5.REFR exit_marker = new TES5.REFR(DOOR.marker_id, refr.portal.x, refr.portal.y, refr.portal.z, refr.portal.xR, refr.portal.yR, refr.portal.zR, 1);

            if (String.IsNullOrEmpty(refr.portal.destination_cell)) // teleports to exterior
            {
                // TODO: Remove after testing
                //add_exterior_references(exit_marker);
            }

            else // teleports to interior
            {
                add_interior_references(exit_marker, Text.editor_id_string(refr.portal.destination_cell));
            }

            return exit_marker;
                      
            
        }
    }
}
