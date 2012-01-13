using System;
using System.Collections.Generic;
using System.Text;

namespace HeadsUp_Trainer
{
    class calcRanges
    {
        private double[,] playersData;
        private int BB;
        public int raiserRange;
        public int callerRange;
        private double[] stacks;
        private double[] ranges;
        private static readonly double THRESHOLD = 0.1;
        private int[] myHand = { 0, 0 };
        private static readonly double[] ICMs = { 0.5, 0.3, 0.2 };
        private static readonly int ICMc = 3;
        private ICM icm;


        public calcRanges()
        {
            icm = new ICM();
            playersData = new double[10, 9];
            stacks = new double[4];
            ranges = new double[4];
        }


        private void resetPlayersData()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int a = 0; a < 9; a++)
                    playersData[i, a] = 0;
            }
        }


        private void reset()
        {
            resetPlayersData();
            for (int i = 0; i < 4; i++)
            {
                stacks[i] = 0;
                ranges[i] = 0;
            }
            raiserRange = 0;
            callerRange = 20;
        }


        private void calcPushRange()
        {
            int myIndex = 1;    // SB
            int oppIndex = 0;   // BB
            int range = 1;
            double EV = 0;

            // betit
            playersData[myIndex, 1] = BB / 2;
            playersData[oppIndex, 1] = BB;
            
            // ranget
            playersData[myIndex, 2] = 0;
            playersData[oppIndex, 2] = callerRange;

            // etsit‰‰n ensimm‰inen positiivinen push range
            for (range = 100; range > 0; range--)
            {
                //icm.handIndexToRandomHand(range, myHand);
                myHand = icm.handIndexToHand(range);
                EV = icm.calcPush(2, myHand, myIndex, playersData, ICMs, ICMc);

                //Console.WriteLine("calcPushRange! Hand: " + icm.handToStr(myHand) + " EV: " + EV);

                if (EV > THRESHOLD)
                    break;
            }

            //Console.WriteLine(playersData[0, 0] + " " + playersData[1, 0]);
            //Console.WriteLine("Ensimmainen positiivinen push range on " + range);
            //Console.WriteLine("Hand: " + icm.handToStr(myHand) + " EV: " + EV);
            raiserRange = range;
        }


        private void calcCallRange()
        {
            int myIndex = 0;    // BB
            int oppIndex = 1;   // SB
            int range = 100;
            double EV = 0;

            // betit
            playersData[myIndex, 1] = BB;
            playersData[oppIndex, 1] = playersData[oppIndex, 0];
            
            // ranget
            playersData[myIndex, 2] = 0;
            playersData[oppIndex, 2] = raiserRange;


            // etsit‰‰n ensimm‰inen positiivinen call range
            for (range = 100; range > 0; range--)
            {
                //icm.handIndexToRandomHand(range, myHand);
                myHand = icm.handIndexToHand(range);
                EV = icm.calcCall(2, myHand, myIndex, oppIndex, playersData, ICMs, ICMc);
                if (EV > THRESHOLD)
                    break;
            }

            //Console.WriteLine(playersData[0, 0] + " " + playersData[1, 0]);
            //Console.WriteLine("Ensimmainen positiivinen call range on " + range);
            //Console.WriteLine("Hand: " + icm.handToStr(myHand) + " EV: " + EV);
            callerRange = range;
        }


        public void calc(int[] iStacks, int iBB, int iIterations)
        {
            reset();
            BB = iBB;

            // tehd‰‰n ennen silmukkaa ensimm‰inen iteraatio, jotta callerRange ja 
            resetPlayersData();
            playersData[0, 0] = iStacks[0];
            playersData[1, 0] = iStacks[1];
            calcPushRange();
            calcCallRange();
            //Console.WriteLine("callerRange: " + callerRange + " raiserRange: " + raiserRange);
           
            for (int i = 1; i < iIterations; i++)
            {
                resetPlayersData();
                playersData[0, 0] = iStacks[0];
                playersData[1, 0] = iStacks[1];

                int raiserRangeOld = raiserRange;
                int callerRangeOld = callerRange;

                double weight = 1.0 / i;

                calcPushRange();
                calcCallRange();

                //Console.WriteLine("BEFORE: raiserRangeOld: " + raiserRangeOld + " callerRangeOld: " + callerRangeOld + " raiserRange: " + raiserRange + " callerRange: " + callerRange);

                //raiserRange = (int)(((1 - weight) * raiserRangeOld + weight * raiserRange) / 2.0 + 0.5);
                //callerRange = (int)(((1 - weight) * callerRangeOld + weight * callerRange) / 2.0 + 0.5);

                raiserRange = (int)(((1 - weight) * raiserRangeOld + weight * raiserRange));
                callerRange = (int)(((1 - weight) * callerRangeOld + weight * callerRange));

                // lopetetaan iterointi heti jos optimi lˆydet‰‰n ennen iterointien maksimim‰‰r‰‰
                //if (raiserRangeOld == raiserRange && callerRangeOld == callerRange)
                //    break;

                //Console.WriteLine("AFTER: raiserRange: " + raiserRange + " callerRange: " + callerRange);

                //Console.WriteLine("callerRange: " + callerRange + " raiserRange: " + raiserRange);
                //Console.WriteLine();
            }
            Console.WriteLine("INSIDE: callerRange: " + callerRange + " raiserRange: " + raiserRange + " iStacks[0]: " + iStacks[0] + " iStacks[1]: " + iStacks[1]);
            
        }
    }
}
