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
using System.Xml;

namespace SNGEGT
{
    public partial class PresetRangeForm : Form
    {
        private int[] ranges;
        private String SETTINGSFILE;
        public PresetRangeForm()
        {
            InitializeComponent();
            SETTINGSFILE = Application.StartupPath + "\\settings.xml";
            
            dataGridView1.RowCount = 7;
            ranges = new int[dataGridView1.RowCount];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i+1;
                
                ranges[i] = (Int32) (100.0 / dataGridView1.RowCount * (i + 1));
                dataGridView1.Rows[i].Cells[1].Value = ranges[i];

            }

            LoadData();

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            XmlDocument settings = new XmlDocument();
            XmlNode node = null;
            XmlNode presetnode = null;
            try
            {
                settings.Load(SETTINGSFILE);
                node = settings.DocumentElement != null ? settings.DocumentElement.FirstChild : null;
                while (node != null)
                {
                    if (node.Name.Equals("Preset"))
                    {
                        presetnode = node;
                        presetnode.RemoveAll();
                        break;
                    }
                    node = node.NextSibling;
                }
            }
            catch (Exception /*exp*/)
            {
                
            }

            if (settings.DocumentElement == null)
                settings.AppendChild(settings.CreateElement("settings"));

            if (presetnode == null)
            {
                presetnode = settings.CreateElement("Preset");
                settings.DocumentElement.AppendChild(presetnode);
            }
            XmlNode buttonnode;
            XmlAttribute buttonnumber;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                buttonnode = settings.CreateElement("ButtonRange");
                buttonnumber = settings.CreateAttribute("number");
                buttonnumber.Value = Convert.ToString(i+1);
                buttonnode.Attributes.Append(buttonnumber);
                
                buttonnode.InnerText = dataGridView1.Rows[i].Cells[1].Value.ToString();
                presetnode.AppendChild(buttonnode);
                ranges[i] = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
            }


            

            try
            {
                settings.Save(SETTINGSFILE);
                MessageBox.Show("Saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (XmlException /*exp*/)
            {
            }
                
            
        }

        public int[] getRanges()
        {
            return ranges;
        }

        public void LoadData()
        {
            XmlDocument settings = new XmlDocument();
            try
            {
                settings.Load(SETTINGSFILE);
                XmlNode node;
                node = settings.DocumentElement.FirstChild;
                while (node != null)
                {
                    if (node.Name.Equals("Preset"))
                    {
                        node = node.FirstChild;
                        while (node != null)
                        {
                            if (node.Name.Equals("ButtonRange"))
                            {
                                try
                                {
                                    int number = Int32.Parse(node.Attributes["number"].Value);
                                    int value = Int32.Parse(node.InnerText);
                                    if (number > 0 && number <= dataGridView1.RowCount && value > 0 && value <= 100)
                                    {
                                        dataGridView1.Rows[number - 1].Cells[1].Value = value;
                                        ranges[number - 1] = value;
                                    }
                                }
                                catch (Exception)
                                {
                                }


                            }

                            node = node.NextSibling;
                        }
                        break;
                    }
                    node = node.NextSibling;
                }
            }
            catch (Exception)
            {
            }    

        }

        private void PresetRangeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }


        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }

        }
    }
}