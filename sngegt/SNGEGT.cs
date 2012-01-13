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

//#define DEBUGOMA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Principal;




namespace SNGEGT
{
    public interface GameChangedNotifier
    {
        void GameChanged(TableData data);
    }


    public partial class SNGPH : Form, GameChangedNotifier
    {
        private Game game;
        private String[] strRanks;
        private TableFinder party;
        private TrackBar LastTrackBar;
        private Label LastLabel;
        private Hashtable structures;
        private BlindSaver blindstruct;
        private GameSelectForm HandHistoryForm;
        private StackToRangeForm StackToRange;
        private PresetRangeForm presetForm;
        private RangeSelector[] RangeSelectors;
        
        private Boolean fillingform;
        private Boolean isok;
        private Boolean incalcs;
        private Bitmap graphBitmap;

        private String iCasinoName;        

        private String SETTINGSFILE = Application.StartupPath + "\\" + "settings.xml";
#if DEBUGOMA
        private SNGEGT.kuvaform kuvaform;
#endif

        
        private string md5(string filename)
        {
            StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(Application.StartupPath + "\\" + filename, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(fs);
            fs.Close();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }
        


        public bool HashMatch(Stream stream, String toMathch)
        {
            MD5 hasher = new MD5CryptoServiceProvider();
            byte[] binhash = hasher.ComputeHash(stream);
            StringBuilder stringhash = new StringBuilder(2 * binhash.Length);
            for (int j = 0; j < binhash.Length; j++)
            {
                stringhash.Append(binhash[j].ToString("x2"));
            }
            hasher.Clear();
            return stringhash.ToString().Equals(toMathch);
        }



        public SNGPH()
        {
            
            InitializeComponent();
            
            isok = true;

            strRanks = new String[101] { "", "KK+,AKs", "QQ+,AK", "JJ+,AK", "TT+,AQ+", "99+,AQ+", "88+,AQo+,ATs+", "88+,AJo+,ATs+", "66+,AT+", "66+,ATo+,A9s+", "55+,ATo+,A8s+,KQs", "44+,A9o+,A8s+,KQs", "44+,A9o+,A7s+,KJs+", "44+,AKs,AQs,AJs,ATs,A9s,A8s,A8o+,A7s,A5s,KJs+", "44+,A8o+,A4s+,KJs+", "33+,A7o+,A4s+,KTs+", "33+,A7o+,A3s+,KQo,KTs+", "33+,A7o+,A2s+,KQo,KTs+", "33+,AKo,AQo,AJo,ATo,A9o,A8o,A7o,A5o,A2s+,KQo,KTs+", "33+,A5o+,A2s+,KQo,KTs+", "33+,A4o+,A2s+,KJo+,KTs+", "33+,A4o+,A2s+,KJo+,KTs+,QJs", "33+,A3o+,A2s+,KJo+,KTs+,QJs", "22+,A2+,KJo+,K9s+,QJs", "22+,A2+,KTo+,K9s+,QJs", "22+,A2+,KTo+,K8s+,QTs+", "22+,A2+,K9o+,K7s+,QTs+,JTs", "22+,A2+,K9o+,K6s+,QJo,QTs+,JTs", "22+,A2+,K9o+,K6s+,QJo,Q9s+,JTs", "22+,A2+,K8o+,K5s+,QJo,Q9s+,JTs", "22+,A2+,K8o+,K4s+,QTo+,Q9s+,JTs", "22+,A2+,K7o+,K4s+,QTo+,Q9s+,JTs", "22+,A2+,K7o+,K2s+,QTo+,Q9s+,JTs", "22+,A2+,K6o+,K2s+,QTo+,Q8s+,JTs", "22+,A2+,K5o+,K2s+,QTo+,Q8s+,J9s+", "22+,A2+,K5o+,K2s+,Q9o+,Q8s+,J9s+", "22+,A2+,K5o+,K2s+,Q9o+,Q8s+,JTo,J9s+", "22+,A2+,K4o+,K2s+,Q9o+,Q8s+,JTo,J9s+", "22+,A2+,K4o+,K2s+,Q9o+,Q6s+,JTo,J9s+,T9s", "22+,A2+,K3o+,K2s+,Q9o+,Q6s+,JTo,J9s+,T9s", "22+,A2+,K2+,Q9o+,Q5s+,JTo,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q5s+,JTo,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q4s+,J9o+,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q3s+,J9o+,J8s+,T8s+", "22+,A2+,K2+,Q7o+,Q3s+,J9o+,J7s+,T8s+", "22+,A2+,K2+,Q6o+,Q2s+,J9o+,J7s+,T8s+", "22+,A2+,K2+,Q6o+,Q2s+,J9o+,J7s+,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J7s+,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J7s+,T9o,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J6s+,T9o,T8s+,98s", "22+,A2+,K2+,Q4o+,Q2s+,J8o+,J5s+,T9o,T7s+,98s", "22+,A2+,K2+,Q4o+,Q2s+,J7o+,J4s+,T9o,T7s+,98s", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J4s+,T9o,T7s+,98s", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J4s+,T8o+,T7s+,97s+", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J3s+,T8o+,T7s+,97s+", "22+,A2+,K2+,Q2+,J7o+,J3s+,T8o+,T6s+,97s+", "22+,A2+,K2+,Q2+,J6o+,J2s+,T8o+,T6s+,97s+,87s", "22+,A2+,K2+,Q2+,J6o+,J2s+,T8o+,T6s+,98o,97s+,87s", "22+,A2+,K2+,Q2+,J6o+,J2s+,T7o+,T6s+,98o,97s+,87s", "22+,A2+,K2+,Q2+,J5o+,J2s+,T7o+,T6s+,98o,96s+,87s", "22+,A2+,K2+,Q2+,J5o+,J2s+,T7o+,T5s+,98o,96s+,87s", "22+,A2+,K2+,Q2+,J4o+,J2s+,T7o+,T4s+,98o,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T4s+,98o,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T4s+,97o+,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T3s+,97o+,96s+,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T3s+,97o+,95s+,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T2s+,97o+,95s+,87o,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T5o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J2+,T5o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J2+,T5o+,T2s+,96o+,94s+,87o,85s+,75s+", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,96o+,94s+,87o,85s+,75s+", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,96o+,94s+,86o+,85s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,95o+,93s+,86o+,84s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,93s+,86o+,84s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,93s+,86o+,84s+,76o,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,92s+,86o+,84s+,76o,74s+,65s", "22+,A2+,K2+,Q2+,J2+,T2+,95o+,92s+,86o+,84s+,76o,74s+,65s,54s", "22+,A2+,K2+,Q2+,J2+,T2+,95o+,92s+,85o+,84s+,76o,74s+,65s,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,83s+,76o,74s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,83s+,75o+,74s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,82s+,75o+,73s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,85o+,82s+,75o+,73s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,85o+,82s+,75o+,73s+,65o,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,84o+,82s+,75o+,73s+,65o,63s+,53s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,75o+,73s+,65o,63s+,53s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,73s+,65o,63s+,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,65o,63s+,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,64o+,63s+,54o,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,64o+,63s+,54o,52s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,83o+,82s+,74o+,72s+,64o+,62s+,54o,52s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,74o+,72s+,64o+,62s+,54o,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,64o+,62s+,54o,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,64o+,62s+,53o+,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,63o+,62s+,53o+,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,63o+,62s+,53o+,52s+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,63o+,62s+,53o+,52s+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,63o+,62s+,52+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,42+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,42+,32" };
            
            Player p0 = new Player(trackBar0, position0, chips0, prepost0, bets0, allin0, labelCall0, labelWin0, labelEVwin0, labelEVlose0);
            Player p1 = new Player(trackBar1, position1, chips1, prepost1, bets1, allin1, labelCall1, labelWin1, labelEVwin1, labelEVlose1);
            Player p2 = new Player(trackBar2, position2, chips2, prepost2, bets2, allin2, labelCall2, labelWin2, labelEVwin2, labelEVlose2);
            Player p3 = new Player(trackBar3, position3, chips3, prepost3, bets3, allin3, labelCall3, labelWin3, labelEVwin3, labelEVlose3);
            Player p4 = new Player(trackBar4, position4, chips4, prepost4, bets4, allin4, labelCall4, labelWin4, labelEVwin4, labelEVlose4);
            Player p5 = new Player(trackBar5, position5, chips5, prepost5, bets5, allin5, labelCall5, labelWin5, labelEVwin5, labelEVlose5);
            Player p6 = new Player(trackBar6, position6, chips6, prepost6, bets6, allin6, labelCall6, labelWin6, labelEVwin6, labelEVlose6);
            Player p7 = new Player(trackBar7, position7, chips7, prepost7, bets7, allin7, labelCall7, labelWin7, labelEVwin7, labelEVlose7);
            Player p8 = new Player(trackBar8, position8, chips8, prepost8, bets8, allin8, labelCall8, labelWin8, labelEVwin8, labelEVlose8);
            Player p9 = new Player(trackBar9, position9, chips9, prepost9, bets9, allin9, labelCall9, labelWin9, labelEVwin9, labelEVlose9);

            
            
            game = new Game(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, labelSumChips, labelBetsSum, labelEVpushCall, labelEVfoldVal, labelEVpushCallVal, labelEVdiffVal);
            
            RangeSelectors = new RangeSelector[10];

            for (int i = 0; i < 10; i++)
                RangeSelectors[i] = (RangeSelector)panel2.Controls["trackBar" + Convert.ToString(i)];                    

            // alkuasetuksiksi Party SNG, neljä pelaajaa, push -moodi, AA, 20/40, 4 positio
            // comboBoxBlinds.Items.AddRange(blindsParty);
            // comboBoxBlinds.SelectedIndex = 5;
            //comboBoxGame.SelectedIndex = 1;

            comboBoxPlayers.SelectedIndex = 2;
            radioButtonPush.Checked = true;
            comboBoxHand.SelectedIndex = 0;
            position3.Checked = true;
            allin2.Checked = true;

            LoadBlindStructs();



            if (comboBoxCasinos.Items.Count == 0)
            {
                isok = false;
                return;
            }

            toolTip1.SetToolTip(trackBar0, "");
            toolTip1.Active = true;

            
             
#if (!DEBUGOMA)
            buttonOpenFile.Hide(); // näytetään kuvanavausnappi vain debug-moodissa
#endif

            Text = "SNGEGT-GT";
            tableTimer.Enabled = true;

#if (DEBUGOMA)
            Text = "SNGEGT-Debug";
            buttonOpenFile.Show();
            tableTimer.Enabled = false;
#endif


            
     

         // kaikille yhteisen sliderin asetukset
            trackBarAll.Maximum = 100;
            trackBarAll.Minimum = 1;
            trackBarAll.SmallChange = 1;
            trackBarAll.LargeChange = 5;
            trackBarAll.Value = 10;
            List<BlindInfo> blindstruct = ((BlindSaver)structures[comboBoxCasinos.SelectedItem.ToString()]).blinds;
            party = new TableFinder(new partyTableReader(((BlindSaver)structures["Party"]).blinds, new IntPtr(-1)));
            
            //MessageBox.Show(Convert.ToString(System.Windows.Forms.SystemInformation.CaptionHeight));

            StackToRange = new StackToRangeForm();
            presetForm = new PresetRangeForm();
            
            for (int i = 0; i < RangeSelectors.Length; i++)
                RangeSelectors[i].setRanges(presetForm.getRanges());
            this.HandHistoryForm = new GameSelectForm(this);
            dataGridView2.ColumnCount = 0;
            
            String cardvalues = "AKQJT98765432";
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns.Add(column);
            
            for (int i = 0; i < cardvalues.Length; i++)
            {
                column = new DataGridViewTextBoxColumn();
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Name = cardvalues[i].ToString();
                column.HeaderText = column.Name;
                dataGridView2.Columns.Add(column);
            }

            dataGridView2.RowCount = 13;

            for (int i = 0; i < cardvalues.Length; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = cardvalues[i];
                dataGridView2.Rows[i].Cells[0].Style.BackColor = SystemColors.Control ;
                dataGridView2.Rows[i].Cells[0].Style.SelectionBackColor = SystemColors.Control;           
                dataGridView2.Rows[i].Height = 14;
            }
            dataGridView2.Columns[0].Width = 20;
            dataGridView2.Columns[0].Frozen = true;

            DataGridViewCell cell;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                for (int j = 1; j < dataGridView2.Columns.Count; j++)
                {
                    cell = dataGridView2.Rows[i].Cells[j];

                    cell.Value = cardvalues[i].ToString() + cardvalues[j - 1].ToString();
                    if (i + 1 == j)
                        continue;

                    if (i + 1 < j)
                        cell.Value += "s";
                    else
                        cell.Value = cardvalues[j-1].ToString() + cardvalues[i].ToString() +"o";
                }

            graphBitmap = new Bitmap(graphImage.Width, graphImage.Height);
            fillingform = false;

            if (System.Windows.Forms.Screen.PrimaryScreen.BitsPerPixel < 32)
            {
                MessageBox.Show("You don't have 32 bit colors enabled. Realtime features does not work without this.\n\n If you need assistance how to enable 32 bit colors please send e-mail to sngegt@gmail.com", "Color depth error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            System.OperatingSystem opsys = System.Environment.OSVersion;

            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            

            if ( opsys.Platform == PlatformID.Win32NT)
            {                
                
                if (opsys.Version.Major == 6 && !p.IsInRole(WindowsBuiltInRole.Administrator))
                {
                MessageBox.Show("Realtime features requires administrator priviledges on Windows Vista.\n\n If you need assistance how to run SNGEGT as administrator please send e-mail to sngegt@gmail.com", "Access rights error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }


        public Boolean isOk()
        {
            return isok;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(SETTINGSFILE);
                XmlNode node = doc.FirstChild.FirstChild;
                while (node != null)
                {
                    if (node.Name == "UiSettings")
                    {
                        node = node.FirstChild;
                        while (node != null)
                        {
                            if (node.Name == "UseOptimalRanges" && node.InnerText == "1")
                            {
                                useOptimalRangesToolStripMenuItem.PerformClick();
                            }
                            else if (node.Name == "UsePresetButtons" && node.InnerText == "1")
                                usePresetButtonsToolStripMenuItem.PerformClick();
                            else if (node.Name == "UseStackBlindRanges" && node.InnerText == "1")
                                useMbasedRangesToolStripMenuItem.PerformClick();

                            node = node.NextSibling;
                        }
                        break;
                    }
                    node = node.NextSibling;
                }
            }
            catch (Exception)
            {
            }
        }


        private void showStrHands(TrackBar iTrackBar, Label iLabel)
        {
            toolTip1.SetToolTip(iTrackBar, "");
            toolTip1.Active = true;
            toolTip1.Show(Convert.ToString(iTrackBar.Value) + "% " + strRanks[iTrackBar.Value], iTrackBar, 130, 0, 1000);
            iLabel.Text = Convert.ToString(iTrackBar.Value) + "%";
        }

        
        private void comboBoxHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBlinds.Items.Count == 0)
                return;

            game.enablePlayers(radioButtonPush.Checked, comboBoxPlayers.SelectedIndex + 2);
            game.blindChanged(comboBoxPlayers.SelectedIndex + 2, radioButtonPush.Checked, checkBoxNoSB.Checked, (string)comboBoxBlinds.SelectedItem);
        }


        private void comboBoxBlinds_SelectedIndexChanged(object sender, EventArgs e)
        {
            game.blindChanged(comboBoxPlayers.SelectedIndex + 2, radioButtonPush.Checked, checkBoxNoSB.Checked, (string)comboBoxBlinds.SelectedItem);
        }


        private void checkBoxNoSB_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex != -1)
                game.blindChanged(comboBoxPlayers.SelectedIndex + 2, radioButtonPush.Checked, checkBoxNoSB.Checked, comboBoxBlinds.SelectedItem.ToString());
        }


        private void labelAction_Click(object sender, EventArgs e)
        {

        }


        private void radioButtonPush_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void radioButtonCall_CheckedChanged(object sender, EventArgs e)
        {
            Gameinfo_Changed(sender, e);
            if (radioButtonPush.Checked)
                labelRange.Text = "Call Range";
            else
                labelRange.Text = "Push Range";
        }
        

        private void labelPosition_Click(object sender, EventArgs e)
        {

        }


        private void chips2_TextChanged(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
          

        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }


        private bool readUi()
        {

            if (blindstruct != null)
            {
                if (comboBoxGame.SelectedItem == null)
                    return false;

                Award award = (Award)comboBoxGame.SelectedItem;                                                            

                double[] ICMs = new double[award.wins.Count];
                int ICMc = award.wins.Count;

                for (int i = 0; i < ICMc; i++)
                    ICMs[i] = award.wins[i] / 100.0;

                game.ReadUi(ICMs, comboBoxPlayers.SelectedIndex + 2, comboBoxHand.SelectedIndex);
                return true;                
            }

            return false;
        }


        private void doCalcs()        
        {
            if (!incalcs)
                SetRanges();

            incalcs = true;
            pictureBox1.Image = null;
            labelWarning.Text = "";

            if (readUi())
            {
                double value = game.calcICM(radioButtonPush.Checked);
                game.UpdateUI(radioButtonPush.Checked);
            }
            incalcs = false;

        }


        private void buttonCompute_Click(object sender, EventArgs e)
        {            
            doCalcs();         
        }


        private void labelSumChips_SizeChanged(object sender, EventArgs e)
        {
            labelSumChips.Left = chips9.Left + (chips9.Width - labelSumChips.Width) / 2;
        }


        private void labelBetsSum_SizeChanged(object sender, EventArgs e)
        {
            labelBetsSum.Left = bets9.Left + (bets9.Width - labelBetsSum.Width) / 2;
        }


        // asettaa komponentit ja tekee laskut joko avatun tiedostonimen tai listaindeksi mukaan
        private void setFormsAndDoCalc(bool readFromPPlist, String fileName)
        {
            if (readFromPPlist && (listBoxTables.SelectedIndex == -1))
                return;

            PokerTable table = null;

            try
            {
                fillingform = true;
                pictureBox1.Image = null;
                labelWarning.Text = "";
                String searchfor = "";

                TableData data = new TableData();

                // getTableData(partyTableData oData, string tableNameStr, int iHwndIndex, string fileName)

                if (readFromPPlist)
                {
                    table = (PokerTable)listBoxTables.SelectedItem;
                    searchfor = table.getCasinoName();
                }
                else if (fileName != "")
                {
                    
                    party.getTableData(ref data, fileName);
                    searchfor = iCasinoName;
                }
                else
                {
                    pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    labelWarning.Text = "Can't find a game";
                    return;
                }
                if (searchfor != "")
                {
                    comboBoxCasinos.SelectedIndex = -1;
                    for (int i = 0; i < comboBoxCasinos.Items.Count; i++)
                        if ((string)comboBoxCasinos.Items[i] == searchfor)
                        {
                            comboBoxCasinos.SelectedIndex = i;

                            break;
                        }
                }

                if (table != null)
                {
                    table.SetBlindStruct(((BlindSaver)structures[searchfor]).blinds);
                    party.getTableData(ref data, table);
                }
                FillForm(data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                pictureBox1.Image = SystemIcons.Error.ToBitmap();
                labelWarning.Text = "Serious error when reading poker table.";
                //labelWarning.Text = e.ToString();
                //textBoxDebug.Text = e.ToString();
            }
            finally
            {
                
            }
        }


        //data.error: 0=onnistui, 1=limppereitä tai ei allin kokoista bettiä, 2=ei preflopissa
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //setFormsAndDoCalc(true, "");           
        }


        private void tableTimer_Tick(object sender, EventArgs e)
        {
            listBoxTables.BeginUpdate();
            listBoxTables.Items.Clear();

            Hashtable foundtables = party.getTables();

            foreach (List<PokerTable> tables in foundtables.Values)
            {
                foreach (PokerTable table in tables)
                    listBoxTables.Items.Add(table);
            }
            listBoxTables.EndUpdate();
        }

        
        private void asiaaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }


        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("kuva OK");
            DialogResult res = MessageBox.Show("Onko party", "Onko party", MessageBoxButtons.YesNo);
            //DialogResult res = DialogResult.No;
            if (res == DialogResult.Yes)
            {
                res = MessageBox.Show("Onko speeed", "Onko speed", MessageBoxButtons.YesNo);
                iCasinoName = "Party";
                if (res == DialogResult.Yes)
                    iCasinoName = "Party speed";
                party = new TableFinder(new partyTableReader(((BlindSaver)structures[iCasinoName]).blinds, new IntPtr(-1)));
            }
            else
            {
                res = MessageBox.Show("Onko Stars", "Onko stars", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    party = new TableFinder(new StarsTable(((BlindSaver)structures["PokerStars"]).blinds, new IntPtr(-1)));
                    iCasinoName = "PokerStars";
                }
                else
                {
                    party = new TableFinder(new FullTiltTable(((BlindSaver)structures["FullTilt"]).blinds, new IntPtr(-1)));
                    iCasinoName = "FullTilt";
                }
            }

            setFormsAndDoCalc(false, openFileDialog.FileName);
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            
#if (DEBUGOMA)
            openFileDialog.ShowDialog();
#endif
        }


        private void get_label(TrackBar sender)
        {
            if (LastTrackBar != sender)
            {
                string sendernumber;
                sendernumber = (sender as TrackBar).Name.Substring(8).Split('_')[0];

                for (int i = 0; i < panel2.Controls.Count; i++)
                {
                    if (panel2.Controls[i].Name == "labelRange" + sendernumber)
                    {
                        LastLabel = panel2.Controls[i] as Label;
                        break;
                    }
                }
                LastTrackBar = sender as TrackBar;
            }
        }


        private void trackBar_MouseHover(object sender, EventArgs e)
        {

            get_label(sender as TrackBar);
            showStrHands(LastTrackBar, LastLabel);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            showStrHands(LastTrackBar, LastLabel);
        }


        private void trackBar_MouseEnter(object sender, EventArgs e)
        {
            get_label(sender as TrackBar);
            timer1.Enabled = true;
        }


        private void trackBar_MouseLeave(object sender, EventArgs e)
        {
            if (!(sender is TrackBar))
                return;
            timer1.Enabled = false;
            toolTip1.Active = false;
        }





        private void listBoxPartyTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            setFormsAndDoCalc(true, "");
        }


