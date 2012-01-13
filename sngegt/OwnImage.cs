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


//#define DEBUG

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace SNGEGT
{


    enum OCRTYPE
    {
        ENUMBER,
        ECARDNUMBER

    };

    
    class Transform
    {
        public Color refcolor;
        public Color DestColor;
        public int tolerance;

        public Transform(Color aRefColor, Color aDestColor, int atolerance)
        {
            refcolor = aRefColor;
            DestColor = aDestColor;
            tolerance = atolerance;
        }
    }


    class OwnImage
    {
        //private byte[] rgbValues;
        //public int Width;
        //System.Drawing.Imaging.BitmapData bmpData;
        private Bitmap bmp;        
        private bool loadedfromfile;                
        private int xmove;
        private List<Transform> transforms;


        public void ClearTransforms()
        {
            transforms.Clear();
        }


        public OwnImage()
        {
            transforms = new List<Transform>();
        }


        public void addTransform(Transform newtrans)
        {
            transforms.Add(newtrans);
        }


        private void Cleanup(IntPtr hwnd, IntPtr hBitmap, IntPtr hdcSrc, IntPtr hdcDest)
        {
            User32.ReleaseDC(hwnd, hdcSrc);
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteObject(hBitmap);
        }


        public void ScreenShot(IntPtr hwnd)
        {
            IntPtr hdcSrc = User32.GetWindowDC(hwnd);
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);

            Rectangle rect = new Rectangle(0, 0, 10, 20);
            User32.GetWindowRect(hwnd, ref rect);

            int width = rect.Width - rect.X;
            int height = rect.Height - rect.Y;

            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            GDI32.SelectObject(hdcDest, hBitmap);

            if (User32.SupportPrintWindow())
            {
                User32.PrintWindow(hwnd, hdcDest, 0);
            }
            else
            {
                GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, 0x00CC0020);
            }

            bmp = new Bitmap(Image.FromHbitmap(hBitmap),
                             Image.FromHbitmap(hBitmap).Width,
                             Image.FromHbitmap(hBitmap).Height);

            LoadImage("");
            saveImage();
            Cleanup(hwnd, hBitmap, hdcSrc, hdcDest);
        }


        // sama kuin ScreenShot, mutta kuvakaappuksessa ei mukana mitään reunuksia
        public void ScreenShotWithoutBorders(IntPtr hwnd)
        {
            IntPtr hdcSrc = User32.GetWindowDC(hwnd);
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);

            Rectangle rect = new Rectangle();
            User32.GetWindowRect(hwnd, ref rect);

            int width  = rect.Width - rect.X - 2 * System.Windows.Forms.SystemInformation.FrameBorderSize.Width;
            int height = rect.Height - rect.Y - (System.Windows.Forms.SystemInformation.CaptionHeight + 2 * System.Windows.Forms.SystemInformation.FrameBorderSize.Width);

            // Debug.WriteLine("ScreenShotWithoutBorders! width: " + width + " height: " + height);

            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            GDI32.SelectObject(hdcDest, hBitmap);

            int starFromX = System.Windows.Forms.SystemInformation.FrameBorderSize.Width;
            int starFromY = System.Windows.Forms.SystemInformation.CaptionHeight + System.Windows.Forms.SystemInformation.FrameBorderSize.Width;

            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, starFromX, starFromY, 0x00CC0020);

            bmp = new Bitmap(Image.FromHbitmap(hBitmap),
                             Image.FromHbitmap(hBitmap).Width,
                             Image.FromHbitmap(hBitmap).Height);

            LoadImage("");
            saveImage();
            Cleanup(hwnd, hBitmap, hdcSrc, hdcDest);            
        }


        public void ChangePixels(Rectangle rect)
        {

            int minr, maxr, ming, maxg, minb, maxb;
            int endx = rect.Right;
            int endy = rect.Bottom;
            Color pixel;
            Color refcolor,DestColor;
            int transformlength = transforms.Count;
            int tolerance;
           
            if (transformlength == 0)
                return;

            for (int x = rect.X; x < endx; x++)
            {
                for (int y = rect.Y; y < endy; y++)
                {

                    pixel = bmp.GetPixel(x, y);
                    for (int i = 0; i < transforms.Count; i++)
                    {
                        refcolor = transforms[i].refcolor;
                        tolerance = transforms[i].tolerance;
                        DestColor = transforms[i].DestColor;
                        minr = Math.Max(refcolor.R - tolerance, 0);
                        maxr = Math.Min(refcolor.R + tolerance, 257);

                        ming = Math.Max(refcolor.G - tolerance, 0);
                        maxg = Math.Min(refcolor.G + tolerance, 257);

                        minb = Math.Max(refcolor.B - tolerance - 1, 0);
                        maxb = Math.Min(refcolor.B + tolerance + 1, 257);


                        if (x == 179 && y == 332)
                        {
                        }
                        if ((pixel.R > minr && pixel.R < maxr) && (pixel.G > ming && pixel.G < maxg) && (pixel.B > minb && pixel.B < maxb))
                            bmp.SetPixel(x, y, DestColor);
                    }
                }
            }
        }
        

        public void LoadImage(string filename)
        {
            if (!filename.Equals(""))
                bmp = new Bitmap(filename);

            loadedfromfile = !filename.Equals("");
        }


        public Color GetPixel(int x, int y)
        {
            return bmp.GetPixel(x, y);
        }


        public int isPixelWhite(int x, int y)
        {
            Color white = Color.FromArgb(255, 255, 255);
            if (bmp.GetPixel(x, y).Equals(white))
                return 1;
            return 0;

        }


        public int isPixelBlack(int x, int y)
        {
            Color black = Color.FromArgb(0, 0, 0);
            if (bmp.GetPixel(x, y).Equals(black))
                return 1;
            return 0;

        }


        public bool PixelMatch(int x, int y, int r, int g, int b)
        {
            if (x < 0 || y < 0)
                Debug.WriteLine("Vakava virhe PixelMatch metodissa. x={0} y={1}", x, y);
            try
            {
                Color pixel = bmp.GetPixel(x, y);
                if (pixel.R == r && pixel.G == g && pixel.B == b)
                    return true;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return false;


        }


        public bool PixelMatch(int x, int y, Color color)
        {
            return PixelMatch(x, y, color.R, color.G, color.B);
        }


        public int GetUpPixel(Color color, Rectangle rect)
        {
            //MessageBox.Show(Convert.ToString(rect.X) +" " +Convert.ToString(rect.Y));

            for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                for (int x = rect.X; x < rect.X + rect.Width; x++)
                {
                    if (PixelMatch(x, y, color.R, color.G, color.B))                                            
                        return y;
                    
                }
            return -1;
        }


        public int GetDownPixel(Color color, Rectangle rect)
        {
            for (int y = rect.Y + rect.Height; y > rect.Y; y--)
                for (int x = rect.X; x < rect.X + rect.Width; x++)

                    if (PixelMatch(x, y, color.R, color.G, color.B))
                        return y + 1;
            return -1;
        }


        public int OCR(Color SearchColor, Rectangle rect,
            int[][] tunnisteet, int xcount, bool forcedloc)
        {
            return OCR(SearchColor, rect, tunnisteet, xcount, forcedloc,true, 0);
        }
        
        
        public String OCR(Rectangle rect, Color searchcolor)
        {
            if (bmp == null)
                return "";

            int endx = rect.Left + rect.Width;
            int endy = rect.Top + rect.Height;
            bool foundpixel = false;
            bool foundincol = false;
            int currx = rect.X;

            int startx = currx;
            int bottomy = rect.Top; //These are intentionally in wrong way :D
            int topy = rect.Bottom;
            String text = "";
            bool handled = true;
            int capcount = 0;
            bool foundfirst = false;


            for (currx = rect.X; currx < endx; currx++)
            {
                foundincol = false;
                for (int curry = rect.Y; curry < endy; curry++)
                {

                    if (check_pixel(new Point(currx, curry), searchcolor))
                    {
                        foundincol = true;
                        handled = false;

                        if (curry > bottomy)
                            bottomy = curry;
                        if (curry < topy)
                            topy = curry;
                    }
                }
                

                if (foundincol && !foundpixel)
                {
                    startx = currx;
                }

                if (!foundincol) 
                {
                    if (foundpixel)
                    {
                        text += NetworkOCR(new Rectangle(startx, topy, currx - startx, bottomy - topy), searchcolor);
                        bottomy = rect.Top;
                        topy = rect.Bottom;
                        handled = true;
                        capcount = 0;
                        foundfirst = true;
                    }
                    else if (foundfirst)
                    {
                        capcount++;
                        if (capcount == 5)
                            return text;
                    }
                }


                foundpixel = foundincol;

                
            }
            if (!handled)
            {
                text += NetworkOCR(new Rectangle(startx, topy, currx - startx, bottomy - topy), searchcolor);
            }
            
            return text;
        }
        
        
        public bool check_pixel(Point loc, Color second)
        {
            Color first = bmp.GetPixel(loc.X, loc.Y);
            if (first.R == second.R && first.B == second.B && first.G == second.G)
                return true;

            return false;
        }


        public int OCR(Color SearchColor, Rectangle rect, 
            int[][] tunnisteet, int xcount,bool forcedloc,bool movetoleft, int maxDifferences)
        {

            int endx = rect.X + rect.Width+1;
            string stack = "";
            int x = rect.X;
            int y = rect.Y;

            y = GetUpPixel(SearchColor, rect);
            
            if (y < 0)
            {
                //Debug.WriteLine("GetUpPixel = -1");
                return -1;
            }


            int endy = GetDownPixel(SearchColor, rect);

            int notfoundx = 10;
            while (!forcedloc && movetoleft && (notfoundx != 0 && x > 0))
            {
                x--;
                if (PixelMatch(x, endy - 2, SearchColor))
                    notfoundx = 10;
                else
                    notfoundx--;
            }
            bool foundcolor = false;
            if (transforms.Count != 0)
            {
                int tx = x;
                notfoundx = 20;

                while (!forcedloc && (((!foundcolor && tx < rect.Right) || notfoundx != 0) && tx < bmp.Width))
                {
                    
                    if (PixelMatch(tx, endy - 2, SearchColor))
                    {
                        notfoundx = 20;
                        foundcolor = true;
                    }
                    else
                        notfoundx--;
                    tx++;
                }

                Rectangle changerect = new Rectangle(x, y, Math.Min(tx - x + 1,bmp.Width-x), endy - y + 1);
                ChangePixels(changerect);
                //bmp.Save(@"c:\muutos.bmp");
            }
            
            
            notfoundx = 0;
            foundcolor = false;

            int i;
            bool first = true;
            //if (debug)
                Debug.WriteLine("X {0} ENDX {1} Y {2} ENDY {3}", x, endx, y, endy);      
            
            while ( x < bmp.Width  && ((x < endx && (!foundcolor || forcedloc)) || (!forcedloc &&notfoundx < 6)))
            {
                int arvo = 0;
                int laskuri = 0;
                int xlaskuri = 0;
                

                for (int jy = y; jy < endy; jy++)
                {
                    if (PixelMatch(x, jy, SearchColor))
                    {
                        foundcolor = true;
                        
                        //ChangePixels(rect, Color.FromArgb(255, 210, 255, 255), SearchColor, 8);
                        if (first)
                        {
                            laskuri = 0 + (jy - y);
                            first = false;
                        }

                        arvo += (int)Math.Pow(2, laskuri);
                        notfoundx = 0;

                    }


                    laskuri++;
                    if (xlaskuri < xcount - 1 && jy == endy - 1 && arvo != 0)
                    {

                        xlaskuri++;
                        x++;
                        jy = y;
                    }
                }

                if (arvo == 0)
                    notfoundx++;


                if (arvo != 0)
                {
                    for (i = 0; i < tunnisteet.Length; i++)
                    {
                        if (arvo == tunnisteet[i][0])
                        {

                            if (tunnisteet[i][1] > -1)
                            {
                                Debug.WriteLine("Regged {0} paikassa {1}\n\n", tunnisteet[i][1], x);
                                stack += Convert.ToString(tunnisteet[i][1]);
                            }
                            else
                                Debug.WriteLine("Regged bypassed char {0}\n\n", x);
                        //Debug.WriteLine("Tunnistettu " + (Char) (tunnisteet[i][1]) + "   " + Convert.ToString(x) +"  " +Convert.ToString(arvo));

                            x += (int)tunnisteet[i][2];
                            xmove = (int)tunnisteet[i][2];
                            break;
                        }
                    }

                    if (i == tunnisteet.Length)
                    {

                        Debug.WriteLine("Not regged!!!: {0} x kohta {1}", arvo, x);
                        if (maxDifferences != 0)
                        {
                            for (i = 0; i < tunnisteet.Length; i++)
                            {
                                int test = tunnisteet[i][0];
                                int mask = test ^ arvo;
                            
                                int founddiffer = 0;
                                for (int bit = 0; bit < 64; bit++)
                                    if (((mask >> bit) & 1)>0)
                                        founddiffer++;

                                Console.Write("Differences ");
                                Console.Write(tunnisteet[i][1]);
                                Console.Write(" ");
                                Console.Write(founddiffer);
                                Debug.WriteLine("");

                                if (founddiffer <= maxDifferences)
                                {
                                    stack += Convert.ToString(tunnisteet[i][1]);
                                    x += (int)tunnisteet[i][2];
                                    xmove = (int)tunnisteet[i][2];
                                   break;
                                }

                            }
                        }

                        if (!forcedloc && i == tunnisteet.Length)
                        {
                            Debug.WriteLine("Not forced, generating smaller regiion");
                            xmove = 0;

                            arvo = OCR(SearchColor, new Rectangle(x - xcount+1, y, xcount, endy - y+1),
                                       tunnisteet, xcount, true,movetoleft, maxDifferences);
                            
                            x += xmove;

                            if (arvo != -1)
                            {
                                for (int j = 0; j < tunnisteet.Length; j++)
                                    if (tunnisteet[j][1] == arvo)
                                    {
                                        stack += Convert.ToString(tunnisteet[j][1]);
                                        break;
                                    }
                            }
                            else if (xmove == 0)
                            {
                                Debug.WriteLine("Regocnization failed: x pos {0}", x);
                                x += 3;
                                //if (stack != "")
                                    //return Int32.Parse(stack);
                                return -1;
                                
                            }
                            Debug.WriteLine("Smaller area handling ends\n");
                        }
                    }                    
                    
                }
                x += 1;
            }

            if (stack == "")
                return -1;
            else
                return Convert.ToInt32(stack);
        }


        private LANDS RegLand(Rectangle arect, Color asearchcolor)
        {
            Rectangle rect = new Rectangle(arect.Location, arect.Size);
            if (GetUpPixel(asearchcolor,arect) == -1)
                return LANDS.EUNKNOWN;


            ImageNet net = new ImageNet(bmp);
            RouteInfo routes = net.CreateRoutes(rect, asearchcolor);

            Point CenterTop = new Point(arect.X + arect.Width / 2, rect.Top);

            bool foundfirst = false;
            bool foundcap = false;
            Point checkpoint = new Point(routes.Left.X,routes.Top.Y);
            for (; checkpoint.X < routes.Right.X; checkpoint.X++)
            {
                bool hashroute = routes.hashRoute(checkpoint, checkpoint, movedirs.all);
                if (foundfirst)
                {
                    if (foundcap && hashroute)
                        return LANDS.EHEART;

                    foundcap = !hashroute;
                }
                else
                    foundfirst = hashroute;
            }


            if (routes.hashRoute(routes.Left, new Point(routes.Top.X, -1), movedirs.upright))
            {
                if (routes.hashRoute(routes.Left, new Point(routes.Bottom.X, -1), movedirs.downright))
                    return LANDS.EDIAMOND;


            }

            int maxy = routes.Top.Y + (routes.Bottom.Y-routes.Top.Y)/2;
            int lastx = routes.Top.X;
            checkpoint = new Point(routes.Top.X,routes.Top.Y);
            for (; checkpoint.Y < maxy; checkpoint.Y++)
            {
                while (check_pixel(checkpoint, asearchcolor))
                    checkpoint.X++;
                checkpoint.X--;
                if (checkpoint.X < lastx)
                    return LANDS.ECROSS;
                lastx = checkpoint.X;

            }
            
           
            return LANDS.ESPADE;
        }


        public Card cardOcr(Rectangle aLandPos, Rectangle aValuePos, Color regColor)
        {
            LANDS land = RegLand(aLandPos, regColor);
            Card card = new Card(LANDS.EUNKNOWN, -1);
            if (land != LANDS.EUNKNOWN)
            {
                //bmp.Save(@"c:\joo.bmp");
                String value = OCR(aValuePos, regColor).ToLower();

                if (value.Trim() != "")
                {
                    card.Land = land;
                    if (value == "j")
                        value = "11";
                    else if (value == "q")
                        value = "12";
                    else if (value == "k")
                        value = "13";
                    else if (value == "a")
                        value = "14";

                    try
                    {
                        card.Value = Convert.ToInt32(value);
                    }
                    catch (FormatException)
                    {
                    }
                }
            }
            return card;
        }


        public void ChangeNonbgPixels(Rectangle changerect, Color backcolor, Color substcolor, int tolerance)
        {
            for (int x = changerect.Left; x <= changerect.Right; x++)
                for (int y = changerect.Top; y <= changerect.Bottom; y++)
                {
                    Color pixcolor = bmp.GetPixel(x, y);
                    int rDiff = Math.Abs(backcolor.R - pixcolor.R);
                    int gDiff = Math.Abs(backcolor.G - pixcolor.G);
                    int bDiff = Math.Abs(backcolor.G - pixcolor.G);
                    if (rDiff > tolerance || gDiff > tolerance || bDiff > tolerance)
                        bmp.SetPixel(x, y, substcolor);
                }
        }


        public void ChangeLikePixels(Rectangle changerect, Color origcolor, Color substcolor, int tolerance)
        {

            for (int x = changerect.Left; x <= changerect.Right; x++)
                for (int y = changerect.Top; y <= changerect.Bottom; y++)
                {
                    Color pixcolor = bmp.GetPixel(x, y);
                    int rDiff = Math.Abs(origcolor.R - pixcolor.R);
                    int gDiff = Math.Abs(origcolor.G - pixcolor.G);
                    int bDiff = Math.Abs(origcolor.G - pixcolor.G);
                    if (rDiff < tolerance && gDiff < tolerance && bDiff < tolerance)
                        bmp.SetPixel(x, y, substcolor);
                }
        }
        

        public String NetworkOCR(Rectangle aRect, Color aSearchColor)
        {

            while (aRect.Bottom >= bmp.Height)
                aRect.Height--;

   
            ImageNet net = new ImageNet(bmp);

            RouteInfo allRoutes = net.CreateRoutes(aRect, aSearchColor);
            
            Rectangle searchrect = aRect;

            int xdiffer = aRect.Width >= 6 ? 1 : 0;

            searchrect.Width = searchrect.Width / 2 - xdiffer;
            RouteInfo leftRoutes = net.CreateRoutes(searchrect, aSearchColor);
            searchrect.Height /= 2;

            Point topCenter = new Point(-1, searchrect.Bottom-1);
            Point bottomCenter = new Point(-1, searchrect.Bottom + 1);

            RouteInfo topLeft = net.CreateRoutes(searchrect, aSearchColor);
            searchrect.Y += aRect.Height / 2;
            searchrect.Height = aRect.Bottom - searchrect.Y;
            RouteInfo bottomLeft = net.CreateRoutes(searchrect, aSearchColor);

            int width = searchrect.Width;
            searchrect = aRect;
            searchrect.X += width + xdiffer;
            searchrect.Width = aRect.Right - searchrect.X;
            RouteInfo rightRoutes = net.CreateRoutes(searchrect, aSearchColor);
            searchrect.Height /= 2;
            RouteInfo topRight = net.CreateRoutes(searchrect, aSearchColor);
            searchrect.Y += aRect.Height / 2;
            searchrect.Height = aRect.Bottom - searchrect.Y;
            RouteInfo bottomRight = net.CreateRoutes(searchrect, aSearchColor);

            Rectangle rightmostrect = new Rectangle(rightRoutes.Right.X - 3, rightRoutes.Top.Y, 3, aRect.Height);
            RouteInfo rightmost = net.CreateRoutes(rightmostrect, aSearchColor);

            if (topRight == null)
            {
                if (allRoutes.hashRoute(new Point(bottomLeft.Left.X,bottomLeft.Bottom.Y), new Point(bottomRight.Bottom.X,bottomRight.Bottom.Y),movedirs.right))
                    return "1";
                return "";
            }

            if (bottomLeft != null && allRoutes.hashRoute(allRoutes.Top,allRoutes.Bottom,movedirs.down) && !bottomLeft.hashRoute(bottomLeft.Left,bottomCenter,movedirs.right|movedirs.up|movedirs.left))
            {
                return ""; // $
            }

            if (aRect.Width <= 3 && aRect.Height > 4)
            {
                if (topLeft != null && topRight != null && bottomRight != null && bottomLeft != null)
                {
                    if (allRoutes.hashRoute(bottomLeft.Left, bottomRight.Right, movedirs.right))
                        return "1";
                }
            }
            

            if (aRect.Width <= 3 || aRect.Height < 3 || aRect.Width > 15)
                return "";


            if (bottomLeft == null || bottomLeft.Bottom.Y <= bottomCenter.Y)
            {
                if (topRight.Top.Y == topLeft.Left.Y && allRoutes.hashRoute(topLeft.Top, topRight.Top, movedirs.right))
                    return "7";
            }

            if (bottomRight == null && bottomLeft == null || topLeft == null || topRight == null)
                return "";
            

            if (topLeft.Top.Y == topRight.Top.Y && !allRoutes.hashRoute(topLeft.Top, topRight.Top, movedirs.right))
            {
                if (topRight.Top.Y != topRight.Right.Y)
                    return "10";
            }


            if (leftRoutes.hashRoute(leftRoutes.Bottom, leftRoutes.Top, movedirs.all))
            {
                if (!topRight.hashRoute(topRight.Bottom, topRight.Top, movedirs.all))
                {
                    if (bottomRight.hashRoute(bottomRight.Bottom, bottomCenter, movedirs.right | movedirs.up))
                        return "6";
                    
                }
                else
                {

                    if (bottomLeft.Left.X < topLeft.Left.X && topRight.Right.X < bottomRight.Right.X &&
                        bottomLeft.Left.Y == bottomRight.Bottom.Y && 
                        !allRoutes.hashRoute(leftRoutes.Bottom, new Point(rightRoutes.Right.X, -1), movedirs.right))
                            return "A";


                    if (!rightmost.hashRoute(rightmost.Top, topCenter, movedirs.all))
                    {
                        
                        if (allRoutes.hashRoute(topLeft.Top, topRight.Top, movedirs.up | movedirs.right | movedirs.left))
                            return "6";
                        return "k";
                        
                    }


                    if (Math.Abs(bottomLeft.Left.Y-allRoutes.Bottom.Y) < 3)
                    {

                        if (allRoutes.hashRoute(bottomLeft.Left, allRoutes.Right, movedirs.right))
                        {
                            if (allRoutes.Left.Y < allRoutes.Bottom.Y)
                                return "4";
                            return "1";
                        }
                        
                    }
                    Point point = new Point(leftRoutes.Right.X, topCenter.Y-1);

                    if (allRoutes.hashRoute(allRoutes.Left, new Point(allRoutes.Right.X, -1), movedirs.right))
                        return "4";

                    if (!bottomRight.hashRoute(bottomRight.Bottom,bottomRight.Right,movedirs.right|movedirs.up)
                        || bottomRight.Bottom.Y > bottomLeft.Bottom.Y)
                        return "q";

                    Point topPoint = new Point(aRect.X, aRect.Y + 2);
                    Point bottomPoint = new Point(aRect.X, aRect.Bottom - 2);

                    Rectangle rect = new Rectangle(aRect.X, aRect.Y + 2, aRect.Width, aRect.Height - 4);
                    RouteInfo check = net.CreateRoutes(rect, aSearchColor);
                    if (check.hashRoute(check.Left,check.Right,movedirs.all))
                        return "8";

                    return "0";
                }
            }

            
            else 
            {
                if (rightmost.hashRoute(rightmost.Bottom, rightmost.Top, movedirs.all))
                {
                    //bmp.Save("C:\\joo.bmp");
                    if (rightRoutes.Top.Y == rightRoutes.Right.Y)
                    {
                        if (!topRight.hashRoute(topRight.Top,topRight.Bottom,movedirs.all))
                            return "5";
                        if (bottomLeft.Left.X == allRoutes.Left.X)
                            return "j";
                        else
                            return "7";
                    }

                    if (topLeft.hashRoute(topLeft.Left, topCenter, movedirs.all))
                    {
                        if (bottomLeft.Bottom.X - allRoutes.Left.X > 2 &&  allRoutes.hashRoute(bottomLeft.Bottom, bottomRight.Bottom, movedirs.right))
                            return "4";
                        return "9";
                    }
                    else
                    {


                        if (rightRoutes.hashRoute(rightRoutes.Bottom,new Point(rightRoutes.Right.X,-1), movedirs.right))
                            return "2";
                        return "3";
                    }

                    
                }
                else
                {
                    if (!rightRoutes.hashRoute(rightRoutes.Top, topCenter, movedirs.all))
                    {
                        return "5";
                    }
                    return "2";
                }

            }

            return "";
        }



        public string saveImage()
        {            
            bmp.Save(Path.Combine(Application.StartupPath,"table.bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
            return "table.bmp";
        }
    }
}
