namespace SNG_Quiz
{
    using System.Drawing;
    partial class SNGquiz
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SNGquiz));
            this.labelPlayersNum = new System.Windows.Forms.Label();
            this.comboBoxPlayers = new System.Windows.Forms.ComboBox();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.labelMode = new System.Windows.Forms.Label();
            this.comboBoxChips = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDifficulty = new System.Windows.Forms.ComboBox();
            this.labelDifficulty = new System.Windows.Forms.Label();
            this.comboBoxBBsize = new System.Windows.Forms.ComboBox();
            this.labelBBsize = new System.Windows.Forms.Label();
            this.comboBoxPosition = new System.Windows.Forms.ComboBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.labelStacks = new System.Windows.Forms.Label();
            this.comboBoxStacks = new System.Windows.Forms.ComboBox();
            this.labelChipsBB = new System.Windows.Forms.Label();
            this.labelChipsSB = new System.Windows.Forms.Label();
            this.labelChipsBTN = new System.Windows.Forms.Label();
            this.labelChipsUTG = new System.Windows.Forms.Label();
            this.textBoxChipsBB = new System.Windows.Forms.TextBox();
            this.textBoxChipsSB = new System.Windows.Forms.TextBox();
            this.textBoxChipsBTN = new System.Windows.Forms.TextBox();
            this.textBoxChipsUTG = new System.Windows.Forms.TextBox();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.Hand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxResults = new System.Windows.Forms.GroupBox();
            this.labelResults = new System.Windows.Forms.Label();
            this.buttonFold = new System.Windows.Forms.Button();
            this.buttonPush = new System.Windows.Forms.Button();
            this.pictureBoxTable = new System.Windows.Forms.PictureBox();
            this.myCard1 = new System.Windows.Forms.PictureBox();
            this.myCard2 = new System.Windows.Forms.PictureBox();
            this.player4card2 = new System.Windows.Forms.PictureBox();
            this.player4card1 = new System.Windows.Forms.PictureBox();
            this.player1card2 = new System.Windows.Forms.PictureBox();
            this.player1card1 = new System.Windows.Forms.PictureBox();
            this.player2card2 = new System.Windows.Forms.PictureBox();
            this.player2card1 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timerTooltip = new System.Windows.Forms.Timer(this.components);
            this.labelGameNumber = new System.Windows.Forms.Label();
            this.player3 = new SNG_Quiz.Player();
            this.player1 = new SNG_Quiz.Player();
            this.player2 = new SNG_Quiz.Player();
            this.player4 = new SNG_Quiz.Player();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.groupBoxResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player4card2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player4card1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player1card2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player1card1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player2card2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player2card1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPlayersNum
            // 
            this.labelPlayersNum.AutoSize = true;
            this.labelPlayersNum.Location = new System.Drawing.Point(10, 65);
            this.labelPlayersNum.Name = "labelPlayersNum";
            this.labelPlayersNum.Size = new System.Drawing.Size(92, 13);
            this.labelPlayersNum.TabIndex = 0;
            this.labelPlayersNum.Text = "Number of players";
            // 
            // comboBoxPlayers
            // 
            this.comboBoxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayers.FormattingEnabled = true;
            this.comboBoxPlayers.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "Random"});
            this.comboBoxPlayers.Location = new System.Drawing.Point(108, 62);
            this.comboBoxPlayers.Name = "comboBoxPlayers";
            this.comboBoxPlayers.Size = new System.Drawing.Size(67, 21);
            this.comboBoxPlayers.TabIndex = 1;
            this.comboBoxPlayers.SelectedIndexChanged += new System.EventHandler(this.setForm);
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "Push",
            "Call",
            "Random"});
            this.comboBoxMode.Location = new System.Drawing.Point(108, 40);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(67, 21);
            this.comboBoxMode.TabIndex = 3;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.setForm);
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(10, 43);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(57, 13);
            this.labelMode.TabIndex = 2;
            this.labelMode.Text = "Quiz mode";
            // 
            // comboBoxChips
            // 
            this.comboBoxChips.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChips.FormattingEnabled = true;
            this.comboBoxChips.Items.AddRange(new object[] {
            "10 000",
            "15 000",
            "20 000"});
            this.comboBoxChips.Location = new System.Drawing.Point(108, 128);
            this.comboBoxChips.Name = "comboBoxChips";
            this.comboBoxChips.Size = new System.Drawing.Size(67, 21);
            this.comboBoxChips.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chips in game";
            // 
            // comboBoxDifficulty
            // 
            this.comboBoxDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDifficulty.FormattingEnabled = true;
            this.comboBoxDifficulty.Items.AddRange(new object[] {
            "Easy",
            "Normal",
            "Hard"});
            this.comboBoxDifficulty.Location = new System.Drawing.Point(108, 18);
            this.comboBoxDifficulty.Name = "comboBoxDifficulty";
            this.comboBoxDifficulty.Size = new System.Drawing.Size(67, 21);
            this.comboBoxDifficulty.TabIndex = 7;
            this.comboBoxDifficulty.SelectedIndexChanged += new System.EventHandler(this.comboBoxDifficulty_SelectedIndexChanged);
            // 
            // labelDifficulty
            // 
            this.labelDifficulty.AutoSize = true;
            this.labelDifficulty.Location = new System.Drawing.Point(10, 21);
            this.labelDifficulty.Name = "labelDifficulty";
            this.labelDifficulty.Size = new System.Drawing.Size(47, 13);
            this.labelDifficulty.TabIndex = 6;
            this.labelDifficulty.Text = "Difficulty";
            // 
            // comboBoxBBsize
            // 
            this.comboBoxBBsize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBBsize.FormattingEnabled = true;
            this.comboBoxBBsize.Items.AddRange(new object[] {
            "200",
            "400",
            "600",
            "800",
            "1200",
            "Random"});
            this.comboBoxBBsize.Location = new System.Drawing.Point(108, 106);
            this.comboBoxBBsize.Name = "comboBoxBBsize";
            this.comboBoxBBsize.Size = new System.Drawing.Size(67, 21);
            this.comboBoxBBsize.TabIndex = 9;
            // 
            // labelBBsize
            // 
            this.labelBBsize.AutoSize = true;
            this.labelBBsize.Location = new System.Drawing.Point(10, 109);
            this.labelBBsize.Name = "labelBBsize";
            this.labelBBsize.Size = new System.Drawing.Size(74, 13);
            this.labelBBsize.TabIndex = 8;
            this.labelBBsize.Text = "Size of the BB";
            // 
            // comboBoxPosition
            // 
            this.comboBoxPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPosition.FormattingEnabled = true;
            this.comboBoxPosition.Items.AddRange(new object[] {
            "BB",
            "SB",
            "Button",
            "UTG",
            "Random"});
            this.comboBoxPosition.Location = new System.Drawing.Point(108, 84);
            this.comboBoxPosition.Name = "comboBoxPosition";
            this.comboBoxPosition.Size = new System.Drawing.Size(67, 21);
            this.comboBoxPosition.TabIndex = 11;
            this.comboBoxPosition.SelectedIndexChanged += new System.EventHandler(this.setForm);
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(10, 87);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(44, 13);
            this.labelPosition.TabIndex = 10;
            this.labelPosition.Text = "Position";
            // 
            // labelStacks
            // 
            this.labelStacks.AutoSize = true;
            this.labelStacks.Location = new System.Drawing.Point(10, 153);
            this.labelStacks.Name = "labelStacks";
            this.labelStacks.Size = new System.Drawing.Size(73, 13);
            this.labelStacks.TabIndex = 12;
            this.labelStacks.Text = "Size of stacks";
            // 
            // comboBoxStacks
            // 
            this.comboBoxStacks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStacks.FormattingEnabled = true;
            this.comboBoxStacks.Items.AddRange(new object[] {
            "Random",
            "User defined"});
            this.comboBoxStacks.Location = new System.Drawing.Point(108, 150);
            this.comboBoxStacks.Name = "comboBoxStacks";
            this.comboBoxStacks.Size = new System.Drawing.Size(67, 21);
            this.comboBoxStacks.TabIndex = 13;
            this.comboBoxStacks.SelectedIndexChanged += new System.EventHandler(this.setForm);
            // 
            // labelChipsBB
            // 
            this.labelChipsBB.AutoSize = true;
            this.labelChipsBB.Location = new System.Drawing.Point(10, 175);
            this.labelChipsBB.Name = "labelChipsBB";
            this.labelChipsBB.Size = new System.Drawing.Size(21, 13);
            this.labelChipsBB.TabIndex = 14;
            this.labelChipsBB.Text = "BB";
            // 
            // labelChipsSB
            // 
            this.labelChipsSB.AutoSize = true;
            this.labelChipsSB.Location = new System.Drawing.Point(10, 196);
            this.labelChipsSB.Name = "labelChipsSB";
            this.labelChipsSB.Size = new System.Drawing.Size(21, 13);
            this.labelChipsSB.TabIndex = 15;
            this.labelChipsSB.Text = "SB";
            // 
            // labelChipsBTN
            // 
            this.labelChipsBTN.AutoSize = true;
            this.labelChipsBTN.Location = new System.Drawing.Point(10, 217);
            this.labelChipsBTN.Name = "labelChipsBTN";
            this.labelChipsBTN.Size = new System.Drawing.Size(29, 13);
            this.labelChipsBTN.TabIndex = 16;
            this.labelChipsBTN.Text = "BTN";
            // 
            // labelChipsUTG
            // 
            this.labelChipsUTG.AutoSize = true;
            this.labelChipsUTG.Location = new System.Drawing.Point(10, 238);
            this.labelChipsUTG.Name = "labelChipsUTG";
            this.labelChipsUTG.Size = new System.Drawing.Size(30, 13);
            this.labelChipsUTG.TabIndex = 17;
            this.labelChipsUTG.Text = "UTG";
            // 
            // textBoxChipsBB
            // 
            this.textBoxChipsBB.Location = new System.Drawing.Point(108, 172);
            this.textBoxChipsBB.Name = "textBoxChipsBB";
            this.textBoxChipsBB.Size = new System.Drawing.Size(67, 20);
            this.textBoxChipsBB.TabIndex = 18;
            // 
            // textBoxChipsSB
            // 
            this.textBoxChipsSB.Location = new System.Drawing.Point(108, 193);
            this.textBoxChipsSB.Name = "textBoxChipsSB";
            this.textBoxChipsSB.Size = new System.Drawing.Size(67, 20);
            this.textBoxChipsSB.TabIndex = 19;
            // 
            // textBoxChipsBTN
            // 
            this.textBoxChipsBTN.Location = new System.Drawing.Point(108, 214);
            this.textBoxChipsBTN.Name = "textBoxChipsBTN";
            this.textBoxChipsBTN.Size = new System.Drawing.Size(67, 20);
            this.textBoxChipsBTN.TabIndex = 20;
            // 
            // textBoxChipsUTG
            // 
            this.textBoxChipsUTG.Location = new System.Drawing.Point(108, 235);
            this.textBoxChipsUTG.Name = "textBoxChipsUTG";
            this.textBoxChipsUTG.Size = new System.Drawing.Size(67, 20);
            this.textBoxChipsUTG.TabIndex = 21;
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.buttonStart);
            this.groupBoxSettings.Controls.Add(this.comboBoxPosition);
            this.groupBoxSettings.Controls.Add(this.labelBBsize);
            this.groupBoxSettings.Controls.Add(this.textBoxChipsUTG);
            this.groupBoxSettings.Controls.Add(this.comboBoxBBsize);
            this.groupBoxSettings.Controls.Add(this.labelPlayersNum);
            this.groupBoxSettings.Controls.Add(this.textBoxChipsBTN);
            this.groupBoxSettings.Controls.Add(this.comboBoxPlayers);
            this.groupBoxSettings.Controls.Add(this.textBoxChipsSB);
            this.groupBoxSettings.Controls.Add(this.labelMode);
            this.groupBoxSettings.Controls.Add(this.textBoxChipsBB);
            this.groupBoxSettings.Controls.Add(this.comboBoxMode);
            this.groupBoxSettings.Controls.Add(this.labelChipsUTG);
            this.groupBoxSettings.Controls.Add(this.label2);
            this.groupBoxSettings.Controls.Add(this.labelChipsBTN);
            this.groupBoxSettings.Controls.Add(this.comboBoxChips);
            this.groupBoxSettings.Controls.Add(this.labelChipsSB);
            this.groupBoxSettings.Controls.Add(this.labelDifficulty);
            this.groupBoxSettings.Controls.Add(this.labelChipsBB);
            this.groupBoxSettings.Controls.Add(this.comboBoxDifficulty);
            this.groupBoxSettings.Controls.Add(this.comboBoxStacks);
            this.groupBoxSettings.Controls.Add(this.labelStacks);
            this.groupBoxSettings.Controls.Add(this.labelPosition);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(183, 285);
            this.groupBoxSettings.TabIndex = 22;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Settings";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(108, 257);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(67, 23);
            this.buttonStart.TabIndex = 22;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Hand,
            this.Action,
            this.EV});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewResults.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewResults.Location = new System.Drawing.Point(14, 33);
            this.dataGridViewResults.MultiSelect = false;
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewResults.RowHeadersWidth = 20;
            this.dataGridViewResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResults.Size = new System.Drawing.Size(163, 162);
            this.dataGridViewResults.TabIndex = 23;
            this.dataGridViewResults.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewResults_RowEnter);
            this.dataGridViewResults.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewResults_CellContentClick);
            // 
            // Hand
            // 
            this.Hand.HeaderText = "Hand";
            this.Hand.Name = "Hand";
            this.Hand.ReadOnly = true;
            this.Hand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Hand.Width = 35;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Action.Width = 40;
            // 
            // EV
            // 
            this.EV.HeaderText = "EV diff.";
            this.EV.Name = "EV";
            this.EV.ReadOnly = true;
            this.EV.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EV.Width = 47;
            // 
            // groupBoxResults
            // 
            this.groupBoxResults.Controls.Add(this.labelResults);
            this.groupBoxResults.Controls.Add(this.dataGridViewResults);
            this.groupBoxResults.Location = new System.Drawing.Point(12, 303);
            this.groupBoxResults.Name = "groupBoxResults";
            this.groupBoxResults.Size = new System.Drawing.Size(183, 196);
            this.groupBoxResults.TabIndex = 24;
            this.groupBoxResults.TabStop = false;
            this.groupBoxResults.Text = "Results";
            // 
            // labelResults
            // 
            this.labelResults.AutoSize = true;
            this.labelResults.Location = new System.Drawing.Point(12, 16);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(0, 13);
            this.labelResults.TabIndex = 24;
            // 
            // buttonFold
            // 
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFold.Location = new System.Drawing.Point(415, 464);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(92, 32);
            this.buttonFold.TabIndex = 26;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.buttonFold_Click);
            // 
            // buttonPush
            // 
            this.buttonPush.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPush.Location = new System.Drawing.Point(524, 464);
            this.buttonPush.Name = "buttonPush";
            this.buttonPush.Size = new System.Drawing.Size(93, 32);
            this.buttonPush.TabIndex = 27;
            this.buttonPush.Text = "Push";
            this.buttonPush.UseVisualStyleBackColor = true;
            this.buttonPush.Click += new System.EventHandler(this.buttonPush_Click);
            // 
            // pictureBoxTable
            // 
            this.pictureBoxTable.BackColor = System.Drawing.Color.Black;
            this.pictureBoxTable.Image = global::SNG_Quiz.Properties.Resources.poyta_rinki_valkoinen2;
            this.pictureBoxTable.Location = new System.Drawing.Point(200, 18);
            this.pictureBoxTable.Name = "pictureBoxTable";
            this.pictureBoxTable.Size = new System.Drawing.Size(625, 440);
            this.pictureBoxTable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxTable.TabIndex = 25;
            this.pictureBoxTable.TabStop = false;
            this.pictureBoxTable.Click += new System.EventHandler(this.pictureBoxTable_Click);
            this.pictureBoxTable.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // myCard1
            // 
            this.myCard1.BackColor = System.Drawing.Color.White;
            this.myCard1.Location = new System.Drawing.Point(564, 344);
            this.myCard1.Name = "myCard1";
            this.myCard1.Size = new System.Drawing.Size(73, 95);
            this.myCard1.TabIndex = 32;
            this.myCard1.TabStop = false;
            // 
            // myCard2
            // 
            this.myCard2.BackColor = System.Drawing.Color.White;
            this.myCard2.Location = new System.Drawing.Point(576, 353);
            this.myCard2.Name = "myCard2";
            this.myCard2.Size = new System.Drawing.Size(73, 95);
            this.myCard2.TabIndex = 33;
            this.myCard2.TabStop = false;
            // 
            // player4card2
            // 
            this.player4card2.BackColor = System.Drawing.Color.White;
            this.player4card2.Location = new System.Drawing.Point(319, 254);
            this.player4card2.Name = "player4card2";
            this.player4card2.Size = new System.Drawing.Size(15, 20);
            this.player4card2.TabIndex = 35;
            this.player4card2.TabStop = false;
            // 
            // player4card1
            // 
            this.player4card1.BackColor = System.Drawing.Color.White;
            this.player4card1.Location = new System.Drawing.Point(315, 249);
            this.player4card1.Name = "player4card1";
            this.player4card1.Size = new System.Drawing.Size(15, 20);
            this.player4card1.TabIndex = 34;
            this.player4card1.TabStop = false;
            // 
            // player1card2
            // 
            this.player1card2.BackColor = System.Drawing.Color.White;
            this.player1card2.Location = new System.Drawing.Point(552, 107);
            this.player1card2.Name = "player1card2";
            this.player1card2.Size = new System.Drawing.Size(15, 20);
            this.player1card2.TabIndex = 37;
            this.player1card2.TabStop = false;
            // 
            // player1card1
            // 
            this.player1card1.BackColor = System.Drawing.Color.White;
            this.player1card1.Location = new System.Drawing.Point(548, 102);
            this.player1card1.Name = "player1card1";
            this.player1card1.Size = new System.Drawing.Size(15, 20);
            this.player1card1.TabIndex = 36;
            this.player1card1.TabStop = false;
            // 
            // player2card2
            // 
            this.player2card2.BackColor = System.Drawing.Color.White;
            this.player2card2.Location = new System.Drawing.Point(704, 254);
            this.player2card2.Name = "player2card2";
            this.player2card2.Size = new System.Drawing.Size(15, 20);
            this.player2card2.TabIndex = 39;
            this.player2card2.TabStop = false;
            // 
            // player2card1
            // 
            this.player2card1.BackColor = System.Drawing.Color.White;
            this.player2card1.Location = new System.Drawing.Point(700, 249);
            this.player2card1.Name = "player2card1";
            this.player2card1.Size = new System.Drawing.Size(15, 20);
            this.player2card1.TabIndex = 38;
            this.player2card1.TabStop = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // timerTooltip
            // 
            this.timerTooltip.Tick += new System.EventHandler(this.timerTooltip_Tick);
            // 
            // labelGameNumber
            // 
            this.labelGameNumber.AutoSize = true;
            this.labelGameNumber.BackColor = System.Drawing.Color.Black;
            this.labelGameNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameNumber.ForeColor = System.Drawing.Color.Yellow;
            this.labelGameNumber.Location = new System.Drawing.Point(210, 30);
            this.labelGameNumber.Name = "labelGameNumber";
            this.labelGameNumber.Size = new System.Drawing.Size(0, 17);
            this.labelGameNumber.TabIndex = 40;
            // 
            // player3
            // 
            this.player3.BackColor = System.Drawing.Color.Transparent;
            this.player3.BetPos = new System.Drawing.Point(22, 12);
            this.player3.Bets = 20000;
            this.player3.Dealer = true;
            this.player3.DealerPos = new System.Drawing.Point(0, 12);
            this.player3.InfoboxPos = new System.Drawing.Point(0, 52);
            this.player3.Location = new System.Drawing.Point(473, 344);
            this.player3.Name = "player3";
            this.player3.Range = 50;
            this.player3.Size = new System.Drawing.Size(200, 114);
            this.player3.Stack = "";
            this.player3.TabIndex = 31;
            this.player3.MouseEnter += new System.EventHandler(this.player1_MouseEnter);
            this.player3.MouseLeave += new System.EventHandler(this.player1_MouseLeave);
            this.player3.MouseHover += new System.EventHandler(this.player1_MouseHover);
            // 
            // player1
            // 
            this.player1.BackColor = System.Drawing.Color.Transparent;
            this.player1.BetPos = new System.Drawing.Point(22, 70);
            this.player1.Bets = 20000;
            this.player1.Dealer = true;
            this.player1.DealerPos = new System.Drawing.Point(0, 70);
            this.player1.InfoboxPos = new System.Drawing.Point(0, 0);
            this.player1.Location = new System.Drawing.Point(473, 30);
            this.player1.Name = "player1";
            this.player1.Range = 50;
            this.player1.Size = new System.Drawing.Size(200, 120);
            this.player1.Stack = "";
            this.player1.TabIndex = 30;
            this.player1.Load += new System.EventHandler(this.player1_Load_1);
            this.player1.MouseEnter += new System.EventHandler(this.player1_MouseEnter);
            this.player1.MouseLeave += new System.EventHandler(this.player1_MouseLeave);
            this.player1.MouseHover += new System.EventHandler(this.player1_MouseHover);
            // 
            // player2
            // 
            this.player2.BackColor = System.Drawing.Color.Transparent;
            this.player2.BetPos = new System.Drawing.Point(20, 67);
            this.player2.Bets = 20000;
            this.player2.Dealer = true;
            this.player2.DealerPos = new System.Drawing.Point(30, 45);
            this.player2.InfoboxPos = new System.Drawing.Point(82, 47);
            this.player2.Location = new System.Drawing.Point(655, 158);
            this.player2.Name = "player2";
            this.player2.Range = 50;
            this.player2.Size = new System.Drawing.Size(170, 130);
            this.player2.Stack = "";
            this.player2.TabIndex = 29;
            this.player2.MouseEnter += new System.EventHandler(this.player1_MouseEnter);
            this.player2.MouseLeave += new System.EventHandler(this.player1_MouseLeave);
            this.player2.MouseHover += new System.EventHandler(this.player1_MouseHover);
            // 
            // player4
            // 
            this.player4.BackColor = System.Drawing.Color.Transparent;
            this.player4.BetPos = new System.Drawing.Point(96, 22);
            this.player4.Bets = 20000;
            this.player4.Dealer = true;
            this.player4.DealerPos = new System.Drawing.Point(100, 0);
            this.player4.InfoboxPos = new System.Drawing.Point(0, 0);
            this.player4.Location = new System.Drawing.Point(213, 206);
            this.player4.Name = "player4";
            this.player4.Range = 50;
            this.player4.Size = new System.Drawing.Size(174, 77);
            this.player4.Stack = "";
            this.player4.TabIndex = 28;
            this.toolTip1.SetToolTip(this.player4, "Joooo");
            this.player4.MouseEnter += new System.EventHandler(this.player1_MouseEnter);
            this.player4.MouseLeave += new System.EventHandler(this.player1_MouseLeave);
            this.player4.MouseHover += new System.EventHandler(this.player1_MouseHover);
            // 
            // SNGquiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 502);
            this.Controls.Add(this.labelGameNumber);
            this.Controls.Add(this.myCard2);
            this.Controls.Add(this.player2card2);
            this.Controls.Add(this.player2card1);
            this.Controls.Add(this.player1card2);
            this.Controls.Add(this.player1card1);
            this.Controls.Add(this.player4card2);
            this.Controls.Add(this.player4card1);
            this.Controls.Add(this.myCard1);
            this.Controls.Add(this.player3);
            this.Controls.Add(this.player1);
            this.Controls.Add(this.player2);
            this.Controls.Add(this.player4);
            this.Controls.Add(this.buttonPush);
            this.Controls.Add(this.buttonFold);
            this.Controls.Add(this.groupBoxResults);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.pictureBoxTable);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SNGquiz";
            this.Text = "SNG Quiz";
            this.Load += new System.EventHandler(this.SNGquiz_Load);
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.groupBoxResults.ResumeLayout(false);
            this.groupBoxResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player4card2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player4card1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player1card2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player1card1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player2card2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player2card1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPlayersNum;
        private System.Windows.Forms.ComboBox comboBoxPlayers;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.ComboBox comboBoxChips;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxDifficulty;
        private System.Windows.Forms.Label labelDifficulty;
        private System.Windows.Forms.ComboBox comboBoxBBsize;
        private System.Windows.Forms.Label labelBBsize;
        private System.Windows.Forms.ComboBox comboBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Label labelStacks;
        private System.Windows.Forms.ComboBox comboBoxStacks;
        private System.Windows.Forms.Label labelChipsBB;
        private System.Windows.Forms.Label labelChipsSB;
        private System.Windows.Forms.Label labelChipsBTN;
        private System.Windows.Forms.Label labelChipsUTG;
        private System.Windows.Forms.TextBox textBoxChipsBB;
        private System.Windows.Forms.TextBox textBoxChipsSB;
        private System.Windows.Forms.TextBox textBoxChipsBTN;
        private System.Windows.Forms.TextBox textBoxChipsUTG;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.GroupBox groupBoxResults;
        private System.Windows.Forms.Label labelResults;
        private System.Windows.Forms.PictureBox pictureBoxTable;
        private System.Windows.Forms.Button buttonFold;
        private System.Windows.Forms.Button buttonPush;
        private Player player4;
        private Player player2;
        private Player player1;
        private Player player3;
        private System.Windows.Forms.PictureBox myCard1;
        private System.Windows.Forms.PictureBox myCard2;
        private System.Windows.Forms.PictureBox player4card2;
        private System.Windows.Forms.PictureBox player4card1;
        private System.Windows.Forms.PictureBox player1card2;
        private System.Windows.Forms.PictureBox player1card1;
        private System.Windows.Forms.PictureBox player2card2;
        private System.Windows.Forms.PictureBox player2card1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn EV;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timerTooltip;
        private System.Windows.Forms.Label labelGameNumber;
    }
}

