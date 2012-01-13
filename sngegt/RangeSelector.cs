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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SNGEGT
{
    public delegate void RangeChangedEvent(object o, EventArgs e);

    public partial class RangeSelector : UserControl
    {
        
        [ Category ( "User Defined" ) , Description ( "Raised when value of range changes" ) ]
        public event RangeChangedEvent RangeChanged;
        
        private readonly Color selectedcolor = Color.Aqua;
        private readonly Color activecolor = Color.Blue;
        private static int[] buttonvalues;
        private object selectedbutton;
        private int playerNumber;
        
        [Category("User Defined"), Description("Sets players position.")]
        public int PlayerNumber
        {
            get
            {
                return playerNumber;
            }
            set
            {
                playerNumber = value;
            }
        }

        private List <Button> buttons;

        String[] strRanks;
        
        public RangeSelector()
        {
            InitializeComponent();
            
            int buttoncount = 7;
            buttons = new List<Button>(buttoncount);

            strRanks = new String[101] { "", "KK+,AKs", "QQ+,AK", "JJ+,AK", "TT+,AQ+", "99+,AQ+", "88+,AQo+,ATs+", "88+,AJo+,ATs+", "66+,AT+", "66+,ATo+,A9s+", "55+,ATo+,A8s+,KQs", "44+,A9o+,A8s+,KQs", "44+,A9o+,A7s+,KJs+", "44+,AKs,AQs,AJs,ATs,A9s,A8s,A8o+,A7s,A5s,KJs+", "44+,A8o+,A4s+,KJs+", "33+,A7o+,A4s+,KTs+", "33+,A7o+,A3s+,KQo,KTs+", "33+,A7o+,A2s+,KQo,KTs+", "33+,AKo,AQo,AJo,ATo,A9o,A8o,A7o,A5o,A2s+,KQo,KTs+", "33+,A5o+,A2s+,KQo,KTs+", "33+,A4o+,A2s+,KJo+,KTs+", "33+,A4o+,A2s+,KJo+,KTs+,QJs", "33+,A3o+,A2s+,KJo+,KTs+,QJs", "22+,A2+,KJo+,K9s+,QJs", "22+,A2+,KTo+,K9s+,QJs", "22+,A2+,KTo+,K8s+,QTs+", "22+,A2+,K9o+,K7s+,QTs+,JTs", "22+,A2+,K9o+,K6s+,QJo,QTs+,JTs", "22+,A2+,K9o+,K6s+,QJo,Q9s+,JTs", "22+,A2+,K8o+,K5s+,QJo,Q9s+,JTs", "22+,A2+,K8o+,K4s+,QTo+,Q9s+,JTs", "22+,A2+,K7o+,K4s+,QTo+,Q9s+,JTs", "22+,A2+,K7o+,K2s+,QTo+,Q9s+,JTs", "22+,A2+,K6o+,K2s+,QTo+,Q8s+,JTs", "22+,A2+,K5o+,K2s+,QTo+,Q8s+,J9s+", "22+,A2+,K5o+,K2s+,Q9o+,Q8s+,J9s+", "22+,A2+,K5o+,K2s+,Q9o+,Q8s+,JTo,J9s+", "22+,A2+,K4o+,K2s+,Q9o+,Q8s+,JTo,J9s+", "22+,A2+,K4o+,K2s+,Q9o+,Q6s+,JTo,J9s+,T9s", "22+,A2+,K3o+,K2s+,Q9o+,Q6s+,JTo,J9s+,T9s", "22+,A2+,K2+,Q9o+,Q5s+,JTo,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q5s+,JTo,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q4s+,J9o+,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q3s+,J9o+,J8s+,T8s+", "22+,A2+,K2+,Q7o+,Q3s+,J9o+,J7s+,T8s+", "22+,A2+,K2+,Q6o+,Q2s+,J9o+,J7s+,T8s+", "22+,A2+,K2+,Q6o+,Q2s+,J9o+,J7s+,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J7s+,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J7s+,T9o,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J6s+,T9o,T8s+,98s", "22+,A2+,K2+,Q4o+,Q2s+,J8o+,J5s+,T9o,T7s+,98s", "22+,A2+,K2+,Q4o+,Q2s+,J7o+,J4s+,T9o,T7s+,98s", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J4s+,T9o,T7s+,98s", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J4s+,T8o+,T7s+,97s+", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J3s+,T8o+,T7s+,97s+", "22+,A2+,K2+,Q2+,J7o+,J3s+,T8o+,T6s+,97s+", "22+,A2+,K2+,Q2+,J6o+,J2s+,T8o+,T6s+,97s+,87s", "22+,A2+,K2+,Q2+,J6o+,J2s+,T8o+,T6s+,98o,97s+,87s", "22+,A2+,K2+,Q2+,J6o+,J2s+,T7o+,T6s+,98o,97s+,87s", "22+,A2+,K2+,Q2+,J5o+,J2s+,T7o+,T6s+,98o,96s+,87s", "22+,A2+,K2+,Q2+,J5o+,J2s+,T7o+,T5s+,98o,96s+,87s", "22+,A2+,K2+,Q2+,J4o+,J2s+,T7o+,T4s+,98o,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T4s+,98o,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T4s+,97o+,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T3s+,97o+,96s+,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T3s+,97o+,95s+,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T2s+,97o+,95s+,87o,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T5o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J2+,T5o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J2+,T5o+,T2s+,96o+,94s+,87o,85s+,75s+", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,96o+,94s+,87o,85s+,75s+", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,96o+,94s+,86o+,85s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,95o+,93s+,86o+,84s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,93s+,86o+,84s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,93s+,86o+,84s+,76o,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,92s+,86o+,84s+,76o,74s+,65s", "22+,A2+,K2+,Q2+,J2+,T2+,95o+,92s+,86o+,84s+,76o,74s+,65s,54s", "22+,A2+,K2+,Q2+,J2+,T2+,95o+,92s+,85o+,84s+,76o,74s+,65s,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,83s+,76o,74s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,83s+,75o+,74s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,82s+,75o+,73s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,85o+,82s+,75o+,73s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,85o+,82s+,75o+,73s+,65o,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,84o+,82s+,75o+,73s+,65o,63s+,53s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,75o+,73s+,65o,63s+,53s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,73s+,65o,63s+,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,65o,63s+,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,64o+,63s+,54o,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,64o+,63s+,54o,52s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,83o+,82s+,74o+,72s+,64o+,62s+,54o,52s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,74o+,72s+,64o+,62s+,54o,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,64o+,62s+,54o,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,64o+,62s+,53o+,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,63o+,62s+,53o+,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,63o+,62s+,53o+,52s+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,63o+,62s+,53o+,52s+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,63o+,62s+,52+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,42+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,42+,32" };


            for (int i = 0; i < buttoncount; i++)
                buttons.Add((Button) Controls["button" +Convert.ToString(i+1)]);
            
            buttonvalues = new int[buttons.Count];

            for (int i = 0; i < buttons.Count; i++)
                buttonvalues[i] = (Int32) (100.0 / buttonvalues.Length*(i+1));

        }

        public void ShowSlider(bool show)
        {
            trackBar.Visible = show;
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Visible = !show;
                if (buttonvalues[i] == Range)
                {
                    buttons[i].BackColor = selectedcolor;
                    selectedbutton = buttons[i];
                }
                else
                    buttons[i].BackColor = SystemColors.Control;
            }

           
        }


        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            button.BackColor = activecolor;
            toolTip1.SetToolTip(button,Convert.ToString(buttonvalues[Convert.ToInt32(button.Tag)]));
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Color backcolor = SystemColors.Control;
            if (sender == selectedbutton)
                backcolor = selectedcolor;
            ((Button)sender).BackColor = backcolor; 
        }

       

        private void button_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectedbutton != null)
                ((Button)selectedbutton).BackColor = SystemColors.Control;
            ((Button)sender).BackColor = selectedcolor;
            selectedbutton = sender;

            
            int tag = Convert.ToInt32(((Control)sender).Tag);


            if (tag >= 0 && tag < buttonvalues.Length)
                Range = buttonvalues[tag];
        
        }

        public int Range 
        {
            set 
            {
                if (value >= trackBar.Minimum && value <= trackBar.Maximum)
                    trackBar.Value = value;
                if (RangeChanged != null)
                    RangeChanged(this, new EventArgs());

            }
            get 
            {
                return trackBar.Value;
            }
        }




        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            labelPercent.Text = Convert.ToString(trackBar.Value) +"%";
            if (RangeChanged != null)
                RangeChanged(this, e);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar, "");
            toolTip1.Active = true;
            toolTip1.Show(Convert.ToString(trackBar.Value) + "% " + strRanks[trackBar.Value], trackBar, 130, 0, 1000);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            trackBar_Scroll(sender, e);
        }

        private void trackBar_MouseEnter(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void trackBar_MouseLeave(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            toolTip1.Hide(trackBar);
        }

        public void setRanges(int[] ranges)
        {
            buttonvalues = ranges;
        }


     

    }
}
