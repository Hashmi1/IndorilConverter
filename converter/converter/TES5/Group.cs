using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES5
{

    class Group
    {
        public enum TYPE : int
        {
            TOP = 0,
            WRLD_CHILD = 1,
            INT_BLCK = 2,
            INT_SUB_BLCK = 3,
            EXT_BLCK = 4,
            EXT_SUB_BLCK = 5,
            CELL_CHILD = 6,
            TOPIC_CHILD = 7,
            PERSIS_REFR = 8,
            TEMP_REFR = 9,
            VIS_DIST = 10
            
        }

        char[] type;
        UInt32 groupSize;
        byte[] label;
        Int32 groupType;
        UInt16 stamp;
        UInt16 unknown1;
        UInt16 version;
        UInt16 unknown2;
        public List<Record> records = new List<Record>();
        public List<Group> subGroups = new List<Group>();
        public List<int> turn = new List<int>();

        public Group()
        {
        }

        public Group(uint formid,TYPE groupType)
        {

            this.label = Binary.toBin(formid);
            this.groupType = (int)groupType;

            this.type = "GRUP".ToCharArray(0, 4);
            stamp = 5906;
            unknown1 = 0;
            version = 0;
            unknown2 = 0;

        }

        public Group(string label)
        {
            
            this.label = Encoding.ASCII.GetBytes(label);
            this.groupType = 0;

            this.type = "GRUP".ToCharArray(0,4);
            stamp = 5906;
            unknown1 = 0;
            version = 0;
            unknown2 = 0;
            
        }

        public void addRecord(Record rec)
        {
            records.Add(rec);
            turn.Add(1);
        }
        public void addSubGroup(Group grp)
        {
            subGroups.Add(grp);
            turn.Add(2);
        }

        public void recalculate_size()
        {
            Log.info("Recalculating GRUP size");
            long old_size = groupSize;

            uint size = 0;
            foreach (Record rec in records)
            {
                rec.recalculate_size();
                size = size + rec.dataSize + 24;
            }
            foreach (Group sub_grup in subGroups)
            {
                sub_grup.recalculate_size();
                size = size + sub_grup.groupSize;
            }

            size = size + 24;
            this.groupSize = size; 
            
            // stupid acursed bug, took me a whole day to find it. I mean come on, what was I thinking.
            // THis bug had me practically rewrite a whole module that had nothing to do with the bug

            if (old_size != size)
            {
               // Log.error("NOOO");
            }
        }

        public void write(BinaryWriter output)
        {
            //recalculate_size();

            output.Write(type);
            output.Write(groupSize);
            output.Write(label);
            output.Write(groupType);
            output.Write(stamp);
            output.Write(unknown1);
            output.Write(version);
            output.Write(unknown2);

            Queue<Record> recs = new Queue<Record>(records);
            Queue<Group> subgrps = new Queue<Group>(subGroups);

            foreach (int trn in turn)
            {
                if (trn == 1)
                {
                    recs.Dequeue().write(output);
                }
                else if (trn == 2)
                {
                    subgrps.Dequeue().write(output);
                    
                }
            }

            
        }

        public void read(BinaryReader input, bool unpack = true)
        {
            
            type = input.ReadChars(4);
            groupSize = input.ReadUInt32();
            label = input.ReadBytes(4);
            groupType = input.ReadInt32();
            stamp = input.ReadUInt16();
            unknown1 = input.ReadUInt16();
            version = input.ReadUInt16();
            unknown2 = input.ReadUInt16();

            Log.info("Reading Group: " + new string(type) + "....." + (label));

            uint read_data = 0;
            while (read_data < (groupSize - 24))
            {
                string temp = new string(input.ReadChars(4));
                input.BaseStream.Position -= 4;

                if (temp.Equals("GRUP"))
                {
                    turn.Add(2);

                    Group grup = new Group();
                    grup.read(input);
                    subGroups.Add(grup);
                    read_data = read_data + grup.groupSize;
                    continue;
                }

                turn.Add(1);
                Record rec = new Record();
                rec.read(input);
                //Log.info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + rec.id);
                //rec.dump();
                records.Add(rec);
                read_data = read_data + rec.dataSize + 24;

            }



        }

        public void dump()
        {
            Log.info("GRUP dump<");
            Log.info(new string(type));
            Log.info(groupSize);
            //Log.info(new string(label));
            Log.info(stamp);
            Log.info(unknown1);
            Log.info(unknown2);
            Log.info(version);
            Log.info(">GRUP dump");
        }



    }

}
