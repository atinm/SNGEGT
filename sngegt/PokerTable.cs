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
    abstract class PokerTable : IComparable
    {
        protected OwnImage img;
        protected TableData data;
        protected IntPtr hwnd;
        public List<BlindInfo> blindStruct;
        protected bool isReadFromWindow;
        public string tablename;
        protected string casinoname;
        protected bool firsttry;
        protected System.Windows.Forms.Timer redrawTimer;
        /*
        protected Point[][] stackpos;
        protected Color[] stackcolors;
        protected Point[] betpos;
        protected Color betcolor;
        protected int[][] dealercheck;

        protected int stackxcount;
        protected int betxcount;
        protected int stackwidth;
        protected int stackheight;
        */
        /**
         * @param name="aBlindStruct" blindi structure
         * @param name="aStackPos" stackien paikka. Koottu siten, ett‰ taulukossa on 
         * listoja mahdollisista positioista. Taulukon indeksi m‰‰r‰‰ mist‰ pelaajasta kyse.
         */

        public PokerTable(IntPtr ihwnd)
        {
            img = new OwnImage();
            data = new TableData();
            this.hwnd = ihwnd;
            if (hwnd.ToInt64() < 0)
                isReadFromWindow = false;
            casinoname = "";

            //System.IO.FileStream joo = new System.IO.FileStream(
        }
        


        // st‰kkien koot
        private void getStackSizes()
        {
            //Debug.WriteLine("Stacks:");
            for (int i = 0; i < 10; i++)
            {
                int stack = getStackSize(i);
                data.stacks[i] = stack;
                //Debug.WriteLine("{0}:\t{1}", i, data.stacks[i]);
            }
        }


        // pˆyd‰lle laitetut maksut, reissut jne.
        private void getBetSizes()
        {
            //Debug.WriteLine("Bets:");
            for (int i = 0; i < 10; i++)
            {
  
                data.bets[i] = getBetSize(i);
                //Debug.WriteLine("{0}:\t{1}", i, data.bets[i]);
            }
        }


        // p‰‰funktio tietojen hakuun
        protected void mainReader()
        {
            Debug.WriteLine("mainReader 1");
            Init();
            Debug.WriteLine("mainReader 2");
            // jakajan positio
            if (data.error != 0)
                return;
            Debug.WriteLine("mainReader 3");
            data.dealerPos = getDealerPos();
            if (data.dealerPos == -1)
                return;
            Debug.WriteLine("mainReader 4");
            FillCards();
            Debug.WriteLine("mainReader 5");
            if (isReadFromWindow)
            {
                if (data.myPos < 0)
                {
                    for (int i = 0; i < 3; i++)  // for (int i = 0; i < 5; i++)
                    {
                        Debug.WriteLine("mainReader 6");
                        Thread.Sleep(100);
                        takeScreenShot();
                        // own position is filled when cards are read
                        FillCards();
                        if (data.myPos >= 0)
                            break;
                    }
                }
            }
            
            if (isReadFromWindow)
            {
                if (data.dealerPos < 0)
                {
                    for (int i = 0; i < 3; i++)  // for (int i = 0; i < 5; i++)
                    {
                        //Debug.WriteLine("mainReader 7");
                        Thread.Sleep(100);
                        takeScreenShot();
                        data.dealerPos = getDealerPos();
                        if (data.dealerPos >= 0)
                            break;
                    }
                }
            }
            if (data.dealerPos >= 0)
                Debug.WriteLine("Dealer position: {0}", data.dealerPos);
            else
                Debug.WriteLine("Error on reading dealer position");

            Debug.WriteLine("Handcards {0},{1}", data.handCards[0], data.handCards[1]);
            Debug.WriteLine("Board cards: {0}, {1}, {2}, {3}, {4}", data.boardCards[0], data.boardCards[1], data.boardCards[2], data.boardCards[3], data.boardCards[4]);

            // st‰kkien koot
            getStackSizes();            
            Debug.WriteLine("Stacks:");
            for (int i = 0; i < 10; i++)
                Debug.WriteLine("{0}:\t{1}", i, data.stacks[i]);

            Debug.WriteLine("Haetaan betit");
            // pelaajan pˆyd‰lle laittamat rahat
            getBetSizes();
            Debug.WriteLine("Bets:");
            for (int i = 0; i < 10; i++)
            {
                //data.bets[i] = getBetSize(i);
                Debug.WriteLine("{0}:\t{1}", i, data.bets[i]);
            }
            Debug.WriteLine("Betsien haku loppuu");
                     
            FillBlindIndex();
            if (data.BBindex >= 0)
                Debug.WriteLine("BBindex: {0}", data.BBindex);
            else
                Debug.WriteLine("Error on reading blind index");

            data.tableType = getPlayerCount();
            Debug.WriteLine("TableType: {0}", data.tableType);

            EndRead();            
        }


        public void readFromWindow(TableData iData)
        {
            Debug.WriteLine("readFromWindow 1");
            if (hwnd.ToInt32() == -1)
                return;
            Debug.WriteLine("readFromWindow 2");
            isReadFromWindow = true;
            img = new OwnImage();
            data = iData;
            // otetaan ikkunan sijainti muistiin
            Rectangle oldWindowRect = new Rectangle();
            User32.GetWindowRect(hwnd, ref oldWindowRect);
            
            // minimoidaan ja suurennetaan ikkuna Partyn grafiikkavirheiden takia
            try
            {
                Debug.WriteLine("readFromWindow 3");
                firsttry = true;
                takeScreenShot();
                Debug.WriteLine("readFromWindow 4");
                firsttry = false;
                mainReader();
                Debug.WriteLine("readFromWindow 5");
            }
            finally
            {
                uint flag = 0;

                if (User32.SupportPrintWindow())
                    flag = 4; //no zorder
                    //flag = 0;

                // Pienennet‰‰n pˆyt‰ siihen mik‰ se oli ennen
                oldWindowRect.Width -= oldWindowRect.X;
                oldWindowRect.Height -= oldWindowRect.Y;
                User32.SetWindowPos(hwnd, new IntPtr(-2), oldWindowRect.X, oldWindowRect.Y, oldWindowRect.Width, oldWindowRect.Height, flag);
                AfterResize(oldWindowRect);                
            }
        }


        public virtual void AfterResize(Rectangle newrect)
        {

        }


        public void SetBlindStruct(List<BlindInfo> blindstruct)
        {
            blindStruct = blindstruct;
        }


        public void readFromImage(string imageFileName, TableData iData)
        {
            isReadFromWindow = false;
            data = iData;
            img.LoadImage(imageFileName);
            mainReader();
        }


        public override string ToString()
        {
            return casinoname +": " +tablename;
        }


        public String getCasinoName()
        {
            return casinoname;
        }


        public void LoadImage(string filename)
        {
            img.LoadImage(filename);
        }


        public IntPtr HWND
        {
            get
            {
                return hwnd;
            }
        }


        protected virtual void ConvertPixel(ref Point aLocation)
        {
            int captdiffy = System.Windows.Forms.SystemInformation.CaptionHeight - 19 + System.Windows.Forms.SystemInformation.FrameBorderSize.Height - 4;
            int captdiffx = System.Windows.Forms.SystemInformation.FrameBorderSize.Width - 4;
            aLocation.X += captdiffx;
            aLocation.Y += captdiffy;
            //System.Windows.Forms.SystemInformation.c
        }


        protected virtual void EndRead()
        {
        }

 
        public abstract int getStackSize(int PlayerNum);
        protected abstract int getBetSize(int PlayerNum);
        protected abstract void FillBlindIndex();
        protected abstract void FillCards();
        protected abstract int getDealerPos();
        protected abstract int getPlayerCount();
        protected abstract void takeScreenShot();
        public abstract bool isActive();
        protected virtual void Init()
        {
        }
        #region IComparable Members

        public int CompareTo(object obj)
        {
           return tablename.CompareTo(obj.ToString());
        }

        #endregion
    }
}
