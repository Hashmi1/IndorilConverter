using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace External
{
    class Notepad : External_Program
    {
        public Notepad() : base("notepad","notepad.exe")
        {
        }

        public void open(string file)
        {
            string command = file;
            base.run(command);
        }

    }
}
