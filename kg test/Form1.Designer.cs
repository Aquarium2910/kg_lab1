namespace kg_test
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
            pictureBox1 = new PictureBox();
            drawBtn = new Button();
            panel1 = new Panel();
            button1 = new Button();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(254, 0);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1099, 1015);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // drawBtn
            // 
            drawBtn.Location = new Point(77, 478);
            drawBtn.Margin = new Padding(3, 4, 3, 4);
            drawBtn.Name = "drawBtn";
            drawBtn.Size = new Size(97, 31);
            drawBtn.TabIndex = 1;
            drawBtn.Text = "Малювати";
            drawBtn.UseVisualStyleBackColor = true;
            drawBtn.Click += drawBtn_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(drawBtn);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(254, 1015);
            panel1.TabIndex = 2;
            // 
            // button1
            // 
            button1.BackColor = Color.Khaki;
            button1.Location = new Point(130, 96);
            button1.Name = "button1";
            button1.Size = new Size(105, 38);
            button1.TabIndex = 8;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = false;
            button1.Click += addBtn_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(130, 43);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(68, 27);
            textBox3.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(23, 43);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(68, 27);
            textBox2.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(130, 20);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 5;
            label3.Text = "Center Y:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 20);
            label2.Name = "label2";
            label2.Size = new Size(68, 20);
            label2.TabIndex = 4;
            label2.Text = "Center X:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 84);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 3;
            label1.Text = "Radius:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(23, 107);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(79, 27);
            textBox1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 1015);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Лабораторна №1";
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button drawBtn;
        private Panel panel1;
        private TextBox textBox1;
        private Label label2;
        private Label label1;
        private Button button1;
        private TextBox textBox3;
        private TextBox textBox2;
        private Label label3;
    }
}
