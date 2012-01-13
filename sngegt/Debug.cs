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
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SNGEGT
{
    class Debug
    {
        public static bool Enabled = false;
        private static StreamWriter writer;
        public static void WriteLine(String format, params object[] param)
        {
            if (Enabled)
            {
                if (writer == null)
                {
                    writer = new StreamWriter(Application.StartupPath + "\\" + "debug.txt", true);
                    //writer = new StreamWriter(Console.OpenStandardOutput());
                }
                writer.WriteLine(format, param);
                writer.Flush();
            }
        }
    }
}
