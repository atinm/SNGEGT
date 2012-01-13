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
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
namespace SNGEGT
{
	/// <summary>
	/// Description of HistoryReader.
	/// </summary>
	public class HistoryReader
	{

		public Hashtable ReadHistory(String filename)
		{
			
			//if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
			//	return;
			//openFileDialog1.FileName = @"C:\Documents and Settings\miika\My Documents\SharpDevelop Projects\HandReader\bin\Debug\Speed Double Diamond.txt";
            Stream filestream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			TextReader tr = new StreamReader(filestream);
			List<Regex> gamestarts = new List<Regex> (1);
			List<Regex> PlayerInfo = new List<Regex>(1);
			List<Regex> PlayerAction = new List<Regex> (1);
			List<Regex> HandCards = new List<Regex> (1);
			List<Regex> BoardCards = new List<Regex> (1);
			List<Regex> Blinds = new List<Regex>(1);
			List<Regex> NoBigBlind = new List<Regex>(1);
			List<Regex> Dealer = new List<Regex>(1);
			List<Regex> NoSmallBlind = new List<Regex>(1);
            List<Regex> Folds = new List<Regex>(1);
            List<Regex> tablename = new List<Regex>(1);
			
			
			String [] PlayerNames = new String[10];
			int tournamenttype = -1;
            bool blindshandled = false;
			
			//Create regular expressions which are used to parse hand histories
			gamestarts.Add(new Regex(@"#Game No : (?<gamenumber>\d+)"));
            gamestarts.Add(new Regex(@"PokerStars Game #(?<gamenumber>\d+)"));
            gamestarts.Add(new Regex(@"Full Tilt Poker Game #(?<gamenumber>\d+).* - (?<small>\d+)/(?<big>\d+) -.*Hold"));

			PlayerInfo.Add(new Regex(@"Seat (?<number>\d+): (?<name>.*?) \( ?\$?(?<stack>\d+[\d.,']*)"));

			Dealer.Add(new Regex(@"Seat #?(?<number>\d+) is the button"));
            Dealer.Add(new Regex(@"The button is in seat #(?<number>\d+)"));
			
            Blinds.Add(new Regex(@"Level[: ]*(?<level>[\dIVX]*).*\((?<small>\d*)/(?<big>\d*)-*(?<ante>\d+)*"));
			//Blinds.Add(new Regex(@"PokerStars Game.*Level.*\((?<small>\d*)/(?<big>\d*)\)"));

            BoardCards.Add(new Regex(@"\*{2,3} (Dealing )?(?<state>.*) \*{2,3} \[\s*(?<cards>.*)\s*\]"));
			HandCards.Add(new Regex(@"Dealt to (?<name>.*?)\s*\[ ?(?<cards>[^\]]*)\]"));
			
            PlayerAction.Add(new Regex(@"(?<name>.*?):?\s*(is all-In|raises|calls)[^\d]*(?<amount>\d+[\d,.']*)"));
            PlayerAction.Add(new Regex(@"(?<name>.*?):?\s*posts[^\d]*(?<amount>\d+[\d,'.]*)\]?"));
            
            NoSmallBlind.Add(new Regex("There is no sma"));
            Folds.Add(new Regex(@"(?<name>.*?):?\s*folds"));

            tablename.Add(new Regex(@"Table\s*(?<name>[^\s]*).*\(Real Money\)"));
            
			//Hashtable <String,WindowsApplication1.partyTableData) GameDatas;
	
			//This hashtable contains information about single games during tournament
			//every game is stored in TableData object
			//hand number is used as key

			Hashtable GameHashTable = new Hashtable();
			//PlayerInfo.Add(new Regex(@"^Seat .*\d+"));
			Match match;
			bool readedline = false;
			String line = "";
			bool HasSmallBlind = true;
			int BigBlind = 0;
			int SmallBlind = 0;


            
			
			while (tr.Peek() >= 0)
			{
				if (!readedline)
					line = tr.ReadLine();

				readedline = false;
				match = ListMatchTest(gamestarts,line);

				if (match == null)
					continue;


				
				SNGEGT.TableData GameData = new SNGEGT.TableData();
                GameData.casinoname = "Party";

                if (line.Contains("PokerStars"))
                {
                    Match joo = Regex.Match(line, @"(Trny:|Tournament).* Level[: ]*(?<level>[\dIVX]*).*\((?<small>\d*)/(?<big>\d*)-*(?<ante>\d+)*");

                    GameData.casinoname = "PokerStars";
                    blindshandled = true;
                    GameData.noSB = true;
                }

                if (line.Contains("Full Tilt"))
                {
                    GameData.casinoname = "FullTilt";
                    int small =  Convert.ToInt32(match.Groups["small"].Value);
                    int big = Convert.ToInt32(match.Groups["big"].Value);
                    for (int i = 0; i < fulltiltblinds.Count; i++)
                        if (fulltiltblinds[i].Smallblind == small && fulltiltblinds[i].Bigblind == big)
                        {
                            GameData.BBindex = i;
                            break;
                        }
                }
				
				String gamenumber = match.Groups["gamenumber"].Value;

                GameHashTable[gamenumber] = GameData;
				Debug.WriteLine("Game " + match.Groups["gamenumber"] + " starts");
				HasSmallBlind = true;
				for (int i = 0; i < PlayerNames.Length; i++)
					PlayerNames[i] = "";

                
                bool game_blind_handled = false;
				//Contunue until end of file
				while (tr.Peek() != -1)
				{
                    
                    /*Blinds contains level group which tells current blindlevel*/
                    if (!game_blind_handled && (match = ListMatchTest(Blinds, line)) != null)
                    {
                        GameData.BBindex = RomanToNumber(match.Groups["level"].Value) - 1;
                        SmallBlind = Convert.ToInt32(match.Groups["small"].Value);
                        BigBlind = Convert.ToInt32(match.Groups["big"].Value);
                        game_blind_handled = true;
                    }

					line = tr.ReadLine();
                    

                    if (line == "Table Closed")
                        break;
					//break when new game starts, mark that we have already readed line
					//on luettu
					if ((match = ListMatchTest(gamestarts,line)) != null)
					{
						readedline = true;
						break;
					}
					
					readedline = false;
					/*playerinfo contains groups:
					name player name
					number players number
					stack players stack
					*/
                    if ((match = ListMatchTest(PlayerInfo, line)) != null)
                    {
                        if (!line.Contains("collected"))
                        {
                            String playername = match.Groups["name"].ToString();
                            int PlayerNumber = Convert.ToInt32(match.Groups["number"].Value) - 1;
                            String strstack = match.Groups["stack"].Value;
                            strstack = strstack.Replace(",", "");
                            strstack = strstack.Replace(".", "");
                            strstack = strstack.Replace("'", "");
                            int Stack = Convert.ToInt32(strstack);
                            PlayerNames[PlayerNumber] = playername;
                            GameData.stacks[PlayerNumber] = Stack;
                            Debug.WriteLine("Got player " + playername + " paikka " + Convert.ToString(PlayerNumber) + "");

                        }
                    }



                    /*Dealer contains number group, which tells dealer position*/
                    else if ((match = ListMatchTest(Dealer, line)) != null)
                    {
                        Debug.WriteLine("Dealer position " + match.Groups["number"] + "-1");
                        GameData.dealerPos = Convert.ToInt32(match.Groups["number"].Value) - 1;

                    }
                    /*When we get boadrdcards we can end processing*/
                    else if ((match = ListMatchTest(BoardCards, line)) != null)
                    {
                        Debug.WriteLine("Got boardcards");
                        break;
                        /*String state = match.Groups["state"].Value;
                        if (state == "flop")
                            for (int i = 0; i < 3 i++)
							
                        Debug.WriteLine(match.Groups["state"] +" " +match.Groups["cards"].Value +"");
                        */
                    }
                    /*HandCards contains groups:
                      cards: handcards
                      name: players own name
                     */
                    else if ((match = ListMatchTest(HandCards, line)) != null)
                    {
                        /*Get numerical values for cards*/

                        int[] cards = handlecards(match.Groups["cards"].Value);
                        GameData.handCards[0] = cards[0];
                        GameData.handCards[1] = cards[1];

                        Debug.WriteLine("Players cards " + match.Groups["cards"].Value + "");
                        //Haetaan pelaajan positio nimen perusteella
                        for (int i = 0; i < 10; i++)
                        {
                            if (match.Groups["name"].Value == PlayerNames[i])
                            {
                                GameData.myPos = i;
                                break;
                            }

                        }
                    }

                    /*Playeraction handles ante, call, raise and all-in situations
                     * at Stars also small and big blind
                     * contains amount group, which contains bet size
                     */
                    else if ((match = ListMatchTest(PlayerAction, line)) != null)
                    {
                        String stramount = match.Groups["amount"].Value;
                        stramount = stramount.Replace(".", "");
                        stramount = stramount.Replace(",", "");
                        stramount = stramount.Replace("'", "");


                        int amount = Convert.ToInt32(stramount);




                        //Search player and add amount to its bet
                        int i;
                        for (i = 0; i < 10; i++)
                        {


                            //Search index of player who made bet
                            if (match.Groups["name"].Value == PlayerNames[i])
                            {

                                Match raisematch = Regex.Match(line, @" raises .*\s*to\s*(?<amount>\d+)");
                                if (raisematch.Success)
                                    amount = Convert.ToInt32(raisematch.Groups["amount"].Value) - GameData.bets[i];


                                if (!line.Contains("posts") && i == GameData.myPos)
                                {
                                    if (line.Contains("all-"))
                                        GameData.SelectedAction = OwnAction.EAll;
                                    else if (line.Contains("raise"))
                                        GameData.SelectedAction = OwnAction.ERaise;
                                    else if (line.Contains("call"))
                                        GameData.SelectedAction = OwnAction.ECall;
                                    break;
                                }

                                if (!line.Contains("posts the ante"))
                                    GameData.bets[i] = Math.Max(0, GameData.bets[i]) + amount;

                                if (line.Contains("posts small"))
                                    GameData.noSB = false;



                                GameData.stacks[i] -= amount;


                                break;
                            }

                        }
                        Debug.WriteLine(match.Groups["amount"].ToString() + "");


                        if (i == GameData.myPos)
                            break;
                    }
                    else if ((match = ListMatchTest(Folds, line)) != null)
                    {
                        string name = match.Groups["name"].Value;
                        int i;
                        for (i = 0; i < 10; i++)
                            if (name == PlayerNames[i])
                                break;

                        if (i == GameData.myPos)
                        {
                            GameData.SelectedAction = OwnAction.EFold;
                            break;
                        }

                    }
                    else if ((match = ListMatchTest(NoSmallBlind, line)) != null)
                        HasSmallBlind = false;

                    else if ((match = ListMatchTest(tablename, line)) != null)
                        if (match.Groups["name"].Value.Equals("Speed"))
                        {
                            GameData.casinoname = "Party speed";
                        }
                        else if (match.Groups["name"].Value.Equals("Turbo"))
                        {
                            GameData.casinoname = "Party Turbo";
                        }




								
				} //inner while(tr.peek())


				
				int playercount = 0;
				bool [] IsPlaying = new bool[10];
				for (int i = 0;i < 10; i++)
					IsPlaying[i] = false;
			
	            
				//Search players who are still playing
				
				for (int i = 0; i < 10; i++)
				{
					if (GameData.stacks[i] >= 0)
					{
						//GameData.stacks[i] = Math.Max(0,GameData.stacks[i]-ante);
						IsPlaying[i] = true;
						playercount++;
						
					}
				}

                
				
				//Something is really wrong if there is less than two players
                if (playercount < 2 || GameData.handCards[0] == -1 || GameData.handCards[1] == -1)
                {
                    GameHashTable.Remove(gamenumber);
                    continue;
                }

                if (tournamenttype == -1)
                {
                    tournamenttype = playercount;
                }

                GameData.tableType = tournamenttype;
                
                GameData.players = playercount;
				//Lisätään blindit betsiin
				int found = 0;
				int currentpos = GameData.dealerPos;
				
				//If we are in headsup start searching from player who isn't dealer				
				if (playercount == 2)
				{
					for (int i = 0; i < 10; i++)
					{
						if ((i != currentpos) && (IsPlaying[i]))
						{
							currentpos = i;
							break;
						}
					}
				}
				
			
			
				while(found < 2 && !blindshandled) //Search atleast 2 players
				{
					while(found < 2 && currentpos < 9)
					{
						currentpos++;
						if (IsPlaying[currentpos])
						{
							found++;
							//Eka löytynyt pelaaja on Smallblind, jota ei välttämättä ole
							if (found == 1 && HasSmallBlind)
							{

								GameData.bets[currentpos] = Math.Max(0,GameData.bets[currentpos]) + Math.Min(SmallBlind, GameData.stacks[currentpos]);
                                GameData.stacks[currentpos] -= Math.Min(SmallBlind, GameData.stacks[currentpos]);
								Debug.WriteLine("SmallBlindin paikka " +Convert.ToString(currentpos) +"");
							}
                            else if (found == 2)
                            {

                                GameData.bets[currentpos] = Math.Max(0,GameData.bets[currentpos])  + Math.Min(BigBlind, GameData.stacks[currentpos]);
                                GameData.stacks[currentpos] -= Math.Min(BigBlind, GameData.stacks[currentpos]);
                            }
						}		
					}
					currentpos=-1;
				}

				
			} //Outer while(tr.Peek)
            tr.Close();
            filestream.Close();
			return GameHashTable;
			
		}
		
