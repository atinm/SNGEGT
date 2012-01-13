using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HeadsUp_Trainer
{
    public partial class Heads_Up_Trainer : Form
    {
        private static readonly int BB = 0;
        private static readonly int SB = 1;
        private static readonly int COMPUTERWONSB = 0;
        private static readonly int COMPUTERWONBB = 1;
        private static readonly int HUMANWONSB = 2;
        private static readonly int HUMANWONBB = 3;
        private static readonly int COMPUTERWONALLIN = 4;
        private static readonly int HUMANWONALLIN = 5;


        private cardsdll card;
        private Game game;
        private GameData data;        
        private Bitmap[] cards;
        private double losses = 0;
        private double wins = 0;


        public Heads_Up_Trainer()
        {
            card = new cardsdll(60, 80);
            game = new Game();
            data = new GameData();

            InitializeComponent();
            buttonFold.Hide();
            buttonAllIn.Hide();

            playerMe.Stack = "";
            playerMe.Bets = 0;
            playerMe.Dealer = false;
            playerOpp.Stack = "";
            playerOpp.Bets = 0;
            playerOpp.Dealer = false;
            playerMe.card1 = myCard1;
            playerMe.card2 = myCard2;
            playerOpp.card1 = oppCard1;
            playerOpp.card2 = oppCard2;
            playerMe.showCards(false);
            playerOpp.showCards(false);

            pictureBoxBoard1.Hide();
            pictureBoxBoard2.Hide();
            pictureBoxBoard3.Hide();
            pictureBoxBoard4.Hide();
            pictureBoxBoard5.Hide();

            labelWinner.Hide();
            timerStart.Enabled = true;
        }


        private void resetStacksAndBets()
        {
            playerMe.Stack = "";
            playerMe.Bets = 0;
            playerMe.Dealer = false;
            playerOpp.Stack = "";
            playerOpp.Bets = 0;
            playerOpp.Dealer = false;
            playerMe.showCards(false);
            playerOpp.showCards(false);
            playerMe.Refresh();
            playerOpp.Refresh();
        }


        private void printBlinds()
        {

        }


        private void pictureBoxTable_Click(object sender, EventArgs e)
        {

        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        private void getNextGame()
        {
            //game.
        }


        // blindit ja pelit
        private void setInfoLabels()
        {
            labelBlinds.Text = "Blinds: " + data.BBsize / 2 + " / " + data.BBsize;
            labelBlindsLeft.Text = "Games: " + data.gameNumber;

            if (wins + losses != 0)
            {
                int winPercentage = (int)((wins / (wins + losses)) * 100);
                int lossesPercentage = (int)((losses / (wins + losses)) * 100);

                labelWins.Text = "Wins: " + wins + " (" + winPercentage + "%)";
                labelLosses.Text = "Losses: " + losses + " (" + lossesPercentage + "%)";
            }
            else
            {
                labelWins.Text = "";
                labelLosses.Text = "";
            }
        }


        private void buttonFold_Click(object sender, EventArgs e)
        {
            // napit voi piilottaa heti kun jompaa kumpaa on painettu
            buttonAllIn.Hide();
            buttonFold.Hide();

            // jos ollaan pikkublindissa ja foldataan, niin vastustaja voittaa pikkublindin
            if (data.myPos == SB)
            {
                calcStacks(COMPUTERWONSB);
            }
            // jos ollaan isossa blindissa ja foldataan tietokoneen alliin, niin vastustaja voittaa ison blindin
            if (data.myPos == SB)
            {
                calcStacks(COMPUTERWONBB);
            }

            // aloitetaan ajastin, jonka lauetessa uusi peli alkaa
            timerStartGame.Enabled = true;
        }


        private void buttonAllIn_Click(object sender, EventArgs e)
        {
            // napit voi piilottaa heti kun jompaa kumpaa on painettu
            buttonAllIn.Hide();
            buttonFold.Hide();

            // jos ollaan pikkublindiss‰ ja vedet‰‰n alli, niin aloitetaan ajastin jonka lauetessa kone tekee p‰‰tˆksen
            if (data.myPos == SB)
            {
                playerMe.Hide();
                playerMe.Bets = data.myStack;
                playerMe.Stack = "All-in";
                playerMe.Dealer = true;
                timerOppAction.Enabled = true;
                playerMe.Show();
            }

            // jos ollaan isossa blindiss‰ ja maksetaan alli, niin tulee showdown
            if (data.myPos == BB)
            {
                setFormsShowdown();
                /*
                playerMe.Hide();
                playerMe.Bets = data.myStack;
                playerMe.Stack = "All-in";
                playerMe.Dealer = false;
                timerOppAction.Enabled = true;
                playerMe.Show();
                timerShowndown.Enabled = true;
                */
            }
        }


        // kun aloitetaan ja p‰‰tet‰‰n jakajan paikka
        private void timerStart_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("timerStart_Tick");

            int[] startCards = { -1, -1 };
            game.getStartCards(startCards);

            myCard2.Hide();
            oppCard2.Hide();
            myCard1.Show();
            oppCard1.Show();
            myCard1.Image = cards[startCards[0]];
            oppCard1.Image = cards[startCards[1]];
            //card.drawCard(myCard1, cards[0], (int)cardsdll.mdFaceUp, 16777215);
            //card.drawCard(oppCard1, cards[1], (int)cardsdll.mdFaceUp, 16777215);
            Thread.Sleep(1000);

            timerStart.Enabled = false;
            timerStartGame.Enabled = true;
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }


        // COMPUTERWONSB, COMPUTERWONBB, HUMANWONSB, HUMANWONBB, COMPUTERWONALLIN, HUMANWONALLIN
        private void calcStacks(int mode)
        {
            if (mode == COMPUTERWONSB)
            {
                game.oppStack += data.BBsize / 2;
                game.myStack -= data.BBsize / 2;
            }
            if (mode == COMPUTERWONBB)
            {
                game.oppStack += data.BBsize;
                game.myStack -= data.BBsize;
            }
            if (mode == HUMANWONSB)
            {
                game.oppStack -= data.BBsize / 2;
                game.myStack += data.BBsize / 2;
            }
            if (mode == HUMANWONBB)
            {
                game.oppStack -= data.BBsize;
                game.myStack += data.BBsize;
            }
            if (mode == COMPUTERWONALLIN)
            {
                if (game.oppStack >= game.myStack)
                {
                    game.oppStack += game.myStack;
                    game.myStack = 0;
                    losses++;
                }
                else
                {
                    int tmp = game.oppStack;
                    game.oppStack += tmp;
                    game.myStack -= tmp;
                }
            }
            if (mode == HUMANWONALLIN)
            {
                if (game.myStack >= game.oppStack)
                {
                    game.myStack += game.oppStack;
                    game.oppStack = 0;
                    wins++;
                }
                else
                {
                    int tmp = game.myStack;
                    game.oppStack -= tmp;
                    game.myStack += tmp;
                }
            }
        }


        // kun tietokoneen tulee p‰‰tt‰‰ meneekˆ se alliin
        private void timerOppAction_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("timerOppAction_Tick");

            timerOppAction.Enabled = false;

            playerOpp.Hide();

            // jos tietokone on pikkublindiss‰ 
            if (data.myPos == BB)
            {
                // tarpeeksi hyv‰ll‰ k‰dell‰ tietokone vet‰‰ allin
                if (data.oppPush)
                {
                    playerOpp.Bets = data.oppStack;
                    playerOpp.Stack = "All-In";
                    buttonAllIn.Text = "Call";
                    buttonAllIn.Show();
                    buttonFold.Show();
                }
                else // muuten se foldaa
                {
                    playerOpp.Stack = "Fold";
                    playerOpp.showCards(false);
                    buttonAllIn.Hide();
                    buttonFold.Hide();
                    calcStacks(HUMANWONSB);
                    timerStartGame.Enabled = true;
                }
            }

            // jos tietokone on isossa blindiss‰
            if (data.myPos == SB)
            {
                buttonAllIn.Hide();
                buttonFold.Hide();

                // tarpeeksi hyv‰ll‰ k‰dell‰ tietokone maksaa allin
                if (data.oppPush)
                {
                    setFormsShowdown();
                    /*
                    playerOpp.Bets = data.oppStack;
                    playerOpp.Stack = "All-In";
                    */
                }
                else // jos k‰si ei ole tarpeeksi hyv‰, niin se foldaa
                {
                    playerOpp.Stack = "Fold";
                    playerOpp.showCards(false);
                    calcStacks(HUMANWONBB);
                    timerStartGame.Enabled = true;
                }
            }

            playerOpp.Show();
        }


        private void setFormsAction()
        {
            // n‰ytet‰‰n omat ja vastustajan kortit
            playerMe.showCards(false);
            playerOpp.showCards(false);
            playerMe.showCards(true);
            playerOpp.showCards(true);
            //card.drawCardBack(oppCard1);
            //card.drawCardBack(oppCard2);
            oppCard1.Image = cards[52];
            oppCard2.Image = cards[52];
            myCard1.Image = cards[data.myHand[0]];
            myCard2.Image = cards[data.myHand[1]];
            //card.drawCard(myCard1, data.myHand[0], (int)cardsdll.mdFaceUp, 16777215);
            //card.drawCard(myCard2, data.myHand[1], (int)cardsdll.mdFaceUp, 16777215);

            // piilotetaan pelaajat, asetetaan muuttujat ja n‰ytet‰‰n ne lopussa uudestaan
            playerMe.Hide();
            playerOpp.Hide();

            // jos ollaan itse buttonissa
            if (data.myPos == SB)
            {
                playerMe.Bets = data.BBsize / 2;
                playerMe.Stack = Convert.ToString(data.myStack - data.BBsize / 2);
                playerMe.Dealer = true;

                playerOpp.Bets = data.BBsize;
                playerOpp.Stack = Convert.ToString(data.oppStack - data.BBsize);
                playerOpp.Dealer = false;

                buttonAllIn.Text = "All-In";
                buttonAllIn.Show();
                buttonFold.Show();
            }
            // ollaan isossa blindiss‰
            else
            {
                playerOpp.Bets = data.BBsize / 2;
                playerOpp.Stack = Convert.ToString(data.oppStack - data.BBsize / 2);
                playerOpp.Dealer = true;

                playerMe.Bets = data.BBsize;
                playerMe.Stack = Convert.ToString(data.myStack - data.BBsize);
                playerMe.Dealer = false;

                // aloitetaan ajastin, jonka lauetessa vastustaja toimii ja laitetaan vasta siell‰ napit n‰kyviin
                buttonAllIn.Hide();
                buttonFold.Hide();
                timerOppAction.Enabled = true;
            }

            playerMe.Show();
            playerOpp.Show();
        }


        private void setFormsShowdown()
        {

            // nappeja ei tarvita
            buttonAllIn.Hide();
            buttonFold.Hide();

            // n‰ytet‰‰n omat ja vastustajan kortit
            playerMe.showCards(false);
            playerOpp.showCards(false);
            playerMe.showCards(true);
            playerOpp.showCards(true);
            oppCard1.Image = cards[data.oppHand[0]];
            oppCard2.Image = cards[data.oppHand[1]];
            myCard1.Image = cards[data.myHand[0]];
            myCard2.Image = cards[data.myHand[1]];

            // piilotetaan pelaajat, asetetaan muuttujat ja n‰ytet‰‰n ne lopussa uudestaan
            playerMe.Hide();
            playerOpp.Hide();

            // jos ollaan itse buttonissa
            if (data.myPos == SB)
            {
                playerMe.Bets = data.myStack;
                playerMe.Stack = "All-in";
                playerMe.Dealer = true;

                playerOpp.Bets = data.oppStack;
                playerOpp.Stack = "All-in";
                playerOpp.Dealer = false;
            }
            // ollaan isossa blindiss‰
            else
            {
                playerOpp.Bets = data.oppStack;
                playerOpp.Stack = "All-in";
                playerOpp.Dealer = true;

                playerMe.Bets = data.myStack;
                playerMe.Stack = "All-in";
                playerMe.Dealer = false;
            }

            playerMe.Show();
            playerOpp.Show();

            // aloitetaan ajastin, jonka lauetessa n‰ytet‰‰n pˆyt‰kortit
            timerShowndown.Enabled = true;
        }


        private void timerStartGame_Tick(object sender, EventArgs e)
        {
            timerStartGame.Enabled = false;
            game.getNewGame(data);
            setInfoLabels();

            if (data.requiresAction)
                setFormsAction();
            else
                setFormsShowdown();
        }


        private string cardToStr(int card)
        {
            string[] suits = { "c", "d", "h", "s" };
            string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
            return values[card % 13] + suits[card / 13];
        }


        private string cardsToStr()
        {
            string str = "Opp hand: " + data.oppHandStr + " (" + cardToStr(data.oppHand[0]) + ", " + cardToStr(data.oppHand[1]) + ")" + " (" + data.oppHand[0] + ", " + data.oppHand[1] + ")";
            str += "\r\nMy hand:  " + data.myHandStr + " (" + cardToStr(data.myHand[0]) + ", " + cardToStr(data.myHand[1]) + ")" + " (" + data.myHand[0] + ", " + data.myHand[1] + ")";
            str += "\r\nBoard:  " + "(" + cardToStr(data.board[0]) + ", " + cardToStr(data.board[1]) + ", " + cardToStr(data.board[2]) + ", " + cardToStr(data.board[3]) + ", " + cardToStr(data.board[4]) + ")" + " (" + data.board[0] + ", " + data.board[1] + ", " + data.board[2] + ", " + data.board[3] + ", " + data.board[4] + ")";
            return str;
        }


        private void timerShowndown_Tick(object sender, EventArgs e)
        {
            timerShowndown.Enabled = false;


            pictureBoxBoard1.Show();
            pictureBoxBoard2.Show();
            pictureBoxBoard3.Show();
            pictureBoxBoard1.Image = cards[data.board[0]];
            pictureBoxBoard2.Image = cards[data.board[1]];
            pictureBoxBoard3.Image = cards[data.board[2]];
            pictureBoxBoard1.Refresh();
            pictureBoxBoard2.Refresh();
            pictureBoxBoard3.Refresh();
            Thread.Sleep(1000);
            pictureBoxBoard4.Show();
            pictureBoxBoard4.Image = cards[data.board[3]];
            pictureBoxBoard4.Refresh();
            Thread.Sleep(1000);
            pictureBoxBoard5.Show();
            pictureBoxBoard5.Image = cards[data.board[4]];
            pictureBoxBoard5.Refresh();
            Thread.Sleep(3000);

            string str = cardsToStr();

            if (data.winner == 0) // ME
            {
                calcStacks(HUMANWONALLIN);
                str += "\r\nYou won with " + data.myFinalHandStr;
                labelWinner.Text = "You won!";
                labelWinner.Show();
                labelWinner.Refresh();
                playerMe.showCards(false);
                playerOpp.showCards(false);
            }
            else if (data.winner == 1) // OPP
            {
                calcStacks(COMPUTERWONALLIN);
                str += "\r\nOpponent won with " + data.oppFinalHandStr;
                labelWinner.Text = "You lost!";
                labelWinner.Show();
                labelWinner.Refresh();
                playerMe.showCards(false);
                playerOpp.showCards(false);
            }
            else  // tasapeli
            {
                str += "\r\nPlayers tie with " + data.oppFinalHandStr;
                labelWinner.Text = "Players tie!";
                labelWinner.Show();
                labelWinner.Refresh();
                playerMe.showCards(false);
                playerOpp.showCards(false);
            }

            textBoxDealer.Text = str;
            Console.WriteLine("\n" + str + "\n");
            textBoxDealer.Refresh();
            
            if (game.myStack <= 0 || game.oppStack <= 0)
            {
                Console.WriteLine("TYHJENNYS!!");
                setInfoLabels();
                resetStacksAndBets();                
                labelLosses.Refresh();
                labelWins.Refresh();                
                pictureBoxTable.Refresh();
            }

            Thread.Sleep(3000);
            labelWinner.Hide();
            pictureBoxBoard1.Hide();
            pictureBoxBoard2.Hide();
            pictureBoxBoard3.Hide();
            pictureBoxBoard4.Hide();
            pictureBoxBoard5.Hide();

            // jos peli p‰‰ttyi kokonaan, niin aloitetaan kokonaan uusi arvonnalla buttonin paikasta
            if (game.myStack <= 0 || game.oppStack <= 0)
            {
                //resetStacksAndBets();
                game.reset();
                timerStart.Enabled = true;
            }
            // muuten aloitetaan uusi normaali peli
            else
                timerStartGame.Enabled = true;
        }


        private void Heads_Up_Trainer_ResizeBegin(object sender, EventArgs e)
        {

        }


        private void Heads_Up_Trainer_Resize(object sender, EventArgs e)
        {

        }

        // { "c", "d", "h", "s" };
        // luodaan kuvat muistiin kun ohjelma k‰ynnistet‰‰n
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (cards == null)
            {
                cards = new Bitmap[53];
                Color colorToFind  = new Color();
                Color colorToFind2 = Color.FromArgb(255, 0, 255);
                Color colorToPaint = new Color();
                Color black        = Color.FromArgb(0, 0, 0);
                Color red          = Color.FromArgb(255, 0, 0);
                Color green        = Color.FromArgb(0, 102, 0);  // c, green 
                Color blue         = Color.FromArgb(5, 67, 255); // d, blue 
                Color red2         = Color.FromArgb(252, 0, 0);  // h, red 
                Color black2       = Color.FromArgb(0, 23, 0);   // s, black 

                for (int i = 0; i < 52; i++)
                {
                    card.drawCard(pictureBox1, i, (int)cardsdll.mdFaceUp, 16777215);
                    cards[i] = new Bitmap(pictureBox1.Image);
                    if (i / 13 == 0) // club
                    {
                        colorToFind  = black;
                        colorToPaint = green;
                    }
                    else if (i / 13 == 1) // diamond
                    {
                        colorToFind = red;
                        colorToPaint = blue;
                    }
                    else if (i / 13 == 2) // heart
                    {
                        colorToFind = red;
                        colorToPaint = red2;
                    }
                    else if (i / 13 == 3) // spade
                    {
                        colorToFind = black;
                        colorToPaint = black2;
                    }
                    
                    for (int x = 0; x < cards[i].Width; x++)
                    {
                        for (int y = 0; y < cards[i].Height; y++)
                        {
                            if (cards[i].GetPixel(x, y) == colorToFind || (i / 13 == 1 && cards[i].GetPixel(x, y) == colorToFind2))
                            {                                
                                cards[i].SetPixel(x, y, colorToPaint);
                            }
                        }
                    }
                }
            }
            card.drawCardBack(pictureBox1);
            cards[52] = new Bitmap(pictureBox1.Image);
            pictureBox1.Visible = false;
        }
    }
}