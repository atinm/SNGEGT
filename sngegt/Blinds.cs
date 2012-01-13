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
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace SNGEGT
{
    public partial class BlindsForm : Form
    {

        private bool changed;
        private string XMLFILE;


        
        public BlindsForm()
        {
            XMLFILE =  Application.StartupPath + "\\blinds.xml";
            InitializeComponent();
            changed = false;
            dataGridViewWin.Rows.Add(10);
            for (int i = 0; i < 10; i++)
                dataGridViewWin.Rows[i].Cells[0].Value = i+1;

            for (int i = 0; i < dataGridViewWin.Columns.Count; i++)
                dataGridViewWin.Columns[i].ValueType = typeof(double);

            for (int i = 0; i < dataGridViewBlinds.Columns.Count; i++)
                dataGridViewBlinds.Columns[i].ValueType = typeof(Int32);
            //treeView1.TreeViewNodeSorter = new CaseInsensitiveComparer();
            
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView1.HitTest(e.X,e.Y).Node;
            DeleteSitetoolStripMenuItem.Visible = false;
            deleteStructureToolStripMenuItem.Visible = false;
            AddSitetoolStripMenuItem.Visible = true;
            AddStructureToolStripMenuItem.Visible = false;
            if (node == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                if (node.Level == 0)
                {
                    if (node.FirstNode == null)
                        DeleteSitetoolStripMenuItem.Visible = true;
                    AddStructureToolStripMenuItem.Visible = true;
                }

                if (node.Level == 1)
                {
                    deleteStructureToolStripMenuItem.Visible = true;
                    AddSitetoolStripMenuItem.Visible = false;
                }
               

            }   
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
        }

        private void AddSitetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.Nodes.Add("");
            node.Tag = new BlindSaver();
            node.BeginEdit();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                if (e.Node.Text == "" )
                    treeView1.Nodes.Remove(e.Node);
                panel1.Enabled = treeView1.Nodes.Count > 0;
                return;
            }

            changed = true;

            
            string newtext = e.Label.Trim();
            e.Node.Name = newtext;

            if (newtext == "")
            {
                treeView1.Nodes.Remove(e.Node);
                return;
            }
            if (treeView1.Nodes.Find(newtext, false).Length > 1)
            {
                MessageBox.Show("This site is already on the list.");
                e.CancelEdit = true;
                if (e.Node.Text == "")
                    treeView1.Nodes.Remove(e.Node);
                else
                    e.Node.BeginEdit();
            }
            if (e.Node.Level == 1)
            {
                TreeNode node = e.Node.Parent.FirstNode;
                while (node != null)
                {
                    if (node.Text == e.Node.Text && node != e.Node)
                    {
                        MessageBox.Show("Structure with same name is already on list");
                        e.CancelEdit = true;
                        e.Node.BeginEdit();
                        return;
                    }
                    node = node.NextNode;
                }
                
                ((Award)e.Node.Tag).name = e.Label;
                changed = true;
            }

            

        }

        private void DeleteSitetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

     




        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            panel1.Visible = e.Node != null;
            if (e.Node == null)
                return;

            if (e.Node.Level == 0)
            {
                dataGridViewBlinds.Rows.Clear();
                

                BlindSaver saver = (BlindSaver)e.Node.Tag;

                
                for (int i = 0; i < saver.blinds.Count; i++)
                {
                    BlindInfo blind = saver.blinds[i];
                    int rowindex = dataGridViewBlinds.Rows.Add();
                    DataGridViewRow row = dataGridViewBlinds.Rows[rowindex];
                    row.Cells[0].Value = blind.Smallblind;
                    row.Cells[1].Value = blind.Bigblind;
                    if (blind.Ante > 0)
                        row.Cells[2].Value = blind.Ante;
                }

                panelBlinds.Visible = true;
                panelBlinds.Height = panel1.Height - 20;
                panelWin.Visible = false;
                
                return;
            }
            if (e.Node.Level == 1)
            {


                Award aw = (Award) e.Node.Tag;

                for (int i = 0; i < dataGridViewWin.Rows.Count; i++ )
                    dataGridViewWin.Rows[i].Cells[1].Value = "";

                for (int i = 0; i < aw.wins.Count; i++)
                    dataGridViewWin.Rows[i].Cells[1].Value = aw.wins[i];
                textBoxPlayerCount.Text = Convert.ToString(aw.playercount);

                panelBlinds.Visible = false;
                panelWin.Top = panelBlinds.Top;
                panelWin.Visible = true;
            }
                    
        }

        private void textBoxSmallblind_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBoxSmallBlind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void buttonSaveStructure_Click(object sender, EventArgs e)
        {
            changed = true;
            TreeNode node = treeView1.SelectedNode;
            
            dataGridViewBlinds.Sort(dataGridViewBlinds.Columns[0], ListSortDirection.Ascending);
            
            
            if (dataGridViewBlinds.RowCount == 0)
            {
                MessageBox.Show("Please fill win structure");
                return;
            }
            
   

            if (textBoxPlayerCount.Text == "" || Convert.ToInt32(textBoxPlayerCount.Text) == 0)
            {
                MessageBox.Show("Please fill how many players participates to tournament.");
                textBoxPlayerCount.Focus();
                return;
            }
            double cumprice = 0;
            int found = 0;

            //Lasketaan palkintojen summa, breakataan tyhj‰‰n
            for (int i = 0; i < dataGridViewWin.Rows.Count; i++)
                if (Convert.ToString(dataGridViewWin.Rows[i].Cells[1].Value) != "")
                {
                    cumprice += Convert.ToDouble(dataGridViewWin.Rows[i].Cells[1].Value);
                    found++;
                }
                

            //Yht‰‰n palkintoa ei merkattu
            if (cumprice != 100)
            {
                MessageBox.Show("Please check award list");
               
                return;
            }
           

            Award aw = (Award) treeView1.SelectedNode.Tag;
            aw.wins.Clear();

            
            //Laitetaan palkintojen arvot saveriin
            for (int i = 0; i < dataGridViewWin.Rows.Count; i++)
            {
                if ((Convert.ToString(dataGridViewWin.Rows[i].Cells[1].Value)) == "")
                    break;
                aw.wins.Add(Convert.ToDouble(dataGridViewWin.Rows[i].Cells[1].Value));
                
            }
            aw.playercount = Convert.ToInt32(textBoxPlayerCount.Text);
            
            MessageBox.Show("Structure updated");
        }



        private void BlindsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                DialogResult dlres = MessageBox.Show("Save changes?", "Close confirmation", MessageBoxButtons.YesNoCancel
                , MessageBoxIcon.Question);

                if (dlres == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                else if (dlres == DialogResult.No)
                    return;

                //Ei tallennettavaa -> pois
                if (treeView1.Nodes.Count == 0)
                    return;

                //Otetaan k‰yttˆˆn sisennys
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter xmlwriter = XmlTextWriter.Create(XMLFILE,settings);
                xmlwriter.WriteStartDocument();
                /*
     * Xml-doc muotoa
     * <Sites>
     *    <Site name="Party">
     *       <GameStructure>
     *           <BlindStruct>
     *              <levels>
     *                  <level>25/50</level>
     *                  <level>800/1600+50</level>
     *              </levels>
     *           </BlindStruct>
     *           <Awards>
     *             <AwardStruct name="10 max" playercount=10>
     *               <award>50</award>
     *               <award>30</award>
     *               <award>20</award>
     *             </Awardstruct>
     *             <AwardStruct name="6 max" playercount=6>
     *               <award>60</award>
     *               <award>40</award>
     *             </AwardStuct>
                 </awards>
     *       </GameStructure>
     *    </Site>
     * </Sites>
     */
                xmlwriter.WriteStartElement("Sites");
               
                
              
                //document.AppendChild(
                TreeNode parentnode = treeView1.Nodes[0];
                
                while (parentnode != null)
                {
                    xmlwriter.WriteStartElement("Site");
                    xmlwriter.WriteAttributeString("name", parentnode.Text);

                    ((BlindSaver)parentnode.Tag).Save(xmlwriter);
                    parentnode = parentnode.NextNode;
                    xmlwriter.WriteEndElement(); //Site

                }

                
                xmlwriter.WriteEndDocument(); //Sites
                xmlwriter.Close();
                        
            }
        }

        public Hashtable LoadXml(Boolean UpdateUi)
        {
            XmlDataDocument document = new XmlDataDocument();
            document.Load(XMLFILE);
            XmlNode node = document.FirstChild;
            XmlNode ChildNode,ChildNode2;
            Hashtable results = new Hashtable();
            while (node != null)
            {

                if (node.Name == "Sites")
                {
                    ChildNode = node.FirstChild;
                    while(ChildNode != null)
                    {
                        if (ChildNode.Name == "Site")
                        {
                            ChildNode2 = ChildNode.FirstChild;
                            BlindSaver saver = new BlindSaver();
                            while (ChildNode2 != null)
                            {
                                if (ChildNode2.Name == "GameStructure")
                                    saver.Load(ChildNode2);
                                
                                ChildNode2 = ChildNode2.NextSibling;
                            }

                            //Tallennetaan saver saitin nimen mukaisesti hajautustauluun
                            results.Add(ChildNode.Attributes["name"].Value,saver);

                        }

                        ChildNode = ChildNode.NextSibling;
                    }
                }

                    

                node = node.NextSibling;
            }
            
            if (UpdateUi)
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                
                //Lis‰t‰‰n kasinon nimen mukainen eka node
                foreach (string sitename in results.Keys)
                {
                    BlindSaver data = (BlindSaver) results[sitename];
                    TreeNode parentnode = treeView1.Nodes.Add(sitename);
                    parentnode.Tag = data;
                    TreeNode childnode;
                    //Lis‰t‰‰n structien nimen mukaiset alanodet, joille annetaan
                    //nimeksi structin nimi, ja tagiin heitet‰‰n saver objekti
                    foreach (Award aw in data.awards)
                    {
                        childnode = parentnode.Nodes.Add(aw.name);
                        childnode.Tag = aw;
                    }
                }

                treeView1.EndUpdate();
                
            }
            return results;
        }




 
        private void dataGridViewBlinds_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridViewBlinds_Validating(object sender, CancelEventArgs e)
        {
            Boolean hasante = false;
            String error = "";
            int i;
            //dataGridViewBlinds.Sort(dataGridViewBlinds.Columns[0], ListSortDirection.Ascending);

            //K‰yd‰‰n rivit l‰pi
            for (i = 0; i < dataGridViewBlinds.RowCount; i++)
            {
                
                int notnull = 0;

                //Katsotaan moniko kahdesta ekasta on tyhj‰
                for (int j = 0; j<2; j++)
                    if (Convert.ToString(dataGridViewBlinds.Rows[i].Cells[j].Value) != "")
                        notnull++;

                //Ei saa olla ainoastaan toinen t‰ytettyn‰
                if (notnull == 1)
                {
                    error = "Please fill both small and big blind.";
                    break;
                }

                //Ante pit‰‰ laittaa, jos sellainen oli edellisell‰ rivill‰
                if (notnull != 0 && hasante && Convert.ToString(dataGridViewBlinds.Rows[i].Cells[2].Value) == "")
                {
                    error = "Please fill ante field.";
                    break;
                }

                
                if (dataGridViewBlinds.Rows[i].Cells[2].Value != null)
                    hasante = true;
            }
            if (error != "")
            {
                MessageBox.Show(error, "Error in blinds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridViewBlinds.CurrentCell = dataGridViewBlinds.Rows[i].Cells[0];
                dataGridViewBlinds.Rows[i].Selected = true;
                dataGridViewBlinds.Invalidate();
            }

        }

        

        private void deleteStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((BlindSaver)treeView1.SelectedNode.Parent.Tag).awards.Remove((Award) treeView1.SelectedNode.Tag);
            treeView1.Nodes.Remove(treeView1.SelectedNode);
            
            changed = true;

        }

        private void ButtonSaveBlinds_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            while (node.Parent != null)
                node = node.Parent;
            BlindSaver saver = (BlindSaver) node.Tag;
            saver.name = treeView1.SelectedNode.Text;
            saver.blinds.Clear();
            for (int i = 0; i < dataGridViewBlinds.RowCount; i++)
            {
                string format = "{0}/{1}";
                DataGridViewRow row = dataGridViewBlinds.Rows[i];
                if (Convert.ToString(row.Cells[2].Value) != "")
                    format += "+{2}";
                if (Convert.ToString(row.Cells[0].Value) == "")
                    break;
                saver.blinds.Add(new BlindInfo(String.Format(format, row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value)));
            }
            changed = true;
            MessageBox.Show("Blinds updated");
        }

        private void createStrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode.Nodes.Add("");
            node.Tag = new Award();
            node.EnsureVisible();
            ((BlindSaver) node.Parent.Tag).awards.Add((Award) node.Tag);
            node.BeginEdit();

        }

        private void dataGridViewWin_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                MessageBox.Show("Value can't be parsed. Check that you have entered decimal number according to your locale.");
            }
        }



        











    }
}