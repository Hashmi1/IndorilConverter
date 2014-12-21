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
using System.Security.Cryptography;
using Utility;

namespace TES5
{
    /// <summary>
    /// 
    /// </summary>
    class PagedData
    {
        public Int64 offset;
        public UInt32 size;

        public PagedData(Int64 offset, UInt32 size)
        {
            this.offset = offset;
            this.size = size;
        }
    }

    class PagingEngine
    {
        static FileStream fstream;
        static SHA256 hasher = SHA256.Create();

        static PagingEngine()
        {
            fstream = new FileStream(Config.Paths.Temporary.page_file, FileMode.Create);
        }

        

        public static PagedData store(byte[] data)
        {
            PagedData token = new PagedData(fstream.Position,(uint)data.Length+32);
                        
            byte[] hash = hasher.ComputeHash(data);

            fstream.Write(hash, 0, hash.Length);
            fstream.Write(data, 0, data.Length);

            return token;
        }

        public static byte[] getData(PagedData token)
        {
            
            long end_ptr = fstream.Position;

            fstream.Position = token.offset;
            byte[] hash_stored = new byte[32];

            fstream.Read(hash_stored,0,hash_stored.Length);
            
            byte[] data = new byte[token.size-32];
            fstream.Read(data, 0, data.Length);

            if (!hash_stored.SequenceEqual(hasher.ComputeHash(data)))
            {
                Log.error("Corrupted PageFile");
            }

            fstream.Position = end_ptr;

            return data;

        }
    }
    
}
