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
using System.Threading;


namespace SNGEGT
{
    class FullTiltTable : PokerTable
    {
        

        public FullTiltTable(List<BlindInfo> blindstruct, IntPtr ihwnd)
            : base(ihwnd)
        {
            if (ihwnd.ToInt64() > 0)
            {
                StringBuilder data = new StringBuilder(255);
                User32.GetWindowText(ihwnd, data, 250);
                String[] splitted = data.ToString().Split(new char[] { '(', ')' },StringSplitOptions.RemoveEmptyEntries);
                tablename = splitted[1];
                if (tablename.Equals("Turbo") || tablename.Equals("Sup Turbo"))
                    tablename = splitted[3];
                
                    
                casinoname = "FullTilt";
            }
            blindStruct = blindstruct;

            
            iFirstBoardCardLoc = new Point(266,205);
 
        }


        public int getStackSize(int aTableIndex, int aPlayerNum)
        {
            if (aTableIndex == -1 || aPlayerNum >= iPossiblestackPos[aTableIndex].Length)
                return -1;



            Size rectSize = new Size(80, 12);
            Rectangle stackrect = new Rectangle(iPossiblestackPos[aTableIndex][aPlayerNum], rectSize);
          
            if (aTableIndex % 2 == 0)
            {
                Color bordercolor = img.GetPixel(stackrect.X+10, stackrect.Y + 17);
                if ((bordercolor.R != 0 || bordercolor.G != 68 || bordercolor.B != 104)
                    && (bordercolor.R != 230 || bordercolor.G != 230  || bordercolor.B != 230))
                    return -1;
            }
            else
            {
                Color bordercolor = img.GetPixel(stackrect.X,stackrect.Y+13);

                if ((bordercolor.R != 129 || bordercolor.G != 129 || bordercolor.B != 129) && (
                    (bordercolor.R < 168 || bordercolor.R > 175) || (bordercolor.G < 168 || bordercolor.G > 175)
                    || (bordercolor.B < 168 || bordercolor.B > 175)))
                {
                    if ((iPossiblestackPos[aTableIndex].Length == 9 && aPlayerNum == 4) ||
                        (iPossiblestackPos[aTableIndex].Length == 6 && aPlayerNum == 3))
                    {
                        stackrect = new Rectangle(new Point(370, 509), rectSize);
                        bordercolor = img.GetPixel(stackrect.X, stackrect.Y + 13);

                        if ((bordercolor.R != 129 || bordercolor.G != 129 || bordercolor.B != 129) && (
                            (bordercolor.R < 168 || bordercolor.R > 175) || (bordercolor.G < 168 || bordercolor.G > 175)
                            || (bordercolor.B < 168 || bordercolor.B > 175))) 
                            return -1;

                        iHandCardLocs[aTableIndex][aPlayerNum] = new Point(366, 446);
                    }
                }                
            }


            String str;
           
            
            Color check = img.GetPixel(stackrect.X, stackrect.Y);
            if (check.R > 230 && check.G > 230 && check.B > 230)
                str = img.OCR(stackrect, Color.Black);
            else
                str = img.OCR(stackrect, Color.White);


            if (str == "")
                return -1;

            if (str.ToUpper().StartsWith("A"))
                return 0;

            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException /*ex*/)
            {
                return -1;
            }
            
        }


        public override int getStackSize(int aPlayerNum)
        {

            if (iTableTypeFound)
                return data.stacks[aPlayerNum];
            else
                return getStackSize(iTableIndex,aPlayerNum);
        }


        protected override int getBetSize(int aPlayerNum)
        {

            if (iTableTypeFound)
                return data.bets[aPlayerNum];

            Rectangle rect = new Rectangle(0, 0, KBetWidth, 9);

            if (iTableIndex == -1 || aPlayerNum >= iPossibleBetPos[iTableIndex].Length)
                return -1;

            Size rectSize = new Size(KBetWidth, 12);
            String result = img.OCR(new Rectangle(iPossibleBetPos[iTableIndex][aPlayerNum], rectSize), Color.White);

            if (result == "")
                return -1;            

            return Convert.ToInt32(result);
        }


        protected override void FillBlindIndex()
        {
            data.BBindex = -1;
            if (isReadFromWindow)
            {
                String[] caps = iWindowCaption.Split(new Char[] { '-', '/' });
                int small = Convert.ToInt32(caps[1].Trim());
                int big = Convert.ToInt32(caps[2].Trim());
                for (int i = 0; i < blindStruct.Count; i++)
                {
                    if (blindStruct[i].Bigblind == big && blindStruct[i].Smallblind == small)
                        data.BBindex = i;
                }

            }
            else
            {
                BlindSelectionForm form = new BlindSelectionForm();
                data.BBindex = form.getindex(blindStruct);
            }     
        }


