using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ivi.Visa;
using Keysight.Visa;
using System.IO;
using System.IO.Ports;
using IE41;
using System.Net.NetworkInformation;
using System.Numerics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        UsbSession rigol_usb;
        IMessageBasedFormattedIO rigol_io;
        bool doneFlag = false;
        bool radioFlag = false;
        int file_count;
        int file_count_CW;
        int x_count;
        int y_count;
        int FreqIndex;
        float[] ResultArray = new float[3201 * 2 + 1];
        float[] ResultArray1 = new float[3201 * 2 + 1];
        float[] ResultArray2 = new float[3201 * 2 + 1];
        float[,] DrawAmpdbData;
        float[,] DrawPhaseData;
        float[,] DrawAmpdbData2;
        float[,] DrawPhaseData2;
        Complex[,,] output_dataarray;
        Complex[,,] output_dataarray1;
        Complex[,,] output_dataarray2;
        Complex[,] output_dataarray_cw;
        Complex[,] output_dataarray1_cw;
        Complex[,] output_dataarray2_cw;
        byte[] my_byte;
        byte[] my_byte1;
        byte[] my_byte2;
        sbyte[] meas;
        double sweepType = 0;
        Thread rigol_scan;
        Thread rigol_scan_cw;
        IE3672 VNA;
        Thread conti_trig;
        int flag = 0;
        SerialPort serialPort1;

        public Form1()
        {
            InitializeComponent();

            comboBox3.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            serialPort1 = new SerialPort();

            Bitmap Rainbow = new Bitmap(20, 200, PixelFormat.Format24bppRgb);
            Color rainbow = Color.FromArgb(255, 255, 255);
            double rred = 0;
            double rgreen = 0;
            double rblue = 0;
            for (int shu = 0; shu < 200; shu++)
            {
                if (shu <= 39)
                {
                    rred = 255;
                    rgreen = 165 / 37 * shu;
                    rblue = 0;
                    rainbow = Color.FromArgb(Convert.ToInt32(rred), Convert.ToInt32(rgreen), Convert.ToInt32(rblue));
                    for (int heng = 0; heng < 20; heng++)
                    {
                        Rainbow.SetPixel(heng, shu, rainbow);
                    }
                }
                if ((shu > 39) && (shu <= 79))
                {
                    rred = 255;
                    rgreen = 2.25 * shu + 77.25;
                    rblue = 0;
                    rainbow = Color.FromArgb(Convert.ToInt32(rred), Convert.ToInt32(rgreen), Convert.ToInt32(rblue));
                    for (int heng = 0; heng < 20; heng++)
                    {
                        Rainbow.SetPixel(heng, shu, rainbow);
                    }
                }
                if ((shu > 79) && (shu <= 119))
                {
                    rred = -6.375 * shu + 758.625;
                    rgreen = 255;
                    rblue = 0;
                    rainbow = Color.FromArgb(Convert.ToInt32(rred), Convert.ToInt32(rgreen), Convert.ToInt32(rblue));
                    for (int heng = 0; heng < 20; heng++)
                    {
                        Rainbow.SetPixel(heng, shu, rainbow);
                    }
                }
                if ((shu > 119) && (shu <= 159))
                {
                    rred = 0;
                    rgreen = -3.1875 * shu + 634.3125;
                    rblue = 6.375 * shu - 758.625;
                    rainbow = Color.FromArgb(Convert.ToInt32(rred), Convert.ToInt32(rgreen), Convert.ToInt32(rblue));
                    for (int heng = 0; heng < 20; heng++)
                    {
                        Rainbow.SetPixel(heng, shu, rainbow);
                    }
                }
                if ((shu > 159) && (shu <= 199))
                {
                    rred = 0;
                    rgreen = -3.1875 * shu + 634.3125;
                    rblue = 255;
                    rainbow = Color.FromArgb(Convert.ToInt32(rred), Convert.ToInt32(rgreen), Convert.ToInt32(rblue));
                    for (int heng = 0; heng < 20; heng++)
                    {
                        Rainbow.SetPixel(heng, shu, rainbow);
                    }
                }
            }
            pictureBox3.Image = Rainbow;
            pictureBox4.Image = Rainbow;
        }

        public void receiveFromAr()
        {
            string readyState = "";
            bool flag = false;
            while (!flag)
            {
                readyState = serialPort1.ReadLine();
                if (readyState == "OK")
                {
                    flag = true;
                    //MessageBox.Show("ok!");
                }
            }
        }

        unsafe public void vna_receivedata(float[] result_array, byte[] mybyte)
        {
            int MaxLenth = int.Parse(textBox1.Text) * 2;
            float* real = stackalloc float[3201];
            float* imagenary = stackalloc float[3201];
            VNA.Sweep(real, imagenary);
            for (int i = 0; i < MaxLenth; i++)
            {
                result_array[2 * i] = *(real + i);
                result_array[2 * i + 1] = *(imagenary + i);
            }

            //信号发生器到达一个点后，矢网扫描多次取平均降噪
            for (int i = 0; i < int.Parse(textBox21.Text) - 1; i++)
            {
                VNA.Sweep(real, imagenary);
                for (int j = 0; j < MaxLenth; j++)
                {
                    result_array[2 * j] = (*(real + j) + (i + 1) * result_array[2 * j]) / (i + 2);
                    result_array[2 * j + 1] = (*(imagenary + j) + (i + 1) * result_array[2 * j + 1]) / (i + 2);
                }
            }


            Buffer.BlockCopy(result_array, 0, mybyte, file_count * MaxLenth * 4, MaxLenth * 4);
        }

        unsafe public void vna_receivedata(float[] result_array, float[] result_array1, float[] result_array2, byte[] mybyte)
        {
            int MaxLenth = int.Parse(textBox1.Text) * 2;
            for (int i = 0; i < MaxLenth * 2; i++)
            {
                result_array[i] = result_array2[i] - result_array1[i];
            }

            Buffer.BlockCopy(result_array, 0, mybyte, file_count * MaxLenth * 4, MaxLenth * 4);
            file_count++;
        }

        unsafe public void vna_receivedata_cw(float[] result_array,byte[] mybyte)
        {
            int MaxLenth = int.Parse(textBox1.Text) * 2;
            float* real = stackalloc float[3201];
            float* imagenary = stackalloc float[3201];
            VNA.Sweep(real, imagenary);
            for (int i = 0; i < MaxLenth; i++)
            {
                result_array[2 * i] = *(real + i);
                result_array[2 * i + 1] = *(imagenary + i);
            }
            Buffer.BlockCopy(result_array, 0, mybyte, file_count_CW * MaxLenth * 4, MaxLenth * 4);
        }

        unsafe public void vna_receivedata_cw(float[] result_array, float[] result_array1, float[] result_array2, byte[] mybyte)
        {
            int MaxLenth = int.Parse(textBox1.Text) * 2;
            for (int i = 0; i < MaxLenth * 2; i++)
            {
                result_array[i] = result_array2[i] - result_array1[i];
            }

            Buffer.BlockCopy(result_array, 0, mybyte, file_count_CW * MaxLenth * 4, MaxLenth * 4);
            file_count_CW++;
        }

        public void Chart_Draw_AmpAndPhase(Complex[,,] outputdataarray,int arrayFlat,int minFlag)
        {
            string guancef = null;
            this.Invoke(new MethodInvoker(() => { guancef = comboBox4.Text; }));
            FreqIndex = Convert.ToInt32((double.Parse(guancef) - double.Parse(textBox4.Text)) / ((double.Parse(textBox5.Text) - double.Parse(textBox4.Text)) / (double.Parse(textBox1.Text) - 1)));
            double red = 255;
            double green = 255;
            double blue = 255;//定义幅值rgb初值
            double pred = 255;
            double pgreen = 255;
            double pblue = 255;//定义相位rgb初值
            float MaxOfAmpdBValue = -10000000;
            float MinOfAmpdBValue = 10000000;
            float MaxOfPhaseValue = 180;
            float MinOfPhaseValue = -180;
            float MinOfAmpdBValue_temp = 10000000;
            Complex[,] TempFreqData = new Complex[x_count, y_count];//画图数据 
            Color amp = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
            Color pha = Color.FromArgb(Convert.ToInt32(pred), Convert.ToInt32(pgreen), Convert.ToInt32(pblue));

            for (int j = 0; j < y_count; j++)
            {
                for (int i = 0; i < x_count; i++)
                {
                    TempFreqData[i, j] = outputdataarray[i, j, FreqIndex];
                }
            }
            for (int j = 0; j < y_count; j++)
            {
                for (int i = 0; i < x_count; i++)
                {
                    if ((System.Math.Abs(TempFreqData[i, j].Real) < 0.0000001) && (System.Math.Abs(TempFreqData[i, j].Imaginary) < 0.0000001))//还未扫描到这里
                    {
                        DrawAmpdbData[i, j] = 100000;//用极大值标记
                        DrawPhaseData[i, j] = 100000;
                    }
                    else
                    {
                        if (arrayFlat == 0)
                        {
                            if (radioButton1.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(20 * Math.Log10(Math.Sqrt(TempFreqData[i, j].Magnitude)));
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 90 / Math.PI);
                            }
                            else if (radioButton2.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(Math.Sqrt(TempFreqData[i, j].Magnitude));
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 90 / Math.PI);
                            }
                        }
                        else if(arrayFlat == 1)
                        {
                            if (radioButton1.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(20 * Math.Log10(TempFreqData[i, j].Magnitude));
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 180 / Math.PI);
                            }
                            else if (radioButton2.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(TempFreqData[i, j].Magnitude);
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 180 / Math.PI);
                            }
                        }
                    }
                }
            }
            for (int j = 0; j < y_count; j++)
            {
                for (int i = 0; i < x_count; i++)
                {
                    if (minFlag == 0)
                    {
                        if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10) || (Math.Abs(DrawPhaseData[i, j] - 100000) < 10))
                            continue;
                        if (DrawAmpdbData[i, j] >= MaxOfAmpdBValue)
                            MaxOfAmpdBValue = DrawAmpdbData[i, j];
                        if (DrawAmpdbData[i, j] <= MinOfAmpdBValue)
                            MinOfAmpdBValue = DrawAmpdbData[i, j];
                        /*if (DrawPhaseData[i, j] >= MaxOfPhaseValue)
                            MaxOfPhaseValue = DrawPhaseData[i, j];
                        if (DrawPhaseData[i, j] <= MinOfPhaseValue)
                            MinOfPhaseValue = DrawPhaseData[i, j];*/
                    }
                    else if(minFlag == 1)
                    {
                        MaxOfAmpdBValue =Convert.ToSingle(textBox12.Text);
                        MinOfAmpdBValue = Convert.ToSingle(textBox13.Text);
                        //MaxOfPhaseValue = Convert.ToSingle(textBox14.Text);
                        //MinOfPhaseValue = Convert.ToSingle(textBox15.Text);
                        //MinOfPhaseValue = -180;
                        MinOfAmpdBValue_temp = MinOfAmpdBValue;
                        MinOfAmpdBValue =(float)numericUpDown1.Value>= Convert.ToSingle(textBox13.Text)?(float)numericUpDown1.Value: Convert.ToSingle(textBox13.Text);
                        //if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10) || (Math.Abs(DrawPhaseData[i, j] - 100000) < 10))
                        //    continue;
                        //if (DrawAmpdbData[i, j] >= MaxOfAmpdBValue)
                        //    MaxOfAmpdBValue = DrawAmpdbData[i, j];
                        if (DrawAmpdbData[i, j] <= MinOfAmpdBValue)
                        {
                            DrawAmpdbData[i, j] = MinOfAmpdBValue;
                            DrawPhaseData[i, j] = -180;
                        }
                        //if (DrawPhaseData[i, j] >= MaxOfPhaseValue)
                        //    MaxOfPhaseValue = DrawPhaseData[i, j];
                        //if (DrawPhaseData[i, j] <= MinOfPhaseValue)
                        //    MinOfPhaseValue = DrawPhaseData[i, j];
                    }
                }
            }

            for (int j = 0; j < y_count; j++)//将尚未扫描的点赋值为当前的最小值
            {
                for (int i = 0; i < x_count; i++)
                {
                    if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10))
                    {
                        DrawAmpdbData[i, j] = MinOfAmpdBValue;
                        DrawPhaseData[i, j] = MinOfPhaseValue;
                    }
                }
            }
            if (arrayFlat == 0)
            {
                this.Invoke(new MethodInvoker(() => { textBox13.Text = (MinOfAmpdBValue>=MinOfAmpdBValue_temp?MinOfAmpdBValue_temp:MinOfAmpdBValue).ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox12.Text = MaxOfAmpdBValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox15.Text = MinOfPhaseValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox14.Text = MaxOfPhaseValue.ToString(); }));

                this.Invoke(new MethodInvoker(() => { numericUpDown1.Minimum = Convert.ToDecimal(textBox13.Text); }));
                this.Invoke(new MethodInvoker(() => { numericUpDown1.Maximum = Convert.ToDecimal(textBox12.Text) - (decimal)0.01; }));
                this.Invoke(new MethodInvoker(() => { numericUpDown1.Value = Convert.ToDecimal(MinOfAmpdBValue)>=numericUpDown1.Maximum?numericUpDown1.Maximum: Convert.ToDecimal(MinOfAmpdBValue); }));                
            }
            else if (arrayFlat == 1)
            {
                this.Invoke(new MethodInvoker(() => { textBox24.Text = MinOfAmpdBValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox25.Text = MaxOfAmpdBValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox22.Text = MinOfPhaseValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox23.Text = MaxOfPhaseValue.ToString(); }));
            }
            
            Bitmap AmpdBPic = new Bitmap(x_count, y_count, PixelFormat.Format24bppRgb);
            Bitmap PhasePic = new Bitmap(x_count, y_count, PixelFormat.Format24bppRgb);

            for (int j = 0; j < y_count; j++)
            {
                for (int i = 0; i < x_count; i++)
                {
                    if (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.2))
                    {
                        red = 0;
                        green = 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MinOfAmpdBValue;
                        blue = 255;
                    }

                    if (((MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.2) <= DrawAmpdbData[i, j]) && (DrawAmpdbData[i, j] <= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.4)))
                    {
                        red = 0;
                        green = 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MinOfAmpdBValue;
                        blue = -255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                    }                   

                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.4)) && (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.6)))
                    {
                        red = 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                        green = 255;
                        blue = 0;
                    }                  

                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.6)) && (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.8)))
                    {
                        red = 255;
                        green = -90 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 255 + 90 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.6 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                        blue = 0;
                    }                 

                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.8)) && (DrawAmpdbData[i, j] <= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 1)))
                    {
                        red = 255;
                        green = -165 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 165 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MaxOfAmpdBValue;
                        blue = 0;
                    }
                    
                    if (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.2))
                    {
                        pred = 0;
                        pgreen = 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * MinOfPhaseValue;
                        pblue = 255;
                    }
                    
                    if (((MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.2) <= DrawPhaseData[i, j]) && (DrawPhaseData[i, j] <= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.4)))
                    {
                        pred = 0;
                        pgreen = 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * MinOfPhaseValue;
                        pblue = -255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.4 * (MaxOfPhaseValue - MinOfPhaseValue));
                    }
                    
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.4)) && (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.6)))
                    {
                        pred = 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.4 * (MaxOfPhaseValue - MinOfPhaseValue));
                        pgreen = 255;
                        pblue = 0;
                    }
                   
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.6)) && (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.8)))
                    {
                        pred = 255;
                        pgreen = -90 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 255 + 90 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.6 * (MaxOfPhaseValue - MinOfPhaseValue));
                        pblue = 0;
                    }
                    
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.8)) && (DrawPhaseData[i, j] <= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 1)))
                    {
                        pred = 255;
                        pgreen = -165 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 165 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * MaxOfPhaseValue;
                        pblue = 0;
                    }
                    
                    amp = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
                    pha = Color.FromArgb(Convert.ToInt32(pred), Convert.ToInt32(pgreen), Convert.ToInt32(pblue));
                    AmpdBPic.SetPixel(i, j, amp);////
                    PhasePic.SetPixel(i, j, pha);                   
                }
            }
            if (arrayFlat == 0)
            {
                pictureBox1.Image = AmpdBPic;
                pictureBox2.Image = PhasePic;
            }
           else if (arrayFlat == 1)
            {
                pictureBox8.Image = AmpdBPic;
                pictureBox7.Image = PhasePic;
            }
        }
        public void Chart_Draw_AmpAndPhase(Complex[,] outputdataarray, int arrayFlat, int minFlag)
        {
            double red = 255;
            double green = 255;
            double blue = 255;//定义幅值rgb初值
            double pred = 255;
            double pgreen = 255;
            double pblue = 255;//定义相位rgb初值
            float MaxOfAmpdBValue = -10000000;
            float MinOfAmpdBValue = 10000000;
            float MaxOfPhaseValue = 180;
            float MinOfPhaseValue = -180;
            float MinOfAmpdBValue_temp = 10000000;
            int len_wid = Convert.ToInt32(textBox1.Text);
            Complex[,] TempFreqData = new Complex[len_wid, len_wid];//画图数据 
            DrawAmpdbData = new float[len_wid, len_wid];//幅度结果数据
            DrawPhaseData = new float[len_wid, len_wid];//相位结果数据
            Color amp = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
            Color pha = Color.FromArgb(Convert.ToInt32(pred), Convert.ToInt32(pgreen), Convert.ToInt32(pblue));
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    TempFreqData[i, j] = outputdataarray[i,j];
                }
            }
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if ((System.Math.Abs(TempFreqData[i, j].Real) < 0.0000001) && (System.Math.Abs(TempFreqData[i, j].Imaginary) < 0.0000001))//还未扫描到这里
                    {
                        DrawAmpdbData[i, j] = 100000;//用极大值标记
                        DrawPhaseData[i, j] = 100000;
                    }
                    else
                    {
                        if (arrayFlat == 0)
                        {
                            if (radioButton1.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(20 * Math.Log10(Math.Sqrt(TempFreqData[i, j].Magnitude)));
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 90 / Math.PI);
                            }
                            else if (radioButton2.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(Math.Sqrt(TempFreqData[i, j].Magnitude));
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 90 / Math.PI);
                            }
                        }
                        else if (arrayFlat == 1)
                        {
                            if (radioButton1.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(20 * Math.Log10(TempFreqData[i, j].Magnitude));
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 180 / Math.PI);
                            }
                            else if (radioButton2.Checked)
                            {
                                DrawAmpdbData[i, j] = Convert.ToSingle(TempFreqData[i, j].Magnitude);
                                DrawPhaseData[i, j] = Convert.ToSingle(TempFreqData[i, j].Phase * 180 / Math.PI);
                            }
                        }
                    }
                }
            }
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if (minFlag == 0)
                    {
                        if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10) || (Math.Abs(DrawPhaseData[i, j] - 100000) < 10))
                            continue;
                        if (DrawAmpdbData[i, j] >= MaxOfAmpdBValue)
                            MaxOfAmpdBValue = DrawAmpdbData[i, j];
                        if (DrawAmpdbData[i, j] <= MinOfAmpdBValue)
                            MinOfAmpdBValue = DrawAmpdbData[i, j];
                        if (DrawPhaseData[i, j] >= MaxOfPhaseValue)
                            MaxOfPhaseValue = DrawPhaseData[i, j];
                        if (DrawPhaseData[i, j] <= MinOfPhaseValue)
                            MinOfPhaseValue = DrawPhaseData[i, j];
                    }
                    else if (minFlag == 1)
                    {
                        MaxOfAmpdBValue = Convert.ToSingle(textBox12.Text);
                        MinOfAmpdBValue = Convert.ToSingle(textBox13.Text);
                        //MaxOfPhaseValue = Convert.ToSingle(textBox14.Text);
                        //MinOfPhaseValue = Convert.ToSingle(textBox15.Text);
                        //MinOfPhaseValue = -180;
                        MinOfAmpdBValue_temp = MinOfAmpdBValue;
                        MinOfAmpdBValue = (float)numericUpDown1.Value >= Convert.ToSingle(textBox13.Text) ? (float)numericUpDown1.Value : Convert.ToSingle(textBox13.Text);
                        //if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10) || (Math.Abs(DrawPhaseData[i, j] - 100000) < 10))
                        //    continue;
                        //if (DrawAmpdbData[i, j] >= MaxOfAmpdBValue)
                        //    MaxOfAmpdBValue = DrawAmpdbData[i, j];
                        if (DrawAmpdbData[i, j] <= MinOfAmpdBValue)
                        {
                            DrawAmpdbData[i, j] = MinOfAmpdBValue;
                            DrawPhaseData[i, j] = -180;
                        }
                        //if (DrawPhaseData[i, j] >= MaxOfPhaseValue)
                        //    MaxOfPhaseValue = DrawPhaseData[i, j];
                        //if (DrawPhaseData[i, j] <= MinOfPhaseValue)
                        //    MinOfPhaseValue = DrawPhaseData[i, j];
                    }
                }
            }
            for (int j = 0; j < len_wid; j++)//将尚未扫描的点赋值为当前的最小值
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10))
                    {
                        DrawAmpdbData[i, j] = MinOfAmpdBValue;
                        DrawPhaseData[i, j] = MinOfPhaseValue;
                    }
                }
            }
            if (arrayFlat == 0)
            {
                this.Invoke(new MethodInvoker(() => { textBox13.Text = (MinOfAmpdBValue >= MinOfAmpdBValue_temp ? MinOfAmpdBValue_temp : MinOfAmpdBValue).ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox12.Text = MaxOfAmpdBValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox15.Text = MinOfPhaseValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox14.Text = MaxOfPhaseValue.ToString(); }));

                this.Invoke(new MethodInvoker(() => { numericUpDown1.Minimum = Convert.ToDecimal(textBox13.Text); }));
                this.Invoke(new MethodInvoker(() => { numericUpDown1.Maximum = Convert.ToDecimal(textBox12.Text) - (decimal)0.01; }));
                this.Invoke(new MethodInvoker(() => { numericUpDown1.Value = Convert.ToDecimal(MinOfAmpdBValue) >= numericUpDown1.Maximum ? numericUpDown1.Maximum : Convert.ToDecimal(MinOfAmpdBValue); }));
            }
            else if (arrayFlat == 1)
            {
                this.Invoke(new MethodInvoker(() => { textBox24.Text = MinOfAmpdBValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox25.Text = MaxOfAmpdBValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox22.Text = MinOfPhaseValue.ToString(); }));
                this.Invoke(new MethodInvoker(() => { textBox23.Text = MaxOfPhaseValue.ToString(); }));
            }
            Bitmap AmpdBPic = new Bitmap(len_wid, len_wid, PixelFormat.Format24bppRgb);
            Bitmap PhasePic = new Bitmap(len_wid, len_wid, PixelFormat.Format24bppRgb);
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.2))
                    {
                        red = 0;
                        green = 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MinOfAmpdBValue;
                        blue = 255;
                    }
                    if (((MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.2) <= DrawAmpdbData[i, j]) && (DrawAmpdbData[i, j] <= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.4)))
                    {
                        red = 0;
                        green = 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MinOfAmpdBValue;
                        blue = -255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                    }
                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.4)) && (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.6)))
                    {
                        red = 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                        green = 255;
                        blue = 0;
                    }
                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.6)) && (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.8)))
                    {
                        red = 255;
                        green = -90 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 255 + 90 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.6 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                        blue = 0;
                    }
                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.8)) && (DrawAmpdbData[i, j] <= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 1)))
                    {
                        red = 255;
                        green = -165 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 165 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MaxOfAmpdBValue;
                        blue = 0;
                    }
                    if (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.2))
                    {
                        pred = 0;
                        pgreen = 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * MinOfPhaseValue;
                        pblue = 255;
                    }
                    if (((MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.2) <= DrawPhaseData[i, j]) && (DrawPhaseData[i, j] <= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.4)))
                    {
                        pred = 0;
                        pgreen = 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * MinOfPhaseValue;
                        pblue = -255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.4 * (MaxOfPhaseValue - MinOfPhaseValue));
                    }
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.4)) && (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.6)))
                    {
                        pred = 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.4 * (MaxOfPhaseValue - MinOfPhaseValue));
                        pgreen = 255;
                        pblue = 0;
                    }
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.6)) && (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.8)))
                    {
                        pred = 255;
                        pgreen = -90 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 255 + 90 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.6 * (MaxOfPhaseValue - MinOfPhaseValue));
                        pblue = 0;
                    }
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.8)) && (DrawPhaseData[i, j] <= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 1)))
                    {
                        pred = 255;
                        pgreen = -165 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 165 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * MaxOfPhaseValue;
                        pblue = 0;
                    }
                    amp = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
                    pha = Color.FromArgb(Convert.ToInt32(pred), Convert.ToInt32(pgreen), Convert.ToInt32(pblue));
                    AmpdBPic.SetPixel(i, j, amp);////
                    PhasePic.SetPixel(i, j, pha);
                }
            }
            if (arrayFlat == 0)
            {
                pictureBox1.Image = AmpdBPic;
                pictureBox2.Image = PhasePic;
            }
            else if (arrayFlat == 1)
            {
                pictureBox8.Image = AmpdBPic;
                pictureBox7.Image = PhasePic;
            }
        }

       unsafe private void button1_Click(object sender, EventArgs e)
        {
            VNA = new IE3672();
            int VNA_state=1;
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("192.168.0.101");
                if (reply.Status != IPStatus.Success)
                {
                    MessageBox.Show("检测不到设备，连接失败，请检查仪器IP地址");
                    return;
                }

                if (VNA.IsConnected)
                {
                    VNA.Disconnect();
                }
                meas = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(comboBox2.Text));
                for (int i = 0; i < 10; i++)
                {
                    
                    VNA_state = VNA.Connect();
                    
                    if (VNA_state == 0)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }

                if (VNA_state == 0)
                {
                    //将输入测量量从string转换为sbyte*类型
                    fixed (sbyte* meaParameter = meas)
                    {
                        if (comboBox1.Text == "扫频")
                            sweepType = 0;
                        else
                            sweepType = 1;
                        VNA.SetParameter(float.Parse(textBox4.Text) * 1e9, float.Parse(textBox5.Text) * 1e9, float.Parse(textBox6.Text)* 1e9,float.Parse(textBox26.Text)*1000, float.Parse(textBox3.Text) * 1e3, int.Parse(textBox1.Text), float.Parse(textBox2.Text), meaParameter,sweepType);
                    }

                    VNA.OpenPower();

                    // 连接成功委托
                    MessageBox.Show("连接成功");
                }
                else
                {
                    MessageBox.Show("连接失败");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("连接失败");
            }
        }

        private void ComboBox4_TextChanged_1(object sender, EventArgs e)
        {
            if (output_dataarray != null)
            {
                Chart_Draw_AmpAndPhase(output_dataarray, 0,0);
                Chart_Draw_AmpAndPhase(output_dataarray2, 1,0);
            }
        }

        unsafe private void button9_Click(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            float guance = 0;
            for (int f = 0; f < int.Parse(textBox1.Text); f++)
            {
                guance = float.Parse(textBox4.Text) + (float.Parse(textBox5.Text) - float.Parse(textBox4.Text)) / (float.Parse(textBox1.Text) - 1) * f;
                comboBox4.Items.AddRange(new object[] { guance });
            }
            comboBox4.Text = textBox4.Text;

            //将输入测量量从string转换为sbyte*类型
            meas = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(comboBox2.Text));
            fixed (sbyte* meaParameter = meas)
            {
                if (comboBox1.Text == "扫频")
                    sweepType = 0;
                else
                    sweepType = 1;
                VNA.SetParameter(float.Parse(textBox4.Text) * 1e9, float.Parse(textBox5.Text) * 1e9, float.Parse(textBox6.Text) * 1e9, double.Parse(textBox26.Text) , float.Parse(textBox3.Text) * 1e3, int.Parse(textBox1.Text), float.Parse(textBox2.Text), meaParameter,sweepType);
                VNA.SetParameter(float.Parse(textBox4.Text) * 1e9, float.Parse(textBox5.Text) * 1e9, float.Parse(textBox6.Text) * 1e9, double.Parse(textBox26.Text) , float.Parse(textBox3.Text) * 1e3, int.Parse(textBox1.Text), float.Parse(textBox2.Text), meaParameter,sweepType);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text=="扫频")
            {
                rigol_scan = new Thread(arduino_Scan);
                rigol_scan.Start();
            }
            else if(comboBox1.Text == "点频")
            {
                rigol_scan_cw = new Thread(arduino_Scan_CW);
                rigol_scan_cw.Start();
            }

        }
        public void arduino_Scan()
        {
            double start_x = double.Parse(textBox9.Text);
            this.Invoke(new MethodInvoker(() => { label27.Text = (Math.Round(double.Parse(textBox9.Text) * 5 / 4095, 2)).ToString() + "V"; }));            
            double stop_x = double.Parse(textBox16.Text);
            this.Invoke(new MethodInvoker(() => { label29.Text = (Math.Round(double.Parse(textBox16.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double start_y = double.Parse(textBox10.Text);
            this.Invoke(new MethodInvoker(() => { label28.Text = (Math.Round(double.Parse(textBox10.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double stop_y = double.Parse(textBox17.Text);
            this.Invoke(new MethodInvoker(() => { label30.Text = (Math.Round(double.Parse(textBox17.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double jiange_x = double.Parse(textBox19.Text);
            this.Invoke(new MethodInvoker(() => { label31.Text = (Math.Round(double.Parse(textBox19.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double jiange_y = double.Parse(textBox18.Text);
            this.Invoke(new MethodInvoker(() => { label32.Text = (Math.Round(double.Parse(textBox18.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            doneFlag = false;
            int sleep_time = int.Parse(textBox20.Text);
            x_count = Convert.ToInt32((stop_x - start_x) / jiange_x + 1);
            y_count = Convert.ToInt32((stop_y - start_y) / jiange_y + 1);
            double x_v = start_x;
            double y_v = start_y;


            file_count = 0;
            my_byte = new byte[int.Parse(textBox1.Text) * 2 * x_count * y_count * 4];//所有测量点数需要的字节数
            my_byte1 = new byte[int.Parse(textBox1.Text) * 2 * x_count * y_count * 4];//所有测量点数需要的字节数
            my_byte2 = new byte[int.Parse(textBox1.Text) * 2 * x_count * y_count * 4];//所有测量点数需要的字节数
            output_dataarray = new Complex[x_count, y_count, int.Parse(textBox1.Text)];//画图数组
            output_dataarray1 = new Complex[x_count, y_count, int.Parse(textBox1.Text)];//画图数组
            output_dataarray2 = new Complex[x_count, y_count, int.Parse(textBox1.Text)];//画图数组
            DrawAmpdbData = new float[x_count, y_count];//幅度结果数据
            DrawPhaseData = new float[x_count, y_count];//相位结果数据
            DrawAmpdbData2 = new float[x_count, y_count];//幅度结果数据
            DrawPhaseData2 = new float[x_count, y_count];//相位结果数据
            int fcount = 1;//文件夹重名计数标志
            string dpath = textBox11.Text + "X-"+textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text+"GHz";
            while (Directory.Exists(dpath))
            {
                dpath = textBox11.Text + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz";
                dpath = dpath + "(" + (++fcount).ToString() + ")";
            }
            string path1 = dpath + "\\" + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz"+ "[0].dat";//文件夹路径
            string path2 = dpath + "\\" + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz" + "[1].dat";//文件夹路径
            Directory.CreateDirectory(dpath);//创建文件夹
            
            FileStream ScanLineMemwrite = new FileStream(path1, FileMode.OpenOrCreate);
            byte[] byteArray = new byte[int.Parse(textBox1.Text) * 2 * 4 * x_count * y_count];//初始化建立一个指定大小的新文件（按行）
            ScanLineMemwrite.Write(byteArray, 0, byteArray.Length);//将空字节写入文件中
            ScanLineMemwrite.Close();
            ScanLineMemwrite = new FileStream(path2, FileMode.OpenOrCreate);
            ScanLineMemwrite.Write(byteArray, 0, byteArray.Length);//将空字节写入文件中
            ScanLineMemwrite.Close();


            serialPort1.Write("1" + x_v.ToString() + "\n");
            receiveFromAr();
            serialPort1.Write("2" + y_v.ToString() + "\n");
            receiveFromAr();
            for (int j = 0; j < y_count; j++)
            {
                for (int i = 0; i < x_count; i++)
                {
                    serialPort1.Write("1" + (x_v + i * jiange_x).ToString() + "\n");
                    receiveFromAr();
                    serialPort1.Write("g\n");
                    receiveFromAr();
                    vna_receivedata(ResultArray1,my_byte1);
                    for (int fr = 0; fr < int.Parse(textBox1.Text); fr++)
                    {
                        output_dataarray1[i, j, fr] = new Complex(ResultArray1[2 * fr], ResultArray1[2 * fr + 1]);
                    }

                    serialPort1.Write("k\n");
                    receiveFromAr();
                    vna_receivedata(ResultArray2,my_byte2);
                    for (int fr = 0; fr < int.Parse(textBox1.Text); fr++)
                    {
                        output_dataarray2[i, j, fr] = new Complex(ResultArray2[2 * fr], ResultArray2[2 * fr + 1]);
                    }
                    vna_receivedata(ResultArray, ResultArray1, ResultArray2, my_byte);

                    for (int fr = 0; fr < int.Parse(textBox1.Text); fr++)
                    {
                        output_dataarray[i, j, fr] = output_dataarray2[i,j,fr]-output_dataarray1[i,j,fr];
                    }
                }
                Chart_Draw_AmpAndPhase(output_dataarray,0,0);//一列画一次图
                Chart_Draw_AmpAndPhase(output_dataarray2, 1,0);
                //将数据写入文件
                FileStream fs = new FileStream(path1, FileMode.Open);
                fs.Write(my_byte, 0, my_byte.Length);
                fs.Close();//一列写一次

                fs = new FileStream(path2, FileMode.Open);
                fs.Write(my_byte2, 0, my_byte2.Length);
                fs.Close();

               if(j!=y_count-1)
                {
                    serialPort1.Write("2" + (y_v + (j + 1) * jiange_y).ToString() + "\n");
                    receiveFromAr();
                }
                //Thread.Sleep(sleep_time);
            }
            doneFlag = true;
            radioFlag = true;
            MessageBox.Show("扫描完成！");

        }

        unsafe public void arduino_Scan_CW()
        {
            double start_x = double.Parse(textBox9.Text);
            this.Invoke(new MethodInvoker(() => { label27.Text = (Math.Round(double.Parse(textBox9.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double stop_x = double.Parse(textBox16.Text);
            this.Invoke(new MethodInvoker(() => { label29.Text = (Math.Round(double.Parse(textBox16.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double start_y = double.Parse(textBox10.Text);
            this.Invoke(new MethodInvoker(() => { label28.Text = (Math.Round(double.Parse(textBox10.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double stop_y = double.Parse(textBox17.Text);
            this.Invoke(new MethodInvoker(() => { label30.Text = (Math.Round(double.Parse(textBox17.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double jiange_x = double.Parse(textBox19.Text);
            this.Invoke(new MethodInvoker(() => { label31.Text = (Math.Round(double.Parse(textBox19.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            double jiange_y = double.Parse(textBox18.Text);
            this.Invoke(new MethodInvoker(() => { label32.Text = (Math.Round(double.Parse(textBox18.Text) * 5 / 4095, 2)).ToString() + "V"; }));
            doneFlag = false;
            int sleep_time = int.Parse(textBox20.Text);
            int MaxLenth = int.Parse(textBox1.Text) * int.Parse(textBox1.Text) * 2;
            int len_wid = int.Parse(textBox1.Text);
            x_count = Convert.ToInt32((stop_x - start_x) / jiange_x + 1);
            y_count = Convert.ToInt32((stop_y - start_y) / jiange_y + 1);
            double x_v = start_x;
            double y_v = start_y;
            file_count_CW = 0;
            my_byte = new byte[int.Parse(textBox1.Text) * int.Parse(textBox1.Text) * 2  * 4];//所有测量点数需要的字节数
            my_byte1 = new byte[int.Parse(textBox1.Text) * int.Parse(textBox1.Text) * 2 * 4];//所有测量点数需要的字节数
            my_byte2 = new byte[int.Parse(textBox1.Text) * int.Parse(textBox1.Text) * 2 * 4];//所有测量点数需要的字节数
            output_dataarray_cw = new Complex[len_wid, len_wid];//画图数组
            output_dataarray1_cw = new Complex[len_wid, len_wid];//画图数组
            output_dataarray2_cw = new Complex[len_wid, len_wid];//画图数组

            int fcount = 1;//文件夹重名计数标志
            string dpath = textBox11.Text + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz";
            while (Directory.Exists(dpath))
            {
                dpath = textBox11.Text + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz";
                dpath = dpath + "(" + (++fcount).ToString() + ")";
            }
            string path1 = dpath + "\\" + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz" + "[0].dat";//文件夹路径
            string path2 = dpath + "\\" + "X-" + textBox9.Text + "V-" + textBox16.Text + "V-Y-" + textBox10.Text + "V-" + textBox17.Text + "V-" + textBox19.Text + "V-" + textBox4.Text + "GHz-" + textBox5.Text + "GHz" + "[1].dat";//文件夹路径
            Directory.CreateDirectory(dpath);//创建文件夹

            FileStream ScanLineMemwrite = new FileStream(path1, FileMode.OpenOrCreate);
            byte[] byteArray = new byte[int.Parse(textBox1.Text) * 2 * 4 * int.Parse(textBox1.Text)];//初始化建立一个指定大小的新文件（按行）
            ScanLineMemwrite.Write(byteArray, 0, byteArray.Length);//将空字节写入文件中
            ScanLineMemwrite.Close();
            ScanLineMemwrite = new FileStream(path2, FileMode.OpenOrCreate);
            ScanLineMemwrite.Write(byteArray, 0, byteArray.Length);//将空字节写入文件中
            ScanLineMemwrite.Close();

            serialPort1.Write("1" + x_v.ToString() + "\n");
            receiveFromAr();
            serialPort1.Write("2" + y_v.ToString() + "\n");
            receiveFromAr();
            for (int j = 0; j < len_wid; j++)
            {
                serialPort1.Write("g\n");
                vna_receivedata_cw(ResultArray1, my_byte1);
                for(int i = 0; i < len_wid; i++)
                {
                    output_dataarray1_cw[i,j] = new Complex(ResultArray1[2 * i], ResultArray1[2 * i + 1]);
                }

                serialPort1.Write("k\n");
                receiveFromAr();
                Thread.Sleep(50);
                //vna_receivedata_cw(ResultArray2, my_byte2);
                Task task1 = new Task(() => { vna_receivedata_cw(ResultArray2, my_byte2); });//异步执行VNA扫描
                task1.Start();
                //serialPort1.Write("s"+Convert.ToInt32(start_x).ToString("D4")+Convert.ToInt32(stop_x).ToString("D4")+"\n");
                //receiveFromAr();
                /*for (int i = 0; i < 50; i++)
                {
                    serialPort1.Write("1"+(80*i).ToString() + "\n");
                    receiveFromAr();
                }*/
                rigol_io.PrintfAndFlush("*TRG");
                task1.Wait();              
                for (int i = 0; i < len_wid; i++)
                {
                    output_dataarray2_cw[i,j] = new Complex(ResultArray2[2 * i], ResultArray2[2 * i + 1]); ;
                }
                vna_receivedata_cw(ResultArray, ResultArray1, ResultArray2,my_byte);
                for(int i = 0; i < len_wid; i++)
                {
                    output_dataarray_cw[i, j] = output_dataarray2_cw[i, j] - output_dataarray1_cw[i, j];
                }

                Chart_Draw_AmpAndPhase(output_dataarray_cw, 0, 0);//一列画一次图
                Chart_Draw_AmpAndPhase(output_dataarray2_cw, 1, 0);
                FileStream fs = new FileStream(path1, FileMode.Open);
                fs.Write(my_byte, 0, my_byte.Length);
                fs.Close();//一列写一次

                fs = new FileStream(path2, FileMode.Open);
                fs.Write(my_byte2, 0, my_byte2.Length);
                fs.Close();
                if (j != y_count - 1)
                {
                    serialPort1.Write("2" + (y_v + (j + 1) * jiange_y).ToString() + "\n");
                    receiveFromAr();
                }
            }

            doneFlag = true;
            radioFlag = true;
            MessageBox.Show("扫描完成！");

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                //如果串口打开则关闭串口
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    button11.Text = "打开串口";
                }
                //如果串口关闭则打开串口并进行相关实例的初始化
                else
                {
                    serialPort1.PortName = comboBox3.Text;
                    serialPort1.Open();
                    button11.Text = "关闭串口";
                    rigol_usb = new UsbSession("USB0::0x1AB1::0x0642::DG1ZA214805695::INSTR", Ivi.Visa.AccessModes.None, 2000);
                    rigol_io = rigol_usb.FormattedIO;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("打开串口失败，请确认串口是否被占用");
            }
        }

        private void button18_Click_2(object sender, EventArgs e)
        {
            VNA.ConfTrig();
        }

        unsafe private void button19_Click_1(object sender, EventArgs e)
        {
            huitu();
        }
        public void t_huitu()
        {
            flag = 0;
            do
            {
                huitu();
            } while (flag == 0);
        }
        unsafe public void huitu()
        {
            int MaxLenth = int.Parse(textBox1.Text) * 2;
            int len_wid = int.Parse((System.Math.Sqrt(MaxLenth / 2)).ToString());
            float* real = stackalloc float[MaxLenth / 2];
            float* imagenary = stackalloc float[MaxLenth / 2];
            byte[] trig_byte = new byte[MaxLenth * 4];
            float[] trig_result = new float[MaxLenth];
            VNA.Sweep(real, imagenary);
            for (int i = 0; i < MaxLenth / 2; i++)
            {
                trig_result[2 * i] = *(real + i);
                trig_result[2 * i + 1] = *(imagenary + i);
            }
            Buffer.BlockCopy(trig_result, 0, trig_byte, 0, MaxLenth * 4);
            string path = "D:\\实验\\A.dat";
            FileStream file = new FileStream(path, FileMode.OpenOrCreate);
            file.Write(trig_byte, 0, trig_byte.Length);
            file.Close();

            //绘图
            double red = 255;
            double green = 255;
            double blue = 255;//定义幅值rgb初值
            double pred = 255;
            double pgreen = 255;
            double pblue = 255;//定义相位rgb初值
            float MaxOfAmpdBValue = -10000000;
            float MinOfAmpdBValue = 10000000;
            float MaxOfPhaseValue = -10000000;
            float MinOfPhaseValue = 10000000;
            float[,,] TempFreqData = new float[len_wid, len_wid, 2];//画图数据 
            DrawAmpdbData = new float[len_wid, len_wid];//幅度结果数据
            DrawPhaseData = new float[len_wid, len_wid];//相位结果数据
            Color amp = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
            Color pha = Color.FromArgb(Convert.ToInt32(pred), Convert.ToInt32(pgreen), Convert.ToInt32(pblue));
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    TempFreqData[i, j, 0] = *(real + i + len_wid * j);
                    TempFreqData[i, j, 1] = *(imagenary + i + len_wid * j);
                }
            }
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if ((System.Math.Abs(TempFreqData[i, j, 0]) < 0.0000001) && (System.Math.Abs(TempFreqData[i, j, 1]) < 0.0000001))//还未扫描到这里
                    {
                        DrawAmpdbData[i, j] = 100000;//用极大值标记
                        DrawPhaseData[i, j] = 100000;
                    }
                    else
                    {
                        DrawAmpdbData[i, j] = float.Parse((10 * System.Math.Log10(TempFreqData[i, j, 0] * TempFreqData[i, j, 0] + TempFreqData[i, j, 1] * TempFreqData[i, j, 1])).ToString());
                        DrawPhaseData[i, j] = float.Parse((Math.Atan2(TempFreqData[i, j, 1], TempFreqData[i, j, 0]) * 180 / Math.PI).ToString());
                    }
                }
            }
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10) || (Math.Abs(DrawPhaseData[i, j] - 100000) < 10))
                        continue;
                    if (DrawAmpdbData[i, j] >= MaxOfAmpdBValue)
                        MaxOfAmpdBValue = DrawAmpdbData[i, j];
                    if (DrawAmpdbData[i, j] <= MinOfAmpdBValue)
                        MinOfAmpdBValue = DrawAmpdbData[i, j];
                    if (DrawPhaseData[i, j] >= MaxOfPhaseValue)
                        MaxOfPhaseValue = DrawPhaseData[i, j];
                    if (DrawPhaseData[i, j] <= MinOfPhaseValue)
                        MinOfPhaseValue = DrawPhaseData[i, j];
                }
            }
            for (int j = 0; j < len_wid; j++)//将尚未扫描的点赋值为当前的最小值
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if ((Math.Abs(DrawAmpdbData[i, j] - 100000) < 10))
                    {
                        DrawAmpdbData[i, j] = MinOfAmpdBValue;
                        DrawPhaseData[i, j] = MinOfPhaseValue;
                    }
                }
            }
            this.Invoke(new MethodInvoker(() => { textBox13.Text = MinOfAmpdBValue.ToString(); }));
            this.Invoke(new MethodInvoker(() => { textBox12.Text = MaxOfAmpdBValue.ToString(); }));
            this.Invoke(new MethodInvoker(() => { textBox15.Text = MinOfPhaseValue.ToString(); }));
            this.Invoke(new MethodInvoker(() => { textBox14.Text = MaxOfPhaseValue.ToString(); }));
            Bitmap AmpdBPic = new Bitmap(len_wid, len_wid, PixelFormat.Format24bppRgb);
            Bitmap PhasePic = new Bitmap(len_wid, len_wid, PixelFormat.Format24bppRgb);
            for (int j = 0; j < len_wid; j++)
            {
                for (int i = 0; i < len_wid; i++)
                {
                    if (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.2))
                    {
                        red = 0;
                        green = 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MinOfAmpdBValue;
                        blue = 255;
                    }
                    if (((MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.2) <= DrawAmpdbData[i, j]) && (DrawAmpdbData[i, j] <= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.4)))
                    {
                        red = 0;
                        green = 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MinOfAmpdBValue;
                        blue = -255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                    }
                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.4)) && (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.6)))
                    {
                        red = 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] - 255 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.4 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                        green = 255;
                        blue = 0;
                    }
                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.6)) && (DrawAmpdbData[i, j] < (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.8)))
                    {
                        red = 255;
                        green = -90 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 255 + 90 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * (MinOfAmpdBValue + 0.6 * (MaxOfAmpdBValue - MinOfAmpdBValue));
                        blue = 0;
                    }
                    if ((DrawAmpdbData[i, j] >= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 0.8)) && (DrawAmpdbData[i, j] <= (MinOfAmpdBValue + (MaxOfAmpdBValue - MinOfAmpdBValue) * 1)))
                    {
                        red = 255;
                        green = -165 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * DrawAmpdbData[i, j] + 165 / (0.2 * (MaxOfAmpdBValue - MinOfAmpdBValue)) * MaxOfAmpdBValue;
                        blue = 0;
                    }
                    if (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.2))
                    {
                        pred = 0;
                        pgreen = 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * MinOfPhaseValue;
                        pblue = 255;
                    }
                    if (((MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.2) <= DrawPhaseData[i, j]) && (DrawPhaseData[i, j] <= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.4)))
                    {
                        pred = 0;
                        pgreen = 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.4 * (MaxOfPhaseValue - MinOfPhaseValue)) * MinOfPhaseValue;
                        pblue = -255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.4 * (MaxOfPhaseValue - MinOfPhaseValue));
                    }
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.4)) && (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.6)))
                    {
                        pred = 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] - 255 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.4 * (MaxOfPhaseValue - MinOfPhaseValue));
                        pgreen = 255;
                        pblue = 0;
                    }
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.6)) && (DrawPhaseData[i, j] < (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.8)))
                    {
                        pred = 255;
                        pgreen = -90 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 255 + 90 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * (MinOfPhaseValue + 0.6 * (MaxOfPhaseValue - MinOfPhaseValue));
                        pblue = 0;
                    }
                    if ((DrawPhaseData[i, j] >= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 0.8)) && (DrawPhaseData[i, j] <= (MinOfPhaseValue + (MaxOfPhaseValue - MinOfPhaseValue) * 1)))
                    {
                        pred = 255;
                        pgreen = -165 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * DrawPhaseData[i, j] + 165 / (0.2 * (MaxOfPhaseValue - MinOfPhaseValue)) * MaxOfPhaseValue;
                        pblue = 0;
                    }
                    amp = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
                    pha = Color.FromArgb(Convert.ToInt32(pred), Convert.ToInt32(pgreen), Convert.ToInt32(pblue));
                    AmpdBPic.SetPixel(i, j, amp);////
                    PhasePic.SetPixel(i, j, pha);
                }
            }
            pictureBox1.Image = AmpdBPic;
            pictureBox2.Image = PhasePic;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            conti_trig = new Thread(t_huitu);
            conti_trig.Start();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            flag = 1;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            int outputV = int.Parse(textBox7.Text);
            label20.Text = (Math.Round((double)outputV * 5 / 4095,2)).ToString()+"V";
            if (outputV >= 0 && outputV <= 4095)
            {
                string output2ar = outputV.ToString();
                serialPort1.Write("1"+ output2ar + "\n");
                receiveFromAr();
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            int outputV = int.Parse(textBox8.Text);
            label24.Text = (Math.Round((double)outputV * 5 / 4095, 2)).ToString() + "V";
            if (outputV >= 0 && outputV <= 4095)
            {
                string output2ar = outputV.ToString();
                serialPort1.Write("2" + output2ar + "\n");
                receiveFromAr();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "扫频")
            {
                rigol_scan.Abort();
            }
            else if (comboBox1.Text == "点频")
            {
                rigol_scan_cw.Abort();
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "扫频")
            {
                rigol_scan.Suspend();
            }
            else if (comboBox1.Text == "点频")
            {
                rigol_scan_cw.Suspend();
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "扫频")
            {
                rigol_scan.Resume();
            }
            else if (comboBox1.Text == "点频")
            {
                rigol_scan_cw.Resume();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Write("k\n");//o:继电器关闭；k:arduino22管脚高电平；z:PCF8591模块5V电压
            receiveFromAr();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Write("g\n");//f:继电器关闭；g:arduino22管脚低电平；x:PCF8591模块0V电压
            receiveFromAr();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioFlag = false;
            if (comboBox1.Text == "扫频")
            {
                Chart_Draw_AmpAndPhase(output_dataarray, 0, 0);
                Chart_Draw_AmpAndPhase(output_dataarray2, 1, 0);
            }
            else if(comboBox1.Text == "点频")
            {
                Chart_Draw_AmpAndPhase(output_dataarray_cw, 0, 0);
                Chart_Draw_AmpAndPhase(output_dataarray2_cw, 1, 0);
            }
            radioFlag = true;
        }


        private void textBox13_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (doneFlag == true && radioFlag == true)
            {
                if (comboBox1.Text == "扫频")
                    Chart_Draw_AmpAndPhase(output_dataarray, 0, 1);
                else if (comboBox1.Text == "点频")
                    Chart_Draw_AmpAndPhase(output_dataarray_cw, 0, 1);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DateTime begin = DateTime.Now;
            serialPort1.Write("k\n");
            for(int i = 0; i < 50; i++)
            {
                serialPort1.Write("1" + (i*80).ToString() + "\n");
                receiveFromAr();
            }
            DateTime end = DateTime.Now;
            textBox27.Text = (end - begin).ToString();
        }
    }
}