		private int [] handlecards(String cardstr)
		{
			//Splitataan pilkuilla ja välilyönneillä poistaen tyhjät
			String [] cards =  cardstr.Split(new char[] {',',' '}, StringSplitOptions.RemoveEmptyEntries);
			int [] result = new int[cards.Length];
			
			String maat = "HDCS";
			String arvot = "0023456789TJQKA";
			//Calculate value for cards, hearth 2 = 0, spade of hearts = 12
			for (int i = 0; i < cards.Length; i++)
			{
				cards[i] = cards[i].ToUpper();
				result[i] = maat.IndexOf(cards[i][1])*13 + arvot.IndexOf(cards[i][0])-2;
			}
			return result;
		}
		
		
		private Match ListMatchTest(List<Regex> patterns, String input)
		{
			Match match = null;
			
			//Käydään saadut patternit läpi ja palautetaan
			//matchi heti jos löytyy
			for (int i = 0; i < patterns.Count; i++)
			{
				match = patterns[i].Match(input);
				if (match.Success)
					return match;
			}
			return null;
		}
		

        private int RomanToNumber(String roman)
        {
            String[] table = new String[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };
            try
            {
                return Convert.ToInt32(roman);
            }
            catch (FormatException /*exp*/)
            {
                for (int i = 0; i < table.Length; i++)
                    if (table[i] == roman)
                        return i+1;
            }
            return 0;

        }


     
        public List<BlindInfo> fulltiltblinds;
		
	}
	
}