        protected override void FillCards()
        {
            FillHandCards(0);

            if (data.handCards[0] == -1)
            {
                

                for (int i = iHandCardLocs[iTableIndex].Length - 1; i >= 0; i--)
                {
                    iHandCardLocs[iTableIndex][i].X += 17;
                    if (iTableIndex % 2 == 0)
                    {

                        if (iHandCardLocs[iTableIndex][i].Y < 40)
                            iHandCardLocs[iTableIndex][i].Y += 15;
                        else
                            iHandCardLocs[iTableIndex][i].Y += 29;

                    }

                    else
                    {
                        iHandCardLocs[iTableIndex][i].Y += 31;
                    }
                }


                try
                {
                    FillHandCards(0);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    for (int i = iHandCardLocs[iTableIndex].Length - 1; i >= 0; i--)
                    {
                        iHandCardLocs[iTableIndex][i].X -= 17;
                        if (iTableIndex % 2 == 0)
                        {
                            if (iHandCardLocs[iTableIndex][i].Y < 55)
                                iHandCardLocs[iTableIndex][i].Y -= 15;
                            else
                                iHandCardLocs[iTableIndex][i].Y -= 29;
                        }
                        else
                        {
                            iHandCardLocs[iTableIndex][i].Y -= 31;
                        }
                    }

                }

                if (iTableIndex % 2 != 0 && data.handCards[0] == -1)
                {
                    if (img.check_pixel(new Point(354, 417), Color.White))
                    {
                        data.handCards[0] = regCard(new Point(349, 415));
                        data.handCards[1] = regCard(new Point(399, 415));
                    }
                    if (data.handCards[0] != -1 && data.handCards[1] != -1)
                        data.myPos = getPlayerCount() == 9 ? 4 : 3;

                }

                if (img.check_pixel(new Point(iFirstBoardCardLoc.X + 2, iFirstBoardCardLoc.Y + 4), Color.White))
                    data.boardCards[0] = regCard(iFirstBoardCardLoc);
            }
        }

        
        private void FillHandCards(int aStartIndex)
        {
            if (iTableIndex == -1)
                return;

            bool foundcards = false;
            Point checkpoint = new Point(0,0);
            Color checkcolor = Color.FromArgb(255,1,1,1);
           
            int cardIndex;
            for (cardIndex = aStartIndex; cardIndex < iHandCardLocs[iTableIndex].Length && !foundcards; cardIndex++)
            {

                if (cardIndex == 2)
                {
                }
                checkpoint = new Point(iHandCardLocs[iTableIndex][cardIndex].X, iHandCardLocs[iTableIndex][cardIndex].Y);
                Color check = img.GetPixel(checkpoint.X + 4, checkpoint.Y + 35);                
                
                if (check.R < 240  || check.G < 240 || check.B < 240)
                {
                    foundcards = false;
                    continue;
                }
                if (cardIndex == 8)
                {
                }
                foundcards = true;
                for (int j = 0; j < 2; j++)
                {
                    if (img.check_pixel(checkpoint, checkcolor))
                    {
                        foundcards = false;
                        break;
                    }

                    checkpoint.Y++;
                }

                checkpoint.X += 5;
                checkpoint.Y -= 2;
                                
                for (int j = 0; j < 15 && foundcards; j++)
                {
                    Color color = img.GetPixel(checkpoint.X, checkpoint.Y);
                    if (color.R > 65 || color.G > 65 || color.B > 65)
                    {
                        foundcards = false;
                        break;
                    }
                    checkpoint.X++;
                }
                checkpoint = iHandCardLocs[iTableIndex][cardIndex];
            }

            if (foundcards)
            {
                Point[] cardPlaces = new Point[] {new Point(checkpoint.X, checkpoint.Y), new Point(0, 0) };
                
                if (img.check_pixel(new Point(checkpoint.X + 15, checkpoint.Y + 35), checkcolor))
                {                   
                    cardPlaces[1] = new Point(checkpoint.X + 15, checkpoint.Y);                    
                }
                else
                {                    
                    cardPlaces[1] = new Point(checkpoint.X + 50, checkpoint.Y);      
                }

                bool foundall = true;
                for (int i = 0; i < 2; i++)
                {
                    data.handCards[i] = regCard(cardPlaces[i]);
                    //img.saveImage();
                    if (data.handCards[i] == -1)
                    {
                        foundall = false;
                        FillHandCards(cardIndex + 1);
                    }
                }

                if (foundall)
                    data.myPos = cardIndex-1;
                
            }

        }


