using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;


namespace Demo
{
    public partial class Personal_Informaion : Form
    {
        String Number;
        String AnswerAge, Answer2, Answer3, Answer4, Answer5, Answer6, AnswerHeight, AnswerWeight;
        bool isAnswerAge, isAnswerHeight, isAnswerWeight, isSignName = false;

    
        private Graphics myGraphics;
        Pen P = new Pen(Color.Black, 1);
        private bool isPainting = false;

        public Personal_Informaion()
        {
            InitializeComponent();
            myGraphics = panel3.CreateGraphics();
            All_Default_Set();
        }

        private void Personal_Informaion_Load(object sender, EventArgs e) //디폴트값 설정
        {
            ShowNum.Text = main.experiment_number.ToString();
        }
        private void Ctrl_KeyUp(object sender, KeyEventArgs e) //'Enter'입력시 다음 입력란 커서로 move
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        public void RadioButton_Check()
        {
            //4번
            if (AnswerButton2_1.Checked)
            {
                Answer2 = AnswerButton2_1.Text.ToString();
            }
            else if (AnswerButton2_2.Checked)
            {
                Answer2 = AnswerButton2_2.Text.ToString();
            }

            //5번
            if (AnswerButton3_1.Checked)
            {
                Answer3 = AnswerButton3_1.Text.ToString();
            }
            else if (AnswerButton3_2.Checked)
            {
                Answer3 = AnswerButton3_3.Text.ToString();
            }
            else if (AnswerButton3_3.Checked)
            {
                Answer3 = AnswerButton3_3.Text.ToString();
            }
            else if (AnswerButton3_4.Checked)
            {
                Answer3 = AnswerButton3_4.Text.ToString();
            }
            else if (AnswerButton3_5.Checked)
            {
                Answer3 = AnswerButton3_5.Text.ToString();
            }
            //6번
            if (AnswerButton4_1.Checked)
            {
                Answer4 = AnswerButton4_1.Text.ToString();
            }
            else if (AnswerButton4_2.Checked)
            {
                Answer4 = AnswerButton4_3.Text.ToString();
            }
            else if (AnswerButton4_3.Checked)
            {
                Answer4 = AnswerButton4_3.Text.ToString();
            }
            else if (AnswerButton4_4.Checked)
            {
                Answer4 = AnswerButton4_4.Text.ToString();
            }
            else if (AnswerButton4_5.Checked)
            {
                Answer4 = AnswerButton4_5.Text.ToString();
            }
            //7번
            if (AnswerButton5_1.Checked)
            {
                Answer5 = AnswerButton5_1.Text.ToString();
            }
            else if (AnswerButton5_2.Checked)
            {
                Answer5 = AnswerButton5_3.Text.ToString();
            }
            else if (AnswerButton5_3.Checked)
            {
                Answer5 = AnswerButton5_3.Text.ToString();
            }
            else if (AnswerButton5_4.Checked)
            {
                Answer5 = AnswerButton5_4.Text.ToString();
            }
            else if (AnswerButton5_5.Checked)
            {
                Answer5 = AnswerButton5_5.Text.ToString();
            }
            //8번
            if (AnswerButton6_1.Checked)
            {
                Answer6 = AnswerButton6_1.Text.ToString();
            }
            else if (AnswerButton6_2.Checked)
            {
                Answer6 = AnswerButton6_2.Text.ToString();
            }
            else if (AnswerButton6_3.Checked)
            {
                Answer6 = AnswerButton6_3.Text.ToString();
            }
            else if (AnswerButton6_4.Checked)
            {
                Answer6 = AnswerButton6_4.Text.ToString();
            }
        }
        public void All_Default_Set()  //입력값 초기화
        {
            AgeBox.Text = AgeBox.Items[0].ToString();
            HeightBox.Text = HeightBox.Items[0].ToString();
            WeightBox.Text = WeightBox.Items[0].ToString();
            AnswerButton2_1.Checked = true;
            AnswerButton3_1.Checked = true;
            AnswerButton4_1.Checked = true;
            AnswerButton5_1.Checked = true;
            AnswerButton6_1.Checked = true;
        }
        public void All_Answer_Clear() //모든 Answer를 false로 초기화
        {
            isAnswerAge = false;
            isAnswerHeight = false;
            isAnswerWeight = false;
            isSignName = false;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            isPainting = true;
            prevX = e.X;
            prevY = e.Y;
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            isPainting = false;
            prevX = null;
            prevY = null;
        }
        int? prevX = null;
        int? prevY = null;

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPainting == true)
            {   //이전 X,Y에서 현재 X,Y로 라인을 그림
                myGraphics.DrawLine(P, new Point(prevX ?? e.X, prevY ?? e.Y), new Point(e.X, e.Y));
                prevX = e.X;
                prevY = e.Y;
            }
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();

