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
using System.Windows.Forms;

namespace SNGEGT
{
    class Player
    {
        public RangeSelector slider;
        public RadioButton position;
        public TextBox chips;
        public Label prepost;
        public TextBox bets;
        public RadioButton allin;
        public Label labelCall;
        public Label labelWin;
        public Label labelEVwin;
        public Label labelEVlose;        
        public Boolean enabled;

        public Player(RangeSelector iSlider, RadioButton iPosition, TextBox iChips, Label iPrepost, TextBox iBets, RadioButton iAllin, Label iLabelCall, Label iLabelWin, Label iLabelEVwin, Label iLabelEVlose)
        {
            slider = iSlider;
            position = iPosition;
            chips = iChips;
            prepost = iPrepost;
            bets = iBets;
            allin = iAllin;
            labelWin = iLabelWin;
            labelCall = iLabelCall;
            labelEVwin = iLabelEVwin;
            labelEVlose = iLabelEVlose;
            enabled = true;
        }


        public void Enable()
        {
            slider.Enabled = true;
            position.Enabled = true;
            chips.Enabled = true;
            prepost.Enabled = true;
            bets.Enabled = true;
            allin.Enabled = true;
            labelCall.Enabled = true;
            labelWin.Enabled = true;
            labelEVwin.Enabled = true;
            labelEVlose.Enabled = true;
            enabled = true;
        }


        public void Disable()
        {
            slider.Enabled = false;
            position.Enabled = false;
            chips.Enabled = false;
            prepost.Enabled = false;
            bets.Enabled = false;
            allin.Enabled = false;
            labelCall.Enabled = false;
            labelWin.Enabled = false;
            labelEVwin.Enabled = false;
            labelEVlose.Enabled = false;
            enabled = false;
        }


        public void disableLabels()
        {
            resetLabels();
            labelCall.Enabled = false;
            labelWin.Enabled = false;
            labelEVwin.Enabled = false;
            labelEVlose.Enabled = false;
        }


        public void enableLabels()
        {
            resetLabels();
            labelCall.Enabled = true;
            labelWin.Enabled = true;
            labelEVwin.Enabled = true;
            labelEVlose.Enabled = true;
        }


        public void resetLabels()
        {
            labelCall.Text = "-";
            labelWin.Text = "-";
            labelEVwin.Text = "-";
            labelEVlose.Text = "-";
            prepost.Text = "-";
        }


        public void setBet(int bet)
        {
            bets.Text = Convert.ToString(bet);         
        }

    }
}
