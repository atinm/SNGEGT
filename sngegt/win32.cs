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
using System.Runtime.InteropServices;

namespace SNGEGT
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public struct POINT
    {
        public int X;
        public int Y;
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
        //[DllImport("GDI32.dll")]
        //public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        [DllImport("GDI32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        
    }


    class User32
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("User32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        
        [DllImport("user32.dll")]
        public static extern long GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        public static extern long GetClientRect(IntPtr hWnd, ref Rectangle lpRect);


        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("User32.dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, PChildCallBack lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("User32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, ref Rectangle lpRect, bool erase);
        
        [DllImport("User32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        


        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, ref IntPtr lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, ref Rectangle lprcUpdate, IntPtr hrgnUpdate, uint flags);
 
        [DllImport("user32.Dll")]
        public static extern bool EnumWindows(PCallBack x, IntPtr y);

        [DllImport("user32.Dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmd);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, int tag);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwnd, int gaFlags);

        public static bool SupportPrintWindow()
        {
            System.OperatingSystem opsys = System.Environment.OSVersion;            
            
            if (opsys.Platform == PlatformID.Win32NT)
            {
                if ((opsys.Version.Major == 5 && opsys.Version.Minor == 1) || opsys.Version.Major == 6)
                    return true;

            }
            return false;
        }

        public delegate bool PCallBack(IntPtr hwnd, IntPtr lParam);
        public delegate bool PChildCallBack(IntPtr hWnd, IntPtr lParam);
    }	
}
