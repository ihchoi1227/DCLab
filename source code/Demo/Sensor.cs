using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using Demo.Properties;


namespace Demo
{



    public partial class Sensor : Form
    {
        int stampCnt, errorCnt = 0;
        bool stampGsr, stampAcc, stampBvp, stampTmp, stampWacc, stampWgyro = false;
        bool errorGsr, errorAcc, errorBvp, errorTmp, errorWacc, errorWgyro = false;
        bool isMainReady = false;
        string timecheck;

        string[] subject_indexer = new string[20];
        string[] gsr1 = new string[500000];
        string[] gsr2 = new string[500000];
        string[] gsrStamp = new string[500000];
        string[] gsr0 = new string[500000];
        //string[] gsrlength = new string[500000];

        string[] acc1 = new string[500000];
        string[] acc2 = new string[500000];
        string[] acc3 = new string[500000];
        string[] acc4 = new string[500000];
        string[] accStamp = new string[500000];
        string[] acc0 = new string[500000];

        string[] bvp1 = new string[500000];
        string[] bvp2 = new string[500000];
        string[] bvpStamp = new string[500000];
        string[] bvp0 = new string[500000];

        string[] tmp1 = new string[500000];
        string[] tmp2 = new string[500000];
        string[] tmpStamp = new string[500000];
        string[] tmp0 = new string[500000];

        string[] Wacc1 = new string[500000];
        string[] Wacc2 = new string[500000];
        string[] Wacc3 = new string[500000];
        string[] Wacc4 = new string[500000];
        string[] WaccStamp = new string[500000];
        string[] Wacc0 = new string[500000];

        string[] Wgyro1 = new string[500000];
        string[] Wgyro2 = new string[500000];
        string[] Wgyro3 = new string[500000];
        string[] Wgyro4 = new string[500000];
        string[] WgyroStamp = new string[500000];
        string[] Wgyro0 = new string[500000];

        string[] timeStamp = new string[1000];

        int lineNumGsr = 0;
        int lineNumAcc = 0;
        int lineNumBvp = 0;
        int lineNumTmp = 0;
        int lineNumWacc = 0;
        int lineNumWgyro = 0;
        int lineTimeStamp = 0;
        


        // The port number for the remote device.
        private const string ServerAddress = "127.0.0.1";
        private const int ServerPort = 28000;

        // ManualResetEvent instances signal completion.
        private static readonly ManualResetEvent ConnectDone = new ManualResetEvent(false);
        private static readonly ManualResetEvent SendDone = new ManualResetEvent(false);
        private static readonly ManualResetEvent ReceiveDone = new ManualResetEvent(false);

        // The response from the remote device.
        private static String _response = String.Empty;
        public static Socket client;


