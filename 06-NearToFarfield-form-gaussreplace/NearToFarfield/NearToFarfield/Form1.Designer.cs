namespace NearToFarfield
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        
        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_source = new System.Windows.Forms.Button();
            this.button_Cal = new System.Windows.Forms.Button();
            this.textBox_Source = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_freq = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_N = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_theta_min = new System.Windows.Forms.TextBox();
            this.textBox_theta_max = new System.Windows.Forms.TextBox();
            this.textBox_phi_min = new System.Windows.Forms.TextBox();
            this.textBox_phi_max = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_delta_theta = new System.Windows.Forms.TextBox();
            this.textBox_delta_phi = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_num_theta = new System.Windows.Forms.TextBox();
            this.textBox_num_phi = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox_L = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox_ds = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_fixed = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox_Source2 = new System.Windows.Forms.TextBox();
            this.button_Source2 = new System.Windows.Forms.Button();
            this.button_Cal2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label19 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(11, 164);
            this.chart1.Margin = new System.Windows.Forms.Padding(2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Gain";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(540, 266);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.Chart1_GetToolTipText);
            // 
            // button_source
            // 
            this.button_source.Location = new System.Drawing.Point(390, 18);
            this.button_source.Name = "button_source";
            this.button_source.Size = new System.Drawing.Size(53, 23);
            this.button_source.TabIndex = 1;
            this.button_source.Text = "浏览";
            this.button_source.UseVisualStyleBackColor = true;
            this.button_source.Click += new System.EventHandler(this.Button_source_Click);
            // 
            // button_Cal
            // 
            this.button_Cal.Location = new System.Drawing.Point(823, 18);
            this.button_Cal.Name = "button_Cal";
            this.button_Cal.Size = new System.Drawing.Size(38, 34);
            this.button_Cal.TabIndex = 3;
            this.button_Cal.Text = "计算";
            this.button_Cal.UseVisualStyleBackColor = true;
            this.button_Cal.Click += new System.EventHandler(this.Button_Cal_Click);
            // 
            // textBox_Source
            // 
            this.textBox_Source.Enabled = false;
            this.textBox_Source.Location = new System.Drawing.Point(100, 18);
            this.textBox_Source.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Source.Name = "textBox_Source";
            this.textBox_Source.Size = new System.Drawing.Size(285, 21);
            this.textBox_Source.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "导入源文件：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "频率(GHz)：";
            // 
            // textBox_freq
            // 
            this.textBox_freq.Location = new System.Drawing.Point(83, 42);
            this.textBox_freq.Name = "textBox_freq";
            this.textBox_freq.Size = new System.Drawing.Size(41, 21);
            this.textBox_freq.TabIndex = 9;
            this.textBox_freq.Text = "10";
            this.textBox_freq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_freq.Validated += new System.EventHandler(this.TextBox_freq_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "点数：";
            // 
            // textBox_N
            // 
            this.textBox_N.Location = new System.Drawing.Point(291, 42);
            this.textBox_N.Name = "textBox_N";
            this.textBox_N.Size = new System.Drawing.Size(37, 21);
            this.textBox_N.TabIndex = 11;
            this.textBox_N.Text = "101";
            this.textBox_N.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_N.Validated += new System.EventHandler(this.TextBox_N_Validated);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(51, 497);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(350, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "一维图：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(460, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "θ范围/°：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(460, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "φ范围/°：";
            // 
            // textBox_theta_min
            // 
            this.textBox_theta_min.Location = new System.Drawing.Point(526, 18);
            this.textBox_theta_min.Name = "textBox_theta_min";
            this.textBox_theta_min.Size = new System.Drawing.Size(25, 21);
            this.textBox_theta_min.TabIndex = 18;
            this.textBox_theta_min.Text = "-3";
            this.textBox_theta_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_theta_min.Validated += new System.EventHandler(this.TextBox_theta_min_Validated);
            // 
            // textBox_theta_max
            // 
            this.textBox_theta_max.Location = new System.Drawing.Point(574, 18);
            this.textBox_theta_max.Name = "textBox_theta_max";
            this.textBox_theta_max.Size = new System.Drawing.Size(25, 21);
            this.textBox_theta_max.TabIndex = 19;
            this.textBox_theta_max.Text = "3";
            this.textBox_theta_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_theta_max.Validated += new System.EventHandler(this.TextBox_theta_max_Validated);
            // 
            // textBox_phi_min
            // 
            this.textBox_phi_min.Location = new System.Drawing.Point(526, 42);
            this.textBox_phi_min.Name = "textBox_phi_min";
            this.textBox_phi_min.Size = new System.Drawing.Size(25, 21);
            this.textBox_phi_min.TabIndex = 20;
            this.textBox_phi_min.Text = "0";
            this.textBox_phi_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_phi_min.Validated += new System.EventHandler(this.TextBox_phi_min_Validated);
            // 
            // textBox_phi_max
            // 
            this.textBox_phi_max.Location = new System.Drawing.Point(574, 42);
            this.textBox_phi_max.Name = "textBox_phi_max";
            this.textBox_phi_max.Size = new System.Drawing.Size(25, 21);
            this.textBox_phi_max.TabIndex = 21;
            this.textBox_phi_max.Text = "180";
            this.textBox_phi_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_phi_max.Validated += new System.EventHandler(this.TextBox_phi_max_Validated);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(557, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(557, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 12);
            this.label10.TabIndex = 23;
            this.label10.Text = "-";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(605, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 24;
            this.label11.Text = "Δθ：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(605, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 25;
            this.label12.Text = "Δφ：";
            // 
            // textBox_delta_theta
            // 
            this.textBox_delta_theta.Location = new System.Drawing.Point(643, 18);
            this.textBox_delta_theta.Name = "textBox_delta_theta";
            this.textBox_delta_theta.Size = new System.Drawing.Size(30, 21);
            this.textBox_delta_theta.TabIndex = 26;
            this.textBox_delta_theta.Text = "0.05";
            this.textBox_delta_theta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_delta_theta.Validated += new System.EventHandler(this.TextBox_delta_theta_Validated);
            // 
            // textBox_delta_phi
            // 
            this.textBox_delta_phi.Location = new System.Drawing.Point(643, 42);
            this.textBox_delta_phi.Name = "textBox_delta_phi";
            this.textBox_delta_phi.Size = new System.Drawing.Size(30, 21);
            this.textBox_delta_phi.TabIndex = 27;
            this.textBox_delta_phi.Text = "18";
            this.textBox_delta_phi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_delta_phi.Validated += new System.EventHandler(this.TextBox_delta_phi_Validated);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(679, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 28;
            this.label13.Text = "θ点数：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(679, 47);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 29;
            this.label14.Text = "φ点数：";
            // 
            // textBox_num_theta
            // 
            this.textBox_num_theta.Enabled = false;
            this.textBox_num_theta.Location = new System.Drawing.Point(725, 18);
            this.textBox_num_theta.Name = "textBox_num_theta";
            this.textBox_num_theta.Size = new System.Drawing.Size(35, 21);
            this.textBox_num_theta.TabIndex = 30;
            this.textBox_num_theta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_num_phi
            // 
            this.textBox_num_phi.Enabled = false;
            this.textBox_num_phi.Location = new System.Drawing.Point(725, 42);
            this.textBox_num_phi.Name = "textBox_num_phi";
            this.textBox_num_phi.Size = new System.Drawing.Size(35, 21);
            this.textBox_num_phi.TabIndex = 31;
            this.textBox_num_phi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(132, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 32;
            this.label15.Text = "边长/m：";
            // 
            // textBox_L
            // 
            this.textBox_L.Location = new System.Drawing.Point(178, 42);
            this.textBox_L.Name = "textBox_L";
            this.textBox_L.Size = new System.Drawing.Size(60, 21);
            this.textBox_L.TabIndex = 33;
            this.textBox_L.Text = "5";
            this.textBox_L.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_L.Validated += new System.EventHandler(this.TextBox_L_Validated);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(336, 47);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 34;
            this.label16.Text = "点间距/m：";
            // 
            // textBox_ds
            // 
            this.textBox_ds.Enabled = false;
            this.textBox_ds.Location = new System.Drawing.Point(393, 44);
            this.textBox_ds.Name = "textBox_ds";
            this.textBox_ds.Size = new System.Drawing.Size(49, 21);
            this.textBox_ds.TabIndex = 35;
            this.textBox_ds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "theta",
            "phi"});
            this.comboBox1.Location = new System.Drawing.Point(131, 106);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(69, 20);
            this.comboBox1.TabIndex = 36;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(130, 129);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(70, 20);
            this.comboBox2.TabIndex = 37;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "Independent axis:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(18, 132);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 39;
            this.label17.Text = "Fixed/°:";
            // 
            // textBox_fixed
            // 
            this.textBox_fixed.Enabled = false;
            this.textBox_fixed.Location = new System.Drawing.Point(77, 128);
            this.textBox_fixed.Name = "textBox_fixed";
            this.textBox_fixed.Size = new System.Drawing.Size(46, 21);
            this.textBox_fixed.TabIndex = 40;
            this.textBox_fixed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(18, 455);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 12);
            this.label18.TabIndex = 41;
            this.label18.Text = "导入馈源面文件：";
            // 
            // textBox_Source2
            // 
            this.textBox_Source2.Enabled = false;
            this.textBox_Source2.Location = new System.Drawing.Point(115, 452);
            this.textBox_Source2.Name = "textBox_Source2";
            this.textBox_Source2.Size = new System.Drawing.Size(270, 21);
            this.textBox_Source2.TabIndex = 42;
            // 
            // button_Source2
            // 
            this.button_Source2.Location = new System.Drawing.Point(390, 452);
            this.button_Source2.Name = "button_Source2";
            this.button_Source2.Size = new System.Drawing.Size(53, 23);
            this.button_Source2.TabIndex = 43;
            this.button_Source2.Text = "浏览";
            this.button_Source2.UseVisualStyleBackColor = true;
            this.button_Source2.Click += new System.EventHandler(this.Button_Source2_Click);
            // 
            // button_Cal2
            // 
            this.button_Cal2.Location = new System.Drawing.Point(823, 452);
            this.button_Cal2.Name = "button_Cal2";
            this.button_Cal2.Size = new System.Drawing.Size(38, 34);
            this.button_Cal2.TabIndex = 44;
            this.button_Cal2.Text = "计算";
            this.button_Cal2.UseVisualStyleBackColor = true;
            this.button_Cal2.Click += new System.EventHandler(this.Button_Cal2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 700);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 45;
            this.label6.Text = "馈源替代后的幅度";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(462, 498);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(350, 200);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 46;
            this.pictureBox2.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(584, 701);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(101, 12);
            this.label19.TabIndex = 47;
            this.label19.Text = "馈源替代后的相位";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 796);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_Cal2);
            this.Controls.Add(this.button_Source2);
            this.Controls.Add(this.textBox_Source2);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBox_fixed);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox_ds);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textBox_L);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBox_num_phi);
            this.Controls.Add(this.textBox_num_theta);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_delta_phi);
            this.Controls.Add(this.textBox_delta_theta);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox_phi_max);
            this.Controls.Add(this.textBox_phi_min);
            this.Controls.Add(this.textBox_theta_max);
            this.Controls.Add(this.textBox_theta_min);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox_N);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_freq);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Source);
            this.Controls.Add(this.button_Cal);
            this.Controls.Add(this.button_source);
            this.Controls.Add(this.chart1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_source;
        private System.Windows.Forms.Button button_Cal;
        private System.Windows.Forms.TextBox textBox_Source;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_freq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_N;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_theta_min;
        private System.Windows.Forms.TextBox textBox_theta_max;
        private System.Windows.Forms.TextBox textBox_phi_min;
        private System.Windows.Forms.TextBox textBox_phi_max;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_delta_theta;
        private System.Windows.Forms.TextBox textBox_delta_phi;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_num_theta;
        private System.Windows.Forms.TextBox textBox_num_phi;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox_L;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_ds;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_fixed;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox_Source2;
        private System.Windows.Forms.Button button_Source2;
        private System.Windows.Forms.Button button_Cal2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label19;
    }
}

