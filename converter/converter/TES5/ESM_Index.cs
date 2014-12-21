using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace TES5
{
    /// <summary>
    /// ESM_Index provides quick lookup functionality for ESM files. Including looking up records from form_ids
    /// and finding reference container Groups for cells.
    /// </summary>
    /// 
    class ESM_Index
    {
        public ESM_Index()
        {        
        }


        Dictionary<uint, TES5.Group> tref_dict = new Dictionary<uint, TES5.Group>();        
        Dictionary<uint, Record> dict_formid = new Dictionary<uint, Record>();
        Dictionary<string, Record> dict_edid = new Dictionary<string, Record>();
        Dictionary<string, List<Record>> dict_type = new Dictionary<string, List<Record>>();
        uint record_count = 0;
        uint group_count = 0;
        uint available_formid = 0;

        public void add_reference(Record cell, Record reference)
        {
            string[] allowed_forms = {"REFR", "ACHR", "NAVM", "PGRE", "PHZD", "LAND", "INFO"};

            string rec_type = new string(reference.type);

            if (!allowed_forms.Contains(rec_type))
            {
                Log.error("Can not add " + rec_type + " as reference.");                
            }

            reference.id = available_formid++;

            if (dict_formid.ContainsKey(reference.id))
            {
                Log.error("Could not assign new available formid");
            }
            
            tref_dict[cell.id].addRecord(reference);
            dict_formid[reference.id] = reference;

            Field EDID = reference.try_find_field("EDID");
            if (EDID != null)
            {
                dict_edid.Add(EDID.readString(), reference);
            }

            if (!dict_type.ContainsKey(new string(reference.type)))
            {
                dict_type.Add(new string(reference.type), new List<Record>());
            }

            dict_type[new string(reference.type)].Add(reference);


        }

        public List<Record> get_record_list(string type)
        {
            if (!dict_type.ContainsKey(type))
            {
                Log.error("ESM does not have any records of the given type");
            }

            return dict_type[type];
        }

        public Record get_record(string editor_id)
        {
            editor_id = editor_id.ToLower();

            if (!dict_edid.ContainsKey(editor_id))
            {
                Log.error("Record with Editor Id : '" + editor_id + "' was not found.");
            }

            return dict_edid[editor_id];
        }

        public Record get_record(uint formid)
        {
            if (!dict_formid.ContainsKey(formid))
            {
                Log.error("No Record found with ID: " + Text.toHex(formid));
                return null;
            }

            return dict_formid[formid];
        }

        public Group get_references(Record r)
        {
            if (!r.isType("CELL"))
            {
                Log.error("ESM_Index asked to find references for non-cell record");
            }

            return get_temp_refr_group(r.id);
        }

        public Group get_temp_refr_group(uint formid)
        {
            if (!tref_dict.ContainsKey(formid))
            {
                return null;
            }

            return tref_dict[formid];
        }

        void parse(Group g)
        {
            group_count++;

            if (g.isType(Group.TYPE.TEMP_REFR))
            {
                tref_dict.Add(Binary.toUInt32(g.label),g);
            }

            foreach (Record r in g.records)
            {
                parse(r);
            }

            foreach (Group sg in g.subGroups)
            {
                parse(sg);
            }

        }

        void parse(Record r)
        {
            record_count++;

            dict_formid.Add(r.id, r);

            Field EDID = r.try_find_field("EDID");

            if (EDID != null)
            {
                string edid = EDID.readString();
                if (dict_edid.ContainsKey(edid))
                {
                    Log.error("Duplicate Editor IDs were found > '" + edid + "' for " + new string (r.type) + ":" + Text.toHex(r.id) 
                            + " and " + new string (dict_edid[edid].type) + ":" + Text.toHex(dict_edid[edid].id) 
                            + " Maybe load in CK and resave?");
                }

                dict_edid.Add(EDID.readString().ToLower(), r);
            }

            if (!dict_type.ContainsKey(new string(r.type)))
            {
                dict_type.Add(new string(r.type),new List<Record>());
            }

            dict_type[new string(r.type)].Add(r);
            
        }

        public void make(TES5.ESM esm)
        {
            Log.info_colored("Indexing ESM file. This can take a while. Please Wait",ConsoleColor.Green);
            foreach (Group g in esm.getGroups())
            {
                parse(g);
            }
            Log.info_colored("Parsing successful. Index has been made.", ConsoleColor.Green);

            available_formid = dict_formid.Keys.ToArray().Max()+1;

            if (dict_formid.ContainsKey(available_formid))
            {
                Log.error("ESM_Index, formids are not continous");
            }
        }

        
    }
}
