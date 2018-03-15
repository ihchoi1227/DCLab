using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class main : Form
    {
        public static int experiment_number = 0;
        public static string Experimenter = "NA";
        public main()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e) //Experiment Information
        {
            this.Hide();

            Personal_Informaion frm2 = new Demo.Personal_Informaion();
            frm2.ShowDialog();
        }
        private void button5_Click(object sender, EventArgs e) //exit
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)//Sensor
        {
            this.Hide();
            Sensor frm3 = new Demo.Sensor();
            frm3.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) //Consent Form
        {
            this.Hide();
            ConsentForm frm1 = new Demo.ConsentForm();
            frm1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e) //Database
        {
            this.Hide();
            Sensor_Value frm4 = new Demo.Sensor_Value();
            frm4.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void databaseBox_Click(object sender, EventArgs e)
        {
            this.Hide();
            Sensor_Value dForm = new Demo.Sensor_Value();
            dForm.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void infoBox_Click(object sender, EventArgs e)
        {
            this.Hide();
            Personal_Informaion pForm = new Demo.Personal_Informaion();
            pForm.ShowDialog();
          //  ConsentForm cForm = new Demo.ConsentForm();
          //  cForm.ShowDialog();
        }

        private void sensorBox_Click(object sender, EventArgs e)
        {
            this.Hide();
            Sensor sForm = new Demo.Sensor();
            sForm.ShowDialog();
        }
    }
}
