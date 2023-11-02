namespace Cadastre
{
    partial class FormTest
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
            comboBox1 = new ComboBox();
            buttonStart = new Button();
            numericUpDownStart = new NumericUpDown();
            numericUpDownSize = new NumericUpDown();
            numericUpDownHeight = new NumericUpDown();
            labelInitialSize = new Label();
            labelSize = new Label();
            labelHeight = new Label();
            labelItemSize = new Label();
            numericUpDownItemSize = new NumericUpDown();
            numericUpDownOperations = new NumericUpDown();
            labelOperations = new Label();
            labelNewHeight = new Label();
            numericUpDownNewHeight = new NumericUpDown();
            numericUpDownInsert = new NumericUpDown();
            numericUpDownDelete = new NumericUpDown();
            numericUpDownFind = new NumericUpDown();
            labelName = new Label();
            labelInsert = new Label();
            labelRemove = new Label();
            labelFind = new Label();
            checkBoxItems = new CheckBox();
            labelResult = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDownStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownItemSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownOperations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNewHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInsert).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelete).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownFind).BeginInit();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(42, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(350, 277);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(97, 45);
            buttonStart.TabIndex = 1;
            buttonStart.Text = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // numericUpDownStart
            // 
            numericUpDownStart.Location = new Point(206, 38);
            numericUpDownStart.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDownStart.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDownStart.Name = "numericUpDownStart";
            numericUpDownStart.Size = new Size(120, 23);
            numericUpDownStart.TabIndex = 2;
            numericUpDownStart.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // numericUpDownSize
            // 
            numericUpDownSize.Location = new Point(350, 39);
            numericUpDownSize.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDownSize.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDownSize.Name = "numericUpDownSize";
            numericUpDownSize.Size = new Size(120, 23);
            numericUpDownSize.TabIndex = 3;
            numericUpDownSize.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // numericUpDownHeight
            // 
            numericUpDownHeight.Location = new Point(499, 39);
            numericUpDownHeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDownHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownHeight.Name = "numericUpDownHeight";
            numericUpDownHeight.Size = new Size(120, 23);
            numericUpDownHeight.TabIndex = 4;
            numericUpDownHeight.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // labelInitialSize
            // 
            labelInitialSize.AutoSize = true;
            labelInitialSize.Location = new Point(206, 20);
            labelInitialSize.Name = "labelInitialSize";
            labelInitialSize.Size = new Size(76, 15);
            labelInitialSize.TabIndex = 5;
            labelInitialSize.Text = "Items at Start";
            // 
            // labelSize
            // 
            labelSize.AutoSize = true;
            labelSize.Location = new Point(350, 20);
            labelSize.Name = "labelSize";
            labelSize.Size = new Size(27, 15);
            labelSize.TabIndex = 6;
            labelSize.Text = "Size";
            // 
            // labelHeight
            // 
            labelHeight.AutoSize = true;
            labelHeight.Location = new Point(499, 21);
            labelHeight.Name = "labelHeight";
            labelHeight.Size = new Size(43, 15);
            labelHeight.TabIndex = 7;
            labelHeight.Text = "Height";
            // 
            // labelItemSize
            // 
            labelItemSize.AutoSize = true;
            labelItemSize.Location = new Point(635, 20);
            labelItemSize.Name = "labelItemSize";
            labelItemSize.Size = new Size(54, 15);
            labelItemSize.TabIndex = 8;
            labelItemSize.Text = "Item Size";
            // 
            // numericUpDownItemSize
            // 
            numericUpDownItemSize.Location = new Point(635, 38);
            numericUpDownItemSize.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDownItemSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownItemSize.Name = "numericUpDownItemSize";
            numericUpDownItemSize.Size = new Size(120, 23);
            numericUpDownItemSize.TabIndex = 9;
            numericUpDownItemSize.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // numericUpDownOperations
            // 
            numericUpDownOperations.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDownOperations.Location = new Point(206, 111);
            numericUpDownOperations.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDownOperations.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDownOperations.Name = "numericUpDownOperations";
            numericUpDownOperations.Size = new Size(120, 23);
            numericUpDownOperations.TabIndex = 10;
            numericUpDownOperations.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // labelOperations
            // 
            labelOperations.AutoSize = true;
            labelOperations.Location = new Point(206, 93);
            labelOperations.Name = "labelOperations";
            labelOperations.Size = new Size(126, 15);
            labelOperations.TabIndex = 11;
            labelOperations.Text = "Number of Operations";
            // 
            // labelNewHeight
            // 
            labelNewHeight.AutoSize = true;
            labelNewHeight.Location = new Point(499, 93);
            labelNewHeight.Name = "labelNewHeight";
            labelNewHeight.Size = new Size(70, 15);
            labelNewHeight.TabIndex = 12;
            labelNewHeight.Text = "New Height";
            // 
            // numericUpDownNewHeight
            // 
            numericUpDownNewHeight.Location = new Point(499, 111);
            numericUpDownNewHeight.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDownNewHeight.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            numericUpDownNewHeight.Name = "numericUpDownNewHeight";
            numericUpDownNewHeight.Size = new Size(120, 23);
            numericUpDownNewHeight.TabIndex = 13;
            numericUpDownNewHeight.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // numericUpDownInsert
            // 
            numericUpDownInsert.DecimalPlaces = 3;
            numericUpDownInsert.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDownInsert.Location = new Point(212, 211);
            numericUpDownInsert.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownInsert.Name = "numericUpDownInsert";
            numericUpDownInsert.Size = new Size(120, 23);
            numericUpDownInsert.TabIndex = 14;
            numericUpDownInsert.Value = new decimal(new int[] { 4, 0, 0, 65536 });
            // 
            // numericUpDownDelete
            // 
            numericUpDownDelete.DecimalPlaces = 3;
            numericUpDownDelete.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDownDelete.Location = new Point(373, 211);
            numericUpDownDelete.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDelete.Name = "numericUpDownDelete";
            numericUpDownDelete.Size = new Size(120, 23);
            numericUpDownDelete.TabIndex = 15;
            numericUpDownDelete.Value = new decimal(new int[] { 4, 0, 0, 65536 });
            // 
            // numericUpDownFind
            // 
            numericUpDownFind.DecimalPlaces = 3;
            numericUpDownFind.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDownFind.Location = new Point(526, 211);
            numericUpDownFind.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownFind.Name = "numericUpDownFind";
            numericUpDownFind.Size = new Size(120, 23);
            numericUpDownFind.TabIndex = 16;
            numericUpDownFind.Value = new decimal(new int[] { 2, 0, 0, 65536 });
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            labelName.Location = new Point(42, 358);
            labelName.Name = "labelName";
            labelName.Size = new Size(85, 37);
            labelName.TabIndex = 17;
            labelName.Text = "Tester";
            // 
            // labelInsert
            // 
            labelInsert.AutoSize = true;
            labelInsert.Location = new Point(212, 193);
            labelInsert.Name = "labelInsert";
            labelInsert.Size = new Size(77, 15);
            labelInsert.TabIndex = 18;
            labelInsert.Text = "Insert chance";
            // 
            // labelRemove
            // 
            labelRemove.AutoSize = true;
            labelRemove.Location = new Point(373, 193);
            labelRemove.Name = "labelRemove";
            labelRemove.Size = new Size(91, 15);
            labelRemove.TabIndex = 19;
            labelRemove.Text = "Remove chance";
            // 
            // labelFind
            // 
            labelFind.AutoSize = true;
            labelFind.Location = new Point(526, 193);
            labelFind.Name = "labelFind";
            labelFind.Size = new Size(71, 15);
            labelFind.TabIndex = 20;
            labelFind.Text = "Find chance";
            // 
            // checkBoxItems
            // 
            checkBoxItems.AutoSize = true;
            checkBoxItems.Location = new Point(44, 93);
            checkBoxItems.Name = "checkBoxItems";
            checkBoxItems.Size = new Size(106, 19);
            checkBoxItems.TabIndex = 21;
            checkBoxItems.Text = "Check all items";
            checkBoxItems.UseVisualStyleBackColor = true;
            // 
            // labelResult
            // 
            labelResult.AutoSize = true;
            labelResult.Location = new Point(306, 376);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(39, 15);
            labelResult.TabIndex = 22;
            labelResult.Text = "Result";
            // 
            // FormTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelResult);
            Controls.Add(checkBoxItems);
            Controls.Add(labelFind);
            Controls.Add(labelRemove);
            Controls.Add(labelInsert);
            Controls.Add(labelName);
            Controls.Add(numericUpDownFind);
            Controls.Add(numericUpDownDelete);
            Controls.Add(numericUpDownInsert);
            Controls.Add(numericUpDownNewHeight);
            Controls.Add(labelNewHeight);
            Controls.Add(labelOperations);
            Controls.Add(numericUpDownOperations);
            Controls.Add(numericUpDownItemSize);
            Controls.Add(labelItemSize);
            Controls.Add(labelHeight);
            Controls.Add(labelSize);
            Controls.Add(labelInitialSize);
            Controls.Add(numericUpDownHeight);
            Controls.Add(numericUpDownSize);
            Controls.Add(numericUpDownStart);
            Controls.Add(buttonStart);
            Controls.Add(comboBox1);
            Name = "FormTest";
            Text = "FormTest";
            ((System.ComponentModel.ISupportInitialize)numericUpDownStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownItemSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownOperations).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNewHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInsert).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelete).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownFind).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private Button buttonStart;
        private NumericUpDown numericUpDownStart;
        private NumericUpDown numericUpDownSize;
        private NumericUpDown numericUpDownHeight;
        private Label labelInitialSize;
        private Label labelSize;
        private Label labelHeight;
        private Label labelItemSize;
        private NumericUpDown numericUpDownItemSize;
        private NumericUpDown numericUpDownOperations;
        private Label labelOperations;
        private Label labelNewHeight;
        private NumericUpDown numericUpDownNewHeight;
        private NumericUpDown numericUpDownInsert;
        private NumericUpDown numericUpDownDelete;
        private NumericUpDown numericUpDownFind;
        private Label labelName;
        private Label labelInsert;
        private Label labelRemove;
        private Label labelFind;
        private CheckBox checkBoxItems;
        private Label labelResult;
    }
}