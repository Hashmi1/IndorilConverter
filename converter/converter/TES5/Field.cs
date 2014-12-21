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
using Utility;
using System.Security.Cryptography;

namespace TES5
{

    class Field
    {
        int size;

        public char[] type;
        //UInt16 dataSize;
        public MemoryStream data;

        public int data_size()
        {
            return size;
        }

        public void replaceData(byte[] data_)
        {
            this.data = new MemoryStream();
            this.data.Write(data_, 0, data_.Length);
            this.data.Position = 0;
            this.size = data_.Length;

            page();
        }

        public BinaryReader getData()
        {
            if (paged)
            {
                return getData_paged();
            }
            
            MemoryStream mstream = new MemoryStream();
            data.Position = 0;
            data.CopyTo(mstream);
            mstream.Position = 0;
            data.Position = 0;
            return new BinaryReader(mstream);
        }

        public Field()
        {

        }

        bool paged = false;

        PagedData token;

        void page()
        {
                     
            if (size <= 12|| !Config.GeneralSettings.paged)
            {
                return;
            }

            paged = true;

            token = PagingEngine.store(data.ToArray());
            data.Close();
            data = null;

            PagingEngine.getData(token);
        }

        BinaryReader getData_paged()
        {
            byte[] bin_data = PagingEngine.getData(token);
            MemoryStream mstream = new MemoryStream();
            mstream.Write(bin_data, 0, bin_data.Length);
            mstream.Position = 0;

            BinaryReader br = new BinaryReader(mstream);
            return br;
        }
        
        public Field(Field f)
        {
            byte[] data_ = f.getData().ReadBytes(f.data_size());

            this.type = f.type;

            this.data = new MemoryStream();
            this.data.Write(data_, 0, data_.Length);
            this.size = data_.Length;
            this.data.Position = 0;

            this.isBig = f.isBig;
            page();
        }



        public Field(string type_, byte[] data_)
        {
            this.type = type_.ToCharArray(0, 4);
            this.data = new MemoryStream();
            this.data.Write(data_, 0, data_.Length);
            this.size = data_.Length;
            this.data.Position = 0;
            page();
        }

        public string readString()
        {
            return Text.toString(getData().ReadChars(size));

        }

        public bool isType(string type_str)
        {
            return type_str.Equals(new string(type));
        }

        public bool isBig = false;

        
        void readBig(BinaryReader input)
        {
            //Log.confirm("Reading BIG field (XXXX). This functionality is being tested.");
                        
            isBig = true;

            if (!new string(input.ReadChars(4)).Equals("XXXX"))
            {
                Log.error("readBig XXXX not found");
            }

            if (input.ReadUInt16() != 4)
            {
                Log.error("readBig XXXX not of size 4");
            }

            this.size = input.ReadInt32();

            this.type = input.ReadChars(4);

            if (input.ReadUInt16() != 0)
            {
                Log.error("XXX follow size is not 0");
            }

            byte[] data__ = input.ReadBytes(size);
            data = new MemoryStream();
            this.data.Write(data__, 0, data__.Length);
            data.Position = 0;

            page();
        }

        public void read(BinaryReader input)
        {

            string type_str = Text.trim(new string(input.ReadChars(4)));
            input.BaseStream.Position -= 4;

            if (type_str.Equals("XXXX"))
            {
                readBig(input);
                return;
            }
            
            type = input.ReadChars(4);
            size = input.ReadUInt16();

            if (Config.GeneralSettings.verbose)
            {
                //Log.info("................................." + new string(type) + " ... " + dataSize);

                Log.info("          Reading Field: " + new string(type));
            }
            

            byte[] dat = input.ReadBytes(size);
            
            data = new MemoryStream();
            data.Write(dat, 0, dat.Length);
            data.Position = 0;

            page();
        }

        void writeBIG(BinaryWriter output)
        {
            Log.confirm("Reading BIG field (XXXX). This functionality is being tested.");

            Field xxxx = new Field("XXXX",Binary.toBin((int)size));
            xxxx.write(output);

            output.Write(type);
            output.Write((UInt16)0);
            output.Write(getData().ReadBytes(size));
        }
        
        public void write(BinaryWriter output)
        {
            if (isBig || size > UInt16.MaxValue)
            {
                isBig = true;
                writeBIG(output);
                return;
            }
            
            output.Write(type);
            output.Write((UInt16)size);
            output.Write(getData().ReadBytes(size));
        }



    }
    
}
