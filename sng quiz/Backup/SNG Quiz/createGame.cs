using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SNG_Quiz
{    

    class createGame
    {
        private static readonly int CHIP1000 = 0;
        private static readonly int CHIP1500 = 1;
        private static readonly int CHIP2000 = 2;

        //private static readonly int EASY = 0;
        //private static readonly int MEDIUM = 1;
        //private static readonly int HARD = 2;

        //private static readonly int FOLD = 0;
        private static readonly int PUSH = 0;
        private static readonly int CALL = 1;
        private static readonly int RANDTYPE = 2;

        //private static readonly int HEADSUP = 0;
        //private static readonly int THREELEFT = 1;
        //private static readonly int FOURLEFT = 2;
        private static readonly int RANDLEFT = 3;

        private static readonly int BB = 0;
        private static readonly int SB = 1;
        //private static readonly int BTN = 2;
        //private static readonly int UTG = 3;
        private static readonly int RANDPOS = 4;

        //private static readonly int FOLDMIN = 0;
        //private static readonly int FOLDMAX = 1;
        //private static readonly int PUSHMIN = 2;
        //private static readonly int PUSHMAX = 3;

        // parametrina saatavat, mutta mahdollisesti muokattavat muuttujat
        private int numOfPlayers = 0;        
        private int pushOrCall   = 0;
        private int difficulty   = 0;
        private int BBsize       = 0;
        private int position     = 0;

        // kokonaan itse laskettavat muuttujat
        private int stackSum = 0;
        private double[] stacks = { 0, 0, 0, 0 };
        private double[] ranges = { 0, 0, 0, 0 };
        private int raiserPos = 0; // call -moodissa korottajan indeksi taulukkoon stacks[]
        private double raiserRange = 0;
        private int myRange = 0;
        int[] myHand = { 0, 0 };
        private double[,] playersData;
        private double maxEV;
        private double minEV;
        private int action; // 0=toiminta -$EV, 1=toiminta +$EV
        private double myEV; // oman liikkeen tarkka EV
        private bool rangeError = true; // jos omaa rangea ei pystyt‰ laskemaan, niin t‰m‰ todeksi
        

        // sekalaiset
        Random random;
        private bool DEBUGCONSOLE = true;
        ICM icm = new ICM();
        private static readonly double[] ICMs = { 0.5, 0.3, 0.2 };
        private static readonly int ICMc = 3;
        //private static readonly double[, ,] EVreq = { { { -2.0, -0.75 }, { 0.75, 2.0 } }, { { -0.9, -0.5 }, { 0.5, 0.9 } }, { { -0.6, -0.2 }, { 0.2, 0.6 } } };
        private static readonly double[, ,] EVreq = { { { -0.6, -0.10 }, { 0.75, 2.0 } }, { { -0.4, -0.10 }, { 0.5, 0.9 } }, { { -0.3, -0.1 }, { 0.2, 0.6 } } };


        public createGame()
        {
            random = new Random();
            playersData = new double[10, 9];
        }


        private void reset()
        {
            stackSum = 0;
            raiserPos = 0;
            for (int i = 0; i < 4; i++) 
            {
                stacks[i] = 0;
                ranges[i] = 0;
            }
            raiserRange = 0;
            myRange = 0;
            resetPlayersData();
            //maxEV = 0;
            //minEV = 0;
            //action = 0;
            rangeError = true;
        }


        private void resetPlayersData() {
            for (int i = 0; i < 10; i++)
            {
                for (int a = 0; a < 9; a++)
                    playersData[i, a] = 0;
            }
        }


        private void EVplusOrMinus()
        {
            // +EV vai -EV p‰‰tˆs
            double rand = random.NextDouble();
            //Console.WriteLine("ACTION rand:" + rand);
            if (rand < 0.5)
                action = 0; // -EV
            else
                action = 1; // +EV
        }


        // asetetaan perusmuuttujat kohdalleen lomakkeelta saatujen tietojen perusteella
        private void setInitialVariables(int iNumOfPlayers, int iPushOrCall, int iDifficulty, int iStartChips, int iBBsize, int iPosition)
        {
            // pelaajien lukum‰‰r‰
            if (iNumOfPlayers < RANDLEFT)
                numOfPlayers = iNumOfPlayers + 2;
            else
                numOfPlayers = random.Next(2, 5);

            // push tai call -moodi
            if (iPushOrCall < RANDTYPE)
                pushOrCall = iPushOrCall;
            else
                pushOrCall = random.Next(0, 2);

            difficulty = iDifficulty;

            // st‰kkien summa
            if (iStartChips == CHIP1000)
                stackSum = 1000 * 10;
            else if (iStartChips == CHIP1500)
                stackSum = 1500 * 10;
            else if (iStartChips == CHIP2000)
                stackSum = 2000 * 10;

            // ison blindin koko
            /*
            int[] blindArray = { 200, 400, 600, 800, 1200, 1600 };
            if (iBBsize < blindArray.Length)
                BBsize = blindArray[iBBsize];
            else
            {
                double[, ,] likelihood = { 
                { { 0.0, 0.1, 0.3, 0.6, 0.8, 1.0}, { 0.1, 0.3, 0.4, 0.6, 0.8, 1.0}, { 0.2, 0.4, 0.5, 0.8, 1.0, 1.0}},  // 1000$ chips  {2-left, 3-left, 4-left}
                { { 0.0, 0.0, 0.2, 0.6, 0.8, 1.0}, { 0.0, 0.2, 0.3, 0.5, 0.8, 1.0}, { 0.1, 0.3, 0.4, 0.7, 0.9, 1.0}},  // 1500$ chips  {2-left, 3-left, 4-left}
                { { 0.0, 0.0, 0.3, 0.6, 0.8, 1.0}, { 0.0, 0.1, 0.2, 0.4, 0.8, 1.0}, { 0.0, 0.2, 0.3, 0.6, 0.8, 1.0}},  // 2000$ chips  {2-left, 3-left, 4-left}
                };
                double q = random.NextDouble();
                //Console.WriteLine("q: {0}", q);
                for (int i = 0; i < blindArray.Length; i++)
                {
                    //Console.WriteLine("if({0} < {1})", q, likelihood[iStartChips, numOfPlayers - 2, i]);
                    if (q < likelihood[iStartChips, numOfPlayers - 2, i])
                    {
                        BBsize = blindArray[i];
                        break;
                    }
                }
            }
            */

            int[] blindArray = { 200, 400, 600, 800, 1200 };
            if (iBBsize < blindArray.Length)
                BBsize = blindArray[iBBsize];
            else
                BBsize = blindArray[random.Next(0, blindArray.Length)];


            // oma positio heads-upissa
            if (numOfPlayers == 2 && iPosition != RANDPOS)
            {
                if (pushOrCall == CALL)
                    position = BB;
                else if (pushOrCall == PUSH)
                    position = SB;
            }
            else if (iPosition == RANDPOS)
            {
                if (pushOrCall == PUSH)
                    position = random.Next(1, numOfPlayers);  // [1-3]
                else if (pushOrCall == CALL)
                    position = random.Next(0, numOfPlayers - 1);  // [0-2]
            }
            else if (pushOrCall == CALL)
                position = iPosition;
            else if (pushOrCall == PUSH)
                position = iPosition; // koska push -moodin ollessa valittuna ei n‰ytet‰ BB:t‰ valintalistassa HUOM!!
            
            // call -moodissa nostajan indeksi
            if (pushOrCall == CALL)
            {
                if (numOfPlayers == 2)
                {
                    raiserPos = SB;
                }
                else
                {
                    raiserPos = random.Next(position + 1, numOfPlayers);
                }
            }
            // push moodissa ollaan itse korottaja
            else
                raiserPos = position;

            // lasketaan minimi ja maksimi EV:t joiden v‰lilt‰ oma k‰si valitaan
            minEV = EVreq[difficulty, action, 0];
            maxEV = EVreq[difficulty, action, 1];
        }


        private bool isStacksOK()
        {
            for (int i = 0; i < numOfPlayers; i++)
                if (stacks[i] < BBsize * 1.7)
                    return false;
            return true;
        }


        private void calcStacks()
        {
            for (int i = 0; i < numOfPlayers; i++)
                stacks[i] = 0;

            while (!isStacksOK())
            {
                if (numOfPlayers == 2)
                {
                    double half;
                    half = random.NextDouble();
                    stacks[0] = (int)((int)stackSum / 100 * half) * 100;
                    stacks[1] = stackSum - stacks[0];
                }
                else if (numOfPlayers == 3)
                {
                    double firstRand, secondRand;
                    firstRand = random.NextDouble();
                    secondRand = random.NextDouble();

                    // lasketaan ensin kaikki nelj‰ st‰kki‰
                    stacks[0] = (int)((int)stackSum / 100 * firstRand) * 100;
                    stacks[2] = stackSum - stacks[0];
                    stacks[1] = (int)((int)stacks[0] / 100 * secondRand) * 100;
                    stacks[0] = stacks[0] - stacks[1];
                    stacks[2] = (int)((int)stacks[2] / 100 * secondRand) * 100;
                    stacks[3] = stackSum - (stacks[0] + stacks[1] + stacks[2]);

                    // ja sitten jaetaan nelj‰s st‰kki kolmen ensimm‰isen kesken
                    int addOn = (int)((int)stacks[3] / 100) * 100;
                    stacks[0] += addOn;                    
                    stacks[1] += addOn;
                    stacks[3] -= addOn;
                    stacks[3] -= addOn;
                    stacks[2] += stacks[3];
                    stacks[3] = 0;
                }
                else if (numOfPlayers == 4)
                {
                    double firstRand, secondRand;
                    firstRand = random.NextDouble();
                    secondRand = random.NextDouble();

                    stacks[0] = (int)((int)stackSum / 100 * firstRand) * 100;
                    stacks[2] = stackSum - stacks[0];

                    stacks[1] = (int)((int)stacks[0] / 100 * secondRand) * 100;
                    stacks[0] = stacks[0] - stacks[1];

                    stacks[2] = (int)((int)stacks[2] / 100 * secondRand) * 100;
                    stacks[3] = stackSum - (stacks[0] + stacks[1] + stacks[2]);
                }
            }
        }


        private void calcRaiserRange()
        {
            int[,] callPercentages = {{25, 0,  0},  // heads-up
                                     {16, 12, 0},   // three left
                                     {14, 12, 10}};  // bubble

            Console.WriteLine("HALY!! numOfPlayers: " + numOfPlayers + " raiserPos: " + raiserPos);
            int callPercent = callPercentages[numOfPlayers - 2, raiserPos - 1];
            Console.WriteLine("vastustajien call Percent: " + callPercent);

            int myIndex = position;
            int myPercent = 1;
            for (myPercent = 1; myPercent < 60; myPercent += 2)
            {
                // alustetaan resetPlayersData, asetetaan st‰kit ja maksetut blindit
                resetPlayersData();
                for (int a = 0; a < numOfPlayers; a++)
                    playersData[a, 0] = stacks[a];
                playersData[0, 1] = BBsize;
                playersData[1, 1] = BBsize / 2;

                // vastustajien call ranget
                for (int a = 0; a < myIndex; a++)
                    playersData[a, 2] = callPercent;

                if (icm.calcPush(numOfPlayers, icm.handIndexToHand(myPercent), myIndex, playersData, ICMs, ICMc) < 0)
                    break;
            }

            // pienennet‰‰n rangea satunnaisesti hieman
            if (myPercent == 61)
                raiserRange = random.Next(20, myPercent);
            else if (raiserRange > 20)
                raiserRange -= random.Next(5, 10);
            else
                raiserRange = myPercent;
        }


        private void calcOpponentsCallRanges()
        {            
            // jos ollaan CALL -moodissa, niin muiden pelaajien call -rangeja ei tarvitse laskea
            if (pushOrCall == CALL)
                return;
            
            // lasketaan call % kaikille ennen omaa positiota oleville
            for (int i = position - 1; i >= 0; i--)
            {
                // alustetaan resetPlayersData, asetetaan st‰kit ja maksetut blindit
                resetPlayersData();
                for (int a = 0; a < numOfPlayers; a++)
                    playersData[a,0] = stacks[a];
                playersData[0,1] = BBsize;
                playersData[1,1] = BBsize / 2;
                int[] hand = { 0, 0 };

                // vastustajan indeksiksi oma paikka ja omaksi indeksiksi se jolle EV lasketaan
                int oppIndex = position;
                int myIndex = i;

                // "vastustaja" allissa
                playersData[oppIndex,1] = playersData[oppIndex,0];
                playersData[oppIndex,2] = raiserRange;

                // vaadittava EV satunnaisesti v‰lille 0.2-0.5
                double EVgoal = random.Next(20, 51) / 100.0;
                //double EVgoal = 0.2;  // callaajat tyytyv‰t 0.2% EV etuun

                // etsit‰‰n karkea aloitusÌndeksi k‰den vahvuudelle
                double EVresult = 0;
                int handIndex = 0;
                if (icm.calcCall(numOfPlayers, icm.handIndexToHand(5), myIndex, oppIndex, playersData, ICMs, ICMc) < EVgoal)
                    handIndex = 5;
                else if (icm.calcCall(numOfPlayers, icm.handIndexToHand(10), myIndex, oppIndex, playersData, ICMs, ICMc) < EVgoal)
                    handIndex = 10;
                else if (icm.calcCall(numOfPlayers, icm.handIndexToHand(20), myIndex, oppIndex, playersData, ICMs, ICMc) < EVgoal)
                    handIndex = 20;                
                else if (icm.calcCall(numOfPlayers, icm.handIndexToHand(50), myIndex, oppIndex, playersData, ICMs, ICMc) < EVgoal)
                    handIndex = 50;
                else if (icm.calcCall(numOfPlayers, icm.handIndexToHand(75), myIndex, oppIndex, playersData, ICMs, ICMc) < EVgoal)
                    handIndex = 75;
                else
                    handIndex = 100;

                //Console.WriteLine("Alotusindeksi = {0}", handIndex);

                for (; handIndex > 1; handIndex--)
                {
                    /*
                    if (i == 2)
                    {
                        Console.WriteLine("\n\n");
                        Console.WriteLine("oppIndex: {0}", oppIndex);
                        Console.WriteLine("myIndex: {0}", myIndex);
                        for (int a = 0; a < 4; a++)
                            Console.WriteLine("{0}\t{1}\t{2}", playersData[a, 0], playersData[a, 1], playersData[a, 2]);
                        Console.WriteLine("hand: ({0}, {1}) index: {2}", icm.handIndexToHand(handIndex)[0], icm.handIndexToHand(handIndex)[1], handIndex);
                        Console.WriteLine("EV: {0}\n\n", icm.calcCall(numOfPlayers, icm.handIndexToHand(handIndex), myIndex, oppIndex, playersData, ICMs, ICMc));
                    }
                    */

                    EVresult = icm.calcCall(numOfPlayers, icm.handIndexToHand(handIndex), myIndex, oppIndex, playersData, ICMs, ICMc);
                    if (EVresult > EVgoal)
                    {
                        //Console.WriteLine(" Kasi ({0}, {1}) EV: {2}", icm.handIndexToHand(handIndex)[0], icm.handIndexToHand(handIndex)[1], icm.calcCall(numOfPlayers, icm.handIndexToHand(handIndex), myIndex, oppIndex, playersData, ICMs, ICMc));
                        break;
                    }
                }

                ranges[i] = handIndex;
                //Console.WriteLine("ranges[{0}] = {1}", i, handIndex);
                //Console.WriteLine("Lopetussindeksi = {0}", handIndex);
            }
        }

        
        private void calcMyCallRange()
        {
            // vastustajan ja oma indeksi
            int oppIndex = raiserPos;
            int myIndex = position;

            // etsit‰‰n satunnaisesti v‰lilt‰ [min,max] sopivaa k‰tt‰ ja pienennet‰‰n aluetta koko ajan
            int min = 1;
            int max = 100;    
            bool founded = false;
            while (!founded)
            {
                // alustetaan resetPlayersData, asetetaan st‰kit ja maksetut blindit
                resetPlayersData();
                for (int a = 0; a < numOfPlayers; a++)
                    playersData[a, 0] = stacks[a];
                playersData[0, 1] = BBsize;
                playersData[1, 1] = BBsize / 2;

                // vastustaja allissa
                playersData[oppIndex, 1] = playersData[oppIndex, 0];
                playersData[oppIndex, 2] = raiserRange;

                // lasketaan EV
                myRange = random.Next(min, max + 1);
                icm.handIndexToRandomHand(myRange, myHand);
                myEV = icm.calcCall(numOfPlayers, myHand, myIndex, oppIndex, playersData, ICMs, ICMc);

                if (myEV > minEV && myEV < maxEV)
                {
                    Console.WriteLine("LOYTYI! myRange: {0} myHand[0]: {1} myHand[1]: {2} EVresults: {3} min: {4} max: {5} hand[", myRange, myHand[0], myHand[1], myEV, min, max);
                    founded = true;
                    rangeError = false;
                    return;
                }
                else if (myEV < minEV)
                    max = myRange - 1;
                else if (myEV > maxEV)
                    min = myRange + 1;

                Console.WriteLine("myRange: {0} myEV: {1} min: {2} max: {3}", myRange, myEV, min, max);

                if (min > max || min == max)
                {
                    Console.WriteLine("min > max || min == max!!!");
                    founded = true;
                    rangeError = true;
                }
            }
        }


        private void calcMyPushRange()
        {
            //Console.WriteLine("2. ranges[{0}] = {1}", 1, ranges[0]);

            // oma indeksi
            int myIndex = position;

            // etsit‰‰n satunnaisesti v‰lilt‰ [min,max] sopivaa k‰tt‰ ja pienennet‰‰n aluetta koko ajan
            int min = 1;
            int max = 100;
            bool founded = false;
            while (!founded)
            {
                // alustetaan resetPlayersData sek‰ asetetaan st‰kit ja maksetut blindit
                resetPlayersData();
                for (int a = 0; a < numOfPlayers; a++)
                    playersData[a, 0] = stacks[a];
                playersData[0, 1] = BBsize;
                playersData[1, 1] = BBsize / 2;

                // vastustajien call -ranget
                for (int a = 0; a < myIndex; a++)
                    playersData[a, 2] = ranges[a];

                /*
                Console.WriteLine("ranges: {0}\t{1}\t{2}\t{3}", ranges[0], ranges[1], ranges[2], ranges[3]);
                for (int a = 0; a < 4; a++)
                    Console.WriteLine("{0}\t{1}\t{2}", playersData[a, 0], playersData[a, 1], playersData[a, 2]);
                */

                // lasketaan EV
                myRange = random.Next(min, max + 1);
                icm.handIndexToRandomHand(myRange, myHand);
                myEV = icm.calcPush(numOfPlayers, myHand, myIndex, playersData, ICMs, ICMc);

                if (myEV > minEV && myEV < maxEV)
                {
                    Console.WriteLine("LOYTYI! myRange: {0} myHand[0]: {1} myHand[1]: {2} myEV: {3} min: {4} max: {5} hand[", myRange, myHand[0], myHand[1], myEV, min, max); 
                    founded = true;
                    rangeError = false;
                    return;
                }
                else if (myEV < minEV)
                    max = myRange - 1;
                else if (myEV > maxEV)
                    min = myRange + 1;

                Console.WriteLine("myRange: {0} myEV: {1} min: {2} max: {3}", myRange, myEV, min, max);

                if (min > max || min == max)
                {
                    Console.WriteLine("min > max || min == max!!!");
                    founded = true;
                    rangeError = true;
                }
            }
        }


        // iNumPlayers: 0-3, 4=random
        // iPushOrCall: 0=push, 1=Call, 2=random
        // iDifficulty: 0=easy, 1=medium, 2=hard
        // iStartChips: 0=1000, 1=1500, 2=2000
        // iBBsize:     0-6 indeksi pelipaikan blinditaulukkoon, 7=random
        // iPosition:   0=BB, 1=SB, ..., 5=random
        public singleGame getGame(int iNumOfPlayers, int iPushOrCall, int iDifficulty, int iStartChips, int iBBsize, int iPosition, int[] iStacks)
        {
            Console.WriteLine("\n------------BEFORE-------------\n");
            Console.WriteLine("iNumOfPlayers: {0}", iNumOfPlayers);
            Console.WriteLine("iPushOrCall: {0}", iPushOrCall);
            Console.WriteLine("iDifficulty: {0}", iDifficulty);
            Console.WriteLine("iStartChips: {0}", iStartChips);
            Console.WriteLine("iBBsize: {0}", iBBsize);
            Console.WriteLine("iPosition: {0}", iPosition);
            Console.WriteLine("stacks: {0}\t{1}\t{2}\t{3}\tsum: {4}", stacks[0], stacks[1], stacks[2], stacks[3], stacks[0] + stacks[1] + stacks[2] + stacks[3]);
            
            // EV:n positiivisuudesta tehd‰‰n p‰‰tˆs vain kerran, koska +EV:t‰ saadaan laskettua
            // harvemmin laakista seuraavassa silmukassa ja siten -EV -p‰‰tˆksi‰ tulisi liikaa
            // jos t‰m‰ laskettaisiin aina uudestaan
            EVplusOrMinus();
                        
            rangeError = true;
            // alustetaan sis‰iset muuttujat saatujen parametrien perusteella
            while (rangeError)
            {
                reset();
                setInitialVariables(iNumOfPlayers, iPushOrCall, iDifficulty, iStartChips, iBBsize, iPosition);

                // st‰kkien koot satunnaisesti tai valmiista taulukosta
                if (iStacks[0] == 0)
                    calcStacks();
                else
                {
                    stacks[0] = iStacks[0];
                    stacks[1] = iStacks[1];
                    stacks[2] = iStacks[2];
                    stacks[3] = iStacks[3];
                }

                // korottajan push range
                calcRaiserRange();

                // jos ollaan push -moodissa, niin lasketaan vastustajien call -ranget
                if (pushOrCall == PUSH)
                {
                    calcOpponentsCallRanges();
                    calcMyPushRange();
                }
                else
                    calcMyCallRange();
            }
            
            if (DEBUGCONSOLE)
            {
                string[] pushOrCallStr = { "push", "call" };
                string[] positionStr = { "BB", "SB", "BTN", "UTG" };
                string[] actionStr = { "-EV", "+EV"};
                Console.WriteLine("\n------------AFTER-------------\n");
                Console.WriteLine("numOfPlayers: {0}", numOfPlayers);
                Console.WriteLine("pushOrCall: {0}", pushOrCallStr[pushOrCall]);
                Console.WriteLine("difficulty: {0}", difficulty);
                Console.WriteLine("stackSum: {0}", stackSum);
                Console.WriteLine("BBsize: {0}", BBsize);
                Console.WriteLine("position: {0}", positionStr[position]);
                Console.WriteLine("raiserPos: {0}", positionStr[raiserPos]);
                Console.WriteLine("raiserRange: {0}", raiserRange);
                Console.WriteLine("action: {0}\tminEV:{1}\tmaxEV:{2}", actionStr[action], minEV, maxEV);
                Console.WriteLine("stacks: {0}\t{1}\t{2}\t{3}\tsum: {4}", stacks[0], stacks[1], stacks[2], stacks[3], stacks[0] + stacks[1] + stacks[2] + stacks[3]);
                Console.WriteLine("ranges: {0}\t{1}\t{2}\t{3}", ranges[0], ranges[1], ranges[2], ranges[3]);
                Console.WriteLine("myRange: " + myRange);
                Console.WriteLine("myEV: " + myEV);
                Console.WriteLine("myHand: ({0}, {1})", myHand[0],myHand[1]);
            }

            return new singleGame(numOfPlayers,
                                   pushOrCall,
                                   BBsize,
                                   position,
                                   raiserPos,
                                   (int)raiserRange,
                                   action,
                                   stacks,
                                   ranges,
                                   myHand,
                                   myEV);
        }
    }
}
