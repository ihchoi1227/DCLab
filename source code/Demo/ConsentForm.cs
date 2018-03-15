using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Demo
{
    public partial class ConsentForm : Form
    {
        int Filenumber = 1;
        private Graphics myGraphics;
        private Graphics myGraphics2;
        Pen P = new Pen(Color.Black, 1);
        private bool isPainting = false;

        public ConsentForm()
        {
            InitializeComponent();
            myGraphics = panel1.CreateGraphics();
            myGraphics2 = panel2.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            main frm1 = new Demo.main();
            frm1.ShowDialog();
        }

        private void ConsentForm_Load(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isPainting = true;
            prevX = e.X;
            prevY = e.Y;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isPainting = false;
            prevX = null;
            prevY = null;
        }
        int? prevX = null;
        int? prevY = null;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPainting == true)
            {   //이전 X,Y에서 현재 X,Y로 라인을 그림
                myGraphics.DrawLine(P, new Point(prevX ?? e.X, prevY ?? e.Y), new Point(e.X, e.Y)); 
                prevX = e.X;
                prevY = e.Y;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            isPainting = true;
            prevX = e.X;
            prevY = e.Y;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPainting == true)
            {
                myGraphics2.DrawLine(P, new Point(prevX ?? e.X, prevY ?? e.Y), new Point(e.X, e.Y));
                prevX = e.X;
                prevY = e.Y;
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            isPainting = false;
            prevX = null;
            prevY = null;
        }

        private void Paint_Clear_Click(object sender, EventArgs e)
        {
            myGraphics.Clear(Color.White);
            Experimenter.Clear();
            Experimenter.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image img = GetAltScreenshot();
            img.Save(@"C:\Users\DCLAB\Desktop\meeting\ConsentForm#" + Filenumber.ToString() + ".jpg");
            MessageBox.Show("Sceen Capture is completely saved");
            Filenumber++;
            img.Dispose();
        }
        public static Bitmap GetAltScreenshot()
        {
            Clipboard.Clear();
            SendKeys.SendWait("{PRTSC}");
            while (!Clipboard.ContainsImage())
            {
                System.Threading.Thread.Sleep(500);
            }
            return new Bitmap(Clipboard.GetImage());
        }

        private void Experimenter_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