        private int regCard(Point aCardLoc)
        {
            Rectangle valuerect = new Rectangle(aCardLoc.X + 1, aCardLoc.Y + 3, 13, 15);
            Rectangle landrect = new Rectangle(aCardLoc.X + 1, aCardLoc.Y + 20, 13, 15);
            img.ChangeNonbgPixels(landrect, Color.White, Color.Yellow, 40);
            img.ChangeNonbgPixels(valuerect, Color.White, Color.Yellow, 40);
            Card card = img.cardOcr(landrect, valuerect, Color.Yellow);
            
            return card.IntValue;

        }


        protected override int getDealerPos()
        {
            Color checkPixel = Color.FromArgb(255,198,217,212);
            
            for (int i = 0; i < iPossibleDealerPos[iTableIndex].Length; i++)
            {

                if (img.check_pixel(iPossibleDealerPos[iTableIndex][i], checkPixel))
                {
                    return i;
                }

            }
            return -1;
        }


        protected override int getPlayerCount()
        {
            if (data.moneysum() > 9000 || data.moneysum() == 2700)
                return 9;
            return 6;
        }


        private void determineTableType()
        {
            
            int tableCount = iPossiblestackPos.GetLength(0);
            iTableTypeFound = false;
            iTableIndex = -1;
            Rectangle betrect = new Rectangle(0,0,KBetWidth,10);
            
            for (int i = 0; i < tableCount; i++)
            {
                int moneysum = 0;
                String str;
                int j = 0; 
                for (j = 0; j < iPossiblestackPos[i].Length; j++)
                { 
                    int stack = -1;
                    try
                    {
                        stack = getStackSize(i, j);
                    }
                    catch (NullReferenceException /*ex*/)
                    {
                        continue;
                    }

                    if (stack != -1)
                        moneysum += stack;
                    data.stacks[j] = stack;
                    
                    betrect.Location = iPossibleBetPos[i][j];
                    try
                    {
           
                        str = img.OCR(betrect, Color.White);
                    }
                    catch (NullReferenceException /*ex*/)
                    {
                        continue;
                    }
                                                   
                    if (str != "")
                        try
                        {

                            data.bets[j] = Convert.ToInt32(str);
                            moneysum += data.bets[j];
                        }
                        catch (FormatException)
                        {
                        }

                }
                if (moneysum == 9000 || moneysum == 13500 || moneysum == 2700 || moneysum == 1800)
                {
                    iTableIndex = i;
                    iTableTypeFound = true;
                    break;
                }
                int count = data.bets.Length;
                
                for (int k = 0; k <count; k++)
                {
                    Debug.WriteLine("Stacks & bets {0} {1}",data.stacks[k],data.bets[k]);
                    data.bets[k] = -1;
                    data.stacks[k] = -1;
                }

                Debug.WriteLine("\r\n");
            }
        }
        

        protected override void takeScreenShot()
        {
            //Debug.WriteLine("takeScreenShot 1");
            StringBuilder capt = new StringBuilder(255);
            User32.GetWindowText(hwnd, capt, 254);
            iWindowCaption = capt.ToString();
             
            int width = 802 + (System.Windows.Forms.SystemInformation.FrameBorderSize.Width - 4)*2;
            int height = 574 + (System.Windows.Forms.SystemInformation.CaptionHeight-19) +(System.Windows.Forms.SystemInformation.FrameBorderSize.Height-4)*2;

            Rectangle rect = new Rectangle();
            User32.GetWindowRect(hwnd, ref rect);
            bool first = true;
            User32.GetWindowRect(hwnd, ref rect);
            
            uint flag = 0;
            if (User32.SupportPrintWindow())
                flag = 6; //no zorder no move

            while (first || ((rect.Width - rect.X != width || rect.Height - rect.Y != height) && User32.IsWindowVisible(hwnd)))
            {
                User32.SetWindowPos(hwnd, new IntPtr(-1), 0, 0, width, height, flag);
                User32.GetWindowRect(hwnd, ref rect);
                first = false;
            }
            AfterResize(rect);
            Thread.Sleep(300);
            //img.ScreenShot(hwnd); 
            img.ScreenShotWithoutBorders(hwnd);
            //Debug.WriteLine("takeScreenShot 2");
        }


        public override bool isActive()
        {
            return true;
        }


        public override void AfterResize(Rectangle newrect)
        {
            User32.SendMessage(hwnd, 0x231, 0, 0);
            User32.SendMessage(hwnd, 5, 0, (newrect.Width + newrect.Height) << 0x10);
            User32.SendMessage(hwnd, 0x232, 0, 0);
            User32.SendMessage(hwnd, 8, 0, 0);
            newrect.X = 0;
            newrect.Y = 0;
            User32.RedrawWindow(hwnd, ref newrect, IntPtr.Zero, 0x701);
        }


