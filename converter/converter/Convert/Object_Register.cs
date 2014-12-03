using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Convert
{
    class Object_Register
    {
        static Object_Register instance;

        public static Object_Register getInstance()
        {
            if (instance == null)
            {
                instance = new Object_Register();
            }

            return instance;
        }

        public enum TYPE : int
        {
            STAT,
            DOOR,
            PORTAL
        }

        private Object_Register()
        {
        }

        Dictionary<uint, TYPE> dict = new Dictionary<uint, TYPE>();

        public TYPE get_type(uint formid)
        {
            if (formid == 0)
            {
                Log.error(" Object_Register: get_type(uint) formid not found");
                //formid = TES5.FormID.set(editor_id);
            }

            if (!dict.ContainsKey(formid))
            {
                Log.error("Object_Register: get_type(formid) not found");
            }

            return dict[formid];

        }

        public TYPE get_type(string editor_id)
        {
            uint formid = TES5.FormID.get(editor_id);            
            return get_type(formid);
        }

        public void set_type(string editor_id, TYPE type)
        {
            uint formid = TES5.FormID.get(editor_id);
            set_type(formid, type);
        }

        public void set_type(uint formid, TYPE type)
        {
            if (formid == 0)
            {
                Log.error("Object_Register: set_type(uint) formid not found");
            }

            dict.Add(formid, type);
        }




    }
}
