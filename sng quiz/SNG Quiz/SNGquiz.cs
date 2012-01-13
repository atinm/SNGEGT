using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SNG_Quiz
{


    public partial class SNGquiz : Form
    {

        private Image bmp;
        private bool updatingForms;
        private createGame calcNewGame;
        private singleGame[] games;
        private bool[] decisions;
        private int currentGameNum;
        private string[] strRanks;
        private Player mouseoverplayer;

        private void showPlayer(int num, bool enable, bool enableCards)
        {
            Player[] players = new Player[4];
            players[0] = player1;
            players[1] = player2;
            players[2] = player3;
            players[3] = player4;

            if (enable == true)
            {
                players[num - 1].Show();
                if (enableCards == true)
                {
                    players[num - 1].showCards(true);
                    if (num != 3)
                    {
                        Cards.drawCardBack(players[num - 1].card1);
                        Cards.drawCardBack(players[num - 1].card2);
                    }
                }
                else
                    players[num - 1].showCards(false);
            }
            else
            {
                players[num - 1].Hide();
                players[num - 1].showCards(false);
            }
        }
        

        public SNGquiz()
        {
            //myCardDll  = new cardsdll(60, 80);
            //oppCardDll = new cardsdll(15, 20);
            updatingForms = true;
            InitializeComponent();
            setInitialForms();
            bmp = new Bitmap(pictureBoxTable.Width, pictureBoxTable.Height);
            updatingForms = false;
            calcNewGame = new createGame();
            games = new singleGame[20];
            decisions = new bool[20];
            currentGameNum = 0;
            strRanks = new String[101] { "", "KK+,AKs", "QQ+,AK", "JJ+,AK", "TT+,AQ+", "99+,AQ+", "88+,AQo+,ATs+", "88+,AJo+,ATs+", "66+,AT+", "66+,ATo+,A9s+", "55+,ATo+,A8s+,KQs", "44+,A9o+,A8s+,KQs", "44+,A9o+,A7s+,KJs+", "44+,AKs,AQs,AJs,ATs,A9s,A8s,A8o+,A7s,A5s,KJs+", "44+,A8o+,A4s+,KJs+", "33+,A7o+,A4s+,KTs+", "33+,A7o+,A3s+,KQo,KTs+", "33+,A7o+,A2s+,KQo,KTs+", "33+,AKo,AQo,AJo,ATo,A9o,A8o,A7o,A5o,A2s+,KQo,KTs+", "33+,A5o+,A2s+,KQo,KTs+", "33+,A4o+,A2s+,KJo+,KTs+", "33+,A4o+,A2s+,KJo+,KTs+,QJs", "33+,A3o+,A2s+,KJo+,KTs+,QJs", "22+,A2+,KJo+,K9s+,QJs", "22+,A2+,KTo+,K9s+,QJs", "22+,A2+,KTo+,K8s+,QTs+", "22+,A2+,K9o+,K7s+,QTs+,JTs", "22+,A2+,K9o+,K6s+,QJo,QTs+,JTs", "22+,A2+,K9o+,K6s+,QJo,Q9s+,JTs", "22+,A2+,K8o+,K5s+,QJo,Q9s+,JTs", "22+,A2+,K8o+,K4s+,QTo+,Q9s+,JTs", "22+,A2+,K7o+,K4s+,QTo+,Q9s+,JTs", "22+,A2+,K7o+,K2s+,QTo+,Q9s+,JTs", "22+,A2+,K6o+,K2s+,QTo+,Q8s+,JTs", "22+,A2+,K5o+,K2s+,QTo+,Q8s+,J9s+", "22+,A2+,K5o+,K2s+,Q9o+,Q8s+,J9s+", "22+,A2+,K5o+,K2s+,Q9o+,Q8s+,JTo,J9s+", "22+,A2+,K4o+,K2s+,Q9o+,Q8s+,JTo,J9s+", "22+,A2+,K4o+,K2s+,Q9o+,Q6s+,JTo,J9s+,T9s", "22+,A2+,K3o+,K2s+,Q9o+,Q6s+,JTo,J9s+,T9s", "22+,A2+,K2+,Q9o+,Q5s+,JTo,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q5s+,JTo,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q4s+,J9o+,J8s+,T9s", "22+,A2+,K2+,Q8o+,Q3s+,J9o+,J8s+,T8s+", "22+,A2+,K2+,Q7o+,Q3s+,J9o+,J7s+,T8s+", "22+,A2+,K2+,Q6o+,Q2s+,J9o+,J7s+,T8s+", "22+,A2+,K2+,Q6o+,Q2s+,J9o+,J7s+,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J7s+,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J7s+,T9o,T8s+,98s", "22+,A2+,K2+,Q5o+,Q2s+,J8o+,J6s+,T9o,T8s+,98s", "22+,A2+,K2+,Q4o+,Q2s+,J8o+,J5s+,T9o,T7s+,98s", "22+,A2+,K2+,Q4o+,Q2s+,J7o+,J4s+,T9o,T7s+,98s", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J4s+,T9o,T7s+,98s", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J4s+,T8o+,T7s+,97s+", "22+,A2+,K2+,Q3o+,Q2s+,J7o+,J3s+,T8o+,T7s+,97s+", "22+,A2+,K2+,Q2+,J7o+,J3s+,T8o+,T6s+,97s+", "22+,A2+,K2+,Q2+,J6o+,J2s+,T8o+,T6s+,97s+,87s", "22+,A2+,K2+,Q2+,J6o+,J2s+,T8o+,T6s+,98o,97s+,87s", "22+,A2+,K2+,Q2+,J6o+,J2s+,T7o+,T6s+,98o,97s+,87s", "22+,A2+,K2+,Q2+,J5o+,J2s+,T7o+,T6s+,98o,96s+,87s", "22+,A2+,K2+,Q2+,J5o+,J2s+,T7o+,T5s+,98o,96s+,87s", "22+,A2+,K2+,Q2+,J4o+,J2s+,T7o+,T4s+,98o,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T4s+,98o,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T4s+,97o+,96s+,86s+", "22+,A2+,K2+,Q2+,J4o+,J2s+,T6o+,T3s+,97o+,96s+,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T3s+,97o+,95s+,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T2s+,97o+,95s+,87o,86s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T6o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J3o+,J2s+,T5o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J2+,T5o+,T2s+,96o+,95s+,87o,85s+,76s", "22+,A2+,K2+,Q2+,J2+,T5o+,T2s+,96o+,94s+,87o,85s+,75s+", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,96o+,94s+,87o,85s+,75s+", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,96o+,94s+,86o+,85s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T4o+,T2s+,95o+,93s+,86o+,84s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,93s+,86o+,84s+,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,93s+,86o+,84s+,76o,75s+,65s", "22+,A2+,K2+,Q2+,J2+,T3o+,T2s+,95o+,92s+,86o+,84s+,76o,74s+,65s", "22+,A2+,K2+,Q2+,J2+,T2+,95o+,92s+,86o+,84s+,76o,74s+,65s,54s", "22+,A2+,K2+,Q2+,J2+,T2+,95o+,92s+,85o+,84s+,76o,74s+,65s,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,83s+,76o,74s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,83s+,75o+,74s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,94o+,92s+,85o+,82s+,75o+,73s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,85o+,82s+,75o+,73s+,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,85o+,82s+,75o+,73s+,65o,64s+,54s", "22+,A2+,K2+,Q2+,J2+,T2+,93o+,92s+,84o+,82s+,75o+,73s+,65o,63s+,53s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,75o+,73s+,65o,63s+,53s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,73s+,65o,63s+,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,65o,63s+,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,64o+,63s+,54o,53s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,84o+,82s+,74o+,72s+,64o+,63s+,54o,52s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,83o+,82s+,74o+,72s+,64o+,62s+,54o,52s+,43s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,74o+,72s+,64o+,62s+,54o,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,64o+,62s+,54o,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,64o+,62s+,53o+,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,63o+,62s+,53o+,52s+,42s+", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,73o+,72s+,63o+,62s+,53o+,52s+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,63o+,62s+,53o+,52s+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,63o+,62s+,52+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,43o,42s+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,42+,32s", "22+,A2+,K2+,Q2+,J2+,T2+,92+,82+,72+,62+,52+,42+,32" };

            // kortit erillisiä komponentteja ennen tätä sijoitusta
            player1.card1 = player1card1;
            player1.card2 = player1card2;
            player2.card1 = player2card1;
            player2.card2 = player2card2;
            player3.card1 = myCard1;
            player3.card2 = myCard2;
            player4.card1 = player4card1;
            player4.card2 = player4card2;

            // piilotetaan aluksi kaikki pelaajat
            showPlayer(1, false, false);
            showPlayer(2, false, false);
            showPlayer(3, false, false);
            showPlayer(4, false, false);
        }


        // ohjelman käynnistyessä ladataan oletusasetukset
        private void setInitialForms() 
        {
            comboBoxDifficulty.SelectedIndex = 1;   // normal
            comboBoxMode.SelectedIndex = 2;         // random
            comboBoxPlayers.SelectedIndex = 0;      // 4 pelaajaa
            comboBoxBBsize.SelectedIndex = 5;       // random
            comboBoxPosition.SelectedIndex = 4;     // random
            comboBoxChips.SelectedIndex = 2;        // 20 000
            comboBoxStacks.SelectedIndex = 0;       // random
            labelChipsBB.Enabled = false;
            textBoxChipsBB.Enabled = false;
            labelChipsSB.Enabled = false;
            textBoxChipsSB.Enabled = false;
            labelChipsBTN.Enabled = false;
            textBoxChipsBTN.Enabled = false;
            labelChipsUTG.Enabled = false;
            textBoxChipsUTG.Enabled = false;
            dataGridViewResults.Rows.Clear();

            buttonFold.Hide();
            buttonPush.Hide();
        }
  

        // tapahtumankäsittelijä moodin, pelaajien lukumäärän tai position muuttumiseen
        private void setForm(object sender, EventArgs e)
        {
            // ei päästetä funktioon ketään muuta muutosten aikana
            if(updatingForms == true)
                return;
            updatingForms = true;

            // tämän hetken tilanne talteen sekä objekteihin että merkkijonoihin
            object mode = comboBoxMode.SelectedItem; 
            object position = comboBoxPosition.SelectedItem;
            object players = comboBoxPlayers.SelectedItem;
            string modeStr = mode.ToString();
            string positionStr = players.ToString();
            string playersStr = players.ToString();
            String[] positions = new String[2];

            if (modeStr == "Push")
            {                
                if (playersStr == "2")
                    positions = new String[] { "SB" };
                else if (playersStr == "3")
                    positions = new String[] { "SB", "BTN", "Random" };
                else //if (playersStr == "4")
                    positions = new String[] { "SB", "BTN", "UTG", "Random" };
            }
            else if (modeStr == "Call")
            {
                if (playersStr == "2")
                    positions = new String[] { "BB" };
                else if (playersStr == "3")
                    positions = new String[] { "BB", "SB", "Random" };
                else //if (playersStr == "4")
                    positions = new String[] { "BB", "SB", "BTN", "Random" };
            }
            else // if (modeStr == "Random")
            {
                if (playersStr == "2")
                    positions = new String[] { "BB", "SB", "Random" };
                else if (playersStr == "3")
                    positions = new String[] { "BB", "SB", "BTN", "Random" };
                else //if (playersStr == "4")
                    positions = new String[] { "BB", "SB", "BTN", "UTG", "Random" };
            }

            comboBoxPosition.BeginUpdate();
            comboBoxPosition.Items.Clear();
            comboBoxPosition.Items.AddRange(positions);
            int pos = (position == null) ? -1 : comboBoxPosition.Items.IndexOf(position);
            if (pos != -1)
                comboBoxPosition.SelectedIndex = pos;
            else
                comboBoxPosition.SelectedIndex = 0;
            comboBoxPosition.EndUpdate();
            
            // positio on sattanut muuttua kun päästään tähän, joten arvot uudestaan talteen
            position = comboBoxPosition.SelectedItem;
            positionStr = players.ToString();            

            String[] modes = new String[2];
            if (positionStr == "BB")
            {
                modes = new String[] { "Call"};
                comboBoxMode.BeginUpdate();
                comboBoxMode.Items.Clear();
                comboBoxMode.Items.AddRange(modes);
                comboBoxPosition.SelectedIndex = 0;
                comboBoxPosition.EndUpdate();
            }

            // jos stäkkien koot käyttäjien määriteltävät ja pelaajien määrä ei ole "Random"
            // niin stäkkien asetus on käytössä. Muuten otetaan pois käytöstä
            players = comboBoxPlayers.SelectedItem;
            playersStr = players.ToString();
            if (comboBoxStacks.SelectedIndex == 1 && playersStr != "Random") // Uder defined
            {
                players = comboBoxPlayers.SelectedItem;
                playersStr = players.ToString();
                if (playersStr == "2") 
                {
                    labelChipsBB.Enabled = true;
                    textBoxChipsBB.Enabled = true;
                    labelChipsSB.Enabled = true;
                    textBoxChipsSB.Enabled = true;
                    labelChipsBTN.Enabled = false;
                    textBoxChipsBTN.Enabled = false;
                    labelChipsUTG.Enabled = false;
                    textBoxChipsUTG.Enabled = false;
                }
                else if (playersStr == "3")
                {
                    labelChipsBB.Enabled = true;
                    textBoxChipsBB.Enabled = true;
                    labelChipsSB.Enabled = true;
                    textBoxChipsSB.Enabled = true;
                    labelChipsBTN.Enabled = true;
                    textBoxChipsBTN.Enabled = true;
                    labelChipsUTG.Enabled = false;
                    textBoxChipsUTG.Enabled = false;
                }
                else if (playersStr == "4")
                {
                    labelChipsBB.Enabled = true;
                    textBoxChipsBB.Enabled = true;
                    labelChipsSB.Enabled = true;
                    textBoxChipsSB.Enabled = true;
                    labelChipsBTN.Enabled = true;
                    textBoxChipsBTN.Enabled = true;
                    labelChipsUTG.Enabled = true;
                    textBoxChipsUTG.Enabled = true;
                }
            }
            else
            {
                comboBoxStacks.SelectedIndex = 0;
                labelChipsBB.Enabled = false;
                textBoxChipsBB.Enabled = false;
                labelChipsSB.Enabled = false;
                textBoxChipsSB.Enabled = false;
                labelChipsBTN.Enabled = false;
                textBoxChipsBTN.Enabled = false;
                labelChipsUTG.Enabled = false;
                textBoxChipsUTG.Enabled = false;
            }

            // muutokset ovat taas mahdollisia
            updatingForms = false;
        }
        

        // peli aloitetaan
        private void buttonStart_Click(object sender, EventArgs e)
        {
            currentGameNum = 0;
            dataGridViewResults.Rows.Clear();
            labelResults.Text = "";
            if (areSettingsOK())
                getNextGame();
        }


        private void setPlayers(singleGame game)
        {
            pictureBoxTable.Refresh();

            if (currentGameNum < 20)
            {
                buttonPush.Show();
                buttonFold.Show();
            }
            else
            {
                buttonPush.Hide();
                buttonFold.Hide();
            }

            buttonFold.Text = "Fold";
            if (game.pushOrCall == 0) // push
                buttonPush.Text = "All-in";
            else
                buttonPush.Text = "Call";

            if (game.numOfPlayers == 2)
            {                
                showPlayer(1, true, true);
                showPlayer(2, false, false);
                showPlayer(3, true, true);
                showPlayer(4, false, false);
                player3.resetVars();
                player1.resetVars();

                if (game.pushOrCall == 0) // push
                {
                    player1.Range  = game.ranges[0];
                    player1.Stack  = Convert.ToString(game.stacks[0] - game.BBsize) + "$";
                    player1.Bets   = game.BBsize;
                    player3.Range  = 0;
                    player3.Stack  = Convert.ToString(game.stacks[1] - (game.BBsize / 2)) + "$";
                    player3.Bets   = game.BBsize / 2;
                    player3.Dealer = true;
                }
                else  // call
                {
                    player1.Range = game.raiserRange;
                    player1.Stack = "ALL-IN";
                    player1.Bets  = game.stacks[1];
                    player1.Dealer = true;
                    player3.Range = 0;
                    player3.Stack = Convert.ToString(game.stacks[0] - game.BBsize) + "$";
                    player3.Bets = game.BBsize;
                }
            }
            //else if (game.numOfPlayers == 3)
            else
            {
                showPlayer(1, true, true);
                showPlayer(2, true, true);
                showPlayer(3, true, true);
                if (game.numOfPlayers == 4)
                    showPlayer(4, true, true);
                else
                    showPlayer(4, false, false);
                
                Player[] players = new Player[game.numOfPlayers];

                if (game.position == 0)
                {
                    players[0] = player3;
                    players[1] = player2;
                    players[2] = player1;
                    if(game.numOfPlayers == 4)
                        players[3] = player4;
                }
                else if (game.position == 1)
                {
                    if (game.numOfPlayers == 3)
                    {
                        players[0] = player1;
                        players[1] = player3;
                        players[2] = player2;
                    }
                    else
                    {
                        players[0] = player4;
                        players[1] = player3;
                        players[2] = player2;
                        players[3] = player1;
                    }
                }
                else if (game.position == 2)
                {
                    if (game.numOfPlayers == 3)
                    {
                        players[0] = player2;
                        players[1] = player1;
                        players[2] = player3;
                    }
                    else
                    {
                        players[0] = player1;
                        players[1] = player4;
                        players[2] = player3;
                        players[3] = player2;
                    }
                }
                else if (game.position == 3)
                {
                    players[0] = player2;
                    players[1] = player1;
                    players[2] = player4;
                    players[3] = player3;
                }
                
                // käydään pelaajat läpi ja asetetaan parametrit
                for (int i = game.numOfPlayers-1; i >= 0; i--)
                {
                    players[i].resetVars();

                    /*
                    // jos tutkittava pelaaja ennen korottajaa, niin piilotetaan kortit
                    if (i > game.raiserPos && game.raiserPos != game.position)
                    //if (i > game.raiserPos)
                        players[i].showCards(false);
                    else
                        players[i].showCards(true);
                    */

                    // korottajan stäkit, betit ja ranget
                    if (i == game.raiserPos)
                    {
                        players[i].showCards(true);

                        // jos joku muu reissaa niin range ja "ALL-IN" näkyviin jos joku muu kuin itse
                        if (game.raiserPos != game.position)
                        {
                            players[i].Range = game.raiserRange;
                            players[i].Stack = "ALL-IN";
                            players[i].Bets = game.stacks[i];
                        }
                        // itse reissaaja
                        else
                        {
                            players[i].Range = 0;
                            if (i == 1) // pikkublindissä
                            {
                                players[i].Stack = Convert.ToString(game.stacks[i] - game.BBsize / 2) + "$";
                                players[i].Bets = game.BBsize / 2;
                            }
                            else // jossain muussa positiossa
                            {
                                players[i].Stack = Convert.ToString(game.stacks[i]) + "$";
                                players[i].Bets = 0;
                            }
                            
                        }
                    }
                    // muiden kuin korottajan stäkit, betit ja ranget
                    else
                    {
                        // jos toimii ennen reissaajaa
                        if (i > game.raiserPos)
                        {
                            players[i].Range = 0;
                            players[i].Stack = Convert.ToString(game.stacks[i]) + "$";
                            players[i].Bets = 0;
                            players[i].showCards(false);
                        }
                        // reissaajan jälkeen
                        else
                        {
                            // jos ollaan itse maksamassa reissaajan allia
                            if (i == game.position)
                            {
                                players[i].showCards(true);
                                players[i].Range = 0;

                                if (i == 0) // isossa blindissä
                                {
                                    players[i].Stack = Convert.ToString(game.stacks[i] - game.BBsize) + "$";
                                    players[i].Bets = game.BBsize;
                                }
                                else if (i == 1) // pikkublindissä
                                {
                                    players[i].Stack = Convert.ToString(game.stacks[i] - game.BBsize / 2) + "$";
                                    players[i].Bets = game.BBsize / 2;
                                }
                                else // jossain muussa positiossa
                                {
                                    players[i].Stack = Convert.ToString(game.stacks[i]) + "$";
                                    players[i].Bets = 0;
                                }
                            }
                            // muut reissaajan jälkeen toimivat pelaajat kuin itse
                            else
                            {
                                // reissaajan ja oman itse välissä olevien pelaajien kortit piilotetaan "foldattuina"
                                if(i < game.raiserPos && i > game.position)
                                    players[i].showCards(false);
                                else
                                    players[i].showCards(true);

                                // jos ollaan itse reissaaja, niin vastustajille call -ranget näkyviin
                                if (game.raiserPos == game.position)
                                    players[i].Range = game.ranges[i];
                                
                                if (i == 0) // isossa blindissä
                                {
                                    players[i].Stack = Convert.ToString(game.stacks[i] - game.BBsize) + "$";
                                    players[i].Bets = game.BBsize;
                                }
                                else if (i == 1) // pikkublindissä
                                {
                                    players[i].Stack = Convert.ToString(game.stacks[i] - game.BBsize / 2) + "$";
                                    players[i].Bets = game.BBsize / 2;
                                }
                                else // jossain muussa positiossa
                                {
                                    players[i].Stack = Convert.ToString(game.stacks[i]) + "$";
                                    players[i].Bets = 0;
                                }
                            }
                        }
                    }
                }
                

                // kolmas pelaaja on aina jakaja
                players[2].Dealer = true;
            }

            for (int i =0; i < games.Length; i++)
                if (games[i] == game)
                {
                    labelGameNumber.Text = (i+1) + " / 20";
                    break;
                }

            // näytetään omat kortit aina
            Cards.drawCard(myCard1, game.myHand[0], Cards.mdFaceUp);
            Cards.drawCard(myCard2, game.myHand[1], Cards.mdFaceUp);
        }

        
        private bool areSettingsOK()
        {
            // stäkkien summan täytyy täsmätä jos ne on asetettu itse
            if (comboBoxStacks.SelectedIndex == 1)
            {
                int[] stackSumArray = { 10000, 15000, 20000 };
                int stackSum = 0;

                object players    = comboBoxPlayers.SelectedItem;
                string playersStr = players.ToString();
                int playersInt    = Convert.ToInt32(playersStr);
                int chipsIndex    = comboBoxChips.SelectedIndex;
                int chipsInt      = Convert.ToInt32(stackSumArray[chipsIndex]);

                try
                {
                    if (playersStr == "2")
                        stackSum = Convert.ToInt32(textBoxChipsBB.Text) + Convert.ToInt32(textBoxChipsSB.Text);
                    if (playersStr == "3")
                        stackSum = Convert.ToInt32(textBoxChipsBB.Text) + Convert.ToInt32(textBoxChipsSB.Text) + Convert.ToInt32(textBoxChipsBTN.Text);
                    if (playersStr == "4")
                        stackSum = Convert.ToInt32(textBoxChipsBB.Text) + Convert.ToInt32(textBoxChipsSB.Text) + Convert.ToInt32(textBoxChipsBTN.Text) + Convert.ToInt32(textBoxChipsUTG.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Sum of the user defined stacks should be " + stackSumArray[comboBoxPlayers.SelectedIndex] + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(e.ToString());
                    return false;
                }

                Console.WriteLine("stackSum: " + stackSum + " chipsInt: " + chipsInt);

                if (stackSum != chipsInt)
                {
                    MessageBox.Show("Sum of the user defined stacks should be " + chipsInt + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }


        private void getNextGame()
        {
            int[] stacks = new int[] { 0, 0, 0, 0 };
            string playersStr    = comboBoxPlayers.SelectedItem.ToString();
            int players          = comboBoxPlayers.SelectedIndex;
            string pushOrCallStr = comboBoxMode.SelectedItem.ToString();
            int difficulty       = comboBoxDifficulty.SelectedIndex;
            int startChips       = comboBoxChips.SelectedIndex;
            int BBsize           = comboBoxBBsize.SelectedIndex;
            string positionStr   = comboBoxPosition.SelectedItem.ToString();

            if (comboBoxStacks.SelectedIndex == 1 && comboBoxPlayers.SelectedIndex != 3)
            {
                if (playersStr == "2")
                {
                    stacks[0] = Convert.ToInt32(textBoxChipsBB.Text);
                    stacks[1] = Convert.ToInt32(textBoxChipsSB.Text);
                }
                if (playersStr == "3")
                {
                    stacks[0] = Convert.ToInt32(textBoxChipsBB.Text);
                    stacks[1] = Convert.ToInt32(textBoxChipsSB.Text);
                    stacks[2] = Convert.ToInt32(textBoxChipsBTN.Text);
                }
                if (playersStr == "4")
                {
                    stacks[0] = Convert.ToInt32(textBoxChipsBB.Text);
                    stacks[1] = Convert.ToInt32(textBoxChipsSB.Text);
                    stacks[2] = Convert.ToInt32(textBoxChipsBTN.Text);
                    stacks[3] = Convert.ToInt32(textBoxChipsUTG.Text);
                }
            }

            int pushOrCall = 0;
            if (pushOrCallStr == "Push")
                pushOrCall = 0;
            if (pushOrCallStr == "Call")
                pushOrCall = 1;
            if (pushOrCallStr == "Random")
                pushOrCall = 2;

            int position = 0;
            if (positionStr == "BB")
                position = 0;
            if (positionStr == "SB")
                position = 1;
            if (positionStr == "BTN")
                position = 2;
            if (positionStr == "UTG")
                position = 3;
            if (positionStr == "Random")
                position = 4;

            games[currentGameNum] = calcNewGame.getGame(players, pushOrCall, difficulty, startChips, BBsize, position, stacks);

            string[] pushOrCallStrs = { "push", "call", "Random"};
            string[] positionStrs = { "BB", "SB", "BTN", "UTG" , "Random"};
            string[] actionStr = { "-EV", "+EV" };
            Console.WriteLine("\n------------getNextGame-------------\n");
            Console.WriteLine("numOfPlayers: {0}", games[currentGameNum].numOfPlayers);
            Console.WriteLine("pushOrCall: {0}", pushOrCallStrs[games[currentGameNum].pushOrCall]);
            Console.WriteLine("BBsize: {0}", games[currentGameNum].BBsize);
            Console.WriteLine("position: {0}", positionStrs[games[currentGameNum].position]);
            Console.WriteLine("raiserPos: {0}", positionStrs[games[currentGameNum].raiserPos]);
            Console.WriteLine("raiserRange: {0}", games[currentGameNum].raiserRange);
            Console.WriteLine("action: {0}", actionStr[games[currentGameNum].action]);
            Console.WriteLine("stacks: {0}\t{1}\t{2}\t{3}\tsum: {4}", games[currentGameNum].stacks[0], games[currentGameNum].stacks[1], games[currentGameNum].stacks[2], games[currentGameNum].stacks[3], games[currentGameNum].stacks[0] + games[currentGameNum].stacks[1] + games[currentGameNum].stacks[2] + games[currentGameNum].stacks[3]);
            Console.WriteLine("ranges: {0}\t{1}\t{2}\t{3}", games[currentGameNum].ranges[0], games[currentGameNum].ranges[1], games[currentGameNum].ranges[2], games[currentGameNum].ranges[3]);
            Console.WriteLine("myEV: " + games[currentGameNum].myEV);
            Console.WriteLine("myHand: ({0}, {1})", games[currentGameNum].myHand[0], games[currentGameNum].myHand[1]);
                        
            setPlayers(games[currentGameNum]);
            currentGameNum++;
            labelGameNumber.Text = currentGameNum+ " / 20";
        }


        private void drawall()
        {            
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(PictureBox.DefaultBackColor);
            g.FillRectangle(Brushes.Green, 100, 10, 200, 300);
            g.FillPie(Brushes.Black, new Rectangle(350, 10, 100, 300), 270, 180);
            g.FillPie(Brushes.Black, new Rectangle(0, 10, 100, 300), 90, 180);
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            ;
        }


        private string handToStr(int[] hand)
        {
            string[] valueStr = { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
            int value1 = hand[0] % 13;
            int land1  = hand[0] / 13;
            int value2 = hand[1] % 13;
            int land2  = hand[1] / 13;

            if (value2 > value1)
            {
                int tmp = value1;
                value1 = value2;
                value2 = tmp;
            }

            //Console.WriteLine("value1: " + value1 + " land1: " + land1 + " value2: " + value2 + " land2: " + land2);

            string tmpStr = valueStr[value1] + valueStr[value2];
            if (land1 == land2)
                return tmpStr + "s";
            else if (value1 == value2)
                return tmpStr;
            else
                return tmpStr + "o";
        }


        private void showResults()
        {
            buttonPush.Hide();
            buttonFold.Hide();
            dataGridViewResults.Rows.Add(20);

            int correctAnswers = 0;
            for (int i = 0; i < 20; i++)
            {
                dataGridViewResults.Rows[i].Cells[0].Value = handToStr(games[i].myHand);
                if ((games[i].myEV < 0 && decisions[i] == false) || (games[i].myEV > 0 && decisions[i] == true)) 
                {
                    correctAnswers++;
                    dataGridViewResults.Rows[i].Cells[1].Style.BackColor = Color.Green;
                }
                else
                    dataGridViewResults.Rows[i].Cells[1].Style.BackColor = Color.Red;

                if(decisions[i] == false)
                    dataGridViewResults.Rows[i].Cells[1].Value = "Fold";
                else if(games[i].pushOrCall == 0) // push
                    dataGridViewResults.Rows[i].Cells[1].Value = "All-in";
                else  // call
                    dataGridViewResults.Rows[i].Cells[1].Value = "Call";

                dataGridViewResults.Rows[i].Cells[2].Value = String.Format("{0:g2}", games[i].myEV);
            }

            string[] ratingStrs = { "Horrible", "Bad", "Poor", "Below Average", "Average", "Above Average", "Good", "Very Good", "Superb", "Excellent"};

            if (correctAnswers < 12)
                labelResults.Text = Convert.ToString(correctAnswers) + " / 20" + " Horrible";
            else
                labelResults.Text = Convert.ToString(correctAnswers) + " / 20 " + ratingStrs[correctAnswers-11];

            labelResults.Show();
        }


        private void saveAction(bool action)
        {
            Console.WriteLine(currentGameNum + " / 20");
            decisions[currentGameNum - 1] = action;
            if (currentGameNum - 1 == 19)
                showResults();
            else
                getNextGame();
        }


        private void buttonFold_Click(object sender, EventArgs e)
        {
            saveAction(false);
        }


        private void buttonPush_Click(object sender, EventArgs e)
        {
            saveAction(true);
        }


        private void dataGridViewResults_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString());
            setPlayers(games[e.RowIndex]);
        }



        private void player1_Load(object sender, EventArgs e)
        {

        }


        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void player1_Load_1(object sender, EventArgs e)
        {

        }

        

        private void pictureBoxTable_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SNGquiz_Load(object sender, EventArgs e)
        {

        }

        private void timerTooltip_Tick(object sender, EventArgs e)
        {
            toolTip1.Show(strRanks[(int) (mouseoverplayer.Range+0.5)], mouseoverplayer);
            timerTooltip.Enabled = false;
            /*toolTip1.ShowAlways = true;
            toolTip1.Hide(this);
            toolTip1.Show("mdasasd", this, 10, 20, 3);
            */
        }

        private void player1_MouseEnter(object sender, EventArgs e)
        {
            
            mouseoverplayer = (Player) sender;
            if (mouseoverplayer.Range == 0)
                return;

            timerTooltip.Enabled = true;

        }

        private void player1_MouseLeave(object sender, EventArgs e)
        {
            timerTooltip.Enabled = false;
        }

        private void player1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(strRanks[(int)(mouseoverplayer.Range + 0.5)], mouseoverplayer);            
            timerTooltip.Enabled = true;

        }

        

        //private static readonly int CHIP1000 = 0;
        //private static readonly int CHIP1500 = 1;
        //private static readonly int CHIP2000 = 2;

        //private static readonly int EASY = 0;
        //private static readonly int MEDIUM = 1;
        //private static readonly int HARD = 2;

        //private static readonly int FOLD = 0;
        //private static readonly int PUSH = 0;
        //private static readonly int CALL = 1;
        //private static readonly int RANDTYPE = 2;

        //private static readonly int HEADSUP = 0;
        //private static readonly int THREELEFT = 1;
        //private static readonly int FOURLEFT = 2;
        //private static readonly int RANDLEFT = 3;

        //private static readonly int BB = 0;
        //private static readonly int SB = 1;
        //private static readonly int BTN = 2;
        //private static readonly int UTG = 3;
        //private static readonly int RANDPOS = 4;
    }

    
}

            
            
            