            main frm1 = new Demo.main();
            frm1.ShowDialog();
        }
        //실험자 정보 및 서명 저장
        private void Input_Click(object sender, EventArgs e)
        {
            //실험번호
            main.experiment_number++;
            main.Experimenter = "#S" + Experimenter.Text;
            Number = "S" + Experimenter.Text;
   
            //이름 사인
            if (!String.Equals(NameSign.Text, ""))
                isSignName= true;
            else
                isSignName = false;

            //1번
            AnswerAge = AgeBox.SelectedItem.ToString();
            if (AgeBox.SelectedIndex != 0)
                isAnswerAge = true;
            else
                isAnswerAge = false;

            //2번
            AnswerHeight = HeightBox.SelectedItem.ToString();
            if (HeightBox.SelectedIndex != 0)
                isAnswerHeight = true;
            else
                isAnswerHeight = false;

            //3번
            AnswerWeight = WeightBox.SelectedItem.ToString();
            if (WeightBox.SelectedIndex != 0)
                isAnswerWeight = true;
            else
                isAnswerWeight = false;

            RadioButton_Check();

            //질문에 모두 대답을 하지 않은 경우 예외 처리
            if ((!isAnswerAge) || (!isAnswerHeight) || (!isAnswerWeight) || (!isSignName))
            {

                MessageBox.Show("Answer is not finished!, Please, Check your Answer!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                main.experiment_number--;
            }
            else
            {
                //각 실험자 폴더가 존재하지 않을 시 폴더 생성후 저장
                DirectoryInfo dir = new DirectoryInfo(@"C:\data\" + main.Experimenter);
                if(dir.Exists == false)
                {
                    dir.Create(); 
                }
                
                //실험자 정보 저장 및 서명 캡쳐 부분
                using (StreamWriter pi = new StreamWriter(@"C:\data\Experimenter.csv", true, Encoding.UTF8))
                {
                    if (main.experiment_number == 1)
                    {
                        pi.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", "번호", "이름", "나이", "키", "몸무게", "성별", "음주", "흡연", "커피", "스트레스");
                        pi.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",  Number, NameSign.Text, AnswerAge, AnswerHeight, AnswerWeight, Answer2, Answer3, Answer4, Answer5, Answer6);
                    }
                    else
                        pi.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", Number, NameSign.Text, AnswerAge, AnswerHeight, AnswerWeight, Answer2, Answer3, Answer4, Answer5, Answer6);
                }   
                Rectangle rect = new Rectangle(1470, 1300, 1070, 420);
                Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                DirectoryInfo dir2 = new DirectoryInfo(@"C:\data\" + "Personal_Sign");
                //Personal_Sign폴더가 존재하지 않을 시 폴더 생성후 저장
                if (dir2.Exists == false)
                {
                    dir2.Create();
                }
                bmp.Save(@"C:\data\Personal_Sign\" + main.Experimenter +  ".jpg", ImageFormat.Jpeg);

                MessageBox.Show("Data is completely saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                myGraphics.Clear(Color.White);
                NameSign.Clear();
                Experimenter.Clear();
                Experimenter.Focus();
         
                ShowNum.Text = main.experiment_number.ToString();
                All_Default_Set();
                All_Answer_Clear();
               
            }

 
        }

        //동의서 이름,싸인 작성 초기화 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            myGraphics.Clear(Color.White);
            NameSign.Clear();
            NameSign.Focus();
        }
    }
}
