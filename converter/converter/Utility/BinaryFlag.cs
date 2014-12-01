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
using Utility;

namespace Utility
{

    class BinaryFlag
    {

        public static UInt16 set(UInt16 flag, UInt16 option)
        {
            return (UInt16)(flag | option);
        }


        public static uint set(uint flag, uint option)
        {
            return (flag | option);
        }

        public static int set(int flag, int option)
        {
            return (flag | option);
        }

        static uint remove(uint flag)
        {
            Log.error("Remove Flag not implemented");
            return 0;
        }

        public static bool isSet(uint flag, uint option)
        {
            if ((flag & option) > 0)
            {
                return true;
            }

            return false;
        }

        public static bool isSet(int flag, int option)
        {
            if ((flag & option) > 0)
            {
                return true;
            }

            return false;
        }
    }

}
