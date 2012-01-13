using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace HeadsUp_Trainer
{
    public partial class Player : UserControl
    {
        private bool idealer;
        private string istack;
        //private double irange;
        private double ibets;
        private Point iinfoboxloc;
        private Point ibetPos;
        public PictureBox card1;
        public PictureBox card2;


        public void resetVars()
        {
            idealer = false;
            istack = "";
            //irange = 0;
            ibets = 0;
        }


        public void showCards(bool show)
        {
            if (show == true)
            {
                card1.BringToFront();
                card1.Show();

                card2.BringToFront();
                card2.Show();
            }
            else
            {
                card1.Hide();
                card2.Hide();
            }
        }


        public string Stack
        {
            get
            {
                return istack;
            }
            set
            {
                istack = value;
                Invalidate(false);
            }
        }

        /*
        public double Range
        {
            get
            {
                return irange;
            }
            set
            {
                irange = value;
                Invalidate(false);
            }
        }
        */

        public double Bets
        {
            get
            {
                return ibets;
            }
            set
            {
                ibets = value;
                Invalidate(true);               
            }
        }



        public Point InfoboxPos
        {
            get
            {
                return iinfoboxloc;
            }
            set
            {
                iinfoboxloc = value;
                Invalidate(false);
            }
        }


        public Point BetPos
        {
            get
            {
                return ibetPos;
            }
            set
            {
                ibetPos = value;
                Invalidate(false);
            }
        }


        public bool Dealer
        {
            get
            {
                return idealer;
            }
            set
            {
                idealer = value;
                pictureBoxDealer.Visible = value;
            }
        }


        public Point DealerPos
        {
            get
            {
                return pictureBoxDealer.Location;
            }
            set
            {
                pictureBoxDealer.Location = value;
            }
        }         

              
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        
        protected void InvalidateEx()
        {
            if (Parent == null)
                return;

            Rectangle rc = new Rectangle(this.Location, this.Size);
            Parent.Invalidate(rc, true);
        }


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //do not allow the background to be painted 
        }

        
        public Player()
        {
            InitializeComponent();            
            InfoboxPos = new Point(0, 0);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);            
        }


        private void Player_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics gb = Graphics.FromImage(bmp);
            
            // boksi
            gb.Clear(Color.Transparent);
            Font f = new Font("Arial",12, FontStyle.Bold);
            Pen pen = new Pen(Color.FromArgb(138,159,252));
            gb.DrawRectangle(pen, InfoboxPos.X, InfoboxPos.Y, 80, 30);
            
            // stäkki, ranget ja betti 
            if(Stack != "")
                gb.DrawString(Stack, f, Brushes.Yellow, new Point(InfoboxPos.X + 5, InfoboxPos.Y + 5));
            //if(Range > 0)
            //    gb.DrawString(Convert.ToString(Range) + "%", f, Brushes.White, new Point(InfoboxPos.X + 5, InfoboxPos.Y + 25));                
            if (Bets > 0)
                gb.DrawString(Convert.ToString(Bets), f, Brushes.White, BetPos);
            
            // jakajan nappi
            if (Dealer == true)
                pictureBoxDealer.Show();
            else
                pictureBoxDealer.Hide();
           
            e.Graphics.DrawImage(bmp, new Point(0, 0));            
            gb.Dispose();
        }


        public void setCardImage(Image img)
        {
            if (img == null)
            {                
                return;
            }            

        }


        private void Player_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxDealer_Click(object sender, EventArgs e)
        {

        }
    }
}
