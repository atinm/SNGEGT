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
using System.Threading;
using System.Windows.Forms;
using System.IO;


namespace SNGEGT
{
    class partyTableReader : PokerTable
    {

        private int[][] CardPos = {
			new int [] {519,36,536,38},
			new int [] {653,66,670,68},     
			new int [] {686,211,703,213},   
			new int [] {662,326,679,328}, 
			new int [] {458,363,475,365}, 
			new int [] {212,365,229,367}, 
			new int [] {21,312,38,314},  
			new int [] {9,206,26,208}, 
			new int [] {31,75,48,77},  
			new int [] {222,61,239,66} 
		};

        private Color StackColor = Color.FromArgb(255, 204, 0, 0);
        private Color BetColor = Color.FromArgb(255, 255, 255, 255);
        protected Color BorderColor = Color.FromArgb(255, 255, 215, 0);
        private IntPtr foldhwnd;
        

        public partyTableReader(List <BlindInfo> blindstruct,IntPtr ihwnd): base(ihwnd)
        {
            
            blindStruct = blindstruct;
            

            foldhwnd = new IntPtr(-1);
            casinoname = "Party";
            if (ihwnd.ToInt64() > 0)
            {
                StringBuilder builder = new StringBuilder(255);
                User32.GetWindowText(ihwnd, builder, 250);
                String[] splitted = builder.ToString().Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);


                if (splitted[0] == "Speed")
                    casinoname = "Party speed";
                else if (splitted[0] == "Turbo")
                    casinoname = "Party Turbo";

                tablename = splitted[1];
            }
        }





        protected override void takeScreenShot()
        {

            Rectangle rect = new Rectangle();
            
            //Minimize and restore because of partys bugs
            if (firsttry)
            {
                User32.ShowWindow(hwnd, 6);
                User32.ShowWindow(hwnd, 9);
            }

            uint flag = 0;
            if (User32.SupportPrintWindow())
                flag = 6; //no zorder no move

            int height = 579;

            User32.SetWindowPos(hwnd, new IntPtr(-1), 300, 0, 796, height, flag);
            User32.GetWindowRect(hwnd, ref rect);
            Thread.Sleep(10);
           

            // give some time to draw graphics
            Thread.Sleep(50);

            // wait until we can read all players stack. Rereading is done if infobox is found
            img.ScreenShot(hwnd);
            int trycount = 0;
            int breakcount = 0;
            for (int i = 0; i < 10; i++)
            {

                if (GetInfoBoxLoc(i).Width != 0 && getStackSize(i) == -1)
                {
                    i--;
                    trycount++;
                    if (trycount > 10)
                    {
                        Debug.WriteLine("Stack reading fails");
                        breakcount++;
                        if (breakcount > 2)
                            return;

                        continue;
                    }
                    if (!User32.SupportPrintWindow())
                        User32.ShowWindow(hwnd, 9);
                    Thread.Sleep(100);
                    img.ScreenShot(hwnd);
                }
                else
                    trycount = 0;
            }


        }



        // game type 10 max or 6 max
        protected override int getPlayerCount()
        {           
            
            if (data.moneysum() > 12000)
                return 10;

            else return 6;
        }

        // Dealer position 0...9
        protected override int getDealerPos()
        {
           

            int[,] checkpos =  
                {{555,131},
				{649,180},
				{674,268},
				{618,349},
				{448,362},
				{243,355},
				{145,306},
				{130,223},
				{213,147},
				{328,162}
			};

            Color checkcolor1 = Color.FromArgb(255, 255, 51, 51); // ennen 2008-08-01
            Color checkcolor2 = Color.FromArgb(255, 255, 48, 49);
            for (int i = 0; i < 10; i++)
            if (img.check_pixel(new Point(checkpos[i, 0], checkpos[i, 1]), checkcolor1) || img.check_pixel(new Point(checkpos[i, 0], checkpos[i, 1]), checkcolor2))
                //if (img.check_pixel(new Point(checkpos[i, 0], checkpos[i, 1]), checkcolor1))
                {
                    Console.WriteLine("Jakaka paikassa: " + i);
                    return i;
                }
            return -1;
        }





