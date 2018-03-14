using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace Demo
{
    public partial class Sensor_Value : Form
    {
        Timer timer;
        String[] linearray = new String[100];

        int bvpCnt;
        double bvpMin = Int32.MaxValue;
        double bvpMax = Int32.MinValue;

        double[] bvpStamp = new double[500000];
        double[] bvpValue = new double[500000]; // 데이터파일에서 읽어온 데이터를 저장할 배열

        int accCnt;
        double accMin;
        double accMax;
        double[] accStamp = new double[500000];
        double[] accValue1 = new double[500000];
        double[] accValue2 = new double[500000];
        double[] accValue3 = new double[500000];

        int gsrCnt;
        double gsrMin;
        double gsrMax;
        double[] gsrStamp = new double[500000];
        double[] gsrValue = new double[500000];

        int zephyrCnt;
        double zephyrMin;
        double zephyrMax;
        double[] zephyrStamp = new double[500000];
        double[] zephyrValue = new double[500000];

        private int Tick = 0; // 현재 폼에서 타이머의 Tick 값
        private int duration = 2; // 초기 interval 값 = 200


        public Sensor_Value()
        {
            InitializeComponent();
        }

        /*
        private void btnOpen_Click(object sender, EventArgs e) // Open 버튼일 눌렸을때 파일 디렉토리에서 파일을 가져와서 올리는 함수
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                ValidateNames = true,
                Filter = "TXT|*.txt|WMV|*.wmv|WAV|*.wav|MP3|*.mp3|MP4|*.mp4|MKV|*.mkv"
            })
            {
              
                
                string[] lines = System.IO.File.ReadAllLines(filename);
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    List<CSVFile> files = new List<CSVFile>();
                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(fileName);
                        files.Add(new CSVFile() { FileName = Path.GetFileName(fi.FullName), Path = fi.FullName });

                    }
                    Subject1.DataSource = files;
                    Subject1.ValueMember = "Path";
                    Subject1.DisplayMember = "FileName";
                }
            }
        }
         */
        private void listFile_SelectedIndexChanged(object sender, EventArgs e)// 리스트에서 다른 파일이 선택되었을때 차트 초기화 후 다시 플로팅
        {
            int index = 0;
            
            while (index < 100)
            {
                if(index == Subject1.SelectedIndex + 1)
                {
                    String[] tmp = linearray[index].Split(',');
                    Information.Text = " Age: " + tmp[1] + " Height: " + tmp[2] + " Weight: " + tmp[3] + " Gender: " + tmp[4] + " Drink: " + tmp[5] + " Smoke: " + tmp[6] + " Coffee: " + tmp[7] + " Stress: " + tmp[8];
                }
                index++;
            }
            string selected = Subject1.SelectedItem.ToString();
            string path = @"C:\DBdata\" + selected + "_bvp.csv";
            //MessageBox.Show(path);


            oneValueRead(path, bvpStamp, bvpValue, ref bvpMax, ref bvpMin, ref bvpCnt);

            chart1.Series.Clear();
            Series s1 = new Series("E_bvpV");
            s1.Font = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Regular);

            chart1.Series.Add(s1);
            
            

            //chart1.ChartAreas["ChartArea1"]
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            chart1.Series["E_bvpV"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["E_bvpV"].BorderWidth = 4;
            chart1.Series["E_bvpV"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.

            chart1.Series["E_bvpV"].Points.AddXY(FromUnixTime(0), -9999); // 초기점 (0, -1)
            //MessageBox.Show(bvpCnt.ToString());
            for (int x = 0; x < bvpCnt; x += 1)
            {
                chart1.Series["E_bvpV"].Points.AddXY(FromUnixTime(bvpStamp[x]), bvpValue[x]); // (읽어온 데이터 플로팅)
            }
            chart1.Series["E_bvpV"].Points.AddXY(FromUnixTime(bvpStamp[bvpCnt - 1]), -9999); // 끝점 (signalLength, -1)


            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).ToOADate();
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5).ToOADate();
            chart1.ChartAreas["ChartArea1"].AxisY.Minimum = -100;
            chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 50;
            //////////////////////////////////////////////////
            path = @"C:\DBdata\" + selected + "_acc.csv";
            threeValueRead(path, accStamp, accValue1, accValue2, accValue3, ref accMax, ref accMin, ref accCnt);

            chart2.Series.Clear();
            Series Wacc1 = new Series("W_accX");
            Series Wacc2 = new Series("W_accY");
            Series Wacc3 = new Series("W_accZ");
            
            chart2.Series.Add(Wacc1);
            chart2.Series.Add(Wacc2);
            chart2.Series.Add(Wacc3);

            chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart2.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            chart2.Series["W_accX"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["W_accX"].BorderWidth = 4;
            chart2.Series["W_accX"].Color = Color.Red;
            chart2.Series["W_accY"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["W_accY"].BorderWidth = 4;
            chart2.Series["W_accY"].Color = Color.Blue;
            chart2.Series["W_accZ"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["W_accZ"].BorderWidth = 4;
            chart2.Series["W_accZ"].Color = Color.Green;

            chart2.Series["W_accX"].Points.AddXY(FromUnixTime(0), -9999); //
            chart2.Series["W_accY"].Points.AddXY(FromUnixTime(0), -9999); //
            chart2.Series["W_accZ"].Points.AddXY(FromUnixTime(0), -9999); //
            for (int x = 0; x < accCnt; x += 1)
            {
                this.chart2.Series["W_accX"].Points.AddXY(FromUnixTime(accStamp[x]), accValue1[x]); // (읽어온 데이터 플로팅)
                this.chart2.Series["W_accY"].Points.AddXY(FromUnixTime(accStamp[x]), accValue2[x]); // (읽어온 데이터 플로팅)
                this.chart2.Series["W_accZ"].Points.AddXY(FromUnixTime(accStamp[x]), accValue3[x]); // (읽어온 데이터 플로팅)
                //MessageBox.Show(accStamp[x] + " " + accValue1[x] + " " + accValue2[x] + " " + accValue3[x]);
            }
            //MessageBox.Show(accStamp[accCnt-1].ToString());
            chart2.Series["W_accX"].Points.AddXY(FromUnixTime(accStamp[accCnt - 1]), -9999);
            chart2.Series["W_accY"].Points.AddXY(FromUnixTime(accStamp[accCnt - 1]), -9999);
            chart2.Series["W_accZ"].Points.AddXY(FromUnixTime(accStamp[accCnt - 1]), -9999);

            chart2.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).ToOADate(); ;
            chart2.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5).ToOADate(); ;
            chart2.ChartAreas["ChartArea1"].AxisY.Minimum = -100;
            chart2.ChartAreas["ChartArea1"].AxisY.Maximum = +100;

            path = @"C:\DBdata\" + selected + "_zephyr.csv";
            

            zephyrValueRead(path, zephyrStamp, zephyrValue, ref zephyrMax, ref zephyrMin, ref zephyrCnt);

            chart3.Series.Clear();
            Series sz = new Series("Z_hrtV");
            chart3.Series.Add(sz);

            chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart3.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart3.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart3.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            chart3.Series["Z_hrtV"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Z_hrtV"].BorderWidth = 4;
            chart3.Series["Z_hrtV"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.

            chart3.Series["Z_hrtV"].Points.AddXY(FromUnixTime(0), -9999); // 초기점 (0, -1)
            //MessageBox.Show(zephyrCnt.ToString());
            for (int x = 0; x < zephyrCnt; x += 1)
            {
                chart3.Series["Z_hrtV"].Points.AddXY(FromUnixTime(zephyrStamp[x]), zephyrValue[x]); // (읽어온 데이터 플로팅)
            }
            chart3.Series["Z_hrtV"].Points.AddXY(FromUnixTime(zephyrStamp[zephyrCnt - 1]), -9999); // 끝점 (signalLength, -1)



            chart3.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).ToOADate();
            chart3.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5).ToOADate();
            chart3.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chart3.ChartAreas["ChartArea1"].AxisY.Maximum = 150;

            path = @"C:\DBdata\" + selected + "_gsr.csv";
            oneValueRead(path, gsrStamp, gsrValue, ref gsrMax, ref gsrMin, ref gsrCnt);

            chart4.Series.Clear();
            Series sg = new Series("E_gsrV");
            chart4.Series.Add(sg);
            chart4.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart4.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart4.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart4.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            chart4.Series["E_gsrV"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart4.Series["E_gsrV"].BorderWidth = 4;
            chart4.Series["E_gsrV"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.

            chart4.Series["E_gsrV"].Points.AddXY(FromUnixTime(0), -9999); // 초기점 (0, -1)
            //MessageBox.Show(zephyrCnt.ToString());
            for (int x = 0; x < gsrCnt; x += 1)
            {
                chart4.Series["E_gsrV"].Points.AddXY(FromUnixTime(gsrStamp[x]), gsrValue[x]); // (읽어온 데이터 플로팅)
            }
            chart4.Series["E_gsrV"].Points.AddXY(FromUnixTime(gsrStamp[gsrCnt - 1]), -9999); // 끝점 (signalLength, -1)



            chart4.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).ToOADate();
            chart4.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5).ToOADate();
            chart4.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chart4.ChartAreas["ChartArea1"].AxisY.Maximum = 15;
            
            //int t = accStamp[1]/10;
            // 가장 초기 프레임

            Tick = 0; // 새로운 파일이 선택되면 다시 x값 0부터 시작.

            //Controls.Add(chart1);


        }
        private void oneValueRead(string filename, double[] stamp, double[] value, ref double max, ref double min, ref int cnt) // 데이터를 읽어와서 Min값과 Max값을 설정한다. 이 값은 y축의 범위를 결정짓는 용도로 사용.
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            max = Int32.MinValue;
            min = Int32.MaxValue;
            int i = 0;
            try
            {
                foreach (string line in lines)
                {

                    string[] temp = line.Split(',');
                    if (temp[0] != " " && temp[1] != " ")
                    {
                        //MessageBox.Show("'" + temp[0] + "'" + temp[1] + "'" + temp[2] + "'" + i.ToString());
                        temp[1] = temp[1].Substring(1, temp[1].Length - 1);
                        // second = second.Substring(0, second.Length - 1);
                        //MessageBox.Show("'" + temp[0] + "'" + temp[1] + "'" + temp[2] + "'" + i.ToString());
                        //double X = Convert.ToDouble(temp[0]);
                        //double Y = Convert.ToDouble(temp[1]);
                        stamp[i] = Convert.ToDouble(temp[0]);
                        value[i] = Convert.ToDouble(temp[1]);

                        if (value[i] < min) min = value[i];
                        if (value[i] > max) max = value[i];
                        i++;
                    }
                }
                cnt = i;

            }
            catch (Exception e)
            {
                cnt = i;
            }

            //MessageBox.Show(bvpCnt.ToString() + "*");

        }
        private void threeValueRead(string filename, double[] stamp, double[] val1, double[] val2, double[] val3, ref double max, ref double min, ref int cnt) // 데이터를 읽어와서 Min값과 Max값을 설정한다. 이 값은 y축의 범위를 결정짓는 용도로 사용.
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            max = Int32.MinValue;
            min = Int32.MaxValue;
            int i = 0;
            try
            {
                foreach (string line in lines)
                {

                    string[] temp = line.Split(',');
                    if (temp[0] != " " && temp[1] != " " && temp[2] != " " && temp[3] != " ")
                    {
                        //MessageBox.Show("'" + temp[0] + "'" + temp[1] + "'" + temp[2] + "'" + i.ToString());
                        temp[1] = temp[1].Substring(1, temp[1].Length - 1);
                        // second = second.Substring(0, second.Length - 1);
                        //MessageBox.Show("'" + temp[0] + "'" + temp[1] + "'" + temp[2] + "'" + temp[3] + "'" + i.ToString());
                        //double X = Convert.ToDouble(temp[0]);
                        //double Y = Convert.ToDouble(temp[1]);
                        stamp[i] = Convert.ToDouble(temp[0]);
                        val1[i] = Convert.ToDouble(temp[1]);
                        val2[i] = Convert.ToDouble(temp[2]);
                        val3[i] = Convert.ToDouble(temp[3]);
                        //MessageBox.Show(val1[i].ToString() + " " + val2[i].ToString() + " " + val3[i].ToString());
                        if (val1[i] < min) min = val1[i];
                        if (val2[i] < min) min = val2[i];
                        if (val3[i] < min) min = val3[i];

                        if (val1[i] > max) max = val1[i];
                        if (val2[i] > max) max = val2[i];
                        if (val3[i] > max) max = val3[i];

                        i++;
                    }
                }
                cnt = i;

            }
            catch (Exception e)
            {
                cnt = i;
            }

            //MessageBox.Show(cnt.ToString() + "*");

        }
        private void zephyrValueRead(string filename, double[] stamp, double[] value, ref double max, ref double min, ref int cnt) // 데이터를 읽어와서 Min값과 Max값을 설정한다. 이 값은 y축의 범위를 결정짓는 용도로 사용.
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            max = Int32.MinValue;
            min = Int32.MaxValue;
            int i = 0;
            try
            {
                foreach (string line in lines.Skip(2))
                {

                    string[] temp = line.Split(',');
                    if (temp[0] != " " && temp[1] != " ")
                    {
                        //MessageBox.Show(line);
                        //MessageBox.Show("'" + temp[0] + "'" + temp[1] + " " + i.ToString());
                        //temp[1] = temp[1].Substring(1, temp[1].Length - 1);
                        // second = second.Substring(0, second.Length - 1);
                        //double X = Convert.ToDouble(temp[0]);
                        //double Y = Convert.ToDouble(temp[1]);
                        //stamp[i] = Convert.ToDouble(temp[0]);
                        stamp[i] = Convert.ToDouble(temp[0]);
                        value[i] = Convert.ToDouble(temp[1]);

                        if (value[i] < min) min = value[i];
                        if (value[i] > max) max = value[i];
                        i++;
                    }
                }
                cnt = i;

            }
            catch (Exception e)
            {
                cnt = i;
            }


            //MessageBox.Show(cnt.ToString() + "*");

        }
        void timer_Tick(object sender, EventArgs e)
        {

            signalChartSetting(Tick);
            Tick += 1;
        }

        private void signalChartSetting(int minX)
        {

            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).AddSeconds(Tick).ToOADate();
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5 + Tick).ToOADate();
            chart2.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).AddSeconds(Tick).ToOADate();
            chart2.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5 + Tick).ToOADate();
            chart3.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).AddSeconds(Tick).ToOADate();
            chart3.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5 + Tick).ToOADate();
            chart4.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(bvpStamp[1]).AddSeconds(Tick).ToOADate();
            chart4.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(bvpStamp[1]).AddSeconds(5 + Tick).ToOADate();

            //chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Min;
            //chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Max; 
            // interval간격마다 x축 값을 +10씩 밀리도록 설정.

        }
        private void Sensor_Value_Load(object sender, EventArgs e)
        {
            int i = 0;
            StreamReader sr = new StreamReader(@"C:\data\Experimenter_A.csv");
            while (!sr.EndOfStream)
            {
                linearray[i] = sr.ReadLine();
                i++;
            }

            panel1.Controls.Add(chart1);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(chart2);
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(chart3);
            panel1.Controls.Add(groupBox4);
            panel1.Controls.Add(chart4);
            panel1.Controls.Add(groupBox5);
            panel1.Controls.Add(chart5);
            panel1.Controls.Add(groupBox6);

            panel2.Controls.Add(Information);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick); // 폼이 활성화되면 timer부터 설정한다. interval은 100으로 잡는다.

            /*
            string dirPath = @"C:\DBdata"; // 데이터파일을 불러올 디렉토리 경로.
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);

            List<CSVFile> files = new List<CSVFile>();
            foreach (var item in di.GetFiles())
            {
                FileInfo fi = new FileInfo(item.FullName);
                files.Add(new CSVFile() { FileName = Path.GetFileName(fi.FullName), Path = fi.FullName });
                // 디렉토리 내부의 모든 파일들을 가져와서 하나씩 files 리스트에 추가.
            }
            Subject1.DataSource = files;
            Subject1.ValueMember = "Path";
            Subject1.DisplayMember = "FileName"; //listFile에 files리스트를 올린다.
            */
            //label1.Text = duration.ToString(); //label1에는 현재 duration값을 표시.


        }

        private void startButton_Click(object sender, EventArgs e)
        {
            timer.Start();
        }


        private void stopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        /*
       
        private void buttonSET_Click(object sender, EventArgs e)
        {
        }

        private void buttonIntervalUp_Click(object sender, EventArgs e)
        {
            if (duration < 350)
            {
                duration += 20;
                signalChartSetting(Tick * 10);
                label1.Text = duration.ToString();
            }
        }

        private void buttonIntervalDown_Click(object sender, EventArgs e)
        {
            if (duration > 50)
            {
                duration -= 20;
                signalChartSetting(Tick * 10);
                label1.Text = duration.ToString();
            }
        }

       
        */

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            main frm1 = new Demo.main();
            frm1.ShowDialog();
        }
        public static DateTime FromUnixTime(double unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            Tick -= 5;
            signalChartSetting(Tick);
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            Tick += 5;
            signalChartSetting(Tick);
        }

        private void goButton_Click(object sender, EventArgs e)
        {

            int val;

            if (Int32.TryParse(textBox1.Text.ToString(), out val))
            {
                Tick = val;
                signalChartSetting(Tick);
            }
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }














    }
}
