/*
    This file is part of SNGEGT.

    SNGEGT is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    SNGEGT is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with SNGEGT.  If not, see <http://www.gnu.org/licenses/>.
*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SNGEGT
{
    enum LANDS
    {
        EUNKNOWN,
        EHEART,
        EDIAMOND,
        ESPADE,
        ECROSS
    }


    class Card
    {
        public Card(LANDS land, int value)
        {
            this.value = value;
            this.land = land;
        }

        public LANDS Land
        {
            get
            {
                return land;
            }
            set
            {
                land = value;
            }

        }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value == 1)
                    this.value = 14;
                else
                    this.value = value;
            }
        }

        public Rectangle LandRect
        {
            get
            {
                return landRect;
            }
            set
            {
                landRect = value;
            }
        }

        public Rectangle ValueRect
        {
            get
            {
                return valueRect;
            }
            set
            {
                valueRect = value;
            }
        }

        public int IntValue
        {
            get
            {
                if (Land == LANDS.EUNKNOWN || Value == -1)
                    return -1;
                else
                    return (int) (Land) * 13 + (Value-2);
            }
        }



        public override String ToString()
        {
            String[] strlands = new String[] { "Unknown", "Hearth", "Diamond", "Spade", "Cross" };
            return strlands[(int)land] + " " + value.ToString();
        }


        private Rectangle landRect;
        private Rectangle valueRect;

        private int value;
        private LANDS land;
    }
}
