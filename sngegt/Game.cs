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

    class Game
    {
        public Player[] players;
        private Label labelBetSum;
        private Label labelChipSum;
        private Label labelEVpushCall;
        private Label labelEVfoldVal;
        private Label labelEVpushCallVal;
        private Label labelEVdiffVal;
        private bool Updating;
        private static readonly short STACK = 0;
        private static readonly short BETS = 1;
        private static readonly short CALLRANGE = 2;
        //private static readonly short HOLDPERC = 3;
        private static readonly short CALLPERC = 4;
        private static readonly short WINPERC = 5;
        private static readonly short EVWIN = 6;
        private static readonly short EVLOSE = 7;
        private static readonly short PREPOST = 8;
        private ICM icm;
        private double[,] playersData;
        private double[] ICMs;
        private double[] results;
        private int playersCount;
        private int handIndex;


        public Game(Player p0, Player p1, Player p2, Player p3, Player p4, Player p5, Player p6, Player p7, Player p8, Player p9, Label iChipSum, Label iBetSum, Label iLabelEVpushCall, Label iLabelEVfoldVal, Label iLabelEVpushCallVal, Label iLabelEVdiffVal)
        {
            icm = new ICM();
            players = new Player[10];
            players[0] = p0;
            players[1] = p1;
            players[2] = p2;
            players[3] = p3;
            players[4] = p4;
            players[5] = p5;
            players[6] = p6;
            players[7] = p7;
            players[8] = p8;
            players[9] = p9;
            labelBetSum = iBetSum;
            labelChipSum = iChipSum;
            labelEVpushCall = iLabelEVpushCall;
            labelEVfoldVal = iLabelEVfoldVal;
            labelEVpushCallVal = iLabelEVpushCallVal;
            labelEVdiffVal = iLabelEVdiffVal;

            for (int i = 0; i < 4; i++)

                players[i].chips.Text = "5000";

            for (int i = 4; i < 10; i++)

                players[i].chips.Text = "100";



            playersData = new double[10, 8];

        }


        public void resetPlayersLabels()
        {
            for (int i = 0; i < 10; i++)
                players[i].resetLabels();
        }


        public void disablePlayers(int from)
        {
            int index = from + 2;
            for (int i = 0; i < 10; i++)
            {
                if (i < index && players[i].enabled == false)
                    players[i].Enable();
                if (i >= index && players[i].enabled == true)
                    players[i].Disable();
            }
        }


        public void setBet(int player, int bet)
        {
            players[player].setBet(bet);
        }


        public int myIndex()
        {
            for (int i = 0; i < 10; i++)
            {
                if (players[i].position.Checked == true)
                    return i;
            }
            return -1;
        }


        public int allinIndex()
        {
            for (int i = 0; i < 10; i++)
                if (players[i].allin.Checked == true)
                    return i;
            return -1;
        }


        // mode:    true  = push
        //          false = call
        // players: player count
        // 
        public void enablePlayers(Boolean mode, int playersCount)
        {
            if (Updating)
                return;

            Updating = true;

            labelEVfoldVal.Text = "-";
            labelEVpushCallVal.Text = "-";
            labelEVdiffVal.Text = "-";

            // Deactivate unnecessary players
            for (int i = 0; i < 10; i++)
            {
                if (i < playersCount && players[i].enabled == false)
                    players[i].Enable();
                if (i >= playersCount && players[i].enabled == true)
                    players[i].Disable();
            }

            // push -mode settings
            if (mode == true)
            {
                labelEVpushCall.Text = "EV Push";

                //first positions button is hidden
                players[0].position.Enabled = false;

                // we need to enable one less than playercount
                for (int i = 1; i < 10; i++)
                {
                    if (i < playersCount)
                        players[i].position.Enabled = true;
                    else
                        players[i].position.Enabled = false;
                }

                // in push mode all players all-in button is hidden
                for (int i = 0; i < 10; i++)
                    players[i].allin.Enabled = false;

                // etsitään oma positio ja 
                int myindex = myIndex();

                // if we are in first posiotion, move to second
                if (myindex == 0)
                {
                    players[1].position.Checked = true;
                    myindex = 1;
                }

                // Set own position to one higher than player count if needed
                if (myindex >= playersCount - 1)
                {
                    //System.Windows.Forms.MessageBox.Show("muutetaan position: " + Convert.ToString(playersCount - 2));
                    players[playersCount - 1].position.Checked = true;
                }

                // hide range setters which are below own position
                myindex = myIndex();
                if (myindex != -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (i < myindex)
                        {
                            players[i].slider.Enabled = true;
                            //players[i].labelRange.Enabled = true;
                            players[i].enableLabels();
                        }
                        else
                        {
                            players[i].slider.Enabled = false;
                            players[i].disableLabels();
                        }
                    }
                }
            }
            // call -mode settings
            else
            {
                labelEVpushCall.Text = "EV Call";

                // we need to enable one less than playercount

                for (int i = 0; i < 10; i++)
                {
                    if (i < playersCount - 1)
                        players[i].position.Enabled = true;
                    else
                        players[i].position.Enabled = false;
                }

                // Set own position to one higher than player count if needed

                int myindex = myIndex();
                if (myindex >= playersCount - 1)
                    players[playersCount - 2].position.Checked = true;
                else if (playersCount == 2 && myindex == 1)
                    players[0].position.Checked = true;
                else if (myindex == -1)
                    players[playersCount - 2].position.Checked = true;

                // All-in buttons are shown to all opponents which are below us and in game
                myindex = myIndex();
                for (int i = 0; i < myindex + 1; i++)
                    players[i].allin.Enabled = false;
                for (int i = myindex + 1; i < playersCount; i++)
                    players[i].allin.Enabled = true;

                int allinindex = allinIndex();
                if (allinindex > playersCount - 1)
                {
                    players[playersCount - 1].allin.Enabled = true;
                    players[playersCount - 1].allin.Checked = true;
                }
                allinindex = allinIndex();
                myindex = myIndex();
                if (allinindex <= myindex)
                {
                    players[myindex + 1].allin.Enabled = true;
                    players[myindex + 1].allin.Checked = true;
                }

                // show range field only for opponent who went all-in 
                for (int i = 0; i < 10; i++)
                {
                    if (players[i].allin.Checked == true)
                        players[i].slider.Enabled = true;
                    else
                        players[i].slider.Enabled = false;
                }

                // Disable other labels, but our own
                myindex = myIndex();
                allinindex = allinIndex();
                for (int i = 0; i < 10; i++)
                {
                    // omasta indeksistä kaikki aktiiviseksi, paitsi labelRange
                    if (i == myindex)
                        players[i].enableLabels();
                    else
                        players[i].disableLabels();
                }
            }
            Updating = false;
        }


        public void blindChanged(int playersCount, Boolean isPushMode, Boolean noSB, string blindStr)
        {
            if (Updating)
                return;

            Updating = true;

            labelEVfoldVal.Text = "-";
            labelEVpushCallVal.Text = "-";
            labelEVdiffVal.Text = "-";

            // Empty bets if we are in push mode
            if (isPushMode == true)
            {
                labelEVpushCall.Text = "EV Push";

                //System.Windows.Forms.MessageBox.Show("isPushMode == true");
                for (int i = 0; i < playersCount; i++)
                    players[i].bets.Text = "";

            }
            else
                labelEVpushCall.Text = "EV Call";

            // Retrieve size of blinds and antes
            int BB = 0, SB = 0, ante = 0;
            string[] words = blindStr.Split(new Char[] { '/', '+' });
            if (words.Length == 2)
            {
                //Convert.
                SB = Convert.ToInt32(words[0], 10);
                BB = Convert.ToInt32(words[1], 10);
            }
            if (words.Length == 3)
            {
                SB = Convert.ToInt32(words[0], 10);
                BB = Convert.ToInt32(words[1], 10);
                ante = Convert.ToInt32(words[2], 10);
            }

            int[] bets = new int[playersCount];
            int chips;
            for (int i = 0; i < playersCount; i++)
            {
                Player test = players[i];
                chips = Convert.ToInt32(players[i].chips.Text, 10);

                // ante
                if (ante != 0)
                {
                    if (chips > ante)
                    {
                        chips -= ante;
                        bets[i] = ante;
                    }
                    else
                    {
                        bets[i] = chips;
                        chips = 0;
                    }
                }

                // BB
                if (i == 0)
                {
                    if (chips > BB)
                    {
                        bets[i] += BB;
                        chips -= BB;
                    }
                    else
                    {
                        bets[i] += chips;
                        chips = 0;
                    }
                }

                // SB
                if (i == 1 && noSB == false)
                {
                    //System.Windows.Forms.MessageBox.Show("blindStr: " + blindStr + " " + "SB: " + Convert.ToString(SB) + " " + "BB: " + Convert.ToString(BB));
                    if (chips > SB)
                    {
                        bets[i] += SB;
                        chips -= SB;
                    }
                    else
                    {
                        bets[i] += chips;
                        chips = 0;
                    }
                }

                // all-in
                if (isPushMode == false && i == allinIndex())
                {
                    bets[i] += chips;
                }
            }
            for (int i = 0; i < playersCount; i++)
            {
                players[i].bets.Text = Convert.ToString(bets[i]);
            }

            // Set labels which are below chips and stacks 
            int betsSum = 0;
            int chipsSum = 0;
            for (int i = 0; i < playersCount; i++)
            {
                betsSum += bets[i];
                chipsSum += Convert.ToInt32(players[i].chips.Text, 10);
            }
            labelBetSum.Text = Convert.ToString(betsSum);
            labelChipSum.Text = Convert.ToString(chipsSum);

            Updating = false;
        }


        public void ReadUi(double[] ICMs, int PlayerCount, int handIndex)
        {
            this.ICMs = ICMs;
            playersCount = PlayerCount;
            this.handIndex = handIndex;
            playersData = new double[10, 9];
            results = new double[2];
            // read content of playersData table
            for (int i = 0; i < playersCount; i++)
            {

                if (players[i].chips.Text != "")
                    playersData[i, STACK] = Convert.ToDouble(players[i].chips.Text);


                if (players[i].bets.Text != "")
                    playersData[i, BETS] = Convert.ToDouble(players[i].bets.Text);

                playersData[i, CALLRANGE] = Convert.ToDouble(players[i].slider.Range);

            }
        }

        public void UpdateUI(bool mode)
        {
            if (mode)
            {
                labelEVfoldVal.Text = string.Format("{0:F1}", results[0] * 100) + "%";
                labelEVpushCallVal.Text = string.Format("{0:F1}", results[1] * 100) + "%";

                if (results[1] * 100 - results[0] * 100 > 0)
                    labelEVdiffVal.Text = "+" + string.Format("{0:F1}", results[1] * 100 - results[0] * 100) + "%";
                else
                    labelEVdiffVal.Text = string.Format("{0:F1}", results[1] * 100 - results[0] * 100) + "%";

                for (int i = 0; i < myIndex(); i++)
                {
                    players[i].labelCall.Text = string.Format("{0:F1}", playersData[i, CALLPERC] * 100);
                    players[i].labelWin.Text = string.Format("{0:F1}", playersData[i, WINPERC] * 100);
                    players[i].labelEVwin.Text = string.Format("{0:F1}", playersData[i, EVWIN] * 100);
                    players[i].labelEVlose.Text = string.Format("{0:F1}", playersData[i, EVLOSE] * 100);
                }
            }
            else
            {
                labelEVfoldVal.Text = string.Format("{0:F1}", results[0] * 100) + "%";
                labelEVpushCallVal.Text = string.Format("{0:F1}", results[1] * 100) + "%";
                //labelEVdiffVal.Text = string.Format("{0:F1}", results[1] * 100 - results[0] * 100) + "%";
                if (results[1] * 100 - results[0] * 100 > 0)
                    labelEVdiffVal.Text = "+" + string.Format("{0:F1}", results[1] * 100 - results[0] * 100) + "%";
                else
                    labelEVdiffVal.Text = string.Format("{0:F1}", results[1] * 100 - results[0] * 100) + "%";


                players[myIndex()].labelWin.Text = string.Format("{0:F1}", playersData[myIndex(), WINPERC] * 100);
                players[myIndex()].labelEVwin.Text = string.Format("{0:F1}", playersData[myIndex(), EVWIN] * 100);
                players[myIndex()].labelEVlose.Text = string.Format("{0:F1}", playersData[myIndex(), EVLOSE] * 100);
            }

            // Always show prepost -datas
            icm.prepost(playersCount, playersData, ICMs, ICMs.Length);
            for (int i = 0; i < playersCount; i++)
                players[i].prepost.Text = string.Format("{0:F1}", playersData[i, PREPOST] * 100);
                        
            // reset labels which  are below chips and bets
            int betsSum = 0;
            int chipsSum = 0;
            for (int i = 0; i < playersCount; i++)
            {
                if (players[i].bets.Text != "")
                    betsSum += Convert.ToInt32(players[i].bets.Text, 10);
                if (players[i].chips.Text != "")
                    chipsSum += Convert.ToInt32(players[i].chips.Text, 10);
            }
            labelBetSum.Text = Convert.ToString(betsSum);
            labelChipSum.Text = Convert.ToString(chipsSum);

        }


        // radioButtonPush.Checked, comboBoxPlayers.SelectedIndex + 2, comboBoxHand.SelectedIndex, ICMs, ICMc
        public double calcICM(bool mode)
        {           
            results = new double[] { 0, 0 };

            // push
            if (mode == true)
            {
                //System.Windows.Forms.MessageBox.Show("playersCount: " + Convert.ToString(playersCount) + " " + "handIndex: " + Convert.ToString(handIndex) + " " + "myIndex: " + Convert.ToString(myIndex()));
                icm.calcPush(playersCount, handIndex, myIndex(), playersData, results, ICMs, ICMs.Length);
            }

            // call
            if (mode == false)
                icm.calcCall(playersCount, handIndex, myIndex(), allinIndex(), playersData, results, ICMs, ICMs.Length);

            return results[1] * 100 - results[0] * 100;
        }


        public void setRange(int player, int range, bool updateui)
        {
            playersData[player, CALLRANGE] = range;
            if (updateui)
                players[player].slider.Range = range;
        }


        public int HandIndex
        {
            get
            {
                return handIndex;
            }
            set
            {
                handIndex = value;
            }
        }


        public void ICMRanges(BlindInfo blinds, bool nosb, Award award, bool isPush)
        {
            calcRanges ranges = new calcRanges();
            int PLAYERCOUNT = 10;
            int[] playerrange = new int[PLAYERCOUNT];
            int[] stacks = new int[PLAYERCOUNT];
            int allin = -1;

            for (int i = 0; i < PLAYERCOUNT; i++)
                playerrange[i] = 1;

            int found = 0;
            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                if (playersData[i, STACK] != 0)
                    stacks[found++] = (int)playersData[i, STACK];
            }

            // jos ollaan Push -moodissa, niin etsitään oma indeksi pääohjelman vasemmanpuolisimmista radio buttoneista
            if (isPush)
            {
                allin = -1;
                for (int i = 0; i < 10; i++)
                {
                    if (players[i].position.Checked == true)
                    {
                        allin = i;
                        break;
                    }
                }
            }
            // call -moodissa korottaja löytyy all-in radio -buttoneiden avulla
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (players[i].allin.Checked == true)
                    {
                        allin = i;
                        break;
                    }
                }
            }

            ranges.calc(found, stacks, allin, blinds.Bigblind, blinds.Ante, nosb, 0.1, award.wins.ToArray(), playerrange);

            Console.Write("playerrange: ");
            for (int i = 0; i < 10; i++)
                Console.Write(playerrange[i] + ", ");
            Console.Write("\n");

            for (int i = 0; i < found; i++)
                setRange(i, playerrange[i],true);
        }


        public double stack(int playernum)
        {
            return playersData[playernum, STACK];
        }


    }
}