        // recognices one card by using cardHashs table
        private int recognizeCard(int baseX, int baseY)
        {
            int[] cardsHash = new int[49];
            int counter = 0;

            int x = 6;
            for (int y = 11; y < 30; y++)
            {
                cardsHash[counter] = img.isPixelWhite(baseX + x, baseY + y);
                counter++;
            }

            x = 11;
            for (int y = 11; y < 30; y++)
            {
                cardsHash[counter] = img.isPixelWhite(baseX + x, baseY + y);
                counter++;
            }

            x = 16;
            for (int y = 11; y < 21; y++)
            {
                cardsHash[counter] = img.isPixelWhite(baseX + x, baseY + y);
                counter++;
            }

            // etsitään äsken laskettu sarja cardHashs -taulukosta
            for (int i = 0; i < 78; i++)
            {
                int index = 0;
                for (index = 0; index < 48; index++)
                {
                    if (cardsHash[index] != cardHashs[i, index + 1])
                        break;
                }
                if (index == 48)
                    return cardHashs[i, 0];
            }

            return -1;
        }



        // handcards
        private void getHandCards()
        {
            data.myPos = -1;
            for (int i = 0; i < CardPos.GetLength(0); i++)
            {
                //Search for white pixel couple of pixel away from top left corner of card
                if (img.check_pixel(new Point(CardPos[i][0] + 5, CardPos[i][1] + 2), Color.White))
                {                    
                    data.handCards[0] = recognizeCard(CardPos[i][0], CardPos[i][1]);
                }

                if (data.handCards[0] != -1)
                {
                    data.myPos = i;
                    //Second card is 2 pixels lower and 17 pixels to right from first
                    data.handCards[1] = recognizeCard(CardPos[i][0]+17, CardPos[i][1]+2);
                    return;
                }
                data.handCards[0] = -1;
            }
            
        }


        // boardcards
        private void getBoardCards()
        {
            

            int[,] checkpos =  
                {{252,187},
				{311,187},
				{370,187},
				{429,187},
				{488,187}};

            
            for (int i = 0; i < 5; i++)
            {
                if (img.check_pixel(new Point(checkpos[i, 0] + 1, checkpos[i, 1] + 5),Color.White))
                    data.boardCards[i] = recognizeCard(checkpos[i,0],checkpos[i,1]);

            }
             
        }

        protected override void FillCards()
        {
            getHandCards();
            getBoardCards();
        }





        private Rectangle GetInfoBoxLoc(int playerNum)
        {
            // two first contains infobox lower left coordinates when some other player is sitting on this position
            // two last contains lower left infobox coords when this is players current position
            int[][] stackLocs = {
			    new int [] {475,103,414,58},
			    new int [] {651,158,674,184},  // 653, 127
			    new int [] {693,252,698,305},
			    new int [] {646,403,693,443},
			    new int [] {460,428,464,450},
			    new int [] {216,428,199,450},
			    new int [] {41,386,4,426},
			    new int [] {5,257,6,204},
			    new int [] {47,137,14,68},
			    new int [] {222,103,148,60},
		    };
            
            int WIDTH = 95;
            int HEIGHT = 29;

            Point checkpoint = new Point(stackLocs[playerNum][0],stackLocs[playerNum][1]);

            if (img.check_pixel(checkpoint, BorderColor))
            {
                int minX = stackLocs[playerNum][0];
                int minY = stackLocs[playerNum][1];
                //Debug.WriteLine("({0},{1})  ({2},{3})", minX + 2, minY - 31, WIDTH, HEIGHT);
                return new Rectangle(minX + 2, minY - 31, WIDTH, HEIGHT);                
            }
            else 
            {
                checkpoint.X = stackLocs[playerNum][2];
                checkpoint.Y = stackLocs[playerNum][3];
                if (img.check_pixel(checkpoint, BorderColor))
                {
                    int minX = stackLocs[playerNum][2];
                    int minY = stackLocs[playerNum][3];
                    //Debug.WriteLine("({0},{1})  ({2},{3})", minX + 2, minY - 31, WIDTH, HEIGHT);
                    return new Rectangle(minX + 2, minY - 31, WIDTH, HEIGHT);                
                }
                
            }
            return new Rectangle(0, 0, 0, 0);
        }