        private void trackBarAll_Scroll(object sender, EventArgs e)
        {
            showStrHands(trackBarAll, labelRangeAll);

            for (int i = 0; i < RangeSelectors.Length; i++)
            {
                RangeSelectors[i].RangeChanged -= RangeChanged;
                RangeSelectors[i].Range = trackBarAll.Value;
                RangeSelectors[i].RangeChanged += RangeChanged;
            }
            
            doCalcs();            
        }


        private void blindStructuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BlindsForm form = new BlindsForm();
            try
            {
                form.LoadXml(true);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                MessageBox.Show("Reading of blinds.xml failed");
                return;
            }
            form.ShowDialog();
            LoadBlindStructs();
        }


        private void LoadBlindStructs()
        {
            BlindsForm form = new BlindsForm();
            try
            {
                structures = form.LoadXml(false);
            }
            catch (Exception /*e*/)
            {
                //MessageBox.Show(e.Message);
                MessageBox.Show("Loading of blinds.xml failed");
                //Application.Exit();
                return;
            }
            comboBoxCasinos.BeginUpdate();
            comboBoxCasinos.Items.Clear();
            foreach (string sitename in structures.Keys)
            {   
                comboBoxCasinos.Items.Add(sitename);                    
            }

            

            comboBoxCasinos.EndUpdate();
            if (comboBoxCasinos.Items.Count > 0)
                comboBoxCasinos.SelectedIndex = 0;


            
            
#if DEBUGOMA
            tabPage4.AllowDrop = true;
#endif
            form = null;
        }


