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

namespace TES3
{
    class NPC_ : Record
    {
        public string editor_id;
        public string game_name;
        public string race;
        string faction;
        public string head;
        //string class_;
        public string hair;

        bool isFemale;
        bool autoCalc;



        public void read()
        {
            base.read();
            editor_id = find_first("NAME").readString().ToLower();
            game_name = find_first("FNAM").readString();
            race = find_first("RNAM").readString();
            head = find_first("BNAM").readString().ToLower();
            hair = find_first("KNAM").readString().ToLower();
            
            faction = find_first("ANAM").readString();

            isFemale = BinaryFlag.isSet(find_first("FLAG").getData().ReadUInt32(), 0x0001);
            autoCalc = BinaryFlag.isSet(find_first("FLAG").getData().ReadUInt32(), 0x0010);
        }





    }
}