        private void Sensor_Load(object sender, EventArgs e)
        {
            textBox1.Text = stampCnt.ToString();
            textBox2.Text = errorCnt.ToString();
            //server_thread = new Thread(new ThreadStart(Serverstart));
            //server_thread.Start();

            // Establish the remote endpoint for the socket.
            var ipHostInfo = new IPHostEntry { AddressList = new[] { IPAddress.Parse(ServerAddress) } };
            var ipAddress = ipHostInfo.AddressList[0];
            var remoteEp = new IPEndPoint(ipAddress, ServerPort);
            // Create a TCP/IP socket.
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Connect to the remote endpoint.
            client.BeginConnect(remoteEp, (ConnectCallback), client);

            chart1.Series.Clear();
            Series s1 = new Series("EDA");
            s1.XValueType = ChartValueType.DateTime;
            chart1.Series.Add(s1);

            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;


            chart1.Series["EDA"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["EDA"].BorderWidth = 1;
            chart1.Series["EDA"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.



            chart4.Series.Clear();
            Series s4_1 = new Series("EmAcc_x");
            s4_1.XValueType = ChartValueType.DateTime;
            chart4.Series.Add(s4_1);
            Series s4_2 = new Series("EmAcc_y");
            s4_2.XValueType = ChartValueType.DateTime;
            chart4.Series.Add(s4_2);
            Series s4_3 = new Series("EmAcc_z");
            s4_3.XValueType = ChartValueType.DateTime;
            chart4.Series.Add(s4_3);

            chart4.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart4.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart4.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart4.ChartAreas["ChartArea1"].AxisX.Interval = 1;


            chart4.Series["EmAcc_x"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart4.Series["EmAcc_x"].BorderWidth = 1;
            chart4.Series["EmAcc_x"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.

            chart4.Series["EmAcc_y"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart4.Series["EmAcc_y"].BorderWidth = 1;
            chart4.Series["EmAcc_y"].Color = Color.Red;

            chart4.Series["EmAcc_z"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart4.Series["EmAcc_z"].BorderWidth = 1;
            chart4.Series["EmAcc_z"].Color = Color.Green;


            chart5.Series.Clear();
            Series s5 = new Series("BVP");
            s5.XValueType = ChartValueType.DateTime;
            chart5.Series.Add(s5);

            chart5.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart5.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart5.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart5.ChartAreas["ChartArea1"].AxisX.Interval = 1;


            chart5.Series["BVP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart5.Series["BVP"].BorderWidth = 1;
            chart5.Series["BVP"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.



            chart6.Series.Clear();
            Series s6 = new Series("Tmp");
            s6.XValueType = ChartValueType.DateTime;
            chart6.Series.Add(s6);

            chart6.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart6.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart6.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart6.ChartAreas["ChartArea1"].AxisX.Interval = 1;


            chart6.Series["Tmp"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart6.Series["Tmp"].BorderWidth = 1;
            chart6.Series["Tmp"].Color = Color.Blue; // 그래프(Line)형식의 Series 재생성.





            chart2.Series.Clear();
            Series s2_1 = new Series("Acc_x");
            s2_1.XValueType = ChartValueType.DateTime;
            chart2.Series.Add(s2_1);

            Series s2_2 = new Series("Acc_y");
            s2_2.XValueType = ChartValueType.DateTime;
            chart2.Series.Add(s2_2);

            Series s2_3 = new Series("Acc_z");
            s2_3.XValueType = ChartValueType.DateTime;
            chart2.Series.Add(s2_3);

            // 그래프(Line)형식의 Series 재생성.

            chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart2.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            chart2.Series["Acc_x"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Acc_x"].BorderWidth = 1;
            chart2.Series["Acc_x"].Color = Color.Blue;

            chart2.Series["Acc_y"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Acc_y"].BorderWidth = 1;
            chart2.Series["Acc_y"].Color = Color.Red;

            chart2.Series["Acc_z"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Acc_z"].BorderWidth = 1;
            chart2.Series["Acc_z"].Color = Color.Green;

            chart3.Series.Clear();
            Series s3_1 = new Series("Gyro_x");
            s3_1.XValueType = ChartValueType.DateTime;
            chart3.Series.Add(s3_1);

            Series s3_2 = new Series("Gyro_y");
            s3_2.XValueType = ChartValueType.DateTime;
            chart3.Series.Add(s3_2);

            Series s3_3 = new Series("Gyro_z");
            s3_3.XValueType = ChartValueType.DateTime;
            chart3.Series.Add(s3_3);
            // 그래프(Line)형식의 Series 재생성.

            chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "mm:ss";
            chart3.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Seconds;
            chart3.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart3.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            chart3.Series["Gyro_x"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Gyro_x"].BorderWidth = 1;
            chart3.Series["Gyro_x"].Color = Color.Blue;

            chart3.Series["Gyro_y"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Gyro_y"].BorderWidth = 1;
            chart3.Series["Gyro_y"].Color = Color.Red;

            chart3.Series["Gyro_z"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Gyro_z"].BorderWidth = 1;
            chart3.Series["Gyro_z"].Color = Color.Green;





        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            //StartClient("device_connect E617F2", client);
            StartClient("device_connect C219F2", client);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            StartClient("device_subscribe gsr ON", client);
            System.Threading.Thread.Sleep(500);
            StartClient("device_subscribe acc ON", client);
            System.Threading.Thread.Sleep(500);
            StartClient("device_subscribe bvp ON", client);
            System.Threading.Thread.Sleep(500);
            StartClient("device_subscribe tmp ON", client);

            server_thread = new Thread(new ThreadStart(Serverstart));
            server_thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isMainReady = true;
            string pathGsr, pathAcc, pathBvp, pathTmp, pathWacc, pathWgyro;

            pathGsr = @"C:\data\" + main.Experimenter + @"\gsr.csv";
            pathAcc = @"C:\data\" + main.Experimenter + @"\acc.csv";
            pathBvp = @"C:\data\" + main.Experimenter + @"\bvp.csv";
            pathTmp = @"C:\data\" + main.Experimenter + @"\tmp.csv";
            pathWacc = @"C:\data\" + main.Experimenter + @"\Wacc.csv";
            pathWgyro = @"C:\data\" + main.Experimenter + @"\Wgyro.csv";
            string pathTime = @"C:\data\" + @"\timestamp.csv";

            StreamWriter sw_gsr = new StreamWriter(pathGsr, true);
            StreamWriter sw_acc = new StreamWriter(pathAcc, true);
            StreamWriter sw_bvp = new StreamWriter(pathBvp, true);
            StreamWriter sw_tmp = new StreamWriter(pathTmp, true);
            StreamWriter sw_Wacc = new StreamWriter(pathWacc, true);
            StreamWriter sw_Wgyro = new StreamWriter(pathWgyro, true);
            StreamWriter sw_Timestamp = new StreamWriter(pathTime, true);

            var line = "";
            for (int i = 1; i <= lineNumGsr; i++)
            {
                line = string.Format("{0}, {1}, {2}, {3}", gsr0[i], gsr1[i], gsr2[i], gsrStamp[i]);
                sw_gsr.WriteLine(line);
            }
            sw_gsr.Close();
            for (int i = 1; i <= lineNumAcc; i++)
            {
                line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", acc0[i], acc1[i], acc2[i], acc3[i], acc4[i], accStamp[i]);
                sw_acc.WriteLine(line);
            }
            sw_acc.Close();

            for (int i = 1; i <= lineNumBvp; i++)
            {
                line = string.Format("{0}, {1}, {2}, {3}", bvp0[i], bvp1[i], bvp2[i], bvpStamp[i]);
                sw_bvp.WriteLine(line);
            }
            sw_bvp.Close();

            for (int i = 1; i <= lineNumTmp; i++)
            {
                line = string.Format("{0}, {1}, {2}, {3}", tmp0[i], tmp1[i], tmp2[i], tmpStamp[i]);
                sw_tmp.WriteLine(line);
            }
            sw_tmp.Close();

            for (int i = 1; i <= lineNumWacc; i++)
            {
                line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", Wacc0[i], Wacc1[i], Wacc2[i], Wacc3[i], Wacc4[i], WaccStamp[i]);
                sw_Wacc.WriteLine(line);
            }
            sw_Wacc.Close();

            for (int i = 1; i <= lineNumWgyro; i++)
            {
                line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", Wgyro0[i], Wgyro1[i], Wgyro2[i], Wgyro3[i], Wgyro4[i], WgyroStamp[i]);
                sw_Wgyro.WriteLine(line);
            }
            sw_Wgyro.Close();

            
            for (int i = 1; i <= lineTimeStamp; i++ )
            {
                line = string.Format("{0}", timeStamp[i]);
                sw_Timestamp.WriteLine(line);
            }
            sw_Timestamp.Close();
            

            StartClient("device_subscribe gsr OFF", client);
            System.Threading.Thread.Sleep(500);
            StartClient("device_subscribe acc OFF", client);
            System.Threading.Thread.Sleep(500);
            StartClient("device_subscribe bvp OFF", client);
            System.Threading.Thread.Sleep(500);
            StartClient("device_subscribe tmp OFF", client);
            pictureBox2.Image = Resources.redball;

            try
            {
                server_thread.Abort();
                Server_open = false;
                sck.Close();
                h_socket.Close();
                Server_Disconnect = true;
                pictureBox1.Image = Resources.redball;
            }
            catch
            {
                MessageBox.Show("LG Watch didn't connect to server");
            }
        }
        public double timestamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return timeSpan.TotalMilliseconds / 1000;
        }
        public void StartClient(String msg, Socket client)
        {


            try
            {

                Send(client, msg + Environment.NewLine);
                Receive(client);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                var client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                //Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint);
                //MessageBox.Show("Socket connected to {0}");
                // Signal that the connection has been made.
                ConnectDone.Set();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                var state = new StateObject { WorkSocket = client };

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            //this.SetChart(14.0, 3.0);
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                var state = (StateObject)ar.AsyncState;
                var client = state.WorkSocket;

                // Read data from the remote device.
                var bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                    _response = state.Sb.ToString();


                    //MessageBox.Show(_response);
                    HandleResponseFromEmpaticaBLEServer(_response);

                    state.Sb.Clear();

                    ReceiveDone.Set();

                    // Get the rest of the data.
                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.Sb.Length > 1)
                    {
                        _response = state.Sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    ReceiveDone.Set();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void Send(Socket client, String data)
        {

            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.

            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);


        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                var client = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.
                client.EndSend(ar);
                // Signal that all bytes have been sent.
                SendDone.Set();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }
        public int lineNum = 0;
        private void HandleResponseFromEmpaticaBLEServer(string response)
        {


            if (response == "R device_connect OK\n")
            {
                pictureBox2.Image = Resources.blue;
            }

            string[] temp = response.Split('\n');
            int cnt;
            cnt = temp.Length;
            lineNum++;


            for (int j = 0; j < cnt - 1; j++)
            {
                string[] temp2 = temp[j].Split(' ');
                if (temp2[0] == "E4_Gsr")
                {
                    string first = temp2[1].ToString();
                    string second = temp2[2].ToString();
                    second = second.Substring(0, second.Length - 1);

                    lineNumGsr++;
                    gsr1[lineNumGsr] = first;
                    gsr2[lineNumGsr] = second;
                    //gsrlength[lineNumGsr] = temp2.Length.ToString();
                    
                    if (stampGsr)
                    {
                        gsr0[lineNumGsr] = timecheck;
                        var third = "CHECK";
                        gsrStamp[lineNumGsr] = third;
                        stampGsr = !stampGsr;
                    }
                    else if (errorGsr)
                    {
                        gsr0[lineNumGsr] = timecheck;
                        var third = "ERROR";
                        gsrStamp[lineNumGsr] = third;
                        errorGsr = !errorGsr;
                        
                    }
                    else
                    {
                        gsr0[lineNumGsr] = timestamp().ToString();
                        var third = "";
                        gsrStamp[lineNumGsr] = third;
                    }



                    double X;
                    double Y;
                    X = Convert.ToDouble(temp2[1]);
                    Y = Convert.ToDouble(temp2[2]);

                    if(lineNumGsr%3==0){
                        this.SetChart(X, Y);
                    }
                    //this.SetChart(X, Y);
                    
                    
                }
                else if (temp2[0] == "E4_Acc")
                {

                    var first = temp2[1].ToString();
                    var second = temp2[2].ToString();
                    var third = temp2[3].ToString();
                    var forth = temp2[4].ToString();
                    forth = forth.Substring(0, forth.Length - 1);

                    lineNumAcc++;
                
                    acc1[lineNumAcc] = first;
                    acc2[lineNumAcc] = second;
                    acc3[lineNumAcc] = third;
                    acc4[lineNumAcc] = forth;


                    if (stampAcc)
                    {
                        acc0[lineNumAcc] = timecheck;
                        var fifth = "CHECK";
                        accStamp[lineNumAcc] = fifth;
                        stampAcc = !stampAcc;

                    }
                    else if (errorAcc)
                    {
                        acc0[lineNumAcc] = timecheck;
                        var fifth = "ERROR";
                        accStamp[lineNumAcc] = fifth;
                        errorAcc = !errorAcc;
                    }
                    else
                    {
                        acc0[lineNumAcc] = timestamp().ToString();
                        var fifth = "";
                        accStamp[lineNumAcc] = fifth; ;
                    }


                    double X;
                    double Y1, Y2, Y3;
                    X = Convert.ToDouble(temp2[1]);
                    Y1 = Convert.ToDouble(temp2[2]);
                    Y2 = Convert.ToDouble(temp2[3]);
                    Y3 = Convert.ToDouble(temp2[4]);

                    if(lineNumAcc%8 ==0){this.SetChartAcc(X, Y1, Y2, Y3);};
                    //this.SetChartAcc(X, Y1, Y2, Y3);

                }
                else if (temp2[0] == "E4_Bvp")
                {

                    var first = temp2[1].ToString();
                    var second = temp2[2].ToString();
                    second = second.Substring(0, second.Length - 1);

                    lineNumBvp++;
                    
                    bvp1[lineNumBvp] = first;
                    bvp2[lineNumBvp] = second;


                    if (stampBvp)
                    {
                        bvp0[lineNumBvp] = timecheck;
                        var third = "CHECK";
                        bvpStamp[lineNumBvp] = third;
                        stampBvp = !stampBvp;
                    }
                    else if (errorBvp)
                    {
                        bvp0[lineNumBvp] = timecheck;
                        var third = "ERROR";
                        bvpStamp[lineNumBvp] = third;
                        errorBvp = !errorBvp;
                    }
                    else
                    {
                        bvp0[lineNumBvp] = timestamp().ToString();
                        var third = "";
                        bvpStamp[lineNumBvp] = third;
                    }

                    double X;
                    double Y;
                    X = Convert.ToDouble(temp2[1]);
                    Y = Convert.ToDouble(temp2[2]);

                    if (lineNumBvp % 13 ==0) { this.SetChartBvp(X, Y); }
                    //this.SetChartBvp(X, Y);
                        
                   
                }
                else if (temp2[0] == "E4_Temperature")
                {
                    var first = temp2[1].ToString();
                    var second = temp2[2].ToString();
                    second = second.Substring(0, second.Length - 1);

                    lineNumTmp++;
                    tmp1[lineNumTmp] = first;
                    tmp2[lineNumTmp] = second;

                    if (stampTmp)
                    {
                        tmp0[lineNumTmp] = timecheck;
                        var third = "CHECK";
                        tmpStamp[lineNumTmp] = third;
                        stampTmp = !stampTmp;
                    }
                    else if (errorTmp)
                    {
                        tmp0[lineNumTmp] = timecheck;
                        var third = "ERROR";
                        tmpStamp[lineNumTmp] = third;
                        errorTmp = !errorTmp;
                    }
                    else
                    {
                        tmp0[lineNumTmp] = timestamp().ToString();
                        var third = "";
                        tmpStamp[lineNumTmp] = third;

                    }
                    double X;
                    double Y;
                    X = Convert.ToDouble(temp2[1]);
                    Y = Convert.ToDouble(temp2[2]);

                    if (lineNumTmp % 5 == 0) { this.SetChartTemp(X, Y); };
                    //this.SetChartTemp(X, Y);

                }
            }
        }
        delegate void SetChartCallback(double X, double Y);
        private void SetChart(double X, double Y)
        {
            //MessageBox.Show(X.ToString() + " " + Y.ToString());
            //X /= (int)1492158000;
            //X *= 10;
            //MessageBox.Show(X.ToString() + " ** " + Y.ToString());
            if (this.InvokeRequired)
            {
                SetChartCallback d = new SetChartCallback(SetChart);
                this.Invoke(d, new object[] { X, Y });
            }
            else
            {
                this.chart1.Series["EDA"].Points.AddXY(FromUnixTime(X), Y);
                this.chart1.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(X).AddSeconds(-3).ToOADate();
                this.chart1.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(X).AddSeconds(1).ToOADate();
                this.chart1.ChartAreas["ChartArea1"].AxisY.Minimum = -1;
                this.chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 5;
            }
        }

        delegate void SetChartAccCallback(double X, double Y1, double Y2, double Y3);
        private void SetChartAcc(double X, double Y1, double Y2, double Y3)
        {
            if (this.InvokeRequired)
            {
                SetChartAccCallback d = new SetChartAccCallback(SetChartAcc);
                this.Invoke(d, new object[] { X, Y1, Y2, Y3 });
            }
            else
            {


                this.chart4.Series["EmAcc_x"].Points.AddXY(FromUnixTime(X), Y1);
                this.chart4.Series["EmAcc_y"].Points.AddXY(FromUnixTime(X), Y2);
                this.chart4.Series["EmAcc_z"].Points.AddXY(FromUnixTime(X), Y3);

                this.chart4.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(X).AddSeconds(-3).ToOADate();
                this.chart4.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(X).AddSeconds(1).ToOADate();
                this.chart4.ChartAreas["ChartArea1"].AxisY.Minimum = -50;
                this.chart4.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
            }
        }

        delegate void SetChartBvpCallback(double X, double Y);
        private void SetChartBvp(double X, double Y)
        {
            if (this.InvokeRequired)
            {
                SetChartBvpCallback d = new SetChartBvpCallback(SetChartBvp);
                this.Invoke(d, new object[] { X, Y });
            }
            else
            {
                this.chart5.Series["BVP"].Points.AddXY(FromUnixTime(X), Y);
                this.chart5.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(X).AddSeconds(-3).ToOADate();
                this.chart5.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(X).AddSeconds(1).ToOADate();
                this.chart5.ChartAreas["ChartArea1"].AxisY.Minimum = -40;
                this.chart5.ChartAreas["ChartArea1"].AxisY.Maximum = 40;
            }
        }
        delegate void SetChartTempCallback(double X, double Y);
        private void SetChartTemp(double X, double Y)
        {
            if (this.InvokeRequired)
            {
                SetChartTempCallback d = new SetChartTempCallback(SetChartTemp);
                this.Invoke(d, new object[] { X, Y });
            }
            else
            {


                this.chart6.Series["Tmp"].Points.AddXY(FromUnixTime(X), Y);
                this.chart6.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(X).AddSeconds(-3).ToOADate();
                this.chart6.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(X).AddSeconds(1).ToOADate();
                this.chart6.ChartAreas["ChartArea1"].AxisY.Minimum = 25;
                this.chart6.ChartAreas["ChartArea1"].AxisY.Maximum = 35;
            }
        }




        /* private void Sensor_Load(object sender, EventArgs e)
         {
         }*/



        Socket sck;        //서버를 만들기 위한 소켓 변수
        Socket h_socket;   //Accept하기위한 소켓 변수
        IPAddress ipAd;   //IP 주소 관련 변수
        IPEndPoint iep;   //IP주소와 포트넘버를 가지고 생성한 최종 주소 관련 변수
        Thread server_thread;  //서버 생성 관련 쓰레드
        Thread receive_thread;  //클라이언트가 보내준 데이터를 받는 쓰레드
        bool Server_open;
        bool Server_Disconnect = false;

        public Sensor()
        {
            InitializeComponent();
        }
        //메인함수로 돌아가기
        private void Go_main_Click(object sender, EventArgs e)
        {
            if (isMainReady)
            {
                isMainReady = !isMainReady;
                this.Hide();

                main frm1 = new Demo.main();
                frm1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Forget to press 'STOP' button!!");
            }
        }

        public void Serverstart()
        {
            try
            {   //통신할 ip주소 및 포트를 가진 서버 생성
                Server_open = true;
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ipAd = IPAddress.Parse("192.168.0.3");
                iep = new IPEndPoint(ipAd, 5001);
                sck.Bind(iep);
                sck.Listen(5);
                if (Server_open)
                {
                    h_socket = sck.Accept();
                    pictureBox1.Image = Resources.blue;
                    receive_thread = new Thread(new ThreadStart(Receive));
                    receive_thread.Start();
                }

            }
            catch
            {
                MessageBox.Show("Can't connect to server.");
                return;
            }
        }

        //클라이언트에서 데이터를 받는 함수
        public void Receive()
        {
            while (h_socket.Connected)
            {
                try
                {
                    byte[] bytes = new byte[1024];
                    int bytesRecieved = h_socket.Receive(bytes);
                    if (bytesRecieved > 0)
                    {
                        string strRecieved = Encoding.UTF8.GetString(bytes, 0, bytesRecieved);


                        string[] temp = strRecieved.Split('\n');

                        string[] temp2 = temp[0].Split(' ');

                        var first = temp2[1].ToString();
                        var second = temp2[2].ToString();
                        var third = temp2[3].ToString();
                        var forth = temp2[4].ToString();


                        lineNumWacc++;
                        Wacc1[lineNumWacc] = first;
                        Wacc2[lineNumWacc] = second;
                        Wacc3[lineNumWacc] = third;
                        Wacc4[lineNumWacc] = forth;


                        if (stampWacc)
                        {
                            Wacc0[lineNumWacc] = timecheck;
                            var fifth = "CHECK";
                            WaccStamp[lineNumWacc] = fifth;
                            stampWacc = !stampWacc;
                        }
                        else if (errorWacc)
                        {
                            Wacc0[lineNumWacc] = timecheck;
                            var fifth = "ERROR";
                            WaccStamp[lineNumWacc] = fifth;
                            errorWacc = !errorWacc;
                        }
                        else
                        {
                            Wacc0[lineNumWacc] = timestamp().ToString();
                            var fifth = "";
                            WaccStamp[lineNumWacc] = fifth;
                        }



                        double X;
                        double Y1, Y2, Y3;
                        X = Convert.ToDouble(temp2[1]);
                        Y1 = Convert.ToDouble(temp2[2]);
                        Y2 = Convert.ToDouble(temp2[3]);
                        Y3 = Convert.ToDouble(temp2[4]);
                        if (lineNumWacc % 3 == 0) { this.SetChartWatchAcc(X, Y1, Y2, Y3); }
                        //this.SetChartWatchAcc(X, Y1, Y2, Y3);


                        string[] temp3 = temp[1].Split(' ');

                        first = temp3[1].ToString();
                        second = temp3[2].ToString();
                        third = temp3[3].ToString();
                        forth = temp3[4].ToString();

                        lineNumWgyro++;
                        Wgyro1[lineNumWgyro] = first;
                        Wgyro2[lineNumWgyro] = second;
                        Wgyro3[lineNumWgyro] = third;
                        Wgyro4[lineNumWgyro] = forth;

                        if (stampWgyro)
                        {
                            Wgyro0[lineNumWgyro] = timecheck;
                            var fifth = "CHECK";
                            WgyroStamp[lineNumWgyro] = fifth;
                            stampWgyro = !stampWgyro;

                        }
                        else if (errorWgyro)
                        {
                            Wgyro0[lineNumWgyro] = timecheck;
                            var fifth = "ERROR";
                            WgyroStamp[lineNumWgyro] = fifth;
                            errorWgyro = !errorWgyro;
                        }
                        else
                        {
                            Wgyro0[lineNumWgyro] = timestamp().ToString();
                            var fifth = "";
                            WgyroStamp[lineNumWgyro] = fifth;
                        }

                        X = Convert.ToDouble(temp3[1]);
                        Y1 = Convert.ToDouble(temp3[2]);
                        Y2 = Convert.ToDouble(temp2[3]);
                        Y3 = Convert.ToDouble(temp2[4]);

                        if (lineNumWgyro % 3 == 0)
                        {
                            this.SetChartWatchGyro(X, Y1, Y2, Y3);
                        }
                        //this.SetChartWatchGyro(X, Y1, Y2, Y3);
                        //Thread.Sleep(2);
                    }

                }
                catch
                {
                    //MessageBox.Show("받는 과정에서 오류가 발생했습니다.");
                }
            }
            
            if (Server_Disconnect)
            {
                pictureBox1.Image = Resources.redball;
                server_thread.Abort();
                sck.Close();
                h_socket.Close();
                Server_Disconnect = false;
                
            }
            else
            {
                pictureBox1.Image = Resources.redball;
                server_thread.Abort();
                sck.Close();
                h_socket.Close();
            }

        }

        delegate void SetChartWatchAccCallback(double X, double Y1, double Y2, double Y3);
        private void SetChartWatchAcc(double X, double Y1, double Y2, double Y3)
        {

            if (this.InvokeRequired)
            {
                SetChartWatchAccCallback d = new SetChartWatchAccCallback(SetChartWatchAcc);
                this.Invoke(d, new object[] { X, Y1, Y2, Y3 });
            }
            else
            {
                this.chart2.Series["Acc_x"].Points.AddXY(FromUnixTime(X), Y1);
                //this.chart2.Series["Acc_y"].Points.AddXY(FromUnixTime(X), Y2);
                //this.chart2.Series["Acc_z"].Points.AddXY(FromUnixTime(X), Y3);

                this.chart2.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(X).AddSeconds(-3).ToOADate();
                this.chart2.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(X).AddSeconds(1).ToOADate();
                this.chart2.ChartAreas["ChartArea1"].AxisY.Minimum = -30;
                this.chart2.ChartAreas["ChartArea1"].AxisY.Maximum = 30;


            }
        }

        delegate void SetChartWatchGyroCallback(double X, double Y1, double Y2, double Y3);
        private void SetChartWatchGyro(double X, double Y1, double Y2, double Y3)
        {

            if (this.InvokeRequired)
            {
                SetChartWatchGyroCallback d = new SetChartWatchGyroCallback(SetChartWatchGyro);
                this.Invoke(d, new object[] { X, Y1, Y2, Y3 });
            }
            else
            {
                this.chart3.Series["Gyro_x"].Points.AddXY(FromUnixTime(X), Y1);
                //this.chart3.Series["Gyro_y"].Points.AddXY(FromUnixTime(X), Y2);
                //this.chart3.Series["Gyro_z"].Points.AddXY(FromUnixTime(X), Y3);

                this.chart3.ChartAreas["ChartArea1"].AxisX.Minimum = FromUnixTime(X).AddSeconds(-3).ToOADate();
                this.chart3.ChartAreas["ChartArea1"].AxisX.Maximum = FromUnixTime(X).AddSeconds(1).ToOADate();
                this.chart3.ChartAreas["ChartArea1"].AxisY.Minimum = -5;
                this.chart3.ChartAreas["ChartArea1"].AxisY.Maximum = 5;


            }
        }
        //zephyr연결
        private void Clear_List_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Users\\DCLAB\\Desktop\\zephyr.appref-ms");
            pictureBox3.Image = Resources.blue;
        }

        public static DateTime FromUnixTime(double unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var stamp = "CHECK";
            stampGsr = true;
            stampAcc = true;
            stampBvp = true;
            stampTmp = true;
            stampWacc = true;
            stampWgyro = true;
            stampCnt++;
            textBox1.Text = stampCnt.ToString();
            lineTimeStamp++;
            timecheck = timestamp().ToString();
            timeStamp[lineTimeStamp] = timecheck + " " + stamp + " " + main.Experimenter;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            var stamp = "ERROR";
            errorGsr = true;
            errorAcc = true;
            errorBvp = true;
            errorTmp = true;
            errorWacc = true;
            errorWgyro = true;
            errorCnt++;
            textBox2.Text = errorCnt.ToString();
            lineTimeStamp++;
            timecheck = timestamp().ToString();
            timeStamp[lineTimeStamp] = timecheck + " " + stamp + " " + main.Experimenter;
        }
    }
}
