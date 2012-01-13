using System;
using System.Collections.Generic;
using System.Text;

namespace HeadsUp_Trainer
{
    class Game
    {
        private static readonly int ME = 0;
        private static readonly int OPP = 1;

        private calcRanges calcranges;
        private ICM icm;
        private Recognize recognize;

        private int[] deck;
        private int[] BBarray = {600, 800, 1200, 2000, 3000};
        //private int[] BBarray = { 3000 };
        private int gameNum;
        public int myStack;
        public int oppStack;
        private int[] myHand;
        private int[] oppHand;
        private Random random;
        private int button;


        public Game()
        {
            deck = new int[52];
            myHand = new int[2];
            oppHand = new int[2];
            random = new Random();
            calcranges = new calcRanges();
            icm = new ICM();
            recognize = new Recognize();
            reset();
        }


        public void reset()
        {
            for (int i = 0; i < 52; i++)
                deck[i] = i;            
            myStack = 10000;
            oppStack = 10000;
            myHand[0] = -1;
            myHand[1] = -1;
            oppHand[0] = -1;
            oppHand[1] = -1;
            gameNum = 1;
            button = 0;
            shuffle();
        }


        private void shuffle()
        {
            
            int n, randint, tmp;
            for (n = 52; n > 1; n--)
            {
                randint = random.Next(0, n);
                tmp = deck[randint];
                deck[randint] = deck[n - 1];
                deck[n - 1] = tmp;
            }
            
            /*
            deck[0] = 48;
            deck[1] = 45;
            deck[2] = 9;
            deck[3] = 6;
            deck[4] = 49;
            deck[5] = 7;
            deck[6] = 51;
            deck[7] = 38;
            deck[8] = 14;
            */
            /*
            for (int i = 0; i < 52; i++)
            {
                Console.Write(deck[i] + ",");
                if(i%10==1 || i==51)
                    Console.WriteLine("");
            }
            */            
        }


        public void getStartCards(int[] cards)
        {
            /*
            cards[0] = 3;
            cards[1] = 4;
            return;
            */
            cards[ME] = 0;
            cards[OPP] = 0;
            while (cards[ME] % 13 == cards[OPP] % 13)
            {
                Console.WriteLine("paskaa");
                shuffle();
                cards[ME] = deck[0];
                cards[OPP] = deck[1];
            }
            if (cards[ME] % 13 > cards[OPP] % 13)
                button = ME;
            else
                button = OPP;
        }


        private string finalHandToStr(int[] results)
        {
            string[] hands = { "high card", "one pair", "two pairs", "trips", "straight", "flush", "full house", "quads", "straight flush"};
            return hands[results[0]];
        }


        // näiden korttien perusteella määritellään ensimmäisen pelin button
        public void getNewGame(GameData data)
        {            
            shuffle();
            data.reset();

            int[] stacks = { 0, 0 };
            if (button % 2 == ME)
            {
                stacks[0] = oppStack;
                stacks[1] = myStack;
                data.myPos = 1; // SB
            }
            else
            {
                stacks[0] = myStack;
                stacks[1] = oppStack;
                data.myPos = 0; // BB
            }
            data.BBsize = BBarray[(gameNum - 1) / 10];
            data.gameNumber = gameNum;

            if (stacks[0] > data.BBsize && stacks[1] > data.BBsize)
            {
                calcranges.calc(stacks, data.BBsize, 10);
                data.requiresAction = true;                
            }
            else
                data.requiresAction = false;

            data.myHand[0] = deck[0];
            data.myHand[1] = deck[1];
            data.oppHand[0] = deck[2];
            data.oppHand[1] = deck[3];
            for (int i = 0; i < 5; i++)
                data.board[i] = deck[4 + i];

            // selvitetään voittaja
            int myHandValue  = recognize.handValue(data.myHand,  data.board, 5, data.myHandResult);
            int oppHandValue = recognize.handValue(data.oppHand, data.board, 5, data.oppHandResult);            
            if (myHandValue > oppHandValue)
                data.winner = ME;
            else if (oppHandValue > myHandValue)
                data.winner = OPP;
            else
                data.winner = 2;    // tasapeli
            Console.WriteLine("myHandValue: " + myHandValue + " oppHandValue" + oppHandValue + " winner: " + data.winner);

            data.myFinalHandStr  = finalHandToStr(data.myHandResult);
            data.oppFinalHandStr = finalHandToStr(data.oppHandResult);

            data.myHandStr  = icm.handToStr(data.myHand);
            data.oppHandStr = icm.handToStr(data.oppHand);

            data.myHandSklanskyValue = icm.handToValue(data.myHand);
            data.oppHandSklanskyValue = icm.handToValue(data.oppHand);

            // voiko tietokonevastustaja mennä alliin (joko push tai call)
            if (button % 2 == ME && data.oppHandSklanskyValue <= calcranges.callerRange)
                data.oppPush = true;    // tietokonevastustaja maksaa allin
            else if (button % 2 == OPP && data.oppHandSklanskyValue <= calcranges.raiserRange)
                data.oppPush = true;    // tietokonevastustaja tekee itse buttonista allin
            else if (stacks[0] <= data.BBsize || stacks[1] <= data.BBsize)
                data.oppPush = true;    // jompi kumpi stäkki < BB, joten allissa pakosti
            else
                data.oppPush = false;   // kaikissa muissa tapauksissa tietokone foldaa

            data.oppStack = oppStack;
            data.myStack = myStack;

            Console.WriteLine("calcranges.raiserRange: " + calcranges.raiserRange + " calcranges.callerRange: " + calcranges.callerRange);
            Console.WriteLine("data.myHandStr: " + data.myHandStr + " SklanskyValue: " + data.myHandSklanskyValue);
            Console.WriteLine("data.oppHandStr: " + data.oppHandStr + " SklanskyValue: " + data.oppHandSklanskyValue);

            button++;
            gameNum++;
        }

    }
}
