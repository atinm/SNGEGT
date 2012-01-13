namespace SNG_Quiz
{
    partial class Player
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxDealer = new System.Windows.Forms.PictureBox();
            
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDealer)).BeginInit();            
            this.SuspendLayout();
            // 
            // pictureBoxDealer
            // 
            this.pictureBoxDealer.Image = global::SNG_Quiz.Properties.Resources.dealer;
            this.pictureBoxDealer.Location = new System.Drawing.Point(62, 49);
            this.pictureBoxDealer.Name = "pictureBoxDealer";
            this.pictureBoxDealer.Size = new System.Drawing.Size(20, 19);
            this.pictureBoxDealer.TabIndex = 0;
            this.pictureBoxDealer.TabStop = false;
            this.pictureBoxDealer.Click += new System.EventHandler(this.pictureBoxDealer_Click);
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBoxDealer);
            this.Name = "Player";
            this.Size = new System.Drawing.Size(309, 260);
            this.Load += new System.EventHandler(this.Player_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Player_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDealer)).EndInit();            
            this.ResumeLayout(false);

        }

        #endregion

        
        private System.Windows.Forms.PictureBox pictureBoxDealer;
        
  
    }
}
