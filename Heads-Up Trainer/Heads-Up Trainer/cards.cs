using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;


namespace HeadsUp_Trainer
{
    public class Cards
    {
        private static readonly Bitmap[] cards = {
            global::HeadsUp_Trainer.Properties.Resources.cl2,
            global::HeadsUp_Trainer.Properties.Resources.cl3,
            global::HeadsUp_Trainer.Properties.Resources.cl4,
            global::HeadsUp_Trainer.Properties.Resources.cl5,
            global::HeadsUp_Trainer.Properties.Resources.cl6,
            global::HeadsUp_Trainer.Properties.Resources.cl7,
            global::HeadsUp_Trainer.Properties.Resources.cl8,
            global::HeadsUp_Trainer.Properties.Resources.cl9,
            global::HeadsUp_Trainer.Properties.Resources.cl10,
            global::HeadsUp_Trainer.Properties.Resources.clj,
            global::HeadsUp_Trainer.Properties.Resources.clq,
            global::HeadsUp_Trainer.Properties.Resources.clk,
            global::HeadsUp_Trainer.Properties.Resources.cla,

            global::HeadsUp_Trainer.Properties.Resources.di2,
            global::HeadsUp_Trainer.Properties.Resources.di3,
            global::HeadsUp_Trainer.Properties.Resources.di4,
            global::HeadsUp_Trainer.Properties.Resources.di5,
            global::HeadsUp_Trainer.Properties.Resources.di6,
            global::HeadsUp_Trainer.Properties.Resources.di7,
            global::HeadsUp_Trainer.Properties.Resources.di8,
            global::HeadsUp_Trainer.Properties.Resources.di9,
            global::HeadsUp_Trainer.Properties.Resources.di10,
            global::HeadsUp_Trainer.Properties.Resources.dij,
            global::HeadsUp_Trainer.Properties.Resources.diq,
            global::HeadsUp_Trainer.Properties.Resources.dik,
            global::HeadsUp_Trainer.Properties.Resources.dia,

            global::HeadsUp_Trainer.Properties.Resources.he2,
            global::HeadsUp_Trainer.Properties.Resources.he3,
            global::HeadsUp_Trainer.Properties.Resources.he4,
            global::HeadsUp_Trainer.Properties.Resources.he5,
            global::HeadsUp_Trainer.Properties.Resources.he6,
            global::HeadsUp_Trainer.Properties.Resources.he7,
            global::HeadsUp_Trainer.Properties.Resources.he8,
            global::HeadsUp_Trainer.Properties.Resources.he9,
            global::HeadsUp_Trainer.Properties.Resources.he10,
            global::HeadsUp_Trainer.Properties.Resources.hej,
            global::HeadsUp_Trainer.Properties.Resources.heq,
            global::HeadsUp_Trainer.Properties.Resources.hek,
            global::HeadsUp_Trainer.Properties.Resources.hea,

            global::HeadsUp_Trainer.Properties.Resources.sp2,
            global::HeadsUp_Trainer.Properties.Resources.sp3,
            global::HeadsUp_Trainer.Properties.Resources.sp4,
            global::HeadsUp_Trainer.Properties.Resources.sp5,
            global::HeadsUp_Trainer.Properties.Resources.sp6,
            global::HeadsUp_Trainer.Properties.Resources.sp7,
            global::HeadsUp_Trainer.Properties.Resources.sp8,
            global::HeadsUp_Trainer.Properties.Resources.sp9,
            global::HeadsUp_Trainer.Properties.Resources.sp10,
            global::HeadsUp_Trainer.Properties.Resources.spj,
            global::HeadsUp_Trainer.Properties.Resources.spq,
            global::HeadsUp_Trainer.Properties.Resources.spk,
            global::HeadsUp_Trainer.Properties.Resources.spa
        };

        /* mode parameters */
        public const int mdFaceUp = 0; /* Draw card face up */
        public const int mdFaceDown = 1; /* Draw card face down */

        public static bool drawCardBack(PictureBox pb)
        {
            try
            {
                pb.Image = global::HeadsUp_Trainer.Properties.Resources.cardSkin;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Card images are not loading correctly.  Make sure all card images are in the right location.");
                return false;
            }
        }

        public static bool drawCard(PictureBox pb, int card, int mode)
        {
            if (mode == mdFaceDown)
                return drawCardBack(pb);

            try
            {
                pb.Image = cards[card];
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Card images are not loading correctly.  Make sure all card images are in the right location.");
                return false;
            }
        }
    }
}
