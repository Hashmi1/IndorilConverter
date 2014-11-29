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

namespace Convert
{
    class ACTI
    {
        public static void dump()
        {
            
            TES3.ESM.open("tes3/morrowind.esm");
            TextWriter fl = File.CreateText("acti.txt");

            while (TES3.ESM.find("ACTI"))
            {

                TES3.Record acti = new TES3.Record();
                acti.read();

                foreach (TES3.SubRecord srec in acti.subRecords)
                {
                    if (srec.isType("NAME"))
                    {
                        Log.info("Found");
                        
                        fl.WriteLine(new string(srec.getData().ReadChars(srec.size)));

                    }
                }

            }
            fl.Close();

        }
    }
}
