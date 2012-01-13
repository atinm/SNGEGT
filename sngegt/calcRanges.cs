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

namespace SNGEGT
{
    class calcRanges
    {
        // objects and variables
        private double[,] playersData;
        private double EVthreshold;
        private int[] myHand;
        private ICM icm;
        private int[,] ranges;
        private int players;
        private int raiserIndex;
        private double[] payoutStructure;


        // constants
        private readonly static int OLD = 0;
        private readonly static int NEW = 1;
        private static readonly int STACK = 0;
        private static readonly int BET = 1;
        private static readonly int RANGE = 2;


        public calcRanges()
        {
            icm = new ICM();
            playersData = new double[10, 9];
            ranges = new int[10,2];
            myHand = new int[2];
            reset();
        }


        private void resetPlayersData()
        {
            for (int i = 0; i < 10; i++)
                for (int a = 0; a < 9; a++)
                    playersData[i, a] = 0;
        }


        private void printICMarray()
        {
#if (DEBUGOMA)
            for (int i = 0; i < 10; i++)
            {
                for (int a = 0; a < 4; a++)
                    Console.Write(playersData[i, a] + "\t");
                Console.Write("\n");
            }
            Console.Write("\n");
#endif
        }


        private void reset()
        {
            resetPlayersData();
            for (int i = 0; i < 10; i++)
            {
                ranges[i, OLD] = 0;
                ranges[i, NEW] = 0;
            }
        }


        //Initialize icm array to values which has been received as parameters
        private void setICMarray(int iPlayers, int[] iStacks, int iRaiserIndex, int iBB, int iAnte, bool iNoSB)
        {
            // zero all
            resetPlayersData();

            // stacks and possible antes
            for (int i = 0; i < iPlayers; i++)
            {
                playersData[i, STACK] = iStacks[i];
                playersData[i, BET] = iAnte;
            }

            // blinds
            playersData[0, BET] += iBB;
            if (iNoSB == false)
                playersData[1, BET] += iBB / 2;

        }


        // We are doing first PUSH
        private int calcPushRange(int myIndex)
        {
            double[] EVs = { 0, 0, 0 };
            int range = 0;

            EVs[1] = icm.calcPush(players, icm.handIndexToHand(1), myIndex, playersData, payoutStructure, payoutStructure.Length);
            EVs[2] = icm.calcPush(players, icm.handIndexToHand(2), myIndex, playersData, payoutStructure, payoutStructure.Length);
            for (range = 3; range <= 100; range++)
            {
                EVs[0] = EVs[1];
                EVs[1] = EVs[2];
                EVs[2] = icm.calcPush(players, icm.handIndexToHand(range), myIndex, playersData, payoutStructure, payoutStructure.Length);

                if ((EVs[0] + EVs[1] + EVs[2]) / 3 < EVthreshold)
                    break;
            }

            return range - 1;
        }


        // We are in callerIndex  and paying allin of raiserIndex 
        private int calcCallRange(int callerIndex, int raiserIndex)
        {
            double[] EVs = { 0, 0, 0 };
            int range = 0;

            // search for first call range which is below given level
            EVs[1] = icm.calcCall(players, icm.handIndexToHand(1), callerIndex, raiserIndex, playersData, payoutStructure, payoutStructure.Length);
            EVs[2] = icm.calcCall(players, icm.handIndexToHand(2), callerIndex, raiserIndex, playersData, payoutStructure, payoutStructure.Length);
            for (range = 3; range <= 100; range++)
            {
                EVs[0] = EVs[1];
                EVs[1] = EVs[2];
                EVs[2] = icm.calcCall(players, icm.handIndexToHand(range), callerIndex, raiserIndex, playersData, payoutStructure, payoutStructure.Length);

                //Debug.WriteLine("calcPushRange! Hand: " + icm.handToStr(myHand) + " EV: " + EV);

                if ((EVs[0] + EVs[1] + EVs[2]) / 3 < EVthreshold)
                    break;
            }

            return range - 1;
        }


        bool rangesAreConstans(int[,] ranges)
        {
            for (int i = 0; i < players; i++)
                if (ranges[i, OLD] != ranges[i, NEW])
                    return false;
            return true;
        }


