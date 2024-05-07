namespace Misya_Yüksek_İrtifa_Yer_İstasyonu
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            label1 = new Label();
            comboBoxCOM = new ComboBox();
            labelStatue = new Label();
            videoBox = new PictureBox();
            groupBox1 = new GroupBox();
            label4 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox1 = new TextBox();
            button2 = new Button();
            groupBox2 = new GroupBox();
            button5 = new Button();
            button4 = new Button();
            label5 = new Label();
            textBox4 = new TextBox();
            button3 = new Button();
            pictureBox1 = new PictureBox();
            groupBox3 = new GroupBox();
            label2 = new Label();
            label10 = new Label();
            label11 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label9 = new Label();
            dateTimePicker1 = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)videoBox).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(196, 17);
            button1.Name = "button1";
            button1.Size = new Size(106, 23);
            button1.TabIndex = 0;
            button1.Text = "Bağlan";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(51, 21);
            label1.TabIndex = 1;
            label1.Text = "PORT:";
            // 
            // comboBoxCOM
            // 
            comboBoxCOM.FormattingEnabled = true;
            comboBoxCOM.Location = new Point(69, 17);
            comboBoxCOM.Name = "comboBoxCOM";
            comboBoxCOM.Size = new Size(121, 23);
            comboBoxCOM.TabIndex = 2;
            // 
            // labelStatue
            // 
            labelStatue.AutoSize = true;
            labelStatue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            labelStatue.Location = new Point(12, 59);
            labelStatue.Name = "labelStatue";
            labelStatue.Size = new Size(62, 21);
            labelStatue.TabIndex = 3;
            labelStatue.Text = "STATUE";
            // 
            // videoBox
            // 
            videoBox.Location = new Point(15, 22);
            videoBox.Name = "videoBox";
            videoBox.Size = new Size(373, 332);
            videoBox.TabIndex = 5;
            videoBox.TabStop = false;
            videoBox.Click += videoBox_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(videoBox);
            groupBox1.Location = new Point(920, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(539, 408);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "VideoCam";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(394, 51);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 10;
            label4.Text = "PORT";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(433, 48);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(394, 22);
            label3.Name = "label3";
            label3.Size = new Size(17, 15);
            label3.TabIndex = 8;
            label3.Text = "IP";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(433, 14);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 7;
            // 
            // button2
            // 
            button2.Location = new Point(394, 86);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "Connect";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(textBox4);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(pictureBox1);
            groupBox2.Location = new Point(344, 17);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(539, 408);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "GPS";
            // 
            // button5
            // 
            button5.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            button5.Location = new Point(394, 318);
            button5.Name = "button5";
            button5.Size = new Size(46, 36);
            button5.TabIndex = 12;
            button5.Text = "-";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            button4.Location = new Point(394, 276);
            button4.Name = "button4";
            button4.Size = new Size(46, 36);
            button4.TabIndex = 11;
            button4.Text = "+";
            button4.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(392, 25);
            label5.Name = "label5";
            label5.Size = new Size(64, 15);
            label5.TabIndex = 8;
            label5.Text = "LOCATION";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(462, 22);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(71, 23);
            textBox4.TabIndex = 7;
            // 
            // button3
            // 
            button3.Location = new Point(394, 51);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 6;
            button3.Text = "GO";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(15, 22);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(373, 332);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(label10);
            groupBox3.Controls.Add(label11);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label6);
            groupBox3.Location = new Point(12, 103);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(316, 322);
            groupBox3.TabIndex = 12;
            groupBox3.TabStop = false;
            groupBox3.Text = "Veri";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F);
            label2.Location = new Point(121, 91);
            label2.Name = "label2";
            label2.Size = new Size(44, 25);
            label2.TabIndex = 5;
            label2.Text = "null";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 14.25F);
            label10.Location = new Point(121, 54);
            label10.Name = "label10";
            label10.Size = new Size(44, 25);
            label10.TabIndex = 4;
            label10.Text = "null";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 14.25F);
            label11.Location = new Point(121, 19);
            label11.Name = "label11";
            label11.Size = new Size(44, 25);
            label11.TabIndex = 3;
            label11.Text = "null";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 14.25F);
            label8.Location = new Point(13, 91);
            label8.Name = "label8";
            label8.Size = new Size(80, 25);
            label8.TabIndex = 2;
            label8.Text = "BASINÇ:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14.25F);
            label7.Location = new Point(13, 54);
            label7.Name = "label7";
            label7.Size = new Size(91, 25);
            label7.TabIndex = 1;
            label7.Text = "SICAKLIK:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F);
            label6.Location = new Point(13, 19);
            label6.Name = "label6";
            label6.Size = new Size(57, 25);
            label6.TabIndex = 0;
            label6.Text = "NEM:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Red;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label9.Location = new Point(80, 60);
            label9.Name = "label9";
            label9.Size = new Size(143, 21);
            label9.TabIndex = 13;
            label9.Text = "NON-CONNECTED";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(174, 647);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(15, 23);
            dateTimePicker1.TabIndex = 14;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1471, 773);
            Controls.Add(dateTimePicker1);
            Controls.Add(label9);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(labelStatue);
            Controls.Add(comboBoxCOM);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Misya Yüksek İrtifa Roket Takımı";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)videoBox).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private ComboBox comboBoxCOM;
        private Label labelStatue;
        private PictureBox videoBox;
        private GroupBox groupBox1;
        private Button button2;
        private Label label4;
        private TextBox textBox2;
        private Label label3;
        private TextBox textBox1;
        private GroupBox groupBox2;
        private Label label5;
        private TextBox textBox4;
        private Button button3;
        private PictureBox pictureBox1;
        private Button button4;
        private GroupBox groupBox3;
        private Label label9;
        private Button button5;
        private Label label2;
        private Label label10;
        private Label label11;
        private Label label8;
        private Label label7;
        private Label label6;
        private DateTimePicker dateTimePicker1;
    }
}