        public override int getStackSize(int PlayerNum)
        {
            Rectangle Stackrect = GetInfoBoxLoc(PlayerNum);
            //Debug.WriteLine("Haetaan stackin koko" + Convert.ToString(PlayerNum));

            if (Stackrect.Width == 0)
                return -1;

            int x = Stackrect.X;
            int y = Stackrect.Y;
            int endy = Stackrect.Y + Stackrect.Height;

            Rectangle testrect = new Rectangle(Stackrect.X + 20,Stackrect.Bottom-12, 70, 12);
            //This is needed because of antialision if cleartype is used
            img.ChangeLikePixels(testrect, StackColor, Color.Yellow, 100);
            //img.saveImage();
            String tulos = img.OCR(testrect, Color.Yellow);


            try
            {
                return Convert.ToInt32(tulos);
            }
            catch (FormatException)
            {
                return -1;
            }
            
        }


        protected override int getBetSize(int playernumber)
        {

            Rectangle[] paikat = {
				new Rectangle(465,173,50,12), 
				new Rectangle(580,213,70,12), 
				new Rectangle(625,265,50,12),
				new Rectangle(605,326,50,12), 
				new Rectangle(422,348,50,12),
				new Rectangle(285,359,50,12),
				new Rectangle(160,329,50,12),
				new Rectangle(135,270,50,12),
				new Rectangle(145,199,50,12),
				new Rectangle(230,174,50,18)
			};

            Rectangle betplace = paikat[playernumber];
            
            //This is needed because of antialision if cleartype is used
            img.ChangeLikePixels(betplace, Color.White, Color.White, 50);
            img.saveImage();
            if (playernumber == 3)                        
            {
                img.ChangeLikePixels(betplace, Color.FromArgb(255, 185, 255, 255), Color.White, 10);
            }
            
            String tulos = img.OCR(betplace, BetColor);
            //With party 3 is sometimes recogniced as j
            tulos = tulos.Replace('j', '3');
     
            try
            {
                //img.saveImage();
                return Convert.ToInt32(tulos);
            }
            catch (FormatException)
            {
                return -1;
            }            
        }


        private bool EnumCallBackBlind(IntPtr hWnd, IntPtr lParam)
        {
            if (data.BBindex >= 0)
                return true;

            StringBuilder builder = new StringBuilder(200);

            if (User32.GetWindowText(hWnd, builder, 200) != 0)
            {

                string caption = builder.ToString();
                if (caption.Contains("Blinds"))
                {
                    String blindtext = caption.Split(new char[] { '(', ')' })[1];
                    blindtext = blindtext.Replace("'", "");
                    blindtext = blindtext.Replace(".", "");
                    blindtext = blindtext.Replace(",", "");

                    BlindInfo blind = new BlindInfo(blindtext);
                    //BlindS§
                    for(int i=0; i<blindStruct.Count; i++)
                        if (blindStruct[i].Equals(blind))
                        {
                            data.BBindex = i;
                            break;
                        }
                }
            }
            return true;
        }



        public override bool isActive()
        {
            return true;
 
        }



        protected override void FillBlindIndex()
        {
            if (isReadFromWindow)
                User32.EnumChildWindows(hwnd, new User32.PChildCallBack(EnumCallBackBlind), new IntPtr(0));
            else
            {               
                {
                    BlindSelectionForm joo = new BlindSelectionForm();
                    data.BBindex = joo.getindex(blindStruct);
                    return;
                }
            }
        }


        protected override void EndRead()
        {
            if(data.dealerPos > -1 && (data.bets[data.dealerPos] == 1 || data.bets[data.dealerPos] == 4))
                data.bets[data.dealerPos] = -1;
        }


