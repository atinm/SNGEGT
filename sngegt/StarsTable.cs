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
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace SNGEGT
{
    class StarsTable : PokerTable
    {
        private Rectangle[][] possibleStacks;
        private Rectangle[][] possibleBets;
        private Rectangle[] stackrects;
        private Rectangle[] betrects;

        private Point[][] possibleDealerPos;
        private Point[] dealerpos;

        private Point[][] possibleHandPos;
        private Point[] handcardlocs;
        private int OwnPos;

        private bool changed_size;

        


        public StarsTable(List<BlindInfo> blindstruct, IntPtr ihwnd): base(ihwnd)
        {
            if (ihwnd.ToInt64() > 0)
            {
                StringBuilder data = new StringBuilder(255);
                User32.GetWindowText(ihwnd,data,250);
                tablename = data.ToString().Split(new char[] { ' ' })[1];
                casinoname = "PokerStars";
            }
            blindStruct = blindstruct;




            possibleStacks = new Rectangle[][] {

                new Rectangle[] //9 max && 6 max
                {
                    new Rectangle(600,75,50,10),
                    new Rectangle(750,130,50,10),
                    new Rectangle(740,315,50,10),
                    new Rectangle(530,378,50,10),
                    new Rectangle(400,455,50,10),
                    new Rectangle(270,378,50,10),
                    new Rectangle(60,316,50,10),
                    new Rectangle(55,130,50,10),
                    new Rectangle(200,74,50,10)
            
                }, 
                new Rectangle[] 
                {
                    new Rectangle(610,70,50,10),
                    new Rectangle(750,128,50,10),
                    new Rectangle(750,289,50,10),
                    new Rectangle(640,401,50,10),
                    new Rectangle(484,448,50,10),
                    new Rectangle(308,448,50,10),
                    new Rectangle(170,401,50,10),
                    new Rectangle(60,289,50,10),
                    new Rectangle(60,128,50,10),
                    new Rectangle(200,70,50,10)
            
                } //10 max
            };

            possibleBets = new Rectangle[][] {
                betrects = new Rectangle[]
                {
                    new Rectangle(430,155,100,20),
                    new Rectangle(540,185,100,20),
                    new Rectangle(560,245,100,20),
                    new Rectangle(450,315,100,20),
                    new Rectangle(381,310,100,20),
                    new Rectangle(250,315,100,20),
                    new Rectangle(163,245,100,20),
                    new Rectangle(187,180,100,20),
                    new Rectangle(284,150,100,20)
                },
            
                new Rectangle[]
                {
                    new Rectangle(430,150,100,20), //OK
                    new Rectangle(550,180,100,20), //Ok
                    new Rectangle(600,225,100,20), //Ok
                    new Rectangle(500,300,100,20), //Ok
                    new Rectangle(460,335,100,20), //OK
                    new Rectangle(320,335,100,20), //Ok
                    new Rectangle(250,295,100,20), //OK
                    new Rectangle(165,225,100,20), //OK
                    new Rectangle(190,180,100,20), //OK
                    new Rectangle(290,150,100,20) //OK
                },
            };

            /*
             */
            possibleDealerPos = new Point[][]
            {
                new Point[] 
                {
                    new Point(523,119),
                    new Point(663,175),
                    new Point(683,271),
                    new Point(561,345),
                    new Point(392,346),
                    new Point(244,346),
                    new Point(118,271),
                    new Point(138,175),
                    new Point(282,120)
                },
                new Point[]
                {
                    new Point(519,118), //OK
                    new Point(658,175), //OK
                    new Point(687,255), //OK
                    new Point(589,337), //OK
                    new Point(529,354), //OK
                    new Point(271,354), //OK
                    new Point(205,334), //OK
                    new Point(115,254), //OK
                    new Point(140,176), //OK
                    new Point(279,120) //OK
                }
            };

            possibleHandPos = new Point[][]
            {
                new Point[]
                {
                    new Point(501,36),
                    new Point(638,93),
                    new Point(711,221),
                    new Point(579,337),
                    new Point(371,361),
                    new Point(167,337),
                    new Point(33,221),
                    new Point(97,93),
                    new Point(243,36),
                },
                new Point[]
                {
                    new Point(500,32),//OK
                    new Point(636,89), //OK
                    new Point(714,196), //OK
                    new Point(617,307), //OK
                    new Point(459,357), //OK
                    new Point(287,357), //OK
                    new Point(124,307), //OK
                    new Point(30,196), //OK
                    new Point(102,89), //OK
                    new Point(245,32), //OK
                }
            };

            Point[][][] pos = { possibleDealerPos, possibleHandPos };
            Rectangle[][][] rect = { possibleStacks, possibleBets };
            int captdiffy = System.Windows.Forms.SystemInformation.CaptionHeight - 19 + System.Windows.Forms.SystemInformation.FrameBorderSize.Height - 4;
            int captdiffx = System.Windows.Forms.SystemInformation.FrameBorderSize.Width - 4;
            
            Debug.WriteLine("BORDERSIZE:", System.Windows.Forms.SystemInformation.BorderSize.Width);
            if (captdiffx != 0 || captdiffy != 0)
            {
                
                for (int i = 0; i <pos.Length; i++)
                    for (int j = 0; j <pos[i].Length; j++)
                        for (int k = 0; k <pos[i][j].Length; k++)
                        {
                            pos[i][j][k].X += captdiffx;
                            pos[i][j][k].Y += captdiffy;
                        }

                for (int i = 0; i <rect.Length; i++)
                    for (int j = 0; j <rect[i].Length; j++)
                        for (int k = 0; k < rect[i][j].Length; k++)
                        {
                            rect[i][j][k].X += captdiffx;
                            rect[i][j][k].Y += captdiffy;
                        }

            }

            redrawTimer = new System.Windows.Forms.Timer();
            this.redrawTimer.Interval = 500;
            this.redrawTimer.Tick += redraw_tick;
            
            
            
        }

        public override int getStackSize(int pos)
        {


            int[][] ident = {
                new int [] {290846,-1,4}, //$
                new int [] {581918,-1,4},
                new int [] {291102,-1,4},
                new int [] {59,-1,1}, //pilkku
                new int[] {508672,0,100}, //ALL-IN
                
                new int [] {523774,0,4},
                new int [] {14,1,2},
                new int [] {460546,2,4},
                new int [] {394498,3,4},
                new int [] {92,4,4},
                
                new int [] {408860,5,4},

                new int [] {130814 ,6,4},
                new int [] {917505,7,4},
                new int [] {458753,7,4},
                new int [] {523758,8,4},
                new int [] {425246,9,4}
                
            };

            int tulos = -1;
            if (pos > -1 && pos < stackrects.Length)
            {
                //Normaalisti stackki on valkoisella, mutta jos on oma paikka, niin se on musta
                tulos = img.OCR(Color.FromArgb(255, 192, 192, 192), stackrects[pos], ident, 2, false);
                if (tulos == -1)
                {
                    tulos = img.OCR(Color.FromArgb(255, 32, 32, 32), stackrects[pos], ident, 2, false);
                }
            }
 
            return tulos;
        }
        protected override int getBetSize(int PlayerNum)
        {
            int[][] betident = {
                new int [] {268067996,-1,2}, //$
                new int [] {4181070,-1,2},
                new int [] {59,-1,-1}, //pilkku

                new int [] {2129790,0,2},
                new int [] {4162316,1,1},
                new int [] {2912706,2,2},
                new int [] {2253122,3,2},
                new int [] {4472,4,3},
                new int [] {2287452,5,2},
                new int [] {2260862,6,2},
                new int [] {4159489,7,2},
                new int [] {2260854,8,2},
                new int [] {2387790,9,2}
                
                
            };


            if (PlayerNum < 0 || PlayerNum >= betrects.Length)
                return -1;

            else
                return img.OCR(Color.FromArgb(255, 255, 246, 207), betrects[PlayerNum], betident, 3, false);
        }
        protected override void FillBlindIndex()
        {

            if (hwnd.ToInt32() == -1)
            {
                BlindSelectionForm joo = new BlindSelectionForm();
                data.BBindex =  joo.getindex(blindStruct);

                return;
            }
            StringBuilder builder = new StringBuilder(255);
            User32.GetWindowText(hwnd, builder, 250);

            Match blindreg = Regex.Match(builder.ToString(), @"Blinds \$?(?<small>\d+)/\$?(?<big>\d+)\s*(Ante \$?(?<ante>\d+))?");
            if (blindreg.Success)
            {
                BlindInfo info;
                int bigblind = Convert.ToInt32(blindreg.Groups["big"].Value);
                int smallblind = Convert.ToInt32(blindreg.Groups["small"].Value);
                int ante = 0;
                if (blindreg.Groups["ante"].Value != "")
                    ante = Convert.ToInt32(blindreg.Groups["ante"].Value);
                
                info = new BlindInfo(smallblind,bigblind,ante);

                for (int i = 0; i < blindStruct.Count; i++)
                    if (blindStruct[i].Equals(info))
                    {
                        data.BBindex = i;
                        return;
                    }

            }
            data.BBindex = -1;
                
            
        }
        protected override void FillCards()
        {
            OwnPos = -1;
            int xpos;
            int secondpos;

            
            

            for (int i = 0; i < handcardlocs.Length; i++)
            {
                if (i == 9)
                { }
                xpos = handcardlocs[i].X;
                secondpos = handcardlocs[i].X+18;
                Color pixcolor = img.GetPixel(xpos + 10, handcardlocs[i].Y + 2);
                if (pixcolor.R == 255 && pixcolor.G == 255 && pixcolor.B == 255)
                {
                    xpos++;
                    secondpos = secondpos-2;
                }

                data.handCards[0] = RegCard(new Point(xpos, handcardlocs[i].Y));
                if (data.handCards[0] != -1)
                {
                    OwnPos = i;
                    data.myPos = i;
                        
                    data.handCards[1] = RegCard(new Point(secondpos, handcardlocs[i].Y + 4));
                        
                    break;
                }
                if (data.handCards[0] != -1)
                    break;
            }

            Point[] boardcardlocs = {
                new Point(272,179), 
                new Point(326,179), 
                new Point(380,179), 
                new Point(434,179), 
                new Point(488,179)
            };

            for (int i = 0; i < 5; i++)
            {
                data.boardCards[i] = RegCard(boardcardlocs[i]);
                if (data.boardCards[i] == -1)
                    break;
            }

        }


        
        protected override int getDealerPos()
        {
            Color searchcolor = Color.FromArgb(255, 179, 35, 19);
            for (int i = 0; i < possibleDealerPos.Length; i++)
            {

                for (int j = 0; j < possibleDealerPos[i].Length; j++)
                {
                    if (img.GetPixel(possibleDealerPos[i][j].X, possibleDealerPos[i][j].Y).Equals(searchcolor))
                    {
                        handcardlocs = possibleHandPos[i];
                        dealerpos = possibleDealerPos[i];
                        stackrects = possibleStacks[i];
                        betrects = possibleBets[i];
                        return j;
                    }
                }
            }
            return -1;        
        }
        protected override int getPlayerCount()
        {
            int moneysum = data.moneysum();
            if (moneysum > 30000)
                return 45;

            if (moneysum > 16000)
                return 18;

            if (moneysum > 13500)
                return 10;

            if (moneysum > 9000)
                return 9;

            return 6;
        }

        protected override void takeScreenShot()
        {
            

            if (firsttry)
            {
                
                int width = 800 + (System.Windows.Forms.SystemInformation.FrameBorderSize.Width - 4)*2;
                int height = 573 + (System.Windows.Forms.SystemInformation.CaptionHeight-19) +(System.Windows.Forms.SystemInformation.FrameBorderSize.Height-4)*2;

                // odotetaan kunnes ikkuna kohdillaan
                Rectangle rect = new Rectangle();
                User32.GetWindowRect(hwnd, ref rect);
                bool eka = true;
                User32.GetWindowRect(hwnd, ref rect);
                changed_size = (rect.Width - rect.X != width || rect.Height - rect.Y != height) && User32.IsWindowVisible(hwnd);
                uint flag = 0;
                if (User32.SupportPrintWindow())
                    flag = 6; //no zorder no move
                    
                while (eka || ((rect.Width - rect.X != width || rect.Height - rect.Y != height) && User32.IsWindowVisible(hwnd)))
                {
                    User32.SetWindowPos(hwnd, new IntPtr(-1), 0, 0, width, height, flag);
                    User32.GetWindowRect(hwnd, ref rect);
                    eka = false;
                }
                
                if (changed_size)
                {
                    //User32.SendMessage(hwnd, 274, 61488, 0);
                    
                    // vähän sleeppiä, jotta ikkunan kaikki grafiikka piirretään OK
                    //User32.SendMessage(hwnd, 274, 61488, 0);
                    changed_size = true;
                }


                rect.X = 0;
                rect.Width = 0;
                

                
                while (((rect.Width - rect.X != width || rect.Height - rect.Y != height) && User32.IsWindowVisible(hwnd)))
                {
                    User32.GetWindowRect(hwnd, ref rect);
                    Thread.Sleep(50);
                }

             

                Thread.Sleep(300);
                firsttry = false;
                AfterResize(rect);
            }
             
            
            img.ScreenShot(hwnd);

        }


        private int RegCard(Point location)
        {
            int land = -1;
            Color pixcolor;

            pixcolor = img.GetPixel(location.X + 5, location.Y + 3);

            int[][] valueident;
            Rectangle searchrect;
            int xcount;

            bool simpledeck = false;

            //Valkoinen kortti -> kaks tai neliväripakka
            if (pixcolor.R == 255 && pixcolor.G == 255 && pixcolor.B == 255)
            {
                //Maan alaosasta otettu tunniste
                int[][] landident = new int[][] {
                    new int [] {95,0,1000},
                    new int [] {31,1,1000},
                    new int [] {895,1,1000},
                    new int [] {59194,2,1000},
                    new int [] {7645,3,1000},
                    

                    
                };

                int y = location.Y + 40;
                bool foundcolor = false;
                int x;
                for (x = location.X + 6; !foundcolor && x < location.X + 10; x++)
                    for (; y > location.Y + 25; y--)
                    {

                        if (img.GetPixel(x, y).R < 230)
                        {
                            foundcolor = true;
                            break;
                        }
                    }

                while (img.GetPixel(x, y).R > 230)
                    x--;

                land = img.OCR(Color.FromArgb(255, 255, 255, 255), new Rectangle
                (x - 2, y - 2, 4, 5), landident, 3, true);

                /*
                land = img.OCR(Color.FromArgb(255, 255, 255, 255), new Rectangle
                (location.X + 4, location.Y + 30,4,5),landident, 3, true);
                */
                if (land == -1)
                    return -1;


                //Tunnistetaan neljän x akselin merkin perusteella
                xcount = 4;
                searchrect = new Rectangle(location.X + 5, location.Y + 10, 4, 4);
                valueident = new int[][] {
                    
                    
                    new int[] {1215, 2,1000 },
                    new int[] {122351,3,1000},
                    new int[] {78608,4,1000},
                    new int[] {8118,5,1000},
                    new int[] {8180,6,1000},
                    new int[] {2047,7,1000},
                    new int[] {121089,8,1000},
                    new int[] {78616,9,1000},
                    new int[] {122911,10,1000},
                    new int[] {130791,11,1000},
                    new int[] {58931,12,1000},
                    new int[] {31,13,1000},
                    new int[] {4400,14,1000}
                
                };
            }
            else
            {

                simpledeck = true;
                pixcolor = img.GetPixel(location.X + 5, location.Y + 40);


                int pixarvo = (pixcolor.R << 16) | (pixcolor.G << 8) | pixcolor.G;

                //Pikselin arvo heittelehtii
                if (pixarvo > 10300000 && pixarvo < 10400000)  //hertta
                    land = 0;
                else if (pixarvo > 2700000 && pixarvo < 3000000) //ruutu
                    land = 1;
                else if (pixarvo > 385000 && pixarvo < 3860000)  //pata
                    land = 2;
                else if (pixarvo > 5500000 && pixarvo < 6000000) //risti
                    land = 3;
                else
                    Debug.WriteLine("ARVO: {0}", pixarvo);

                valueident = new int[][] { 
                    new int[] {913, 2,1000 },
                    new int[] {113,3,1000},
                    new int[] {32766,4,1000},
                    new int[] {30,5,1000},
                    new int[] {232,6,1000},
                    new int[] {63,7,1000},
                    new int[] {7,8,1000},
                    new int[] {120,9,1000},
                    new int[] {25,10,1000},
                    new int[] {31,11,1000},
                    new int[] {4016,12,1000},
                    new int[] {630,14,1000}

                };

                searchrect = new Rectangle(location.X + 3, location.Y + 3, 10, 6);
                xcount = 5;



            }

            //maata ei tunnistettu
            if (land == -1)
                return -1;

            int value = img.OCR(Color.FromArgb(255, 255, 255, 255), searchrect, valueident, xcount, true);

            if (simpledeck )
            {
                if (value == 3 && img.GetPixel(location.X + 5, location.Y + 12).R == 253)
                    value = 13;
                if (value == 8)
                if (value == 8 && img.GetPixel(location.X + 13, location.Y + 17).R == 255)
                    value = 14;
            }

            if (value == -1)
            {
                if (simpledeck && img.OCR(Color.White, new Rectangle(location.X + 10, location.Y + 12, 3, 5), new int[][] { new int[] { 2047, 4, 1000 } }, 2, true) == 4)
                    value = 4;
                else
                    return -1;
            }


            //2 -> 0, ässä 12
            return land * 13 + (value - 2);
            //if (img.GetPixel(location.X,location.Y) == 

        }

        public override bool isActive()
        {
            return true;
        }

        public override void  AfterResize(Rectangle newrect)
        {
            User32.SendMessage(hwnd, 0x231, 0, 0);
            User32.SendMessage(hwnd, 5, 0, (newrect.Width + newrect.Height) << 0x10);
            User32.SendMessage(hwnd, 0x232, 0, 0);
            User32.SendMessage(hwnd, 8, 0, 0);
            newrect.X = 0;
            newrect.Y = 0;
            User32.RedrawWindow(hwnd, ref newrect, IntPtr.Zero, 0x701);
        }

        private void redraw_tick(object sender, EventArgs e)
        {
            if (hwnd.ToInt32() == -1)
                return;
            Rectangle rectangle1 = new Rectangle();
            User32.GetWindowRect(hwnd, ref rectangle1);
            rectangle1.Width = rectangle1.Width - rectangle1.X;
            rectangle1.Height = rectangle1.Height - rectangle1.Y;
            rectangle1.X = 0;
            rectangle1.Y = 0;

            bool tulos = User32.RedrawWindow(hwnd, ref rectangle1, IntPtr.Zero, 0x701);
            if (tulos)
                Debug.WriteLine("HERE WE GO");
        }


    }
}
