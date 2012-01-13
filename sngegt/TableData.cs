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
    public enum OwnAction
    {
        ENone,EAll,ERaise,ECall,EFold
    };

    public class TableData
    {
        // these are readed from screen capture
        public int dealerPos;       // 0-9
        public int myPos;           // 0-9
        public int[] handCards;     // 0-51
        public int[] boardCards;    // 0-51
        public int[] stacks;        // 0-
        public int[] bets;          // 0-
        public int BBindex;         // indeksi 0-9
        public int tableType;       // 10=10max, 6=6max

        // ja nämä parsitaan edellisten perusteella partyTableFinder -luokassa
        public int error;           // 0=onnistui, 1=limppereitä tai ei allin kokoista bettiä, 2=ei preflopissa
        public bool mode;           // true=push, false=call
        public int players;         // 1-10
        public int handIndex;       // 0-168        
        public bool noSB;           // true jos ei pikkublindiä        
        public int BBsize;          // 40-
        public int allinIndex;      // 0-9 mistä positiosta alli vedettiin
        public String errorStr;
        public String casinoname;
        public OwnAction SelectedAction;

        public TableData()
        {
            initialize();
        }

        public void initialize()
        {
            error = 0;
            players = -1;
            myPos = -1;
            stacks = new int[10] {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
            bets = new int[10]   {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};            
            dealerPos = -1;
            handCards = new int[2] { -1, -1 };
            handIndex = -1;
            boardCards = new int[5]{-1,-1,-1,-1,-1};
            noSB = false;
            BBindex = -1;
            BBsize = -1;
            errorStr = "";
            allinIndex = 9;
            tableType = -1;
            casinoname = "";
            SelectedAction = OwnAction.ENone;
        }

        public TableData(TableData old)
        {
            initialize();
            error = old.error;
            players = old.players;
            myPos = old.myPos;
            for (int i = 0; i < 10; i++)
            {
                stacks[i] = old.stacks[i];
                bets[i] = old.bets[i];
            }
            dealerPos = old.dealerPos;
            for (int i = 0; i < 2; i++)
                handCards[i] = old.handCards[i];
            handIndex = -1;
            for (int i = 0; i < 5; i++)
                boardCards[i] = old.boardCards[i];
            noSB = old.noSB;
            BBindex = old.BBindex;
            BBsize = old.BBsize;
            errorStr = old.errorStr;
            allinIndex = old.allinIndex;
            tableType = old.tableType;
            casinoname = old.casinoname;
            SelectedAction = old.SelectedAction;

        }

        public int moneysum()
        {
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                if (stacks[i] != -1)
                    sum += stacks[i];
                if (bets[i] != -1)
                    sum += bets[i];

               
            }
            return sum;
        }
    }
}
