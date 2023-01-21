using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftMod.Windows
{
    public partial class CrosshairDesigner : Form
    {
        Bitmap img = null;
        public CrosshairDesigner(Image bmp)
        {
            InitializeComponent();

            img = new Bitmap(bmp);

            RefreshTransparentPanel();

            pictureBox1.Refresh();
        }

        public void RefreshTransparentPanel()
        {
            Bitmap transparentBkg = new Bitmap(img.Width, img.Height);

            using (Graphics g = Graphics.FromImage(transparentBkg))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.Clear(Color.Gray);

                pictureBox1.BackgroundImage = transparentBkg;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            e.Graphics.DrawImage(pictureBox1.BackgroundImage,
                new RectangleF(0, 0, pictureBox1.Width, pictureBox1.Height));

            ImageAttributes imgAttr = new ImageAttributes();
            imgAttr.SetColorKey(Color.Black, Color.Black); // wont be using black for it anyways so why not

            e.Graphics.DrawImage(img,
                new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, // junk
                imgAttr);
        }

        bool Drawing = false, Earasing = false;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Drawing = true;
                    break;

                case MouseButtons.Right:
                    Earasing = true;
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Drawing = false;
                    break;

                case MouseButtons.Right:
                    Earasing = false;
                    break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Minecraft.SetToastCrosshair(img);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = new Point(e.X / (pictureBox1.Width / img.Width), e.Y / (pictureBox1.Height / img.Height));

            if (mousePos.X < 0 || mousePos.Y < 0 || mousePos.X + 1 > img.Width || mousePos.Y + 1 > img.Height)
                return;

            if (Drawing)
                img.SetPixel(mousePos.X, mousePos.Y, Color.White);

            if (Earasing)
                img.SetPixel(mousePos.X, mousePos.Y, Color.Black);

            if (Drawing || Earasing)
                pictureBox1.Invalidate();
        }
    }
}