        // tällä taulukolla 2d.bmp == 2db.bmp ja 8d.bmp == 8db.bmp, mutta kortit samoja, joten se ei haittaa
        private static readonly short[,] cardHashs = {
            {26, 0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 2c.bmp
            {26, 0,0,0,0,0,1,1,1,1,1,1,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 2cg.bmp
            {13, 0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 2d.bmp
            {13, 0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 2db.bmp
            { 0, 0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 2h.bmp
            {39, 0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 2s.bmp
            {27, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 3c.bmp
            {27, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 3cg.bmp
            {14, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 3d.bmp
            {14, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 3db.bmp
            { 1, 0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 3h.bmp
            {40, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 3s.bmp
            {28, 0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 4c.bmp
            {28, 0,0,0,1,1,1,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 4cg.bmp
            {15, 0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 4d.bmp
            {15, 0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 4db.bmp
            { 2, 0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 4h.bmp
            {41, 0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 4s.bmp
            {29, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 5c.bmp
            {29, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 5cg.bmp
            {16, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 5d.bmp
            {16, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 5db.bmp
            { 3, 0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 5h.bmp
            {42, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 5s.bmp
            {30, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 6c.bmp
            {30, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 6cg.bmp
            {17, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 6d.bmp
            {17, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 6db.bmp
            { 4, 0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 6h.bmp
            {43, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 6s.bmp
            {31, 1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 7c.bmp
            {31, 1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 7cg.bmp
            {18, 1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 7d.bmp
            {18, 1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 7db.bmp
            { 5, 1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 7h.bmp
            {44, 1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 7s.bmp
            {32, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 8c.bmp
            {32, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 8cg.bmp
            {19, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 8d.bmp
            {19, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 8db.bmp
            { 6, 0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 8h.bmp
            {45, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 8s.bmp
            {33, 0,1,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 9c.bmp
            {33, 0,1,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 9cg.bmp
            {20, 0,1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 9d.bmp
            {20, 0,1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 9db.bmp
            { 7, 0,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 9h.bmp
            {46, 0,1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// 9s.bmp
            {34, 0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},	// Tc.bmp
            {34, 0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},	// Tcg.bmp
            {21, 0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},	// Td.bmp
            {21, 0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},	// Tdb.bmp
            { 8, 0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},	// Th.bmp
            {47, 0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},	// Ts.bmp
            {35, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Jc.bmp
            {35, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// jcg.bmp
            {22, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Jd.bmp
            {22, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// jdb.bmp
            { 9, 0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Jh.bmp
            {48, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Js.bmp
            {36, 0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},	// Qc.bmp
            {36, 0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},	// qcg.bmp
            {23, 0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},	// Qd.bmp
            {23, 0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},	// qdb.bmp
            {10, 0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},	// Qh.bmp
            {49, 0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},	// Qs.bmp
            {37, 0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Kc.bmp
            {37, 0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,1,1,1},	// kcg.bmp
            {24, 0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Kd.bmp
            {24, 0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// kdb.bmp
            {11, 0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Kh.bmp
            {50, 0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},	// Ks.bmp
            {38, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1},	// Ac.bmp
            {38, 0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1},	// acg.bmp
            {25, 0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1},	// Ad.bmp
            {25, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1},	// adb.bmp
            {12, 0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1},	// Ah.bmp
            {51, 0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1}	// As.bmp
            };

        /*
          2h/00  2d/13  2c/26  2s/39
          3h/01  3d/14  3c/27  3s/40
          4h/02  4d/15  4c/28  4s/41
          5h/03  5d/16  5c/29  5s/42
          6h/04  6d/17  6c/30  6s/43
          7h/05  7d/18  7c/31  7s/44
          8h/06  8d/19  8c/32  8s/45
          9h/07  9d/20  9c/33  9s/46
          Th/08  Td/21  Tc/34  Ts/47
          Jh/09  Jd/22  Jc/35  Js/48
          Qh/10  Qd/23  Qc/36  Qs/49
          Kh/11  Kd/24  Kc/37  Ks/50
          Ah/12  Ad/25  Ac/38  As/51
        */
        
     
        

  
 
    }
}
