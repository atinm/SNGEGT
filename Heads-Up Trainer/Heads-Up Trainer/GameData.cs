using System;
using System.Collections.Generic;
using System.Text;

namespace HeadsUp_Trainer
{
    class GameData
    {
        public int BBsize = 0;
        public int gameNumber = 0;
        public int myPos = 0; // 0=BB, 1=SB
        public int[] myHand = { 0, 0 };
        public int[] oppHand = { 0, 0 };
        public string myHandStr = "";
        public string oppHandStr = "";
        public int[] board = { 0, 0, 0, 0, 0 };
        public bool oppPush = true;
        public int winner = 0; // 0=me, 1=opp, 2=tasapeli
        public int[] myHandResult = { 0, 0, 0, 0, 0, 0 };
        public int[] oppHandResult = { 0, 0, 0, 0, 0, 0 };
        public string myFinalHandStr = "";
        public string oppFinalHandStr = "";
        public int myHandSklanskyValue = 0;
        public int oppHandSklanskyValue = 0;
        public int myStack = 0;
        public int oppStack = 0;
        public bool requiresAction = false; // jos st‰kit > BB, niin t‰m‰ p‰‰ll‰

        public void reset()
        {
            BBsize = 0;
            gameNumber = 0;
            myPos = 0;
            myHand[0] = 0;
            myHand[1] = 0;
            oppHand[0] = 0;
            oppHand[1] = 0;
            myHandStr = "";
            oppHandStr = "";
            oppPush = true;
            winner = -1;
            myFinalHandStr = "";
            oppFinalHandStr = "";
            myFinalHandStr = "";
            oppFinalHandStr = "";
            myHandSklanskyValue = -1;
            oppHandSklanskyValue = -1;
            myHandSklanskyValue = -1;
            myStack = 0;
            oppStack = 0;
            requiresAction = false;
            for (int i = 0; i < 5; i++)
                board[i] = 0;
            for (int i = 0; i < 6; i++)
            {
                myHandResult[i] = 0;
                oppHandResult[i] = 0;
            }
        }
    }


}