        protected override void Init()
        {

            iPossibleDealerPos = new Point[][] {
                new Point [] //9 max black
                {
                    new Point(598,132),
                    new Point(656,191),
                    new Point(615,309),

                    new Point(507,370),                    
                    new Point(311,370),                    
                    new Point(231,363),

                    new Point(133,241),                                                                               
                    new Point(162,151),
                    new Point(298,131),


                },


                new Point [] // 9 max standard
                {
                    new Point(534,162),
                    new Point(618,205),
                    new Point(644,309),

                    new Point(509,390),
                    new Point(312,400),
                    new Point(254,387),
                    
                    new Point(120,286),
                    new Point(139,220),
                    new Point(279,151),
                },

            };

            iPossibleBetPos = new Point[][]
            {

               new Point[] //9 max black
               {
                   new Point(490-KBetWidth,122),
                   new Point(595-KBetWidth,172),
                   new Point(617-KBetWidth,240),

                   new Point(560,312),
                   new Point(446,317),
                   new Point(290,346),

                   new Point(200,292),
                   new Point(135-KBetWidth,198),
                   new Point(160,122)

               },
           
               new Point[] //9 max standard
               {
                   new Point(450-KBetWidth,165),
                   new Point(575-KBetWidth,203),
                   new Point(645-KBetWidth,265),

                   new Point(580,317),
                   new Point(448,364),
                   new Point(290,362),

                   new Point(174,308),
                   new Point(150,230),
                   new Point(290,177)
               },

             

            };


            iPossiblestackPos = new Point[][]
            {
                new Point [] //9 max black
                {
                    new Point(498,82),
                    new Point(652,156),
                    new Point(650,295),

                    new Point(548,419),
                    new Point(335,433),
                    new Point(118,419),

                    new Point(20,295),
                    new Point(22,156),                    
                    new Point(175,82),

                },

                new Point [] //9 max standard
                {
                new Point(495,117),
                new Point(675,190),
                new Point(710,346),

                new Point(565,453),
                new Point(365,479),
                new Point(175,453),

                new Point(20,346),
                new Point(65,190),
                new Point(240,117),
                },



            };

            iHandCardLocs = new Point[][] 
            {
                new Point [] //9 max black theme
                {
                    new Point(510,0),
                    new Point(663,60),
                    new Point(663,199),

                    new Point(559,323),
                    new Point(347,337),
                    new Point(129,323),

                    new Point(31,199),
                    new Point(31,60),
                    new Point(181,0)                    
                },

                new Point [] //9 max standard theme
                {
                    new Point(474,23),
                    new Point(654,96),
                    new Point(687,252),

                    new Point(540,359),
                    new Point(349,385),
                    new Point(152,359),

                    new Point(8,252),
                    new Point(40,96),
                    new Point(220,23)
                },

            };

            for (int i = iPossibleDealerPos.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = iPossibleDealerPos[i].Length - 1; j >= 0; j--)
                {
                    //iPossibleDealerPos[i][j].X += 2;
                    iPossibleDealerPos[i][j].Y += 7;
                }
            }


            determineTableType();
            if (iTableIndex == -1)
            {
                data.error = 9;
                data.errorStr = "Can't determine table type. All stacks must be visible.";
            }

        }


        protected override void EndRead()
        {

            int playercount = 0;
            for (int i = 0; i < data.stacks.Length; i++)
                if (data.stacks[i] != -1 || data.bets[i] != -1)
                    playercount++;

            if (playercount > 2 && data.stacks[data.dealerPos] == -1 && data.bets[data.dealerPos] == -1)
            {
                for (int i = data.dealerPos-1; i >= 0; i--)
                {
                    if (data.stacks[i] != -1 || data.bets[i] != -1)
                    {
                        data.dealerPos = i;
                        return;
                    }
                }

                for (int i = data.stacks.Length-1; i > data.dealerPos; i--)
                {
                    if (data.stacks[i] != -1 || data.bets[i] != -1)
                    {
                        data.dealerPos = i;
                        return;
                    }
                }

            }
        }


        Point[][] iPossibleDealerPos;
        Point[][] iPossiblestackPos;
        Point[][] iPossibleBetPos;
        Point[][] iHandCardLocs;
        Point iFirstBoardCardLoc;
        
        const int KBetWidth = 50;
        private String iWindowCaption;
        private int iTableIndex;
        private bool iTableTypeFound;
    }
}
