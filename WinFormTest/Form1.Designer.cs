﻿namespace WinFormTest
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
            textBox1 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            ctlKeyFileEncryptoBtn = new Button();
            button5 = new Button();
            ctlDownloadButton = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(574, 132);
            button1.Name = "button1";
            button1.Size = new Size(97, 23);
            button1.TabIndex = 1;
            button1.Text = "AesEncrypt";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(477, 426);
            textBox1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(574, 161);
            button2.Name = "button2";
            button2.Size = new Size(97, 23);
            button2.TabIndex = 2;
            button2.Text = "Compress";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(574, 190);
            button3.Name = "button3";
            button3.Size = new Size(97, 23);
            button3.TabIndex = 3;
            button3.Text = "EncodingSubString";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(574, 219);
            button4.Name = "button4";
            button4.Size = new Size(97, 23);
            button4.TabIndex = 4;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // ctlKeyFileEncryptoBtn
            // 
            ctlKeyFileEncryptoBtn.Location = new Point(574, 261);
            ctlKeyFileEncryptoBtn.Name = "ctlKeyFileEncryptoBtn";
            ctlKeyFileEncryptoBtn.Size = new Size(97, 23);
            ctlKeyFileEncryptoBtn.TabIndex = 5;
            ctlKeyFileEncryptoBtn.Text = "RSA加解密";
            ctlKeyFileEncryptoBtn.UseVisualStyleBackColor = true;
            ctlKeyFileEncryptoBtn.Click += ctlKeyFileEncryptoBtn_Click;
            // 
            // button5
            // 
            button5.Location = new Point(574, 290);
            button5.Name = "button5";
            button5.Size = new Size(97, 23);
            button5.TabIndex = 6;
            button5.Text = "JsonHelperTest";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // ctlDownloadButton
            // 
            ctlDownloadButton.Location = new Point(574, 319);
            ctlDownloadButton.Name = "ctlDownloadButton";
            ctlDownloadButton.Size = new Size(97, 23);
            ctlDownloadButton.TabIndex = 7;
            ctlDownloadButton.Text = "Download";
            ctlDownloadButton.UseVisualStyleBackColor = true;
            ctlDownloadButton.Click += ctlDownloadButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 441);
            label1.Name = "label1";
            label1.Size = new Size(43, 17);
            label1.TabIndex = 8;
            label1.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 466);
            Controls.Add(label1);
            Controls.Add(ctlDownloadButton);
            Controls.Add(button5);
            Controls.Add(ctlKeyFileEncryptoBtn);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button ctlKeyFileEncryptoBtn;
        private Button button5;
        private Button ctlDownloadButton;
        private Label label1;
    }
}