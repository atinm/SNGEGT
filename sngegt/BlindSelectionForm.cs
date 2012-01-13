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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SNGEGT
{
    public partial class BlindSelectionForm : Form
    {
        private int selected;
        public BlindSelectionForm()
        {
            InitializeComponent();

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selected = listBox1.SelectedIndex;
                this.Close();
            }
        }

        public int getindex(List <BlindInfo> data)
        {
            selected = -1;
            listBox1.Items.Clear();
            for (int i = 0; i < data.Count; i++)
                listBox1.Items.Add(data[i]);

            ShowDialog();
            return selected;
        }
    }
}