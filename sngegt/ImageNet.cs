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

namespace SNGEGT
{
    public enum movedirs
    {
        left = 1,
        right = 2,
        up = 4,
        down = 8,
        upleft = 16,
        upright = 32,
        downright = 64,
        all = left | right | up | down
    }

    internal class Node
    {
        private List<Node> iNeighbours;
        private Point iLocation;

        private bool iEnabled;
        private bool iCheckingNeighbours;
        public bool Enabled
        {
            get
            {
                return iEnabled;
            }
        }

        public Point Location
        {
            get
            {
                return iLocation;
            }
        }

        public Node(Point aLocation, bool aEnabled)
        {
            iEnabled = aEnabled;
            iLocation = aLocation;
            iNeighbours = new List<Node>();
            iCheckingNeighbours = false;
        }

        public bool IsRouteTo(Point aPoint, movedirs aMovedir)
        {
            
            if (aPoint.X == -1 || aPoint.X == iLocation.X)
                if (aPoint.Y == -1 || aPoint.Y == iLocation.Y)
                    return true;
            bool result = false;

            if (!iCheckingNeighbours)
            {
                iCheckingNeighbours = true;
                foreach (Node node in iNeighbours)
                {
                    if ((aMovedir & movedirs.upleft) != 0 && (node.Location.Y >= iLocation.Y || node.Location.X >= iLocation.X))
                        continue;

                    else if ((aMovedir & movedirs.upright) != 0 && (node.Location.Y >= iLocation.Y || node.Location.X <= iLocation.X))
                        continue;

                    else if ((aMovedir & movedirs.downright) != 0 && (node.Location.Y <= iLocation.Y || node.Location.X <= iLocation.X))
                        continue;
                    
                    if (aMovedir != movedirs.upleft && aMovedir != movedirs.upright && aMovedir != movedirs.downright)
                    {
                        if ((aMovedir & movedirs.left) == 0 && node.Location.X < iLocation.X)
                            continue;

                        if ((aMovedir & movedirs.right) == 0 && node.Location.X > iLocation.X)
                            continue;

                        if ((aMovedir & movedirs.up) == 0 && node.Location.Y < iLocation.Y)
                            continue;

                        if ((aMovedir & movedirs.down) == 0 && node.Location.Y > iLocation.Y)
                            continue;
                    }

                    


                    if (node.IsRouteTo(aPoint, aMovedir))
                    {
                        result = true;
                        break;
                    }
                }
                
            }
            

            return result;
        }

        public void AddNeighbour(Node node)
        {
            iNeighbours.Add(node);
        }

        
        public void TurnNetwork()
        {
            foreach (Node node in iNeighbours)
                node.AddNeighbour(this);
            iNeighbours.Clear();
        }

        internal bool CheckingRoutes
        {
            get
            {
                return iCheckingNeighbours;
            }
            set
            {
                iCheckingNeighbours = value;
            }
        }
    };

    public class RouteInfo
    {

        public bool hashRoute(Point aStartPoint, Point aEndPoint, movedirs aMovedir)
        {
            for (int i = 0; i < iRoutes.GetLength(0); i++)
                for (int j = 0; j < iRoutes.GetLength(1); j++)
                    iRoutes[i, j].CheckingRoutes = false;
            int xindex = aStartPoint.X - iSearchPoint.X;
            int yindex = aStartPoint.Y - iSearchPoint.Y;
            
            return iRoutes[xindex, yindex].Enabled && iRoutes[xindex, yindex].IsRouteTo(aEndPoint, aMovedir);
        }



        internal RouteInfo(Point aSearchPoint, Node[,] aRoutes)
        {
            iSearchPoint = aSearchPoint;
            iRoutes = aRoutes;
            left = new Point(Int32.MaxValue, -1);
            right = new Point(-1, -1);
            top = new Point(-1, Int32.MaxValue);
            bottom = new Point(-1, -1);
           

            for (int i = 0; i < iRoutes.GetLength(0); i++)
            {
                for (int j = 0; j < iRoutes.GetLength(1); j++)
                {
                    if (iRoutes[i, j].Enabled)
                    {
                        
                        int x = iSearchPoint.X + i;
                        int y = iSearchPoint.Y + j;
                        
                        if (left.X >= x)
                            left = new Point(x, y);
                        if (right.X < x )
                            right = new Point(x, y);
                        if (y < top.Y)
                            top = new Point(x, y);
                        if (y > bottom.Y)
                            bottom = new Point(x, y);
                    }
                }
            }
        }

        public Point Top
        {
            get { return top; }
        }

        public Point Left
        {
            get { return left; }
        }

        public Point Right
        {
            get { return right; }
        }

        public Point Bottom
        {
            get { return bottom; }
        }

        private Point iSearchPoint;
        private Node[,] iRoutes;
        private Point left;
        private Point top;
        private Point right;
        private Point bottom;


    }

    public class ImageNet
    {
        public ImageNet(Bitmap abmp)
        {
            ibmp = abmp;
        }

        public RouteInfo CreateRoutes(Rectangle rect, Color searchcolor)
        {
            Node [,] Routes = new Node[rect.Width + 1, rect.Height + 1];
            //First search all pixels on the area


            bool found = false;
            for (int x = rect.X, i = 0; x <= rect.Right; x++, i++)
            {
                for (int y = rect.Y, j = 0; y <= rect.Bottom; y++, j++)
                {
                    Color color = ibmp.GetPixel(x, y);
                    bool enabled = (color.R == searchcolor.R) && (color.G == searchcolor.G) && (color.B == searchcolor.B);
                    found = found | enabled;
                    Routes[i, j] = new Node(new Point(x, y), enabled);
                }
            }

            if (!found)
                return null;


   
            for (int x = rect.X, i = 0; x <= rect.Right; x++, i++)
            {
                for (int y = rect.Y, j = 0; y <= rect.Bottom; y++, j++)
                {
                    
                    List<Point> CheckPoints = new List<Point>(5);

                    for (int xdiff = -1; xdiff < 2; xdiff++)
                        for (int ydiff = -1; ydiff < 2; ydiff++)
                        {
                            if (xdiff != 0 || ydiff != 0)
                                CheckPoints.Add(new Point(x + xdiff, y + ydiff));
                        }
                    

                    foreach (Point checkpoint in CheckPoints)
                    {
                        int xidx = checkpoint.X - rect.X;
                        int yidx = checkpoint.Y - rect.Y;

                        if (xidx < 0 || yidx < 0 || xidx >= Routes.GetLength(0) || yidx >= Routes.GetLength(1))
                            continue;

                        if (Routes[xidx, yidx].Enabled)
                            Routes[x-rect.X,y-rect.Y].AddNeighbour(Routes[xidx, yidx]);
                    }
                }
            }

            return new RouteInfo(rect.Location,Routes); //True
        }



        private Bitmap ibmp;
    }
}
