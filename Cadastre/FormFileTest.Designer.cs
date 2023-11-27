namespace Cadastre
{
    partial class FormFileTest
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox1 = new TextBox();
            numericUpDown4 = new NumericUpDown();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(548, 54);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.DecimalPlaces = 3;
            numericUpDown1.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDown1.Location = new Point(29, 56);
            numericUpDown1.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 1;
            numericUpDown1.Value = new decimal(new int[] { 4, 0, 0, 65536 });
            // 
            // numericUpDown2
            // 
            numericUpDown2.DecimalPlaces = 3;
            numericUpDown2.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDown2.Location = new Point(155, 56);
            numericUpDown2.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(120, 23);
            numericUpDown2.TabIndex = 2;
            numericUpDown2.Value = new decimal(new int[] { 2, 0, 0, 65536 });
            // 
            // numericUpDown3
            // 
            numericUpDown3.DecimalPlaces = 3;
            numericUpDown3.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDown3.Location = new Point(281, 56);
            numericUpDown3.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(120, 23);
            numericUpDown3.TabIndex = 3;
            numericUpDown3.Value = new decimal(new int[] { 4, 0, 0, 65536 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 23);
            label1.Name = "label1";
            label1.Size = new Size(77, 15);
            label1.TabIndex = 4;
            label1.Text = "Insert chance";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(155, 23);
            label2.Name = "label2";
            label2.Size = new Size(71, 15);
            label2.TabIndex = 5;
            label2.Text = "Find chance";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(281, 23);
            label3.Name = "label3";
            label3.Size = new Size(91, 15);
            label3.TabIndex = 6;
            label3.Text = "Remove chance";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(29, 94);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(1199, 541);
            textBox1.TabIndex = 7;
            // 
            // numericUpDown4
            // 
            numericUpDown4.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDown4.Location = new Point(407, 54);
            numericUpDown4.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDown4.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(120, 23);
            numericUpDown4.TabIndex = 8;
            numericUpDown4.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(407, 23);
            label4.Name = "label4";
            label4.Size = new Size(126, 15);
            label4.TabIndex = 9;
            label4.Text = "Number of Operations";
            // 
            // FormFileTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1260, 658);
            Controls.Add(label4);
            Controls.Add(numericUpDown4);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUpDown3);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(button1);
            Name = "FormFileTest";
            Text = "FormFileTest";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private NumericUpDown numericUpDown4;
        private Label label4;
    }
}