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

namespace TES3
{

    class REGN : Record
    {
        public string editor_id;
        public string region_name;

        public void read()
        {
            base.read();

            for (int i = 0; i < subRecords.Count; i++)
            {
                SubRecord srec = subRecords[i];
                BinaryReader srec_data = srec.getData();
                switch (new string(srec.name))
                {
                    case ("NAME"):
                        editor_id = new string(srec_data.ReadChars(srec.size));
                        break;
                    case ("FNAM"):
                        region_name = new string(srec_data.ReadChars(srec.size));
                        break;
                }
            }
        }
    }

}
