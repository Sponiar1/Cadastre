﻿namespace Cadastre
{
    partial class GenerateTreeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            numericUpDown3 = new NumericUpDown();
            numericUpDown4 = new NumericUpDown();
            numericUpDown5 = new NumericUpDown();
            numericUpDown6 = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            numericUpDown7 = new NumericUpDown();
            numericUpDown8 = new NumericUpDown();
            label8 = new Label();
            label9 = new Label();
            numericUpDown9 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown9).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(164, 295);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Generate";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.DecimalPlaces = 2;
            numericUpDown1.Location = new Point(231, 30);
            numericUpDown1.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1000000000, 0, 0, int.MinValue });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 1;
            // 
            // numericUpDown2
            // 
            numericUpDown2.DecimalPlaces = 2;
            numericUpDown2.Location = new Point(231, 59);
            numericUpDown2.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 1000000000, 0, 0, int.MinValue });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(120, 23);
            numericUpDown2.TabIndex = 2;
            // 
            // numericUpDown3
            // 
            numericUpDown3.DecimalPlaces = 2;
            numericUpDown3.Location = new Point(231, 88);
            numericUpDown3.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numericUpDown3.Minimum = new decimal(new int[] { 1000000000, 0, 0, int.MinValue });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(120, 23);
            numericUpDown3.TabIndex = 3;
            numericUpDown3.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // numericUpDown4
            // 
            numericUpDown4.DecimalPlaces = 2;
            numericUpDown4.Location = new Point(231, 117);
            numericUpDown4.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numericUpDown4.Minimum = new decimal(new int[] { 1000000000, 0, 0, int.MinValue });
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(120, 23);
            numericUpDown4.TabIndex = 4;
            numericUpDown4.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // numericUpDown5
            // 
            numericUpDown5.Location = new Point(231, 146);
            numericUpDown5.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numericUpDown5.Name = "numericUpDown5";
            numericUpDown5.Size = new Size(120, 23);
            numericUpDown5.TabIndex = 5;
            numericUpDown5.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // numericUpDown6
            // 
            numericUpDown6.Location = new Point(231, 204);
            numericUpDown6.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown6.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown6.Name = "numericUpDown6";
            numericUpDown6.Size = new Size(120, 23);
            numericUpDown6.TabIndex = 6;
            numericUpDown6.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 30);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 7;
            label1.Text = "Bottom Left X";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 59);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 8;
            label2.Text = "Bottom Left Y";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(60, 88);
            label3.Name = "label3";
            label3.Size = new Size(80, 15);
            label3.TabIndex = 9;
            label3.Text = "Upper Right X";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(60, 117);
            label4.Name = "label4";
            label4.Size = new Size(80, 15);
            label4.TabIndex = 10;
            label4.Text = "Upper Right Y";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(60, 146);
            label5.Name = "label5";
            label5.Size = new Size(99, 15);
            label5.TabIndex = 11;
            label5.Text = "Number of Lands";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(60, 204);
            label6.Name = "label6";
            label6.Size = new Size(43, 15);
            label6.TabIndex = 12;
            label6.Text = "Height";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(60, 234);
            label7.Name = "label7";
            label7.Size = new Size(75, 15);
            label7.TabIndex = 13;
            label7.Text = "Size of Lands";
            // 
            // numericUpDown7
            // 
            numericUpDown7.Location = new Point(231, 234);
            numericUpDown7.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown7.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown7.Name = "numericUpDown7";
            numericUpDown7.Size = new Size(120, 23);
            numericUpDown7.TabIndex = 14;
            numericUpDown7.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // numericUpDown8
            // 
            numericUpDown8.Location = new Point(231, 263);
            numericUpDown8.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown8.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown8.Name = "numericUpDown8";
            numericUpDown8.Size = new Size(120, 23);
            numericUpDown8.TabIndex = 15;
            numericUpDown8.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(60, 265);
            label8.Name = "label8";
            label8.Size = new Size(97, 15);
            label8.TabIndex = 16;
            label8.Text = "Size of Properties";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(60, 177);
            label9.Name = "label9";
            label9.Size = new Size(121, 15);
            label9.TabIndex = 17;
            label9.Text = "Number of properties";
            // 
            // numericUpDown9
            // 
            numericUpDown9.Location = new Point(231, 175);
            numericUpDown9.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numericUpDown9.Name = "numericUpDown9";
            numericUpDown9.Size = new Size(120, 23);
            numericUpDown9.TabIndex = 18;
            numericUpDown9.Value = new decimal(new int[] { 25000, 0, 0, 0 });
            // 
            // GenerateTreeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(423, 346);
            Controls.Add(numericUpDown9);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(numericUpDown8);
            Controls.Add(numericUpDown7);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUpDown6);
            Controls.Add(numericUpDown5);
            Controls.Add(numericUpDown4);
            Controls.Add(numericUpDown3);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(button1);
            Name = "GenerateTreeForm";
            Text = "GenerateTreeForm";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown7).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown8).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown9).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        private NumericUpDown numericUpDown5;
        private NumericUpDown numericUpDown6;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private NumericUpDown numericUpDown7;
        private NumericUpDown numericUpDown8;
        private Label label8;
        private Label label9;
        private NumericUpDown numericUpDown9;
    }
}