        // iPlayers: player count
        // iStacks: stack sizes BEFORE setting blinds and antes
        // iRaiserIndex: raiser index in array
        // iBB: Big blind size
        // iNoSB: True if there isn't smallblind otherwise false
        // iICMs: Award structure
        // iThreshold: Minimal EV threshold
        // oRanges: Calculation results, call and push ranges
        public void calc(int iPlayers, int[] iStacks, int iRaiserIndex, int iBB, int iAnte, bool iNoSB, double iEVthreshold, double[] iPayoutStructure, int[] oRanges)
        {

            Debug.WriteLine("iPlayers: " + iPlayers);
            Debug.WriteLine("iRaiserIndex: " + iRaiserIndex);
            Debug.WriteLine("iBB: " + iBB);

            double weight = 0;

            players = iPlayers;
            raiserIndex = iRaiserIndex;
            EVthreshold = iEVthreshold;
            payoutStructure = iPayoutStructure;

            // Initialze playersData -table for ICM -calculations
            setICMarray(iPlayers, iStacks, iRaiserIndex, iBB, iAnte, iNoSB);
            for (int i = 0; i < iPlayers; i++)
                playersData[i, RANGE] = 20;    // TÄTÄ VOI MUUTTAA JOKSUS REALISTISEMMAKSI
            
            // calculate first ranges for raiser and all callers before entering loop
            ranges[raiserIndex, NEW] = calcPushRange(raiserIndex);            
            for (int callerIndex = iRaiserIndex - 1; callerIndex >= 0; callerIndex--)
            {
                setICMarray(iPlayers, iStacks, iRaiserIndex, iBB, iAnte, iNoSB);   // ICM -taulukko aina uusiksi
                playersData[iRaiserIndex, BET] = playersData[iRaiserIndex, STACK]; // reissaajan betti stäkin kokoiseksi
                playersData[iRaiserIndex, RANGE] = ranges[raiserIndex, NEW];       // reissaajan range edellä lasketuksi
                //printICMarray();
                ranges[callerIndex, NEW] = calcCallRange(callerIndex, raiserIndex);// maksajan range talteen
            }

            printICMarray();
            for (int w = 0; w < iPlayers; w++)
                Console.Write(ranges[w, NEW] + " ");
#if (DEBUGOMA)
            Debug.WriteLine("\n******************************************");
#endif
            // then iterate until ranges ranges becomes stable
            int q = 0;
            while(true)
            {
                q++;
#if (DEBUGOMA)
                Debug.WriteLine("\n******************* NEW ROUND " + q + " **********************\n");
#endif
                weight = 1.0 / q;

                // set playersData array for ICM -calculations
                setICMarray(iPlayers, iStacks, iRaiserIndex, iBB, iAnte, iNoSB);
                for (int a = 0; a < iPlayers; a++)
                {
                    ranges[a, OLD] = ranges[a, NEW];
                    playersData[a, RANGE] = ranges[a, NEW];
                }

#if (DEBUGOMA)
                Debug.WriteLine("Inside loop");
#endif
                printICMarray();
                                
                // calculate new ranges for raiser and all callers before entering loop
                ranges[raiserIndex, NEW] = calcPushRange(raiserIndex);
#if (DEBUGOMA)
                Debug.WriteLine("ranges[raiserIndex, NEW] = " + ranges[raiserIndex, NEW]);
#endif
                for (int callerIndex = iRaiserIndex - 1; callerIndex >= 0; callerIndex--)
                {
                    setICMarray(iPlayers, iStacks, iRaiserIndex, iBB, iAnte, iNoSB);   // ICM -taulukko aina uusiksi
                    playersData[iRaiserIndex, BET] = playersData[iRaiserIndex, STACK]; // reissaajan betti stäkin kokoiseksi
                    playersData[iRaiserIndex, RANGE] = ranges[raiserIndex, NEW];       // reissaajan range edellä lasketuksi                    
                    ranges[callerIndex, NEW] = calcCallRange(callerIndex, raiserIndex);// maksajan range talteen
#if (DEBUGOMA)
                    Debug.WriteLine("Inside loop 2 callerIndex: " + callerIndex);
#endif
                    printICMarray();
#if (DEBUGOMA)
                    Debug.WriteLine("ranges[callerIndex, NEW] = " + ranges[callerIndex, NEW]);
#endif
                }

                printICMarray();
#if (DEBUGOMA)
                Console.Write("Before  : ");

                for (int w = 0; w < iPlayers; w++)
                    Console.Write(ranges[w, OLD] + " ");
                Debug.WriteLine("");
#endif
                // weight new results with old
                for (int a = 0; a < iPlayers; a++)
                {
                    ranges[a, NEW] = (int)(((1 - weight) * ranges[a, OLD] + weight * ranges[a, NEW]));
                }
#if (DEBUGOMA)
                Console.Write("Jalkeen: ");
                for (int w = 0; w < iPlayers; w++)
                    Console.Write(ranges[w, NEW] + " ");
                Debug.WriteLine("");
#endif

                if (rangesAreConstans(ranges))
                    break;
                //raiserRange = (int)(((1 - weight) * raiserRangeOld + weight * raiserRange));
                //callerRange = (int)(((1 - weight) * callerRangeOld + weight * callerRange));
            }

            for (int w = 0; w < iPlayers; w++)
                oRanges[w] = ranges[w, NEW];
        }
    }
}
