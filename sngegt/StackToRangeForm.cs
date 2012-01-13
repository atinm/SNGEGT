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
using System.IO;
using System.Collections;
namespace SNGEGT
{


    public partial class StackToRangeForm : Form
    {
        private List <stackblind> stackinfos;

        const String xmlfile = "settings.xml";
       
        
        public StackToRangeForm()
        {
            InitializeComponent();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                dataGridView1.Columns[i].ValueType = typeof(UInt32);
            stackinfos = new List<stackblind>(5);
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Application.StartupPath, xmlfile));
                XmlNode node = doc.DocumentElement == null ? null : doc.DocumentElement.FirstChild;

                while (node != null)
                {
                    XmlNode childnode;
                    if (node.Name == "BlindStacks")
                    {
                        childnode = node.FirstChild;
                        while (childnode != null)
                        {
                            stackblind tmp = new stackblind(-1, -1);
                            if (tmp.Load(childnode))
                                stackinfos.Add(tmp);
                            childnode = childnode.NextSibling;
                        }
                    }
                    node = node.NextSibling;
                }

            }
            catch (Exception)
            {
            }
            stackinfos.Sort();
            
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = stackinfos.Count+1;
            int divindex = dataGridView1.Columns["StackDivBlind"].Index;
            int rangeindex = dataGridView1.Columns["Range"].Index;
            for (int i = 0; i < stackinfos.Count; i++)
            {
                dataGridView1.Rows[i].Cells[divindex].Value = stackinfos[i].division;
                dataGridView1.Rows[i].Cells[rangeindex].Value = stackinfos[i].range;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc=  new XmlDocument();
            try
            {
                doc.Load(Path.Combine(Application.StartupPath,xmlfile));
            }
            catch (Exception)
            {
                doc = new XmlDocument();
            }

            XmlNode node = null;

            if (doc.DocumentElement == null)
            {
                doc.AppendChild(doc.CreateElement("settings"));
               
            }
            
            for (int i =0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                node = doc.DocumentElement.ChildNodes[i]; 
                if (node.Name.Equals("BlindStacks"))
                {
                    doc.DocumentElement.RemoveChild(node);
                    
                }
            }

            node = doc.DocumentElement.AppendChild(doc.CreateElement("BlindStacks"));

            stackinfos.Clear();

            stackinfos.Capacity = dataGridView1.RowCount;

            int divindex = dataGridView1.Columns["StackDivBlind"].Index;
            int rangeindex = dataGridView1.Columns["Range"].Index;

            for (int i = 0; i < dataGridView1.RowCount; i++)
                if (dataGridView1.Rows[i].Cells[divindex].Value != null
                    && dataGridView1.Rows[i].Cells[rangeindex].Value != null)
                {
                    stackinfos.Add(new stackblind(Convert.ToInt32(dataGridView1.Rows[i].Cells[divindex].Value), Convert.ToInt32(dataGridView1.Rows[i].Cells[rangeindex].Value)));

                }
            stackinfos.Sort();

            for (int i = 0; i < stackinfos.Count; i++)
            {
                stackinfos[i].Save(node);
            }

            try
            {
                doc.Save(Path.Combine(Application.StartupPath,xmlfile));
            }
            catch (Exception)
            {
                MessageBox.Show("Saving failed","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(this,"Saving completed");

        }

        public int GetRange(int stack,int big_blind)
        {
            for (int i = stackinfos.Count - 1; i >= 0; i--)
                if (stackinfos[i].division < stack / big_blind)
                    return stackinfos[i].range;
            return 10;
        }

        private void StackToRangeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;

            }
        }

        private void dataGridView1_CellErrorTextNeeded(object sender, DataGridViewCellErrorTextNeededEventArgs e)
        {
            
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
            
           
    }

    public class stackblind : IComparable
    {
        public int division;
        public int range;

        public stackblind(int adivision, int arange)
        {
            range = (int)arange;
            division = (int)adivision;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            return division.CompareTo(((stackblind) obj).division);
        }
        #endregion

        public void Save(XmlNode node)
        {
            XmlDocument doc = node.OwnerDocument;
            XmlNode parnode = node.AppendChild(doc.CreateElement("BlindStack"));
            parnode.AppendChild(doc.CreateElement("StackDivBlind")).InnerText = Convert.ToString(division);
            parnode.AppendChild(doc.CreateElement("Range")).InnerText = Convert.ToString(range);
        }

        public bool Load(XmlNode node)
        {
            range = -1;
            division = -1;

            try
            {
                if (node.Name == "BlindStack")
                {
                    XmlNode childnode = node.FirstChild;
                    while (childnode != null)
                    {
                        if (childnode.Name == "StackDivBlind")
                            division = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "Range")
                            range = Convert.ToInt32(childnode.InnerText);

                        childnode = childnode.NextSibling;
                    }
                }
            }

            catch (Exception)
            {
                return false;
            }
            return range != -1 && division != -1;
        }



    }

}