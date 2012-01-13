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


namespace SNGEGT
{
    partial class BlindsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddSitetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteSitetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelWin = new System.Windows.Forms.Panel();
            this.dataGridViewWin = new System.Windows.Forms.DataGridView();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPlayerCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSaveStructure = new System.Windows.Forms.Button();
            this.panelBlinds = new System.Windows.Forms.Panel();
            this.ButtonSaveBlinds = new System.Windows.Forms.Button();
            this.dataGridViewBlinds = new System.Windows.Forms.DataGridView();
            this.SmallBlindColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BigBlindColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AnteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuTree.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelWin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWin)).BeginInit();
            this.panelBlinds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlinds)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddSitetoolStripMenuItem,
            this.DeleteSitetoolStripMenuItem,
            this.AddStructureToolStripMenuItem,
            this.deleteStructureToolStripMenuItem});
            this.contextMenuTree.Name = "contextMenuTree";
            this.contextMenuTree.Size = new System.Drawing.Size(153, 92);
            this.contextMenuTree.Text = "Add new";
            // 
            // AddSitetoolStripMenuItem
            // 
            this.AddSitetoolStripMenuItem.Name = "AddSitetoolStripMenuItem";
            this.AddSitetoolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.AddSitetoolStripMenuItem.Text = "Add site";
            this.AddSitetoolStripMenuItem.Click += new System.EventHandler(this.AddSitetoolStripMenuItem_Click);
            // 
            // DeleteSitetoolStripMenuItem
            // 
            this.DeleteSitetoolStripMenuItem.Name = "DeleteSitetoolStripMenuItem";
            this.DeleteSitetoolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.DeleteSitetoolStripMenuItem.Text = "Delete site";
            this.DeleteSitetoolStripMenuItem.Click += new System.EventHandler(this.DeleteSitetoolStripMenuItem_Click);
            // 
            // AddStructureToolStripMenuItem
            // 
            this.AddStructureToolStripMenuItem.Name = "AddStructureToolStripMenuItem";
            this.AddStructureToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.AddStructureToolStripMenuItem.Text = "Add structure";
            this.AddStructureToolStripMenuItem.Click += new System.EventHandler(this.createStrToolStripMenuItem_Click);
            // 
            // deleteStructureToolStripMenuItem
            // 
            this.deleteStructureToolStripMenuItem.Name = "deleteStructureToolStripMenuItem";
            this.deleteStructureToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteStructureToolStripMenuItem.Text = "Delete structure";
            this.deleteStructureToolStripMenuItem.Click += new System.EventHandler(this.deleteStructureToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.contextMenuTree;
            this.treeView1.HideSelection = false;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(6, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(169, 521);
            this.treeView1.TabIndex = 14;
            this.treeView1.TabStop = false;
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.panelWin);
            this.panel1.Controls.Add(this.panelBlinds);
            this.panel1.Location = new System.Drawing.Point(181, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(361, 524);
            this.panel1.TabIndex = 17;
            // 
            // panelWin
            // 
            this.panelWin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panelWin.Controls.Add(this.dataGridViewWin);
            this.panelWin.Controls.Add(this.label6);
            this.panelWin.Controls.Add(this.textBoxPlayerCount);
            this.panelWin.Controls.Add(this.label5);
            this.panelWin.Controls.Add(this.buttonSaveStructure);
            this.panelWin.Location = new System.Drawing.Point(3, 243);
            this.panelWin.Name = "panelWin";
            this.panelWin.Size = new System.Drawing.Size(355, 278);
            this.panelWin.TabIndex = 43;
            this.panelWin.Visible = false;
            // 
            // dataGridViewWin
            // 
            this.dataGridViewWin.AllowUserToAddRows = false;
            this.dataGridViewWin.AllowUserToDeleteRows = false;
            this.dataGridViewWin.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewWin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Position,
            this.Column1});
            this.dataGridViewWin.Location = new System.Drawing.Point(149, 20);
            this.dataGridViewWin.MultiSelect = false;
            this.dataGridViewWin.Name = "dataGridViewWin";
            this.dataGridViewWin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewWin.Size = new System.Drawing.Size(180, 255);
            this.dataGridViewWin.TabIndex = 47;
            this.dataGridViewWin.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewWin_DataError);
            // 
            // Position
            // 
            this.Position.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Position.Frozen = true;
            this.Position.HeaderText = "Place";
            this.Position.Name = "Position";
            this.Position.ReadOnly = true;
            this.Position.Width = 59;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column1.HeaderText = "Win %";
            this.Column1.MaxInputLength = 4;
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Width = 62;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Player count";
            // 
            // textBoxPlayerCount
            // 
            this.textBoxPlayerCount.Location = new System.Drawing.Point(22, 20);
            this.textBoxPlayerCount.Name = "textBoxPlayerCount";
            this.textBoxPlayerCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayerCount.TabIndex = 42;
            this.textBoxPlayerCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSmallblind_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-106, -17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "Win  structure";
            // 
            // buttonSaveStructure
            // 
            this.buttonSaveStructure.Location = new System.Drawing.Point(22, 91);
            this.buttonSaveStructure.Name = "buttonSaveStructure";
            this.buttonSaveStructure.Size = new System.Drawing.Size(96, 23);
            this.buttonSaveStructure.TabIndex = 44;
            this.buttonSaveStructure.TabStop = false;
            this.buttonSaveStructure.Text = "Save structure";
            this.buttonSaveStructure.UseVisualStyleBackColor = true;
            this.buttonSaveStructure.Click += new System.EventHandler(this.buttonSaveStructure_Click);
            // 
            // panelBlinds
            // 
            this.panelBlinds.AutoSize = true;
            this.panelBlinds.Controls.Add(this.ButtonSaveBlinds);
            this.panelBlinds.Controls.Add(this.dataGridViewBlinds);
            this.panelBlinds.Location = new System.Drawing.Point(3, 12);
            this.panelBlinds.Name = "panelBlinds";
            this.panelBlinds.Size = new System.Drawing.Size(355, 234);
            this.panelBlinds.TabIndex = 42;
            this.panelBlinds.Visible = false;
            // 
            // ButtonSaveBlinds
            // 
            this.ButtonSaveBlinds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSaveBlinds.Location = new System.Drawing.Point(22, 185);
            this.ButtonSaveBlinds.Name = "ButtonSaveBlinds";
            this.ButtonSaveBlinds.Size = new System.Drawing.Size(75, 23);
            this.ButtonSaveBlinds.TabIndex = 4;
            this.ButtonSaveBlinds.Text = "Save blinds";
            this.ButtonSaveBlinds.UseVisualStyleBackColor = true;
            this.ButtonSaveBlinds.Click += new System.EventHandler(this.ButtonSaveBlinds_Click);
            // 
            // dataGridViewBlinds
            // 
            this.dataGridViewBlinds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewBlinds.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBlinds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBlinds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SmallBlindColumn,
            this.BigBlindColumn,
            this.AnteColumn});
            this.dataGridViewBlinds.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridViewBlinds.Location = new System.Drawing.Point(22, 3);
            this.dataGridViewBlinds.MultiSelect = false;
            this.dataGridViewBlinds.Name = "dataGridViewBlinds";
            this.dataGridViewBlinds.Size = new System.Drawing.Size(307, 176);
            this.dataGridViewBlinds.TabIndex = 3;
            this.dataGridViewBlinds.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewBlinds_DataError);
            this.dataGridViewBlinds.Validating += new System.ComponentModel.CancelEventHandler(this.dataGridViewBlinds_Validating);
            // 
            // SmallBlindColumn
            // 
            this.SmallBlindColumn.HeaderText = "Small blind";
            this.SmallBlindColumn.MaxInputLength = 5;
            this.SmallBlindColumn.Name = "SmallBlindColumn";
            // 
            // BigBlindColumn
            // 
            this.BigBlindColumn.HeaderText = "Big blind";
            this.BigBlindColumn.MaxInputLength = 5;
            this.BigBlindColumn.Name = "BigBlindColumn";
            // 
            // AnteColumn
            // 
            this.AnteColumn.HeaderText = "Ante";
            this.AnteColumn.MaxInputLength = 5;
            this.AnteColumn.Name = "AnteColumn";
            // 
            // BlindsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 533);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlindsForm";
            this.Text = "Blind administration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BlindsForm_FormClosing);
            this.contextMenuTree.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelWin.ResumeLayout(false);
            this.panelWin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWin)).EndInit();
            this.panelBlinds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlinds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuTree;
        private System.Windows.Forms.ToolStripMenuItem AddSitetoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteSitetoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteStructureToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridViewBlinds;
        private System.Windows.Forms.DataGridViewTextBoxColumn SmallBlindColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BigBlindColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnteColumn;
        private System.Windows.Forms.Panel panelBlinds;
        private System.Windows.Forms.Button ButtonSaveBlinds;
        private System.Windows.Forms.Panel panelWin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPlayerCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonSaveStructure;
        private System.Windows.Forms.DataGridView dataGridViewWin;
        private System.Windows.Forms.ToolStripMenuItem AddStructureToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}