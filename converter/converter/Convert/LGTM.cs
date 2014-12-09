using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Convert
{
    class LGTM
    {
        static TES5.Record tomb;
        static TES5.Record cave;
        static TES5.Record mine;
        static TES5.Record dwemer;
        static TES5.Record daedric;
        static TES5.Record redoran;
        static TES5.Record telvanni;
        static TES5.Record imperial;
        static TES5.Record hlaalu;
        static TES5.Record fort;
        static TES5.Record velothi;
        static TES5.Record default_;

        static Dictionary<CellTYPE.TYPE, TES5.Record> dict;

        static LGTM()
        {
            read_templates();

            dict = new Dictionary<CellTYPE.TYPE, TES5.Record>()
            {
                {CellTYPE.TYPE.	TOMB	,	tomb	},
                {CellTYPE.TYPE.	CAVE	,	cave	},
                {CellTYPE.TYPE.	MINE	,	mine	},
                {CellTYPE.TYPE.	DWEMER	,	dwemer	},
                {CellTYPE.TYPE.	DAEDRIC	,	daedric	},
                {CellTYPE.TYPE.	REDORAN	,	redoran	},
                {CellTYPE.TYPE.	TELVANNI	,	telvanni	},
                {CellTYPE.TYPE.	IMPERIAL	,	imperial	},
                {CellTYPE.TYPE.	HLAALU	,	hlaalu	},
                {CellTYPE.TYPE.	FORT	,	fort	},
                {CellTYPE.TYPE.	VELOTHI	,	velothi	},
                {CellTYPE.TYPE.	DEFAULT	,	default_	}

            };
        }

        static bool has_read = false;
                        
        private static void read_templates()
        {
            

            has_read = true;

            TES5.ESM esm = TES5.ESM.read_from_file(Config.Paths.light_templates);

            foreach (TES5.Group g in esm.groups)
            {
                if (g.isType(TES5.Group.TYPE.TOP) && Encoding.ASCII.GetString(g.label).Equals("LGTM"))
                {
                    foreach (TES5.Record r in g.records)
                    {
                        
                        # region assign templates
                        TES5.Field edid = r.find_field("EDID");
                        string editor_id = Text.trim(new string(edid.getData().ReadChars(edid.dataSize)));
                        r.reset_formid(editor_id);
                        Log.info(editor_id);
                        if (editor_id.Equals("mw_ltmp_default"))
                        {
                            default_ = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_daedric"))
                        {
                            daedric = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_fort"))
                        {
                            fort = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_dwemer"))
                        {
                            dwemer = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_tomb"))
                        {
                            tomb = velothi = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_hlaalu"))
                        {
                            hlaalu = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_redoran"))
                        {
                            redoran = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_telvanni"))
                        {
                            telvanni = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_mine"))
                        {
                            cave = mine = r;
                        }

                        else if (editor_id.Equals("mw_ltmp_imperial"))
                        {
                            imperial = r;
                        }
                        #endregion

                    }

                }
            }


            if (default_ == null || hlaalu == null || fort == null || imperial == null || redoran == null || telvanni == null || dwemer == null || daedric == null || cave == null || tomb == null)
            {
                Log.error("TES5:LTMP Not all Lighting templates were assigned");
            }

                      

        }

        public static TES5.Record get(CellTYPE.TYPE t)
        {


            if (!dict.ContainsKey(t))
            {
                Log.error("LTMP does not know this lighting  template");
            }

            return dict[t];
        }

        public static TES5.Group convert()
        {
            if (!has_read)
            {
                read_templates();
            }

            TES5.Group g = new TES5.Group("LGTM");

            g.addRecord(tomb);
            g.addRecord(cave);
            g.addRecord(dwemer);
            g.addRecord(daedric);
            g.addRecord(redoran);
            g.addRecord(telvanni);
            g.addRecord(imperial);
            g.addRecord(hlaalu);
            g.addRecord(fort);
            g.addRecord(default_);

            return g;
        }
        
    }
}
