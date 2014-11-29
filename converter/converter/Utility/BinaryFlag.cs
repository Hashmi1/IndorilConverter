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
