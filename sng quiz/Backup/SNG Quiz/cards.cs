using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows;
using System.Windows.Forms;


namespace SNG_Quiz
{
    public enum eSUIT : int
    {
        CLUBS = 0,
        DIAMOND = 1,
        HEARTS = 2,
        SPADES = 3
    }

    public enum eRank : int
    {
        ACE = 0,
        TWO = 1,
        THREE = 2,
        FOUR = 3,
        FIVE = 4,
        SIX = 5,
        SEVEN = 6,
        EIGHT = 7,
        NINE = 8,
        TEN = 9,
        JACK = 10,
        QUEEN = 11,
        KING = 12
    }

    public enum eBACK : int
    {
        CROSSHATCH = 53, /* XP = CROSSHATCH */
        WEAVE1 = 54, /* XP = SKY */
        WEAVE2 = 55, /* XP = MINERAL */
        ROBOT = 56, /* XP = FISH */
        FLOWERS = 57, /* XP = FROG */
        VINE1 = 58, /* XP = MOONFLOWER */
        VINE2 = 59, /* XP = ISLAND */
        FISH1 = 60, /* XP = SQUARES */
        FISH2 = 61, /* XP = MAGENTA */
        SHELLS = 62, /* XP = SANDDUNES */
        CASTLE = 63, /* XP = SPACE */
        ISLAND = 64, /* XP = LINES */
        CARDHAND = 65, /* XP = TOYCARS */
        UNUSED = 66, /* XP = UNUSED */
        THE_X = 67, /* XP = THE_X */
        THE_O = 68  /* XP = THE_0 */
    }

    public class cardsdll
    {
        private readonly int[] cards = {
                12,25,38,51,
                0,13,26,39,
                1,14,27,40,
                2,15,28,41,
                3,16,29,42,
                4,17,30,43,
                5,18,31,44,
                6,19,32,45,
                7,20,33,46,
                8,21,34,47,
                9,22,35,48,
                10,23,36,49,
                11,24,37,50
            };


        [DllImport("cards.dll")]
        private static extern bool cdtInit(ref int width, ref int height);

        [DllImport("cards.dll")]
        private static extern void cdtTerm();

        [DllImport("cards.dll")]
        private static extern bool cdtDraw(IntPtr hdc, int x, int y, int card, int mode, int color);

        [DllImport("cards.dll")]
        private static extern bool cdtDrawExt(IntPtr hdc, int x, int y, int dx, int dy, int card, int mode, long color);

        [DllImport("cards.dll")]
        private static extern bool cdtAnimate(IntPtr hdc, int cardback, int x, int y, int frame);


        /* mode parameters */
        public const int mdFaceUp = 0; /* Draw card face up */
        public const int mdFaceDown = 1; /* Draw card face down */
        const int mdHilite = 2; /* Same as FaceUp except drawn inverted */
        const int mdGhost = 3; /* Draw a ghost card -- for ace piles */
        const int mdRemove = 4; /* draw background specified by color */
        const int mdInvisibleGhost = 5; /* ? */
        const int mdDeckX = 6; /* Draw X */
        const int mdDeckO = 7; /* Draw O */


        public cardsdll(int width, int height)
        {
            //int width = 60;
            //int height = 80;
            if (!cdtInit(ref width, ref height))
                throw new Exception("cards.dll did not load");
        }


        public void Dispose()
        {
            cdtTerm();
        }


        // tekee muunnoksen Windowsin ja oman korttinumeroinnin välillä
        private int toWindowsCard(int card)
        {          
            for (int i = 0; i < 52; i++)
                if (cards[i] == card)
                    return i;
                       
            return 0;
        }

        
        public bool drawCard(PictureBox box,  int card, int mode, int color)
        {
            Graphics g = box.CreateGraphics();
            IntPtr hdc = g.GetHdc();
            bool ok = cdtDraw(hdc, 0, 0, toWindowsCard(card), mode, color);
            if (!ok)
            {
                g.ReleaseHdc();
                return false;
            }
            Bitmap img = copyHDC(hdc, box.Width, box.Height);

            box.Image = img;
            g.ReleaseHdc();
            return true;
        }


        // cdtDraw(IntPtr hdc, int x, int y, int card, int mode, int color);
        public bool drawCardBack(PictureBox box)
        {
            //return drawCard(box,(int)back, mdFaceDown, 207);
            Graphics g = box.CreateGraphics();
            IntPtr hdc = g.GetHdc();
            bool ok = cdtDraw(hdc, 0, 0, (int)eBACK.CROSSHATCH, mdFaceDown, 207);
            if (!ok)
            {
                g.ReleaseHdc();
                return false;
            }
            Bitmap img = copyHDC(hdc, box.Width, box.Height);

            box.Image = img;
            g.ReleaseHdc();
            return true;
        }


        public Bitmap copyHDC(IntPtr hdcSrc, int width, int height)
        {            
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            Rectangle rect = new Rectangle(0, 0, 10, 20);  
            
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            GDI32.SelectObject(hdcDest, hBitmap);            
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, 0x00CC0020);

            Bitmap bmp = new Bitmap(Image.FromHbitmap(hBitmap),
                             Image.FromHbitmap(hBitmap).Width,
                             Image.FromHbitmap(hBitmap).Height);
            
            
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteObject(hBitmap);
            
            return bmp;
        }


        class GDI32
        {
            [DllImport("GDI32.dll")]
            public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest,
                                             int nWidth, int nHeight, IntPtr hdcSrc,
                                             int nXSrc, int nYSrc, int dwRop);
            [DllImport("GDI32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth,
                                                            int nHeight);
            [DllImport("GDI32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
            [DllImport("GDI32.dll")]
            public static extern bool DeleteDC(IntPtr hdc);
            [DllImport("GDI32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("GDI32.dll")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
