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
            TES5.Group LGTM_GRUP = esm.find_TOP_group_OR_FAIL("LGTM","No LGTM Group in Template File");


            default_ = LGTM_GRUP.find_record("mw_ltmp_default");
            daedric = LGTM_GRUP.find_record("mw_ltmp_daedric");
            fort = LGTM_GRUP.find_record("mw_ltmp_fort");
            dwemer = LGTM_GRUP.find_record("mw_ltmp_dwemer");
            tomb = velothi = LGTM_GRUP.find_record("mw_ltmp_tomb");
            hlaalu = LGTM_GRUP.find_record("mw_ltmp_hlaalu");
            redoran = LGTM_GRUP.find_record("mw_ltmp_redoran");
            telvanni = LGTM_GRUP.find_record("mw_ltmp_telvanni");
            cave = mine = LGTM_GRUP.find_record("mw_ltmp_mine");
            imperial = LGTM_GRUP.find_record("mw_ltmp_imperial");
            
            if (default_ == null || hlaalu == null || fort == null || imperial == null || redoran == null || telvanni == null || dwemer == null || daedric == null || cave == null || tomb == null)
            {
                Log.error("TES5:LTMP Not all Lighting templates were assigned");
            }


            default_.reset_formid("mw_ltmp_default");
            daedric.reset_formid("mw_ltmp_daedric");
            fort.reset_formid("mw_ltmp_fort");
            dwemer.reset_formid("mw_ltmp_dwemer");
            tomb.reset_formid("mw_ltmp_tomb");
            hlaalu.reset_formid("mw_ltmp_hlaalu");
            redoran.reset_formid("mw_ltmp_redoran");
            telvanni.reset_formid("mw_ltmp_telvanni");
            cave.reset_formid("mw_ltmp_mine");
            imperial.reset_formid("mw_ltmp_imperial");
            


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
