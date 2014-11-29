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
