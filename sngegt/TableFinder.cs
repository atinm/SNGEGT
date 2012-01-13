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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace SNGEGT
{


    class TableFinder
    {

        public const  string partystr = "Party";
        public const string starsstr = "Stars";
        public const string fulltiltstr = "FullTilt";

        // This variable stores calculated values and returns them to main program
        private TableData iData;        

        private PokerTable reader;
        private String[] tableNames;

        private IntPtr tableHwnd;        
        private List <BlindInfo> BlindStruct;
        private SortedDictionary<String, SortedDictionary<Int64, PokerTable>> foundtables;
        
        private IntPtr StarsProcess;
        private IntPtr FulltiltProcess;

        private Dictionary <String,List <BlindInfo>> iCasinoBlinds;
        

        private Hashtable TableName2Hwnd;

        public TableFinder(PokerTable ireader)
        {
            reader = ireader;
            iData = new TableData();
            tableNames = new String[8];            
            tableHwnd = new IntPtr(-1);
            
            TableName2Hwnd = new Hashtable();
            BlindStruct = ireader.blindStruct;
            foundtables = new SortedDictionary<string,SortedDictionary<Int64,PokerTable>>();
            foundtables[partystr] = new SortedDictionary<Int64,PokerTable>();
            foundtables[starsstr] = new SortedDictionary<Int64,PokerTable>();
            foundtables[fulltiltstr] = new SortedDictionary<Int64, PokerTable>();
            iCasinoBlinds = new Dictionary<string,List<BlindInfo>>();
        }


        public void setBlinds(String casinoname,List<BlindInfo> blinds)
        {
            iCasinoBlinds[casinoname] = blinds;
        }


        private bool EnumStarsCallBack(IntPtr hwnd, IntPtr lparam)
        {
            StringBuilder builder = new StringBuilder(200);

            User32.GetWindowText(hwnd, builder, 200);
            string tableName = builder.ToString();

            return true;
        }

        private bool EnumWindowCallBack(IntPtr hwnd, IntPtr lparam)
        {
            StringBuilder builder = new StringBuilder(250);
            User32.GetWindowText(hwnd, builder, 200);
            string tableName = builder.ToString();

            IntPtr windowprocess = new IntPtr(0);
            User32.GetWindowThreadProcessId(hwnd, ref windowprocess);

            Int64 hwndvalue = hwnd.ToInt64();
          
            //Party window
            if (tableName.Contains("Good Luck") && !tableName.Contains("Poker Lobby"))
            {
                if (!foundtables[partystr].ContainsKey(hwndvalue))
                {
                    //List<BlindInfo> blinds = iCasinoBlinds[partystr];
                    foundtables[partystr][hwndvalue] = new partyTableReader(null, hwnd);
                }
            }            

            if (tableName.StartsWith("PokerStars Lobby - Logged in as"))
                StarsProcess = windowprocess;
            
            if (tableName.StartsWith("Full Tilt Poker"))
                FulltiltProcess = windowprocess;            

            if (windowprocess.ToInt64() == StarsProcess.ToInt64() && tableName.Contains("Table ") && tableName.Contains("Hold"))
            {
                if (!foundtables[starsstr].ContainsKey(hwndvalue))
                {
                    //List<BlindInfo> blinds = iCasinoBlinds[starsstr];
                    foundtables[starsstr][hwndvalue] = new StarsTable(null, hwnd);
                }
            }

            else if (windowprocess.ToInt64() == FulltiltProcess.ToInt64() && ((tableName.Contains("Sit & Go") || tableName.Contains("Sit&Go")) && tableName.Contains("Hold'em")) && !tableName.Contains("Chat") ) 
            {
                if (!foundtables[fulltiltstr].ContainsKey(hwndvalue))
                {
                    foundtables[fulltiltstr][hwndvalue] = new FullTiltTable(null, hwnd);
                }
            }
            
            return true;
        }


        // Returns tables 
        public Hashtable getTables()
        {

           
            List <Int64> todelete = new List <Int64> (5);
            foreach (SortedDictionary <Int64,PokerTable> tables in foundtables.Values)
            {
                //Remove closed windows
                foreach (Int64 hwnd in tables.Keys)
                {
                    if (!User32.IsWindow(new IntPtr(hwnd)))
                    {
                        todelete.Add(hwnd);
                    }
                }
                foreach (Int64 del in todelete)
                    tables.Remove(del);
            }
            

            
            User32.EnumWindows(new User32.PCallBack(EnumWindowCallBack),new  IntPtr(0));
            Hashtable result = new Hashtable();
            foreach (String casinoname in foundtables.Keys)
            {
                List<PokerTable> tables = new List<PokerTable>();
                foreach (PokerTable table in foundtables[casinoname].Values)
                    if (table.isActive())
                        tables.Add(table);

                result[casinoname] = tables;
            }

            return result;
                    
        }
            
        //Internal debugging function which loads specified image
        public void getTableData(ref TableData oData, string filename)
        {
            if (filename == "From oData")
                iData = new TableData(oData);
            else
                iData = new TableData();
                       
            oData = new TableData();            

            if (filename != "From oData" && filename != "")
                reader.readFromImage(filename, iData);

            parseGame(oData);
        }


        // Gets tables information to oData         
        public void getTableData(ref TableData oData, PokerTable table)
        {

            iData = new TableData();
            table.readFromWindow(iData);
  
            parseGame(oData);
        }


        // Handles game data parsing moves dealer to 0 position and checks stacks
        public void parseGame(TableData oData) 
        {
            oData.error = iData.error;
            oData.errorStr = iData.errorStr;
            if (iData.error != 0)
                return;
            Debug.WriteLine("\n************************************************\n");
            

            // Check for serious errors

            //  postflop
            if (iData.boardCards[0] >= 0 || iData.boardCards[1] >= 0)
            {
                oData.error = 1;
                oData.errorStr = "This program is only useful in preflop. Can't continue.";
                return;
            }

            // Couldn't find own position
            if (iData.myPos < 0)
            {
                oData.error = 3;
                oData.errorStr = "Couldn't find own position. Can't continue.";
                
                return;
            }

            if (iData.BBindex < 0)
            {
                oData.error = 2;
                oData.errorStr = "Couldn't find blind size. Can't continue";
                return;
            }
            
            if (iData.dealerPos < 0)
            {
                oData.error = 3;
                oData.errorStr = "Couldn't find dealer position. Can't continue.";
                return;
            }


            Debug.WriteLine("Starting point:");
            for (int i = 0; i < 10; i++)
            {
                Console.Write("{0}\t{1}\t{2}", i, iData.stacks[i], iData.bets[i]);
                if (i == iData.myPos)
                    Console.Write(" me");
                if (i == iData.dealerPos)
                    Console.Write(" dealer ");
                Debug.WriteLine("");
            }
            Debug.WriteLine("");
           
            int BBsize = blindIndextoSize(iData.BBindex);
            
            // Count player amount
            int playersCount = 0;
            for (int i = 0; i < 10; i++)
                if (iData.bets[i] > 0 || iData.stacks[i] > 0)
                    playersCount++;

            // Put players to two dimensional table, dealer at position 0
            
            int POS    = 0; // position on table 0-9            
            int STACK  = 1;
            int BET    = 2;            

            int[,] tmpPlayers = new int[playersCount, 3];
            int index = 0;
            for (int i = iData.dealerPos; i < 10; i++)
            {
                if (iData.bets[i] > 0 || iData.stacks[i] > 0)
                {
                    tmpPlayers[index, POS] = i;
                    if (iData.stacks[i] > -1)
                        tmpPlayers[index, STACK] = iData.stacks[i];
                    if (iData.bets[i] > -1)
                        tmpPlayers[index, BET] = iData.bets[i];
                    index++;
                }
            }
            if (iData.dealerPos > 0)
            {
                for (int i = 0; i < iData.dealerPos; i++)
                {
                    if (iData.bets[i] > 0 || iData.stacks[i] > 0)
                    {
                        tmpPlayers[index, POS] = i;
                        if (iData.stacks[i] > -1)
                            tmpPlayers[index, STACK] = iData.stacks[i];
                        if (iData.bets[i] > -1)
                            tmpPlayers[index, BET] = iData.bets[i];
                        index++;
                    }
                }
            }
            

            Debug.WriteLine("tmpPlayers: button first");
            for (int i = 0; i < playersCount; i++)
                Debug.WriteLine("{0}\t{1}\t{2}", tmpPlayers[i, POS], tmpPlayers[i, STACK], tmpPlayers[i, BET]);
            

            int[] oldPos = new int[10]; // Here we store old indexes
            int myNewIndex = 0;         // .. so that we can get our own index in oData table

            // two players -> small blind is button
            if (playersCount == 2)
            {
                // if we are button we are in push mode
                if (iData.dealerPos == iData.myPos)
                {
                    oData.mode = true;  // push
                    oData.stacks[0] = tmpPlayers[1, STACK] + tmpPlayers[1, BET] + anteSize(iData.BBindex); //opp
                    oData.stacks[1] = tmpPlayers[0, STACK] + tmpPlayers[0, BET] + anteSize(iData.BBindex); //me
                }
                // otherwise opponent may have gone all-in
                else
                {
                    // If opponent has gone all-in
                    if (tmpPlayers[0, STACK] == 0)
                    {
                        oData.allinIndex = 1;
                        oData.mode = false;  // call
                        oData.stacks[0] = tmpPlayers[1, STACK] + tmpPlayers[1, BET] + anteSize(iData.BBindex);
                        oData.stacks[1] = tmpPlayers[0, STACK] + tmpPlayers[0, BET] + anteSize(iData.BBindex);
                    }
                    // No all-in -> set error code
                    else
                    {
                        oData.allinIndex = 1;
                        oData.mode = false;  // call
                        oData.stacks[0] = tmpPlayers[1, STACK] + tmpPlayers[1, BET] + anteSize(iData.BBindex);
                        oData.stacks[1] = tmpPlayers[0, STACK] + tmpPlayers[0, BET] + anteSize(iData.BBindex);
                        oData.error = 11;
                        oData.errorStr = "There is less than all in sized bet. Can't continue.";
                    }
                }
            }
            
            // More than 2 players
            else
            {
                // Index 1 and 2 contains small blind and big blind
                int SBpos = 1;
                int BBpos = 2;
                bool foundBBpos = false;
                
                // Check for big blind                                
                if (tmpPlayers[BBpos, BET] > 0 && tmpPlayers[BBpos, BET] <= BBsize)
                {
                    Debug.WriteLine("Iso blindi löydettiin");
                    foundBBpos = true;
                }

                //If normal big blind hasn't paid anything, player next to dealer 
                //is only blind payer (Partys nosb)
                if (foundBBpos == false && !iData.noSB)
                {
                    BBpos = SBpos;
                    oData.noSB = true;
                }

                Debug.WriteLine("BBpos: {0} oData.noSB: {1}", BBpos, oData.noSB);

                // Put stacks and blinds to result object by goind counterclockwise                
                index = 0;
                for (int i = BBpos; i >= 0; i--)
                {
                    oldPos[index] = tmpPlayers[i, POS];
                    oData.stacks[index] = tmpPlayers[i, STACK] + tmpPlayers[i, BET] + anteSize(iData.BBindex);
                    oData.bets[index] = tmpPlayers[i, BET];
                    index++;
                }
                if (BBpos < playersCount - 1)
                {
                    for (int i = playersCount - 1; i > BBpos; i--)
                    {
                        oldPos[index] = tmpPlayers[i, POS];
                        oData.stacks[index] = tmpPlayers[i, STACK] + tmpPlayers[i, BET] + anteSize(iData.BBindex);
                        oData.bets[index] = tmpPlayers[i, BET];
                        index++;
                    }
                }

                // Search our own position from tables
                for (int i = 0; i < 10; i++)
                {
                    if (oldPos[i] == iData.myPos)
                    {
                        myNewIndex = i;                        
                        break;
                    }
                }
            }

            Debug.WriteLine("\nMy new index: {0}", myNewIndex);
            Debug.WriteLine("\noData.stacks:");
            for (int i = 0; i < playersCount; i++)
                Debug.WriteLine("{0}\t{1}\t{2}", i, oData.stacks[i], oData.bets[i]);
            

            // Set remaining table data
            if (iData.BBindex >= 0)
                oData.BBindex = iData.BBindex;
            else
                oData.BBindex = 0;
            oData.BBsize = BBsize;
            for (int i = 0; i < 5; i++)
                oData.boardCards[i] = iData.boardCards[i];
            oData.handCards[0] = iData.handCards[0];
            oData.handCards[1] = iData.handCards[1];
            oData.handIndex = indexFromHand(iData.handCards);
            oData.myPos = myNewIndex;
            oData.players = playersCount;
            oData.tableType = iData.tableType;

            
            if (playersCount > 2)
            {
                // // If we are first to act, we surely are in PUSH-mode
                if (playersCount - 1 == myNewIndex)
                    oData.mode = true;
                else
                {
                    //Go table from UTG towards our own position. If we came to our own position without all-in 
                    //we are in push mode else call

                    int i = 0;
                    int limpersOrRaisers = 0;
                    int allins = 0;
                    for (i = playersCount - 1; i > myNewIndex; i--)
                    {
                        //Opponent wen't all-in
                        if (oData.bets[i] >= 0 && oData.bets[i] + anteSize(iData.BBindex) == oData.stacks[i])
                        {
                            allins++;
                            oData.allinIndex = i;
                        }

                        // opponent went all-in but didn't go allin
                        else if (oData.bets[i] > 0 && oData.bets[i] + anteSize(iData.BBindex) < oData.stacks[i])
                        {
                            //If bet is smaller than big blind and bet maker still has stack, small blind folded                                                        
                            if (oData.bets[i] < BBsize && i == 1)
                                continue;

                            // Limping 
                            limpersOrRaisers++;
                            if(oData.allinIndex < 0)
                                oData.allinIndex = i;  // Set all-in index to better. THIS IS WRONG!!!!!!
                        }
                        
                    }
                    // limpers -> can't calculate
                    if (limpersOrRaisers > 0)
                    {
                        oData.error = 12;
                        BlindInfo blind = BlindStruct[iData.BBindex];

                        oData.errorStr = "There is limpers or less than all in sized bet. Can't continue.";
                        oData.mode = true;
                    }
                    // no limpers, but one all-in, can be calculated in call-mode
                    else if (limpersOrRaisers == 0 && allins == 1)
                    {
                        oData.mode = false;
                    }
                    // multiple all-ins can't calculate
                    else if (allins > 1)
                    {
                        oData.error = 13;
                        oData.errorStr = "There is more than one all in. Can't continue.";
                        oData.mode = false;
                    }
                    // No limpers, or all-ins  -> push mode
                    else if (allins == 0 && limpersOrRaisers == 0)
                        oData.mode = true;
                }
            }

            // Own cards not found
            if (iData.handCards[0] < 0 || iData.handCards[1] < 0)
            {
                oData.handIndex = 0; // AA oletuksena
                oData.error = 14;
                oData.errorStr = "Couldn't recognize cards. Can't continue.";
            }

            // Unable to determine blind size
            else if (iData.BBindex < 0)
            {
                oData.BBindex = 0; // 20/40 oletuksena
                oData.error = 15;
                oData.errorStr = "Couldn't find blind size. Can't continue.";             
            }

            // There has to be more than 2 players
            else if (oData.players < 2)
            {                
                oData.error = 4;
                oData.errorStr = "Couldn't find player count. Can't continue.";
            }

            // Sum of chips doesn't match
            int chipSum = 0;
            for (int i = 0; i < 10; i++)
                if (oData.stacks[i] > 0)
                    chipSum += oData.stacks[i];
           
            float chipsPerPlayer = chipSum / oData.tableType;
            if (chipsPerPlayer != 1500 && chipsPerPlayer != 2000 && chipsPerPlayer != 300)
            {
                Debug.WriteLine("oData.tableType: {0} chipSum: {1}", oData.tableType, chipSum);

                // Error is serious, if none of the stacks can be read
                if (chipSum == 0)
                {
                    oData.error = 5;
                    oData.errorStr = "Couldn't find any stack size. Can't continue.";
                }
                // ... and less serious if we got something
                else
                {
                    oData.error = 16;
                    oData.errorStr = "Couldn't find all stacks sizes. Can't continue.";
                }
            }


            // Print all calculated infos
            Debug.WriteLine("\noData.error: {0}", oData.error);
            Debug.WriteLine("oData.errorStr: {0}", oData.errorStr);
            Debug.WriteLine("oData.mode: {0}", oData.mode);
            Debug.WriteLine("oData.myPos: {0}", oData.myPos);
            Debug.WriteLine("handCards: {0} {1}", oData.handCards[0], oData.handCards[1]);
            Debug.WriteLine("handIndex: {0}", oData.handIndex);
            Debug.WriteLine("noSB: {0}", oData.noSB);
            Debug.WriteLine("BBsize: {0}", oData.BBsize);
            Debug.WriteLine("BBindex: {0}", oData.BBindex);
            Debug.WriteLine("allinIndex: {0}", oData.allinIndex);
            Debug.WriteLine("Stacks:");
            for (int i = 0; i < playersCount; i++)
                Debug.WriteLine("{0}\t{1}", i, oData.stacks[i], oData.bets[i]);
        }
        

        // antesize of given blind level
        private int anteSize(int BBindex)
        {
            
            if (BBindex > 0 && BBindex<= BlindStruct.Count)
                return BlindStruct[BBindex].Ante;
            else
                return 0;
        }

        
        //Big blind on given level
        private int blindIndextoSize(int BBindex)
        {

            if (BBindex <= BlindStruct.Count && BBindex >= 0)
                return BlindStruct[BBindex].Bigblind;
            else
                return 40;
        }


        //Sets blindstructure which should be used by reader
        public void SetBlindStructure(List <BlindInfo> blindinfos  )
        {

            BlindStruct = blindinfos;
            reader.SetBlindStruct(blindinfos);                        
        }


        // returns hand index  AA=0, 22=168
        public int indexFromHand(int[] iCards)
        {
            // Failure return AA, THIS ISN'T CORRECT!!!!!!
            if (iCards[0] < 0 || iCards[1] < 0)
                return 0;

            int[] card = { 0, 0 };

            // suited
            if (iCards[0] / 13 == iCards[1] / 13)
            {
                if (iCards[0] > iCards[1])
                    return indexArray[iCards[0] % 13, iCards[1] % 13];
                else
                    return indexArray[iCards[1] % 13, iCards[0] % 13];
            }
            // unsuited
            else
            {
                if (iCards[0] % 13 > iCards[1] % 13)
                    return indexArray[iCards[1] % 13, iCards[0] % 13];
                else
                    return indexArray[iCards[0] % 13, iCards[1] % 13];
            }
        }

        
        private static readonly short[,] indexArray = {
        {168, 167, 164, 159, 152, 143, 132, 119, 104, 87, 68, 47, 24},
        {166, 165, 162, 157, 150, 141, 130, 117, 102, 85, 66, 45, 22},
        {163, 161, 160, 155, 148, 139, 128, 115, 100, 83, 64, 43, 20},
        {158, 156, 154, 153, 146, 137, 126, 113,  98, 81, 62, 41, 18},
        {151, 149, 147, 145, 144, 135, 124, 111,  96, 79, 60, 39, 16},
        {142, 140, 138, 136, 134, 133, 122, 109,  94, 77, 58, 37, 14},
        {131, 129, 127, 125, 123, 121, 120, 107,  92, 75, 56, 35, 12},
        {118, 116, 114, 112, 110, 108, 106, 105,  90, 73, 54, 33, 10},
        {103, 101,  99,  97,  95,  93,  91,  89,  88, 71, 52, 31,  8},
        {86,   84,  82,  80,  78,  76,  74,  72,  70, 69, 50, 29,  6},
        {67,   65,  63,  61,  59,  57,  55,  53,  51, 49, 48, 27,  4},
        {46,   44,  42,  40,  38,  36,  34,  32,  30, 28, 26, 25,  2},
        {23,   21,  19,  17,  15,  13,  11,   9,   7,  5,  3,  1,  0}
        };
        
    }
}
