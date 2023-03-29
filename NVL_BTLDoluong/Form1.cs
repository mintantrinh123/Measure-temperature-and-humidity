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
using System.IO.Ports;//sử dụng thư viện kết nối COMPORT
using System.Xml;
using System.Threading;


namespace NVL_BTLDoluong
{
    public partial class Form1 : Form
    {
        public event EventHandler OnUpdateConnection;
        int mode = 0;
        float level1 = 20;
        float level2 = 25;
        float level3 = 30;
        float levelhumi = 50;
        float timedelay = 2;
        int fanstt;
        int ledstt;
        int valvestt;
        int heaterstt;
        public Form1()

        {
            InitializeComponent();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        string[] baud = { "1200", "2400", "4800", "9600", "192000", "38400" };
        private void Form1_Load(object sender, EventArgs e)
        {
            auto.Enabled = false;
            manu.Enabled = false;
            apply.Enabled = false;
            sp1.Enabled = false;
            sp2.Enabled = false;
            sp3.Enabled = false;
            lv1.Enabled = false;
            lv2.Enabled = false;
            lv3.Enabled = false;
            lvhumi.Enabled = false;
            tdelay.Enabled = false;
            off.Enabled = false;

            coil.Enabled = false;
            heater.Enabled = false;
            led.Enabled = false;

            lv1.Text = Convert.ToString(level1);
            lv2.Text = Convert.ToString(level2);
            lv3.Text = Convert.ToString(level3);
            lvhumi.Text = Convert.ToString(levelhumi);
            tdelay.Text = Convert.ToString(timedelay);
            

            string[] listnamecom = SerialPort.GetPortNames();
            listcom.Items.AddRange(listnamecom);
            listbaud.Items.AddRange(baud);

           
            float hm = 0;// thiết kế biểu đồ ghi/đọc tín hiệu 
            chart1.Series["Humidity        "].Points.AddY(hm);
            chart1.Series["Temperature"].Points.AddY(hm);
            chart1.Series["Humidity        "].IsValueShownAsLabel = false;
            chart1.Series["Temperature"].IsValueShownAsLabel = false;
            chart1.ChartAreas["ChartArea1"].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        bool press = true;


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = listcom.Text; 
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            

            if (listcom.Text == "")
            {
                MessageBox.Show("Select COM PORT.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (serialPort1.IsOpen)
                    {

                        MessageBox.Show("COM PORT is connected or ready for use.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (listcom.Text != "")
                    {
                        serialPort1.PortName = listcom.Text;
                        if (listbaud.Text != "")
                        {
                            serialPort1.BaudRate = Convert.ToInt32(listbaud.Text);
                            {
                                serialPort1.Open();
                                stt.BackColor = Color.LimeGreen;
                                stt.Text = "Connected";
                                listcom.Enabled = false;
                                auto.Enabled = true;
                                manu.Enabled = true;

                            }
                        }
                        else
                        {
                            MessageBox.Show("Baudrate is uncorrect", "Seclect BaudRate", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show("COM PORT is not compatible with the device.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }



                }
                catch (Exception)
                {
                    MessageBox.Show("COM PORT is not found. Please check your COM and CABLE", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    stt.BackColor = Color.Red;
                    stt.Text = "Disconnected!";
                    listcom.Enabled = true;

                    auto.Enabled = false;
                    manu.Enabled = false;
                    apply.Enabled = false;
                    sp1.Enabled = false;
                    sp2.Enabled = false;
                    sp3.Enabled = false;
                    off.Enabled = false;
                }
                else
                {
                    MessageBox.Show("COM PORT is disconnected or ready for use.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception)
            {
                MessageBox.Show("COM PORT is not found. Please check your COM and CABLE", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button3_Click(object sender, EventArgs e) // chương trình cho nút EXIT
        {
            {
                DialogResult dg = MessageBox.Show("You want to close the program", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    Application.Exit();
                    serialPort1.Close();
                }
            }
        }
        float temperature;
        float humidity;
        int ldr;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //timer1.Interval = 10000;// khai báo hàm đọc nhận tính hiệu analog từ cảm biến DHT11
            string dataFromArduino = serialPort1.ReadLine().ToString();
            string[] dataTempHumid = dataFromArduino.Split(',');
           try
            {
                temperature = (float)(Math.Round(Convert.ToDecimal(dataTempHumid[0]), 1));
                humidity = (float)(Math.Round(Convert.ToDecimal(dataTempHumid[1]), 1));
                ldr = (int)(Math.Round(Convert.ToDecimal(dataTempHumid[2]),1));
            }
            catch
            {
                return;
            }

            /////////////////////////////////////////////////////////////

            if (mode == 1)
            {
                if (temperature <= level1) 
                {
                    if (fanstt != 0)
                    {
                        fanstt = 0;
                        serialPort1.Write("0");

                    }
                    else if (fanstt == 0)
                    {
                        
                    }


                    sth.BackColor = Color.Orange;
                    off.BackColor = Color.Orange;

                    sp1.BackColor = Color.AliceBlue;
                    sp2.BackColor = Color.AliceBlue;
                    sp3.BackColor = Color.AliceBlue;
                    

                }

                else if ((temperature > level1) && (temperature <= level2))
                {

                    if (fanstt != 1)
                    {
                        fanstt = 1;
                        serialPort1.Write("1");

                    }
                    else if (fanstt == 1)
                    {
                        
                    }



                    sth.BackColor = Color.AliceBlue;

                    sp1.BackColor = Color.Orange;

                    sp2.BackColor = Color.AliceBlue;
                    sp3.BackColor = Color.AliceBlue;
                    off.BackColor = Color.AliceBlue;
                    

                }
                else if ((temperature > level2) && (temperature <= level3))
                {
                    if (fanstt != 2)
                    {
                        fanstt = 2;
                        serialPort1.Write("2");

                    }
                    else if (fanstt == 2)
                    {
                        
                    }



                    sth.BackColor = Color.AliceBlue;
                    sp2.BackColor = Color.Orange;

                    sp1.BackColor = Color.AliceBlue;
                    sp3.BackColor = Color.AliceBlue;
                    off.BackColor = Color.AliceBlue;
                    
                }
                else if (temperature > level3) 
                {
                    if (fanstt != 3)
                    {
                        fanstt = 3;
                        serialPort1.Write("3");

                    }
                    else if (fanstt == 3)
                    {
                        
                    }



                    sth.BackColor = Color.AliceBlue;
                    sp3.BackColor = Color.Orange;

                    sp2.BackColor = Color.AliceBlue;
                    sp1.BackColor = Color.AliceBlue;
                    off.BackColor = Color.AliceBlue;
                    
                }
                if (ldr < 500)
                {
                    if (ledstt != 1)
                    {
                        ledstt = 1;
                        serialPort1.Write("L");

                    }
                    else if (ledstt == 1)
                    {
                        
                    }
                    stl.BackColor = Color.Orange;

                }
                else if (ldr >= 500) 
                {
                    if (ledstt != 0)
                    {
                        ledstt = 0;
                        serialPort1.Write("M");

                    }
                    else if (ledstt == 0)
                    {
                        
                    }
                    
                    stl.BackColor = Color.AliceBlue;
                   

                }
                if (humidity <= levelhumi)
                {
                    if (valvestt != 1)
                    { 
                        valvestt = 1;
                        serialPort1.Write("C");
                    }
                    else if (valvestt == 1)
                    {
                       
                    }

                    stc.BackColor = Color.Orange;

                }
                else if (humidity > (levelhumi + 20))
                {
                    if (valvestt != 0)
                    {
                        valvestt = 0;
                        serialPort1.Write("D");
                    }
                    else if (valvestt == 0)
                    {
                        
                    }
                    stc.BackColor = Color.AliceBlue;
                }
                if (temperature < level1)
                {
                    if (heaterstt != 1)
                    {
                        heaterstt = 1;
                        serialPort1.Write("H");
                    }
                    else if (valvestt == 1)
                    {

                    }

                    sth.BackColor = Color.Orange;

                }
                else if (temperature >= (level1 + timedelay))
                {
                    if (heaterstt != 0)
                    {
                        heaterstt = 0;
                        serialPort1.Write("I");
                    }
                    else if (valvestt == 0)
                    {

                    }
                    sth.BackColor = Color.AliceBlue;
                }
            }
            


           
           
            BeginInvoke(new Action(() => //thiết kế từ bản đồ thị, ghi giá trị đọc được từ arduino
           {
               chart1.Series["Humidity        "].Points.AddY(humidity.ToString());
               chart1.Series["Temperature"].Points.AddY(temperature.ToString());
               chart1.Series["Humidity        "].LegendText = "Humidity: " + humidity.ToString() + " %";

               chart1.Series["Temperature"].LegendText = "Temperature: " + temperature.ToString() + " °C";
               chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                
               
           }));
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
                
        }
    
        private void timer2_Tick(object sender, EventArgs e)
        {
               
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void auto_Click(object sender, EventArgs e)
        {
            auto.BackColor = Color.Blue;
            auto.ForeColor = Color.White;
            sp1.Enabled = false;   
            sp2.Enabled = false;
            sp3.Enabled = false;
            off.Enabled = false;
            lv1.Enabled = true;
            lv2.Enabled = true;
            lv3.Enabled = true;
            lvhumi.Enabled = true;
            tdelay.Enabled = true;
            apply.Enabled = true;

            coil.Enabled = false;
            heater.Enabled = false;
            led.Enabled = false;

            mode = 1;

            manu.BackColor = Color.LightSkyBlue;
            manu.ForeColor = Color.Black;
            MessageBox.Show("Hãy thiết lập ngưỡng nhiệt độ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void apply_Click(object sender, EventArgs e)
        {
                if ((lv1.Text != "") && (lv2.Text != "") && (lv3.Text != ""))
                {
                    level1 = float.Parse(lv1.Text);
                    level2 = float.Parse(lv2.Text);
                    level3 = float.Parse(lv3.Text);
                    levelhumi = float.Parse(lvhumi.Text);
                    timedelay = float.Parse(tdelay.Text);
                }
                else
                {
                    MessageBox.Show("Hãy nhập đủ ngưỡng nhiệt độ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void manu_Click(object sender, EventArgs e)
        {
            manu.BackColor = Color.Blue;
            manu.ForeColor = Color.White;
            lv1.Enabled = false;
            lv2.Enabled = false;
            lv3.Enabled = false;
            lvhumi.Enabled = false;
            apply.Enabled = false;
            tdelay.Enabled = false;
            sp1.Enabled = true;
            sp2.Enabled = true;
            sp3.Enabled = true;
            off.Enabled = true;

            coil.Enabled = true;
            heater.Enabled = true;
            led.Enabled = true;

            mode = 2;

            auto.BackColor = Color.LightSkyBlue;
            auto.ForeColor = Color.Black;
        }

        private void UpdateConnection(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                stt.Text = "Connected";
                stt.ForeColor = Color.Green;
            }
            else
            {
                stt.Text = "Disconnected";
                stt.ForeColor = Color.Red;
            }
        }
        private void timer2_Tick_1(object sender, EventArgs e)
        {
                if (this.OnUpdateConnection == null)
                {
                    this.OnUpdateConnection += UpdateConnection;
                }
                this.OnUpdateConnection(this, EventArgs.Empty);
        }

        private void sp1_Click(object sender, EventArgs e)
        {
            fanstt = 1;
            serialPort1.Write("1");
            sp1.BackColor = Color.Orange;

            sp2.BackColor = Color.AliceBlue;
            sp3.BackColor = Color.AliceBlue;
            off.BackColor = Color.AliceBlue;
        }

        private void sp2_Click(object sender, EventArgs e)
        {
            fanstt = 2;
            serialPort1.Write("2");
            sp2.BackColor = Color.Orange;

            sp1.BackColor = Color.AliceBlue;
            sp3.BackColor = Color.AliceBlue;
            off.BackColor = Color.AliceBlue;
        }

        private void sp3_Click(object sender, EventArgs e)
        {
            fanstt = 3;
            serialPort1.Write("3");
            sp3.BackColor = Color.Orange;

            sp2.BackColor = Color.AliceBlue;
            sp1.BackColor = Color.AliceBlue;
            off.BackColor = Color.AliceBlue;
        }

        private void off_Click(object sender, EventArgs e)
        {
            fanstt = 0;
            serialPort1.Write("0");
            off.BackColor = Color.Orange;

            sp1.BackColor = Color.AliceBlue;
            sp2.BackColor = Color.AliceBlue;
            sp3.BackColor = Color.AliceBlue;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        int cst = 1;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (cst == 1)
            {
                valvestt = 1;
                serialPort1.Write("C");
                cst = 0;
                stc.BackColor = Color.Orange;
                coil.Text = "OFF";
            }
            else
            {
                valvestt = 0;
                serialPort1.Write("D");
                cst = 1;
                stc.BackColor = Color.AliceBlue;
                coil.Text = "ON";
            }
        }
  

        int lst = 1;
        private void LED_Click(object sender, EventArgs e)
        {
            if (lst == 1)
            {
                ledstt = 1;
                serialPort1.Write("L");
                lst = 0;
                stl.BackColor = Color.Orange;
                led.Text = "OFF";
            }
            else
            {
                ledstt = 0;
                serialPort1.Write("M");
                lst = 1;
                stl.BackColor = Color.AliceBlue;
                led.Text = "ON";
            }
        }

        private void stl_Paint(object sender, PaintEventArgs e)
        {

        }
        int hst = 1;
        private void heater_Click_1(object sender, EventArgs e)
        {
            if (hst == 1)
            {
                heaterstt = 1; 
                serialPort1.Write("H");
                hst = 0;
                sth.BackColor = Color.Orange;
                heater.Text = "OFF";
            }
            else
            {
                heaterstt = 0;
                serialPort1.Write("I");
                hst = 1;
                sth.BackColor = Color.AliceBlue;
                heater.Text = "ON";
            }
        }
    }
}
