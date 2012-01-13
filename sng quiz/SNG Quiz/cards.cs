using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;


namespace SNG_Quiz
{
    public class Cards
    {
        private static readonly Bitmap[] cards = {
            global::SNG_Quiz.Properties.Resources.cl2,
            global::SNG_Quiz.Properties.Resources.cl3,
            global::SNG_Quiz.Properties.Resources.cl4,
            global::SNG_Quiz.Properties.Resources.cl5,
            global::SNG_Quiz.Properties.Resources.cl6,
            global::SNG_Quiz.Properties.Resources.cl7,
            global::SNG_Quiz.Properties.Resources.cl8,
            global::SNG_Quiz.Properties.Resources.cl9,
            global::SNG_Quiz.Properties.Resources.cl10,
            global::SNG_Quiz.Properties.Resources.clj,
            global::SNG_Quiz.Properties.Resources.clq,
            global::SNG_Quiz.Properties.Resources.clk,
            global::SNG_Quiz.Properties.Resources.cla,

            global::SNG_Quiz.Properties.Resources.di2,
            global::SNG_Quiz.Properties.Resources.di3,
            global::SNG_Quiz.Properties.Resources.di4,
            global::SNG_Quiz.Properties.Resources.di5,
            global::SNG_Quiz.Properties.Resources.di6,
            global::SNG_Quiz.Properties.Resources.di7,
            global::SNG_Quiz.Properties.Resources.di8,
            global::SNG_Quiz.Properties.Resources.di9,
            global::SNG_Quiz.Properties.Resources.di10,
            global::SNG_Quiz.Properties.Resources.dij,
            global::SNG_Quiz.Properties.Resources.diq,
            global::SNG_Quiz.Properties.Resources.dik,
            global::SNG_Quiz.Properties.Resources.dia,

            global::SNG_Quiz.Properties.Resources.he2,
            global::SNG_Quiz.Properties.Resources.he3,
            global::SNG_Quiz.Properties.Resources.he4,
            global::SNG_Quiz.Properties.Resources.he5,
            global::SNG_Quiz.Properties.Resources.he6,
            global::SNG_Quiz.Properties.Resources.he7,
            global::SNG_Quiz.Properties.Resources.he8,
            global::SNG_Quiz.Properties.Resources.he9,
            global::SNG_Quiz.Properties.Resources.he10,
            global::SNG_Quiz.Properties.Resources.hej,
            global::SNG_Quiz.Properties.Resources.heq,
            global::SNG_Quiz.Properties.Resources.hek,
            global::SNG_Quiz.Properties.Resources.hea,

            global::SNG_Quiz.Properties.Resources.sp2,
            global::SNG_Quiz.Properties.Resources.sp3,
            global::SNG_Quiz.Properties.Resources.sp4,
            global::SNG_Quiz.Properties.Resources.sp5,
            global::SNG_Quiz.Properties.Resources.sp6,
            global::SNG_Quiz.Properties.Resources.sp7,
            global::SNG_Quiz.Properties.Resources.sp8,
            global::SNG_Quiz.Properties.Resources.sp9,
            global::SNG_Quiz.Properties.Resources.sp10,
            global::SNG_Quiz.Properties.Resources.spj,
            global::SNG_Quiz.Properties.Resources.spq,
            global::SNG_Quiz.Properties.Resources.spk,
            global::SNG_Quiz.Properties.Resources.spa
        };

        /* mode parameters */
        public const int mdFaceUp = 0; /* Draw card face up */
        public const int mdFaceDown = 1; /* Draw card face down */

        public static bool drawCardBack(PictureBox pb)
        {
            try
            {
                pb.Image = global::SNG_Quiz.Properties.Resources.cardSkin;
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
