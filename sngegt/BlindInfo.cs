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
using System.Text.RegularExpressions;

namespace SNGEGT
{
    public class BlindInfo: IComparable
    {

        private int smallblind;
        public int Smallblind
        {
            get
            {
                

                return smallblind;
            }
            set
            {
                smallblind = value;
            }
        }
            
        private int bigblind;
        public int Bigblind
        {
            get
            {
                return bigblind;
            }
            set
            {
                bigblind = value;
            }
        }

        private int ante;
        public int Ante
        {
            get
            {
                return ante;
            }
            set
            {
                ante = value;
            }
        }


        public BlindInfo(int asmallblind, int abigblind, int aante)
        {
            Smallblind = asmallblind;
            Bigblind = abigblind;
            Ante = aante;
        }

        public BlindInfo(String blindstr)
        {
            Match match = Regex.Match(blindstr, @"(?<small>\d+)/(?<big>\d+)\s?([-+](?<ante>\d+))?");
            Ante = 0;
            if (match.Success)
            {
                
                Smallblind = Convert.ToInt32(match.Groups["small"].Value);
                Bigblind = Convert.ToInt32(match.Groups["big"].Value);

         
                if (match.Groups["ante"].Value != "")
                    Ante = Convert.ToInt32(match.Groups["ante"].Value);

            }
        }


        public override String ToString()
        {
            String format = "{0}/{1}";
            if (ante != 0)
                format = format + "+{2}";

            return String.Format(format, smallblind, bigblind, ante);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BlindInfo))
                return false;
            BlindInfo joo = (BlindInfo) obj;
            return joo.smallblind == this.Smallblind && joo.bigblind == this.bigblind && joo.ante == this.ante;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }



        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (!(obj is BlindInfo))
                throw new Exception("Can't compare");

            BlindInfo compare = (BlindInfo)obj;
            return this.smallblind.CompareTo(compare.smallblind);


        }

        #endregion
    }
}