        private void comboBoxCasinos_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxGame.BeginUpdate();
            comboBoxGame.Items.Clear();
            comboBoxBlinds.BeginUpdate();
            comboBoxBlinds.Items.Clear();

            if (comboBoxCasinos.SelectedIndex == -1)
            {
                comboBoxGame.EndUpdate();
                comboBoxBlinds.EndUpdate();

                return;
            }

            BlindSaver saver = (BlindSaver)structures[comboBoxCasinos.SelectedItem];

            // add gametypes
            foreach (Award award in saver.awards)
            {
                comboBoxGame.Items.Add(award);
            }
            if (comboBoxGame.Items.Count > 0)
                comboBoxGame.SelectedIndex = 0;
            // add blind sizes to combobox
            foreach (BlindInfo blind in saver.blinds)
                comboBoxBlinds.Items.Add(blind);

            comboBoxBlinds.EndUpdate();
            comboBoxGame.EndUpdate();

            if (comboBoxBlinds.Items.Count > 0)
                comboBoxBlinds.SelectedIndex = 0;
            blindstruct = saver;
            if (party != null)
                party.SetBlindStructure(blindstruct.blinds);
        }


        private void Gameinfo_Changed(object sender, EventArgs e)
        {
            if (fillingform)
                return;

            game.enablePlayers(radioButtonPush.Checked, comboBoxPlayers.SelectedIndex + 2);
            if (comboBoxBlinds.SelectedIndex == -1)
                return;

            game.blindChanged(comboBoxPlayers.SelectedIndex + 2, radioButtonPush.Checked, checkBoxNoSB.Checked, comboBoxBlinds.SelectedItem.ToString());
            readUi();            
            doCalcs();            
            if (tabControl1.SelectedTab == tabPage2)
            {
                RunGraph();               
            }
        }


        private void RunGraph()
        {
            backgroundWorker1.CancelAsync();
            while (backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
            readUi();
            backgroundWorker1.RunWorkerAsync();
        }


        private void FillForm(TableData data)
        {
            Image img = null;            
            // Serious errors 1-10 -> end
            if (data.error >= 1 && data.error <= 10)
            {
                pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                labelWarning.Text = data.errorStr;
                return;
            }

            fillingform = true;

            for (int i = 0; i < 10; i++)
            {
                game.players[i].chips.Text = "0";
                game.players[i].bets.Text = "0";
            }
            for (int i = 0; i < data.players; i++)
            {
                if (data.stacks[i] >= 0)
                    game.players[i].chips.Text = Convert.ToString(data.stacks[i]);
                else
                    game.players[i].chips.Text = "";
            }


            
            if (data.mode == true)
                radioButtonPush.Checked = true;
            else
                radioButtonCall.Checked = true;

            //Search for struct where there is wanted amount of players
            for (int i = 0; i < blindstruct.awards.Count; i++)
            {
                if (blindstruct.awards[i].playercount == data.tableType)
                {
                    for (int j = 0; j < comboBoxGame.Items.Count; j++)
                        if (blindstruct.awards[i] == comboBoxGame.Items[j])
                        {
                            comboBoxGame.SelectedIndex = j;
                            break;
                        }
                    break;
                }
            }

            comboBoxPlayers.SelectedIndex = data.players - 2;
            comboBoxHand.SelectedIndex = data.handIndex;
            comboBoxBlinds.SelectedIndex = data.BBindex;
            checkBoxNoSB.Checked = data.noSB;
            game.players[data.myPos].position.Checked = true;
            game.players[data.allinIndex].allin.Checked = true;
            
            if (data.error <= 0)
            {
                fillingform = false;
                Gameinfo_Changed(this, null);
            }
            // Less serious errors
            else
            {
                // callers
                if (data.error == 11 && data.BBindex >= 0)
                {
                    BlindInfo info = (BlindInfo)(comboBoxBlinds.SelectedItem);
                    for (int i = 0; i < 10; i++)
                        if (data.stacks[i] > 0)
                            game.players[i].bets.Text = Convert.ToString((Math.Min(info.Ante, data.stacks[i])));


                    game.players[0].bets.Text = Convert.ToString(Math.Min(data.stacks[0], info.Ante + info.Bigblind));
                    if (!checkBoxNoSB.Checked)
                        game.players[1].bets.Text = Convert.ToString(Math.Min(data.stacks[1], info.Ante + info.Smallblind));                                        
                }
                img = SystemIcons.Warning.ToBitmap();     
            }
            
            fillingform = false;
            Gameinfo_Changed(this, null);

            pictureBox1.Image = img;
            labelWarning.Text = data.errorStr;
            Activate();

        }

        #region GameChangedNotifier Members
        public void GameChanged(TableData data)
        {
            TableData temp = new TableData(data);
            if (temp.casinoname != "")
            {
                comboBoxCasinos.SelectedIndex = -1;
                for (int i = 0; i < comboBoxCasinos.Items.Count; i++)
                    if (comboBoxCasinos.Items[i].ToString() == temp.casinoname)
                    {
                        comboBoxCasinos.SelectedIndex = i;
                        break;
                    }
            }

            party.getTableData(ref temp, "From oData");
            FillForm(temp);
        }
        #endregion


        private void openHandHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogHistory.ShowDialog();
        }


        private void openFileDialogHistory_FileOk(object sender, CancelEventArgs e)
        {
            HandHistoryForm.Visible = true;            
            HandHistoryForm.SelectGame(openFileDialogHistory.FileName,((BlindSaver)structures["FullTilt"]).blinds );
            
        }



   


        private void listBoxTables_MouseEnter(object sender, EventArgs e)
        {
            tableTimer.Enabled = false;
        }


        private void listBoxTables_MouseLeave(object sender, EventArgs e)
        {
        tableTimer.Enabled = true;
        }


        private void button1_Click_2(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Application.StartupPath, "table.bmp")))
                return;
            int i = 1;
            String path = Path.Combine(Application.StartupPath, "errors");
            if (!System.IO.Directory.Exists(path))
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (IOException)
                {
                    MessageBox.Show("Can't create errors directory");
                    return;
                }

            while (File.Exists(Path.Combine(path, "error_" + Convert.ToString(i)) + ".bmp"))
                i++;

            String filename = Path.Combine(path, "error_" + Convert.ToString(i) + ".bmp");
            File.Copy(Path.Combine(Application.StartupPath, "table.bmp"), filename);

            MessageBox.Show("Image " + "errors\\" + "error_" + Convert.ToString(i) + ".bmp saved");
        }


        private void stackrangeRatiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StackToRange.Show();
        }


        private void chips_Validating(object sender, CancelEventArgs e)
        {
            TextBox joo = sender as TextBox;
            readUi();
            doCalcs();            
        }


        private void SetRanges()
        {
            BlindInfo blinds = (BlindInfo)comboBoxBlinds.SelectedItem;
#if (DEBUGOMA)
            //MessageBox.Show("NG: Nyt muutetaan rangeja");
            if (useOptimalRangesToolStripMenuItem.Checked)
            {
                game.ICMRanges(blinds, checkBoxNoSB.Checked, (Award)comboBoxGame.SelectedItem, radioButtonPush.Checked);                
            }
#endif
            if (useOptimalRangesToolStripMenuItem.Checked)
                game.ICMRanges(blinds, checkBoxNoSB.Checked, (Award)comboBoxGame.SelectedItem, radioButtonPush.Checked);                
        

            
            
            if (useMbasedRangesToolStripMenuItem.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    int stack = (int)game.stack(i);
                    if (stack == -1)
                        continue;
                    int range = StackToRange.GetRange(stack, blinds.Bigblind);
                    game.setRange(i, range, true);
                }
            }
        
        }


        private void presetRangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presetForm.ShowDialog();
            for (int i = 0; i < RangeSelectors.Length; i++)
                RangeSelectors[i].setRanges(presetForm.getRanges());
            
        }


        private void usePresetButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RangeSelectors.Length; i++)
                RangeSelectors[i].ShowSlider(!usePresetButtonsToolStripMenuItem.Checked);
        }


        private void RangeChanged(object o, EventArgs e)
        {
            
            RangeSelector selector = (RangeSelector) o;
            if (!incalcs)
            {
                incalcs = true;
                doCalcs();
            }
        }


        private void updateGraph()
        {
        
            //How many percent we move at time
            int HOP = 1;
            
            int BOTTOMCAP = 20;                
            int LEFTCAP = 30;

            Graphics g = Graphics.FromImage(graphBitmap);
            Pen pen = new Pen(Color.Black);
            pen.Width = 3;

            int width = graphImage.Width - LEFTCAP;
            int height = graphImage.Height - BOTTOMCAP;
            g.Clear(this.BackColor);

            //This list contains results
            List<double> results = new List<double>(100);
            double min = int.MaxValue, max = int.MinValue;
            double val;

            //Go through percents 1..100 and set each players range to i. Save result to list
            for (int i = 1; i < 100 && !backgroundWorker1.CancellationPending; i += HOP)
            {
                for (int j = 0; j < RangeSelectors.Length; j++)
                    game.setRange(RangeSelectors[j].PlayerNumber, i, false);

                val = game.calcICM(radioButtonPush.Checked) * 100;
                if (val > max)
                    max = val;
                if (val < min)
                    min = val;

                results.Add(val);
            }

            if (backgroundWorker1.CancellationPending)
                return;

            double foundmin = min;
            double foundmax = max;

            max = max + 50;

            // -0.5 ja 0.5 because of *100
            max = Math.Max(50, max);

            min = Math.Min(-50, min);
            
            double interval = height / Math.Abs(max - min);


            //These lists contains positive and negative areas
            List<Rectangle> positive = new List<Rectangle>(results.Count);
            List<Rectangle> negative = new List<Rectangle>(results.Count);



            float xmove = ((float)width) / results.Count;
            int startx = LEFTCAP;

            
            int zerolevel = (int)(height - interval * Math.Abs(min));

            for (int i = 0; i < results.Count && !backgroundWorker1.CancellationPending; i++)
            {
                Size size = new Size((int)xmove + 1, (int)(interval * Math.Abs(results[i])));
                if (results[i] > 0)
                    positive.Add(new Rectangle(new Point((int)(LEFTCAP + i * (xmove)), zerolevel - size.Height), size));
                else
                    negative.Add(new Rectangle(new Point((int)(LEFTCAP + i * (xmove)), zerolevel), size));

            }

            if (backgroundWorker1.CancellationPending)
                return;

            List<Rectangle> markers = new List<Rectangle>(11);
            int h = 0;
            for (float x = LEFTCAP; x <= width + LEFTCAP; x += xmove * 10, h++)
            {
                if (h == 5)
                    markers.Add(new Rectangle((int)x, zerolevel - 10, 2, 10));
                else
                    markers.Add(new Rectangle((int)x, zerolevel - 5, 2, 5));
            }
            markers.Add(new Rectangle(width + LEFTCAP - 2, zerolevel - 5, 2, 5));


            //Draw the lines
            if (positive.Count > 0)
                g.FillRectangles(Brushes.Green, positive.ToArray());
            if (negative.Count > 0)
                g.FillRectangles(Brushes.Red, negative.ToArray());

            g.FillRectangles(Brushes.Black, markers.ToArray());
            g.DrawLine(pen, new Point(LEFTCAP, zerolevel), new Point(LEFTCAP + width, zerolevel));
            g.DrawString(String.Format("{0:F1}", foundmax / 100), SystemFonts.DefaultFont, Brushes.Black, new Point(0, 0));
            g.DrawString(String.Format("{0:F1}", foundmin / 100), SystemFonts.DefaultFont, Brushes.Black, new Point(0, height));
            pen.Dispose();
            g.Dispose();        
        }


        private void graphImage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(graphBitmap, 0, 0);
        }


        private void SNGPH_KeyUp(object sender, KeyEventArgs e)
        {
            
        
            if (e.KeyCode != Keys.ControlKey)
                return;

            POINT curpos = new POINT();
            User32.GetCursorPos(ref curpos);
            IntPtr handle = User32.WindowFromPoint(curpos);
            handle = User32.GetAncestor(handle, 2);
            
            for (int i = 0; i < listBoxTables.Items.Count; i++)
                if (((PokerTable)listBoxTables.Items[i]).HWND == handle)
                {
                    listBoxTables.SelectedIndex = i;
                    setFormsAndDoCalc(true, null);
                    e.Handled = true;
                    return;
                }

            pictureBox1.Image = SystemIcons.Error.ToBitmap();
            labelWarning.Text = "Couldn't find selected window from pokertables";
        }


        private void buttonAll_Click(object sender, EventArgs e)
        {
          
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            
        }


        private void button1_Click_3(object sender, EventArgs e)
        {
            readUi();
            bool matches;
            String combohand;
            Color matchcolor;
            bool suitedmatch = false;

            char upsearch, leftsearch;
            DataGridViewRow row = null;


            for (int i = 0; i < comboBoxHand.Items.Count; i++)
            {

                matchcolor = Color.Red;

                combohand = comboBoxHand.Items[i].ToString();
                game.HandIndex = i;
                matches = game.calcICM(radioButtonPush.Checked) >= Convert.ToDouble(numericUpDown1.Value);

                //Pari
                if (combohand.Length == 2)
                {
                    upsearch = combohand[0];
                    leftsearch = upsearch;
                    if (matches)
                        matchcolor = Color.Green;
                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                    {
                        object value = dataGridView2.Rows[j].Cells[0].Value;
                        if (dataGridView2.Rows[j].Cells[0].Value.Equals(leftsearch))
                        {
                            row = dataGridView2.Rows[j];
                            break;
                        }
                    }
                }

                else
                {
                    upsearch = combohand[1];
                    // if offsuited mathches set to green, if offsuite doesn't match and suited matches
                    //set to blue
                    if (combohand[2] == 'o')
                    {
                        if (matches)
                            matchcolor = Color.Green;
                        else if (suitedmatch)
                            matchcolor = Color.Blue;

                        suitedmatch = false;
                    }
                    else
                    {
                        suitedmatch = matches;
                    }
                }

                row.Cells[upsearch.ToString()].Style.BackColor = matchcolor;
                row.Cells[upsearch.ToString()].Style.SelectionBackColor = matchcolor;
            }




            //Go through lines starting ignoring header
            for (int i = 1; i < dataGridView2.RowCount; i++)
            {
                String rowheader = dataGridView2.Rows[i].Cells[0].Value.ToString();
                //skip empty
                for (int j = 1; j <= i; j++)
                {
                    bool found = false;

                    String columnheader = dataGridView2.Columns[j].Name;

                    //Search for cell where header is same as current column                        
                    for (int k = 0; (k < j && !found); k++)
                    {

                        if (dataGridView2.Rows[k].Cells[0].Value.ToString().Equals(columnheader))
                        {
                            DataGridViewCell cell = dataGridView2.Rows[k].Cells[rowheader];
                            Color color;
                            if (cell.Style.BackColor == Color.Blue)
                            {
                                cell.Style.BackColor = Color.Green;
                                color = Color.Red;
                            }
                            else
                                color = cell.Style.BackColor;

                            dataGridView2.Rows[i].Cells[j].Style.BackColor = color;
                            dataGridView2.Rows[i].Cells[j].Style.SelectionBackColor = color;

                            found = true;
                            break;
                        }
                    }
                }
            }

        }



        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage2)
            {
                readUi();
                backgroundWorker1.RunWorkerAsync();
            }
        }


        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage5)
            
                this.Height += 100;
            
            else
                this.Height -= 100;

        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            
            
        updateGraph();
        if (backgroundWorker1.CancellationPending)
            e.Cancel = true;
    

        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
                graphImage.Invalidate();
            //MessageBox.Show("Threadi loppu");
        }


        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView) sender;
            comboBoxHand.SelectedItem = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            tabControl2.SelectedTab = tabPage4;
        }


        private void autosetitem_Click(object sender, EventArgs e)
        {
        }
        

        private void useOptimalRangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void tabPage4_DragOver(object sender, DragEventArgs e)
        {
#if DEBUGOMA
            if (kuvaform == null)
                kuvaform = new SNGEGT.kuvaform();

            kuvaform.Location = new Point(Right + 10, this.Location.Y);
            String [] files = (String []) (e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop));
            for (int i = 0; i < files.Length; i++)
            {

                kuvaform.pictureBox1.Load(files[i]);
                kuvaform.Visible = true;
                openFileDialog.FileName = files[i];
                openFileDialog_FileOk(sender, new CancelEventArgs());
            
            }
