using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES5;
using System.IO;
using Utility;

namespace Utility

{
    // Checks for incorrect esp/esm files with size issues

    class FileCheck
    {
        public static void check_field(Field field, Record r)
        {
            Log.infoX( new string(r.type) + "," + new string(field.type) + "," + field.dataSize);
            if (field.dataSize != field.data.Length)
            {
                Log.error("Field Error");
            }

            if (field.dataSize == 0 )
            {
                //Log.error("Field Error");
            }
        }

        public static void check_record(Record r)
        {
            //Log.infoX(new string(r.type));
            uint size = r.dataSize;
            r.recalculate_size();

            if (size != r.dataSize)
            {
                Log.info("Record Error");
            }

            foreach (Field f in r.fields)
            {
                check_field(f,r);                
            }

        }

        public static void check_group(Group g)
        {
            Queue<Record> recs = new Queue<Record>(g.records);
            Queue<Group> grps = new Queue<Group>(g.subGroups);

            foreach (int t in g.turn)
            {
                if (t == 1)
                {
                    check_record(recs.Dequeue());
                }

                else if (t == 2)
                {
                    check_group(grps.Dequeue());
                }
            }
        }

        public static void start()
        {
            
            ESM p1 = new ESM("tes5/experiment.esp", FileMode.Open);
            //ESM p2 = new ESM("tes5/experimentO.esp", FileMode.Create);

            BinaryReader br1 = p1.getReader();
            //BinaryWriter br2 = p2.getWriter();

            Record r1 = new Record();
            
            r1.read(br1);
            //r1.write(br2);
            
            while (p1.fstream.Position < p1.fstream.Length)
            {
                Group g = new Group();
                g.read(br1);
                check_group(g);
              //  g.write(br2);
            }

            p1.close();
            //p2.close();
        }
    }
}
