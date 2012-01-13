using System;
using System.Collections.Generic;
using System.Text;

namespace SNG_Quiz
{
    class singleGame
    {
        public int numOfPlayers;        
        public int pushOrCall;
        public int BBsize;
        public int position;
        public int raiserPos;
        public int raiserRange;
        public int action;
        public double[] stacks;
        public double[] ranges;
        public int[] myHand;
        public double myEV;

        public singleGame(int iNumOfPlayers,
                          int iPushOrCall,
                          int iBBsize,
                          int iPosition,
                          int iRaiserPos,
                          int iRaiserRange,
                          int iAction,
                          double[] iStacks,
                          double[] iRanges,
                          int[] iMyHand,
                          double iMyEV)
        {
            numOfPlayers = iNumOfPlayers;
            pushOrCall = iPushOrCall;
            BBsize = iBBsize;
            position = iPosition;
            raiserPos = iRaiserPos;
            raiserRange = iRaiserRange;
            action = iAction;

            stacks = new double[4];
            ranges = new double[4];
            myHand = new int[2];

            for (int i = 0; i < 4; i++)
            {
                stacks[i] = iStacks[i];
                ranges[i] = iRanges[i];
            }

            myHand[0] = iMyHand[0];
            myHand[1] = iMyHand[1];

            myEV = iMyEV;
        }
    }
}
