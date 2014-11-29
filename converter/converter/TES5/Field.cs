using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace TES5
{

    class Field
    {
        
        public char[] type;
        public UInt16 dataSize;
        public MemoryStream data;

        public void replaceData(byte[] data_)
        {
            this.data = new MemoryStream();
            this.data.Write(data_, 0, data_.Length);
            this.data.Position = 0;
            this.dataSize = (ushort)data_.Length;
        }

        public BinaryReader getData()
        {
            
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

        public Field(string type_, char[] zstring)
        {
            this.type = type_.ToCharArray(0, 4);
            byte[] temp = Encoding.ASCII.GetBytes(zstring);
            data = new MemoryStream();
            this.data.Write(temp, 0, temp.Length);
            this.dataSize = (UInt16)temp.Length;
            data.Position = 0;
        }

        public Field(string type_, byte[] data_)
        {
            this.type = type_.ToCharArray(0, 4);
            this.data = new MemoryStream();
            this.data.Write(data_, 0, data_.Length);
            this.dataSize = (UInt16)data_.Length;
            this.data.Position = 0;
        }

        public bool isType(string type_str)
        {
            return type_str.Equals(new string(type));
        }

        public void read(BinaryReader input)
        {

            type = input.ReadChars(4);

            if (isType("XXXX"))
            {
                Log.error("Field XXXX encountered");
            }

            dataSize = input.ReadUInt16();
        

            //Log.info("................................." + new string(type) + " ... " + dataSize);

            //Log.info("          Reading Field: " + new string(type));

            byte[] dat = input.ReadBytes(dataSize);
            data = new MemoryStream();
            data.Write(dat, 0, dat.Length);
            data.Position = 0;
                
        }
        
        public void write(BinaryWriter output)
        {
            output.Write(type);
            output.Write(dataSize);
            output.Write(data.ToArray());
        }



    }
    
}
