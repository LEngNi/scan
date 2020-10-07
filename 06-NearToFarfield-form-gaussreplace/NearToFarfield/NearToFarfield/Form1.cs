using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;

namespace NearToFarfield
{
    public partial class Form1 : Form
    {
        string path1;//source路径
        string path2;//馈源面source路径
        double freq;//频率
        int N;//点数
        double L;//边长
        double ds;//点间距
        double theta_min;  //theta范围
        double theta_max;//theta范围
        double deltatheta;  //theta精度
        int num_theta;//theta点数
        double phi_min;//phi范围
        double phi_max;//phi范围
        double deltaphi;//phi精度
        int num_phi;//phi点数
        double[] theta;//弧度
        double[] phi;
        double[] theta_angle;//角度
        double[] phi_angle;
        double[,] Far_dB;//dB形式存储远场数据

        public Form1()
        {
            InitializeComponent();//初始化
            freq = double.Parse(textBox_freq.Text) * 1e9;
            N = int.Parse(textBox_N.Text);
            L = double.Parse(textBox_L.Text);
            ds = L / (N - 1);
            textBox_ds.Text = Convert.ToString(ds);
            theta_min = double.Parse(textBox_theta_min.Text) * Math.PI / 180;
            theta_max = double.Parse(textBox_theta_max.Text) * Math.PI / 180;
            deltatheta = double.Parse(textBox_delta_theta.Text) * Math.PI / 180;
            num_theta = (int)Math.Round((theta_max - theta_min) / deltatheta) + 1;
            textBox_num_theta.Text = Convert.ToString(num_theta);
            phi_min = double.Parse(textBox_phi_min.Text) * Math.PI / 180;
            phi_max = double.Parse(textBox_phi_max.Text) * Math.PI / 180;
            deltaphi = double.Parse(textBox_delta_phi.Text) * Math.PI / 180;
            num_phi = (int)Math.Round((phi_max - phi_min) / deltaphi) + 1;
            textBox_num_phi.Text = Convert.ToString(num_phi);
            theta = new double[num_theta];
            phi = new double[num_phi];
            theta_angle = new double[num_theta];
            phi_angle = new double[num_phi];
            Far_dB = new double[num_phi, num_theta];

        }