#endif
        }


        private void tabPage4_DragOver_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void enableDebugToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

            Debug.Enabled = enableDebugToolStripMenuItem.Checked;
            tableTimer.Enabled = true;
        }

        private void SNGPH_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = null;
            try
            {
                try
                {
                    doc.Load(SETTINGSFILE);
                    node = doc.FirstChild.FirstChild;
                }
                catch (Exception /*exp*/)
                {
                }

                
                while (node != null)
                {
                    XmlNode nextnode = node.NextSibling;
                    if (node.Name == "UiSettings")
                    {
                        doc.FirstChild.RemoveChild(node);
                        break;
                    }
                    node = nextnode;
                }


                if (doc.FirstChild == null)
                    node = doc.AppendChild(doc.CreateElement("Settings"));
                node = doc.FirstChild;

                XmlElement element = doc.CreateElement("UiSettings");
                node = node.AppendChild(element);


                
                element = doc.CreateElement("UseOptimalRanges");
                element.InnerText = useOptimalRangesToolStripMenuItem.Checked ? "1" : "0";
                node.AppendChild(element);
            

                element = doc.CreateElement("UsePresetButtons");
                element.InnerText = usePresetButtonsToolStripMenuItem.Checked ? "1" : "0";
                node.AppendChild(element);

                element = doc.CreateElement("UseStackBlindRanges");
                element.InnerText = useMbasedRangesToolStripMenuItem.Checked ? "1" : "0";
                node.AppendChild(element);

                doc.Save(SETTINGSFILE);
            }
            catch (Exception)
            {
            }

        }
    }
}
