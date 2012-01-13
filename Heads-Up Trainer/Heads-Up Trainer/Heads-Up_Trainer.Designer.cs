namespace HeadsUp_Trainer
{
    partial class Heads_Up_Trainer
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
            this.textBoxDealer = new System.Windows.Forms.TextBox();
            this.buttonFold = new System.Windows.Forms.Button();
            this.buttonAllIn = new System.Windows.Forms.Button();
            this.pictureBoxTable = new System.Windows.Forms.PictureBox();
            this.myCard1 = new System.Windows.Forms.PictureBox();
            this.myCard2 = new System.Windows.Forms.PictureBox();
            this.oppCard1 = new System.Windows.Forms.PictureBox();
            this.oppCard2 = new System.Windows.Forms.PictureBox();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.labelBlinds = new System.Windows.Forms.Label();
            this.labelBlindsLeft = new System.Windows.Forms.Label();
            this.pictureBoxBoard1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxBoard2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxBoard3 = new System.Windows.Forms.PictureBox();
            this.pictureBoxBoard4 = new System.Windows.Forms.PictureBox();
            this.pictureBoxBoard5 = new System.Windows.Forms.PictureBox();
            this.timerStartGame = new System.Windows.Forms.Timer(this.components);
            this.timerOppAction = new System.Windows.Forms.Timer(this.components);
            this.timerShowndown = new System.Windows.Forms.Timer(this.components);
            this.labelWins = new System.Windows.Forms.Label();
            this.labelLosses = new System.Windows.Forms.Label();
            this.labelWinner = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.playerMe = new HeadsUp_Trainer.Player();
            this.playerOpp = new HeadsUp_Trainer.Player();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oppCard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oppCard2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDealer
            // 
            this.textBoxDealer.Location = new System.Drawing.Point(0, 407);
            this.textBoxDealer.Multiline = true;
            this.textBoxDealer.Name = "textBoxDealer";
            this.textBoxDealer.ReadOnly = true;
            this.textBoxDealer.Size = new System.Drawing.Size(325, 69);
            this.textBoxDealer.TabIndex = 1;
            // 
            // buttonFold
            // 
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFold.Location = new System.Drawing.Point(336, 417);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(120, 48);
            this.buttonFold.TabIndex = 2;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.buttonFold_Click);
            // 
            // buttonAllIn
            // 
            this.buttonAllIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAllIn.Location = new System.Drawing.Point(467, 417);
            this.buttonAllIn.Name = "buttonAllIn";
            this.buttonAllIn.Size = new System.Drawing.Size(120, 48);
            this.buttonAllIn.TabIndex = 3;
            this.buttonAllIn.Text = "All-In";
            this.buttonAllIn.UseVisualStyleBackColor = true;
            this.buttonAllIn.Click += new System.EventHandler(this.buttonAllIn_Click);
            // 
            // pictureBoxTable
            // 
            this.pictureBoxTable.Image = global::HeadsUp_Trainer.Properties.Resources.table;
            this.pictureBoxTable.Location = new System.Drawing.Point(0, -28);
            this.pictureBoxTable.Name = "pictureBoxTable";
            this.pictureBoxTable.Size = new System.Drawing.Size(602, 434);
            this.pictureBoxTable.TabIndex = 0;
            this.pictureBoxTable.TabStop = false;
            this.pictureBoxTable.Click += new System.EventHandler(this.pictureBoxTable_Click);
            // 
            // myCard1
            // 
            this.myCard1.BackColor = System.Drawing.Color.Transparent;
            this.myCard1.Location = new System.Drawing.Point(352, 287);
            this.myCard1.Name = "myCard1";
            this.myCard1.Size = new System.Drawing.Size(71, 95);
            this.myCard1.TabIndex = 6;
            this.myCard1.TabStop = false;
            // 
            // myCard2
            // 
            this.myCard2.BackColor = System.Drawing.Color.Transparent;
            this.myCard2.Location = new System.Drawing.Point(364, 297);
            this.myCard2.Name = "myCard2";
            this.myCard2.Size = new System.Drawing.Size(71, 95);
            this.myCard2.TabIndex = 7;
            this.myCard2.TabStop = false;
            this.myCard2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // oppCard1
            // 
            this.oppCard1.BackColor = System.Drawing.Color.Transparent;
            this.oppCard1.Location = new System.Drawing.Point(352, 7);
            this.oppCard1.Name = "oppCard1";
            this.oppCard1.Size = new System.Drawing.Size(71, 95);
            this.oppCard1.TabIndex = 8;
            this.oppCard1.TabStop = false;
            // 
            // oppCard2
            // 
            this.oppCard2.BackColor = System.Drawing.Color.Transparent;
            this.oppCard2.Location = new System.Drawing.Point(364, 17);
            this.oppCard2.Name = "oppCard2";
            this.oppCard2.Size = new System.Drawing.Size(71, 95);
            this.oppCard2.TabIndex = 9;
            this.oppCard2.TabStop = false;
            // 
            // timerStart
            // 
            this.timerStart.Interval = 1000;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // labelBlinds
            // 
            this.labelBlinds.AutoSize = true;
            this.labelBlinds.BackColor = System.Drawing.Color.Black;
            this.labelBlinds.ForeColor = System.Drawing.Color.Yellow;
            this.labelBlinds.Location = new System.Drawing.Point(13, 13);
            this.labelBlinds.Name = "labelBlinds";
            this.labelBlinds.Size = new System.Drawing.Size(0, 13);
            this.labelBlinds.TabIndex = 10;
            // 
            // labelBlindsLeft
            // 
            this.labelBlindsLeft.AutoSize = true;
            this.labelBlindsLeft.BackColor = System.Drawing.Color.Black;
            this.labelBlindsLeft.ForeColor = System.Drawing.Color.Yellow;
            this.labelBlindsLeft.Location = new System.Drawing.Point(13, 26);
            this.labelBlindsLeft.Name = "labelBlindsLeft";
            this.labelBlindsLeft.Size = new System.Drawing.Size(0, 13);
            this.labelBlindsLeft.TabIndex = 11;
            // 
            // pictureBoxBoard1
            // 
            this.pictureBoxBoard1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBoard1.Location = new System.Drawing.Point(113, 142);
            this.pictureBoxBoard1.Name = "pictureBoxBoard1";
            this.pictureBoxBoard1.Size = new System.Drawing.Size(71, 95);
            this.pictureBoxBoard1.TabIndex = 12;
            this.pictureBoxBoard1.TabStop = false;
            // 
            // pictureBoxBoard2
            // 
            this.pictureBoxBoard2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBoard2.Location = new System.Drawing.Point(192, 142);
            this.pictureBoxBoard2.Name = "pictureBoxBoard2";
            this.pictureBoxBoard2.Size = new System.Drawing.Size(71, 95);
            this.pictureBoxBoard2.TabIndex = 13;
            this.pictureBoxBoard2.TabStop = false;
            // 
            // pictureBoxBoard3
            // 
            this.pictureBoxBoard3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBoard3.Location = new System.Drawing.Point(271, 142);
            this.pictureBoxBoard3.Name = "pictureBoxBoard3";
            this.pictureBoxBoard3.Size = new System.Drawing.Size(71, 95);
            this.pictureBoxBoard3.TabIndex = 14;
            this.pictureBoxBoard3.TabStop = false;
            this.pictureBoxBoard3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBoxBoard4
            // 
            this.pictureBoxBoard4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBoard4.Location = new System.Drawing.Point(350, 142);
            this.pictureBoxBoard4.Name = "pictureBoxBoard4";
            this.pictureBoxBoard4.Size = new System.Drawing.Size(71, 95);
            this.pictureBoxBoard4.TabIndex = 15;
            this.pictureBoxBoard4.TabStop = false;
            // 
            // pictureBoxBoard5
            // 
            this.pictureBoxBoard5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBoard5.Location = new System.Drawing.Point(429, 142);
            this.pictureBoxBoard5.Name = "pictureBoxBoard5";
            this.pictureBoxBoard5.Size = new System.Drawing.Size(71, 95);
            this.pictureBoxBoard5.TabIndex = 16;
            this.pictureBoxBoard5.TabStop = false;
            // 
            // timerStartGame
            // 
            this.timerStartGame.Interval = 1000;
            this.timerStartGame.Tick += new System.EventHandler(this.timerStartGame_Tick);
            // 
            // timerOppAction
            // 
            this.timerOppAction.Interval = 1000;
            this.timerOppAction.Tick += new System.EventHandler(this.timerOppAction_Tick);
            // 
            // timerShowndown
            // 
            this.timerShowndown.Interval = 1000;
            this.timerShowndown.Tick += new System.EventHandler(this.timerShowndown_Tick);
            // 
            // labelWins
            // 
            this.labelWins.AutoSize = true;
            this.labelWins.BackColor = System.Drawing.Color.Black;
            this.labelWins.ForeColor = System.Drawing.Color.Yellow;
            this.labelWins.Location = new System.Drawing.Point(14, 39);
            this.labelWins.Name = "labelWins";
            this.labelWins.Size = new System.Drawing.Size(0, 13);
            this.labelWins.TabIndex = 17;
            // 
            // labelLosses
            // 
            this.labelLosses.AutoSize = true;
            this.labelLosses.BackColor = System.Drawing.Color.Black;
            this.labelLosses.ForeColor = System.Drawing.Color.Yellow;
            this.labelLosses.Location = new System.Drawing.Point(14, 51);
            this.labelLosses.Name = "labelLosses";
            this.labelLosses.Size = new System.Drawing.Size(0, 13);
            this.labelLosses.TabIndex = 18;
            // 
            // labelWinner
            // 
            this.labelWinner.AutoSize = true;
            this.labelWinner.BackColor = System.Drawing.Color.Black;
            this.labelWinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWinner.ForeColor = System.Drawing.Color.Yellow;
            this.labelWinner.Location = new System.Drawing.Point(268, 115);
            this.labelWinner.Name = "labelWinner";
            this.labelWinner.Size = new System.Drawing.Size(77, 24);
            this.labelWinner.TabIndex = 19;
            this.labelWinner.Text = "You lost";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(17, 211);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(71, 95);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // playerMe
            // 
            this.playerMe.BackColor = System.Drawing.Color.Transparent;
            this.playerMe.BetPos = new System.Drawing.Point(22, 5);
            this.playerMe.Bets = 20000;
            this.playerMe.Dealer = true;
            this.playerMe.DealerPos = new System.Drawing.Point(0, 5);
            this.playerMe.InfoboxPos = new System.Drawing.Point(0, 50);
            this.playerMe.Location = new System.Drawing.Point(263, 301);
            this.playerMe.Name = "playerMe";
            this.playerMe.Size = new System.Drawing.Size(124, 100);
            this.playerMe.Stack = "20000";
            this.playerMe.TabIndex = 5;
            // 
            // playerOpp
            // 
            this.playerOpp.BackColor = System.Drawing.Color.Transparent;
            this.playerOpp.BetPos = new System.Drawing.Point(22, 50);
            this.playerOpp.Bets = 20000;
            this.playerOpp.Dealer = true;
            this.playerOpp.DealerPos = new System.Drawing.Point(0, 50);
            this.playerOpp.InfoboxPos = new System.Drawing.Point(0, 0);
            this.playerOpp.Location = new System.Drawing.Point(263, 7);
            this.playerOpp.Name = "playerOpp";
            this.playerOpp.Size = new System.Drawing.Size(83, 87);
            this.playerOpp.Stack = "20000";
            this.playerOpp.TabIndex = 4;
            // 
            // Heads_Up_Trainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(600, 478);
            this.Controls.Add(this.pictureBoxBoard5);
            this.Controls.Add(this.pictureBoxBoard4);
            this.Controls.Add(this.pictureBoxBoard3);
            this.Controls.Add(this.pictureBoxBoard2);
            this.Controls.Add(this.pictureBoxBoard1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelWinner);
            this.Controls.Add(this.labelLosses);
            this.Controls.Add(this.labelWins);
            this.Controls.Add(this.labelBlindsLeft);
            this.Controls.Add(this.labelBlinds);
            this.Controls.Add(this.oppCard2);
            this.Controls.Add(this.oppCard1);
            this.Controls.Add(this.myCard2);
            this.Controls.Add(this.myCard1);
            this.Controls.Add(this.playerMe);
            this.Controls.Add(this.playerOpp);
            this.Controls.Add(this.buttonAllIn);
            this.Controls.Add(this.buttonFold);
            this.Controls.Add(this.textBoxDealer);
            this.Controls.Add(this.pictureBoxTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Heads_Up_Trainer";
            this.Text = "Heads-Up Trainer";
            this.ResizeBegin += new System.EventHandler(this.Heads_Up_Trainer_ResizeBegin);
            this.Resize += new System.EventHandler(this.Heads_Up_Trainer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myCard2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oppCard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oppCard2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTable;
        private System.Windows.Forms.TextBox textBoxDealer;
        private System.Windows.Forms.Button buttonFold;
        private System.Windows.Forms.Button buttonAllIn;
        private Player playerOpp;
        private Player playerMe;
        private System.Windows.Forms.PictureBox myCard1;
        private System.Windows.Forms.PictureBox myCard2;
        private System.Windows.Forms.PictureBox oppCard1;
        private System.Windows.Forms.PictureBox oppCard2;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Label labelBlinds;
        private System.Windows.Forms.Label labelBlindsLeft;
        private System.Windows.Forms.PictureBox pictureBoxBoard1;
        private System.Windows.Forms.PictureBox pictureBoxBoard2;
        private System.Windows.Forms.PictureBox pictureBoxBoard3;
        private System.Windows.Forms.PictureBox pictureBoxBoard4;
        private System.Windows.Forms.PictureBox pictureBoxBoard5;
        private System.Windows.Forms.Timer timerStartGame;
        private System.Windows.Forms.Timer timerOppAction;
        private System.Windows.Forms.Timer timerShowndown;
        private System.Windows.Forms.Label labelWins;
        private System.Windows.Forms.Label labelLosses;
        private System.Windows.Forms.Label labelWinner;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