        public void Button_source_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            path1 = ofd.FileName;
            textBox_Source.Text = path1;
        }
        private void Button_Source2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            path2 = ofd.FileName;
            textBox_Source2.Text = path2;
        }
        private void TextBox_L_Validated(object sender, EventArgs e)
        {
            L = double.Parse(textBox_L.Text);
            ds = L / (N - 1);
            textBox_ds.Text = Convert.ToString(ds);
        }
        private void TextBox_N_Validated(object sender, EventArgs e)
        {
            N = int.Parse(textBox_N.Text);
            ds = L / (N - 1);
            textBox_ds.Text = Convert.ToString(ds);
        }
        private void TextBox_freq_Validated(object sender, EventArgs e)
        {
            freq = double.Parse(textBox_freq.Text)*1e9; 
        }
        private void TextBox_theta_min_Validated(object sender, EventArgs e)
        {
            theta_min = double.Parse(textBox_theta_min.Text) * Math.PI / 180;
            num_theta = (int)Math.Round((theta_max - theta_min) / deltatheta) + 1;
            textBox_num_theta.Text = Convert.ToString(num_theta);
            theta = new double[num_theta];
            theta_angle = new double[num_theta];
            Far_dB = new double[num_phi, num_theta];
        }
        private void TextBox_theta_max_Validated(object sender, EventArgs e)
        {
            theta_max = double.Parse(textBox_theta_max.Text) * Math.PI / 180;
            num_theta = (int)Math.Round((theta_max - theta_min) / deltatheta) + 1;
            textBox_num_theta.Text = Convert.ToString(num_theta);
            theta = new double[num_theta];
            theta_angle = new double[num_theta];
            Far_dB = new double[num_phi, num_theta];
        }
        private void TextBox_delta_theta_Validated(object sender, EventArgs e)
        {
            deltatheta = double.Parse(textBox_delta_theta.Text) * Math.PI / 180;
            num_theta = (int) Math.Round((theta_max - theta_min) / deltatheta) + 1;
            textBox_num_theta.Text = Convert.ToString(num_theta);
            theta = new double[num_theta];
            theta_angle = new double[num_theta];
            Far_dB = new double[num_phi, num_theta];
        }
        private void TextBox_phi_min_Validated(object sender, EventArgs e)
        {
            phi_min = double.Parse(textBox_phi_min.Text) * Math.PI / 180;
            num_phi = (int)Math.Round((phi_max - phi_min) / deltaphi) + 1;
            textBox_num_phi.Text = Convert.ToString(num_phi);
            phi = new double[num_phi];
            phi_angle = new double[num_phi];
            Far_dB = new double[num_phi, num_theta];
        }
        private void TextBox_phi_max_Validated(object sender, EventArgs e)
        {
            phi_max = double.Parse(textBox_phi_max.Text) * Math.PI / 180;
            num_phi = (int)Math.Round((phi_max - phi_min) / deltaphi) + 1;
            textBox_num_phi.Text = Convert.ToString(num_phi);
            phi = new double[num_phi];
            phi_angle = new double[num_phi];
            Far_dB = new double[num_phi, num_theta];
        }
        private void TextBox_delta_phi_Validated(object sender, EventArgs e)
        {
            deltaphi = double.Parse(textBox_delta_phi.Text) * Math.PI / 180;
            num_phi = (int)Math.Round((phi_max - phi_min) / deltaphi) + 1;
            textBox_num_phi.Text = Convert.ToString(num_phi);
            phi = new double[num_phi];
            phi_angle = new double[num_phi];
            Far_dB = new double[num_phi, num_theta];
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = Convert.ToString(comboBox1.SelectedItem);
            if (a == "theta")
            {
                textBox_fixed.Text = "Phi=";
                comboBox2.DataSource = phi_angle;
            }
            else
            {
                textBox_fixed.Text = "Theta=";
                comboBox2.DataSource = theta_angle;
            }
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox2.SelectedIndex;
            chart1.Series[0].Points.Clear();
            chart1.ChartAreas[0].AxisY.Title = "Gain/dB";
            if (textBox_fixed.Text=="Phi=")
            {
                chart1.ChartAreas[0].AxisX.Maximum = theta_angle[num_theta-1];
                chart1.ChartAreas[0].AxisX.Minimum = theta_angle[0];
                chart1.ChartAreas[0].AxisX.Title = "theta/°";
                for (int j = 0; j < num_theta; j++)
                {
                    chart1.Series[0].Points.AddXY(theta_angle[j], Far_dB[i, j]);
                    //chart1.Series["0"].Points.DataBindXY(theta, E0_theta_dB);
                }
            }
            else
            {
                chart1.ChartAreas[0].AxisX.Maximum = phi_angle[num_phi-1];
                chart1.ChartAreas[0].AxisX.Minimum = phi_angle[0];
                chart1.ChartAreas[0].AxisX.Title = "phi/°";
                for (int j = 0; j < num_phi; j++)
                {
                    chart1.Series[0].Points.AddXY(phi_angle[j], Far_dB[j, i]);
                }
            }


        }
        private void Chart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                this.Cursor = Cursors.Cross;//在数据线上显示十字
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("({0},{1:F3})", dp.XValue, dp.YValues[0]);
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void Button_Cal_Click(object sender, EventArgs e)
        {
            double[,] source = ReadTxttest(path1, N);//导入源文件
            Complex[,] Ex = new Complex[N, N];//极化方向幅度与相位一起复数表达
            Complex[,] Ey = new Complex[N, N];
            double[,] X = new double[N, N];//点阵范围，积分坐标范围
            double[,] Y = new double[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    //将幅度相位联合复数表达
                    int temp = i * N + j;
                    Ex[i, j] = new Complex(source[temp, 0] * Math.Cos(source[temp, 1] * Math.PI / 180), source[temp, 0] * Math.Sin(source[temp, 1] * Math.PI / 180));
                    Ey[i, j] = new Complex(source[temp, 2] * Math.Cos(source[temp, 3] * Math.PI / 180), source[temp, 2] * Math.Sin(source[temp, 3] * Math.PI / 180));
                    X[i, j] = -L / 2 + j * ds;
                    Y[i, j] = -L / 2 + i * ds;
                }
            }

            for (int i = 0; i < num_theta; i++)
            {
                theta[i] = theta_min + i * deltatheta;
                theta_angle[i] = theta[i] * 180 / Math.PI;
            }

            Complex[] temp_E = new Complex[num_theta];
            double[,] E_Abs = new double[num_phi,num_theta];
            
            Parallel.For(0, num_phi, i =>
            {
                phi[i] = phi_min + i * deltaphi;
                phi_angle[i] = phi[i] * 180 / Math.PI;
                temp_E = PatternCalculate(Ex, Ey, X, Y, theta, phi[i], freq);
                for (int j = 0; j < num_theta; j++)
                {
                    E_Abs[i, j] = Complex.Abs(temp_E[j]);  //远场方向图幅度
                }
            });

            Far_dB = LinearTodB(E_Abs, num_phi, num_theta);
        }
        private void Button_Cal2_Click(object sender, EventArgs e)
        {
            int N = 801, i, j;
            double[,] source = ReadTxttest(path2, N);//导入源文件
            double[] tempx = new double[N];
            double[] tempy = new double[N];
            for(i=0; i<N; i ++)
            {
                tempx[i] = source[i, 0];
                tempy[i] = source[i, 2];
            }
            int index = (tempx.Max() >= tempy.Max()) ? 0 : 2;
            double[,] Abs_E0 = new double[N, N];
            double[,] Pha_E0 = new double[N, N];

            for (i = 0; i < N; i++)
            {
                for (j = 0; j < N; j++)
                {
                    int temp = i * N + j;
                    Abs_E0[i, j] = source[temp, index];
                    Pha_E0[i, j] = source[temp, index + 1];
                }
            }

            double[,] Abs_E2;
            double[,] Pha_E2;
            int Ni, Nj;
            GaussReplace(out Abs_E2, out Pha_E2, out Ni, out Nj, Abs_E0, Pha_E0, 15);
            Bitmap bitdata1 = Rainbowpic(Abs_E2, Ni, Nj);
            Bitmap bitdata2 = Rainbowpic(Pha_E2, Ni, Nj);

            //绘二维图
            Image img1 = Image.FromHbitmap(bitdata1.GetHbitmap());
            Image img2 = Image.FromHbitmap(bitdata2.GetHbitmap());
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox1.Image = img1;
            pictureBox2.Image = img2;
            pictureBox1.Show();
            pictureBox2.Show();
        }

        public static Complex[] PatternCalculate(Complex[,] Ex, Complex[,] Ey,double[,] X, double[,] Y, double[] theta, double phi, double freq)//远场计算函数
        {
            //计算函数
            int Nx = Ex.GetLength(0);
            int Ny = Ey.GetLength(0);
            double dx = X[0, 1] - X[0, 0];
            double dy = Y[1, 0] - Y[0, 0];
            double ds = dx * dy;
            double c = 2.99792458E8;
            double lambda = c / freq;
            double beta = 2 * Math.PI / lambda;
            double r = 1E9 * lambda;
            int Num_theta = theta.Length;
            Complex[] Px = new Complex[Num_theta];
            Complex[] Py = new Complex[Num_theta];
            Complex[] E_theta = new Complex[Num_theta];//theta方向分量
            Complex[] E_phi = new Complex[Num_theta];//phi方向分量
            Complex[] E = new Complex[Num_theta];//合成量
            Complex temp = new Complex(Math.Sin(beta * r), Math.Cos(beta * r));

            Parallel.For(0, Num_theta, i =>
            {
                double sincos = Math.Sin(theta[i]) * Math.Cos(phi);
                double sinsin = Math.Sin(theta[i]) * Math.Sin(phi);

                for (int m = 0; m < Nx; m++)
                {
                    for (int n = 0; n < Ny; n++)
                    {
                        double x = X[m, n];
                        double y = Y[m, n];
                        Complex temp1= Complex.FromPolarCoordinates(1, beta * (x * sincos + y * sinsin))*ds;
                        Complex tempx = Ex[m, n] * temp1;
                        Complex tempy = Ey[m, n] * temp1;
                        Px[i] = Px[i] + tempx;
                        Py[i] = Py[i] + tempy;
                    }
                }

                E_theta[i] = (Px[i] * Math.Cos(phi) + Py[i] * Math.Sin(phi)) * temp * beta / (2 * Math.PI * r);
                E_phi[i] = (Py[i] * Math.Cos(phi) - Px[i] * Math.Sin(phi)) * temp * beta * Math.Cos(theta[i]) / (2 * Math.PI * r);
                E[i] = Complex.Sqrt(E_theta[i] * E_theta[i] + E_phi[i] * E_phi[i]);
            });

            return E;
        }

        public static double[,] ReadTxttest(string Path, int N)  //读取txt数据
        {
            //初始化二维数组
            int rows = N * N;
            double[,] array = new double[rows, 4];
            string[] a;
            string[] lines = File.ReadAllLines(Path);
            for(int i = 0; i < rows; i++)//并行更慢
            {
                String line = lines[i];
                a = line.Split('\t');
                for (int j = 0; j < 4; j++)
                {
                    array[i, j] = double.Parse(a[j]);
                }
            }
            return array;
        }

        public static double[,] Gaussbeam(int N)//归一化束腰处等相位面高斯波束，点数(奇数)，束腰半径
        {
            double[,] Gauss = new double[N, N];
            double ds = 0.01;
            double L = (N - 1) * ds;//馈源边长
            double w0 = L / 2;//束腰半径大小
            double tempx;
            double tempy;
            double tempr2;
            for(int i=0;i<N;i++)
            {
                for(int j=0;j<N;j++)
                {
                    tempx = (i - (N - 1) / 2) * ds;
                    tempy = (j - (N - 1) / 2) * ds;
                    tempr2 = tempx * tempx + tempy * tempy;
                    if (tempr2 >= L * L / 4)
                        Gauss[i, j] = 0;
                    else
                        Gauss[i,j] = Math.Exp(-tempr2 / (w0 * w0));
                }
            }
            return Gauss;
        }

        public static void GaussReplace(out double[,] Abs_E2, out double[,] Pha_E2, out int N_i, out int N_j, double[,] Abs_E0, double[,] Pha_E0, int Nx)//高斯波束替代馈源，Nx为横向馈源个数
        {
            double feedL =(2* (650 / (2 * Nx))+1) / 100.0;//馈源边长保留到小数点后两位
            int beam_N = (int)(feedL*100-1);//每个馈源占用格点数，偶数，即Gaussbeam中的N-1，相邻馈源公用边界点
            double L = (beam_N * Nx + beam_N / 2) / 100.0; //馈源面横向边长
            int Ny = (int)Math.Ceiling(Nx * 5.0 / 7.0);//纵向馈源个数
            int N = Nx * Ny;//馈源总个数
            int[] Line = new int[N];//第n个馈源所在的行
            int[] X = new int[N];//馈源圆心坐标
            int[] Y = new int[N];
            int n, i, j;
            for(n=0;n<N;n++)
            {
                Line[n] = (int)Math.Floor(n*1.0 / Nx);//第n个馈源所在的行
                X[n] = beam_N / 2 + (n % Nx) * beam_N + (Line[n] % 2) * beam_N / 2;
                Y[n] = (int)Math.Floor(beam_N / 2.0 + Line[n] * beam_N * Math.Sqrt(3.0) / 2);
            }
            N_i = (int)(L * 100+1);//馈源面横向点数
            N_j = Y[N - 1] + beam_N / 2 + 1;//馈源面纵向点数

            //将801*801点数据转换成上面算出的馈源面大小中
            double[,] Abs_E1 = new double[N_i, N_j];
            double[,] Pha_E1 = new double[N_i, N_j];
            int offset1 = 400 - N_i / 2;//偏移量
            int offset2 = 400 - N_j / 2;
            for(i=0;i<N_i;i++)
            {
                for(j=0;j<N_j;j++)
                {
                    Abs_E1[i, j] = Abs_E0[offset1 + i, offset2 + j];
                    Pha_E1[i, j] = Pha_E0[offset1 + i, offset2 + j];
                }
            }

            //每个馈源范围内场积分(累加)求平均，得到每个馈源的幅度值分布
            int beam_N0 = beam_N + 1;//每个馈源的点数
            double[] av_Ex = new double[N];
            offset1 = (beam_N0 - 1) / 2;//求平均能量时用到的每个馈源偏移量

            double temp;
            for (n = 0; n < N; n++)//并行更慢
            {
                temp = 0;
                for (i = 0; i < beam_N0; i++)
                {
                    for (j = 0; j < beam_N0; j++)
                    {
                        temp += Math.Pow(Abs_E1[X[n] - offset1 + i, Y[n] - offset1 + j], 2);
                    }
                }
                av_Ex[n] = Math.Sqrt(temp) / (beam_N0 * beam_N0);
            }


            //替代成Gaussbeam阵列
            Abs_E2 = new double[N_i, N_j];
            Pha_E2 = new double[N_i, N_j];

            int temp1;
            int temp2;

            for(n=0; n<N; n++)
            {
                for (i = 0; i < beam_N0; i++)
                {
                    for (j = 0; j < beam_N0; j++)
                    {
                        temp = av_Ex[n] * Gaussbeam(beam_N0)[i, j];
                        temp1 = X[n] - offset1 + i;
                        temp2 = Y[n] - offset1 + j;
                        if ( temp> Abs_E2[temp1, temp2])
                        {
                            Abs_E2[temp1, temp2] = temp;
                            Pha_E2[temp1, temp2] = -Pha_E0[X[n], Y[n]];
                        }
                    }
                }
            }
        }

        public static Bitmap Rainbowpic(double[,] E, int num_i, int num_j)//化成伪彩色图
        {
            double E_Max = E[0, 0];
            double E_Min = E[0, 0];
            for (int i = 0; i < num_i; i++)
            {
                for (int j = 0; j < num_j; j++)
                {
                    if (E[i, j] > E_Max)
                    {
                        E_Max = E[i, j];
                    }
                    if (E[i, j] < E_Min)
                    {
                        E_Min = E[i, j];
                    }
                }
            }

            Bitmap bitdata = new Bitmap(num_i, num_j); //创建原图大小的空白位图

            int temp_pic = 0;
            int temp_r = 0;
            int temp_g = 0;
            int temp_b = 0;
            for (int i = 0; i < num_i; i++)
            {
                for (int j = 0; j < num_j; j++)
                {

                    temp_pic = (int)((E[i, j] - E_Min) * 255 / (E_Max - E_Min));

                    if (temp_pic <= 63)
                    {
                        temp_r = 0;
                        temp_g = 254 - 4 * temp_pic;
                        temp_b = 255;
                    }
                    else if (temp_pic <= 127)
                    {
                        temp_r = 0;
                        temp_g = 4 * temp_pic - 254;
                        temp_b = 510 - 4 * temp_pic;
                    }
                    else if (temp_pic <= 191)
                    {
                        temp_r = 4 * temp_pic - 510;
                        temp_g = 255;
                        temp_b = 0;
                    }
                    else
                    {
                        temp_r = 255;
                        temp_g = 1022 - 4 * temp_pic;
                        temp_b = 0;
                    }

                    Color c = Color.FromArgb(temp_r, temp_g, temp_b);    //通过位byte数组获取每个像素大小
                    bitdata.SetPixel(i, j, c);            //设置每个像素
                }
            }
            return bitdata;
        }

        public static double[,] LinearTodB(double[,] E, int num_i, int num_j)//线性变成dB形式
        {
            double[,] E_dB = new double[num_i, num_j];
            double E_Max = E[0, 0];
            double E_Min = E[0, 0];
            for (int i = 0; i < num_i; i++)
            {
                for (int j = 0; j < num_j; j++)
                {
                    if (E[i, j] > E_Max)
                    {
                        E_Max = E[i, j];
                    }
                    if (E[i, j] < E_Min)
                    {
                        E_Min = E[i, j];
                    }
                }
            }

            for (int i = 0; i < num_i; i++)
            {
                for (int j = 0; j < num_j; j++)
                {
                    E_dB[i, j] = 20 * Math.Log10(E[i, j] / E_Max);  //远场方向图幅度dB形式
                }
            }
            return E_dB;
        }

    }
}
