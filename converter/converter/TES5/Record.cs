﻿/*
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
using Utility;

namespace TES5
{
   
    class Record
    {
        public enum FLAGS : uint
        {
            compressed = 0x00040000
        }

        protected bool compressed;

        public bool isCompressed()
        {
            return compressed;
        }


        public void clone(Record r, string editor_id)
        {
            this.compressed = r.isCompressed();
            this.type = r.type;
            this.dataSize = r.dataSize;
            this.flags = r.flags;

            Field EDID = r.find_field_OR_FAIL("EDID", "To Be Cloned Record has no Editor_ID, is it a REFR?");
            EDID.replaceData(Text.editor_id(editor_id));

            this.id = FormID.set(editor_id);

            this.revision = r.revision;
            this.version = r.version;
            this.unknown = r.unknown;
            this.fields = new List<Field>();// r.fields;

            foreach (Field f in r.fields)
            {
                this.fields.Add(new Field(f));
            }
        }

  

        // START Data
        public char[] type;
        public UInt32 dataSize { get; protected set; }
        public UInt32 flags;
        public UInt32 id;
        public UInt32 revision;
        public UInt16 version;
        public UInt16 unknown;
        public List<Field> fields = new List<Field>();

        //byte[] compressed_data;
        // END Data



        public void setFlag(uint option)
        {
            flags = BinaryFlag.set(flags, option);
        }

        public bool flagSet(uint option)
        {
            return BinaryFlag.isSet(flags, option);
        }

        // Call to assign new formid. LGTM calls this
        public void reset_formid(string editor_id)
        {
            this.id = FormID.set(editor_id);
        }
        
        public Record(string type,bool compressed=false)
        {
            if (type == null)
            {
                return;
            }
            
            this.type = type.ToCharArray(0, 4);
            //this.flags = flags;
            if (compressed)
            {
                this.compressed = compressed;
                this.flags = BinaryFlag.set((uint)0, (uint)FLAGS.compressed);
            }
            this.id = FormID.getNew();
            this.revision = 5906;
            this.version = 43;
            this.unknown = 0;
        }

        public Record(string type, string editor_id)
        {
            this.type = type.ToCharArray(0, 4);
            this.flags = 0;

            this.id = FormID.set(editor_id);
            this.revision = 5906;
            this.version = 43;
            this.unknown = 0;
        }
        
        public Record(string type, string editor_id ,UInt32 flags)
        {
            this.type = type.ToCharArray(0, 4);
            this.flags = flags;

            this.id = FormID.set(editor_id);
            this.revision = 5906;
            this.version = 43;
            this.unknown = 0;
        }

        public bool isType(string type_str)
        {            
            return type_str.Equals(new string(type));
        }
                        
        public void recalculate_size()
        {
            //Log.info("Recalculating RECORD size");

            //compressed = BinaryFlag.isSet

            long old_size = dataSize;

            if (!compressed)
            {
                uint size = 0;

                for (int i = 0; i < fields.Count; i++)
                {
                    size = size + (uint)fields[i].data_size() + 6;
                }

                this.dataSize = size;
            }

            if (compressed)
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter out2 = new BinaryWriter(mstream);


                for (int i = 0; i < fields.Count; i++)
                {
                    fields[i].write(out2);
                }
                mstream.Position = 0;

                byte[] uncompressed = mstream.ToArray();
                int uncompressed_size = uncompressed.Length;

                byte[] compressed_ = Ionic.Zlib.ZlibStream.CompressBuffer(uncompressed);

                int compressed_size = compressed_.Length;

                this.dataSize = (uint)compressed_size + 4;
            }

            if (dataSize != old_size)
            {
              //  Log.error(-1);
            }
        }

        public void addField(Field field)
        {
            fields.Add(field);
        }

        
        //long temp_store;

        public void read(BinaryReader input, bool unpack = true)
        {            
            type = input.ReadChars(4);
            dataSize = input.ReadUInt32();
            flags = input.ReadUInt32();
            id = input.ReadUInt32();
            revision = input.ReadUInt32();
            version = input.ReadUInt16();
            unknown = input.ReadUInt16();

            long reading_size = dataSize;

            if (Config.GeneralSettings.verbose)
            {
                Log.info("     Reading Record: " + new string(type) + " " + id.ToString("X") + " >>> " + uint.MaxValue.ToString("X"));
            }

            // If compressed flag is set then uncompress the data
            if (BinaryFlag.isSet(flags,0x00040000))
            {
                this.compressed = true;
                UInt32 decompressed_size = input.ReadUInt32();
                byte[] compressed_data = input.ReadBytes((int)dataSize-4);

                //Log.infoX(compressed_data[0] + " " + compressed_data[1]);

                byte[] uncompressed_data = (Ionic.Zlib.ZlibStream.UncompressBuffer(compressed_data));

          //      temp_store = (int)dataSize;

                if (uncompressed_data.Length != decompressed_size)
                {
                    Log.error("Uncompressed Data was not of expected size: in Record.read()");
                }

                MemoryStream mstream = new MemoryStream();
                mstream.Write(uncompressed_data, 0, uncompressed_data.Length);
                mstream.Position = 0;

                //dataSize = (uint)uncompressed_data.Length;

                input = new BinaryReader(mstream);

                reading_size = uncompressed_data.Length;

            }

            

            if (!unpack)
            {
                Log.error("Not Implemented !unpack at Record.read()");
            }

            uint read_data = 0;
            while (read_data < reading_size)
            {
                Field field = new Field();
                field.read(input);
                fields.Add(field);

                if (field.isBig)
                {
                    read_data = read_data + (uint)field.data_size() + 6 + /*XXX size*/ + 10;
                }
                else
                {
                    read_data = read_data + (uint)field.data_size() + 6;
                }
            }

            

        }

        public void dump()
        {
            Log.info(new string(type) + "  ....  " + dataSize);
            

            foreach (Field f in fields)
            {
                Log.info("          " + new string(f.type) + "  ....  " + f.data_size());                
            }

        }

        public void write(BinaryWriter output)
        {

            Log.info("Writing RECORD: " + new string(type));

            // handle if compression set
            if (BinaryFlag.isSet(flags, 0x00040000))
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter out2 = new BinaryWriter(mstream);


                for (int i = 0; i < fields.Count; i++)
                {
                    fields[i].write(out2);
                }

                mstream.Position = 0;

                byte[] uncompressed = mstream.ToArray();
                int uncompressed_size = uncompressed.Length;

                byte[] compressed_ = Ionic.Zlib.ZlibStream.CompressBuffer(uncompressed);
                int compressed_size = compressed_.Length;

                dataSize = (uint)compressed_size + 4;

                output.Write(type);
                output.Write(dataSize);
                output.Write(flags);
                output.Write(id);
                output.Write(revision);
                output.Write(version);
                output.Write(unknown);

                output.Write(uncompressed_size);
                output.Write(compressed_);
                return;

            }


            
            // Non-Compressed case
            recalculate_size();

            output.Write(type);
            output.Write(dataSize);
            output.Write(flags);
            output.Write(id);
            output.Write(revision);
            output.Write(version);
            output.Write(unknown);
            
            for (int i = 0; i < fields.Count; i++)
            {
                fields[i].write(output);
            }


        }

        // Finds the given FIELD or reports error if not found
        public Field find_field_OR_FAIL(string f_name, string error)
        {
            Field f = try_find_field(f_name);
            if (f == null)
            {
                Log.error("Field: " + f_name + " not found in Record: " + new string(type) + " formid: " + id + '\n' + '\n' + error);
            }

            return f;
        }

        public Field try_find_field(string f_name)
        {
            foreach (Field f in fields)
            {
                if (f.isType(f_name))
                {
                    return f;
                }
            }

            return null;
        }

    }

}
