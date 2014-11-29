/*
Copyright 2014 Hashmi1

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

namespace TES3
{
    class REFR : Record
    {

        public string editor_id = null;

        public float scale = 1;

        public float x = 0;
        public float y = 0;
        public float z = 0;

        public float xR = 0;
        public float yR = 0;
        public float zR = 0;
                
        public int read(int readable_size)
        {
            int read_size = 0;
            while (read_size < readable_size)
            {
                SubRecord subrec = new SubRecord();
                subrec.read();
                read_size = read_size + subrec.size + 8;

                BinaryReader srec_data = subrec.getData();

                if (subrec.isType("FRMR"))
                {
                    ESM.rewind(subrec.size + 8);
                    return (read_size - subrec.size -8);
                }
                else if (subrec.isType("NAME"))
                {
                    editor_id = new string(srec_data.ReadChars(subrec.size));
                    //Log.info(editor_id);
                }
                else if (subrec.isType("XSCL"))
                {
                    scale = srec_data.ReadSingle();
                }
                else if (subrec.isType("DATA"))
                {
                    x = srec_data.ReadSingle();
                    y = srec_data.ReadSingle();
                    z = srec_data.ReadSingle();
                    xR = srec_data.ReadSingle();
                    yR = srec_data.ReadSingle();
                    zR = srec_data.ReadSingle();
                }



            }
            return read_size;

        }

    }
    
}

