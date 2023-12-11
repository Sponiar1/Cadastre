namespace Cadastre
{
    partial class FormBinary
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
            buttonSave = new Button();
            label1 = new Label();
            button2 = new Button();
            buttonLoad = new Button();
            buttonNew = new Button();
            buttonFindProperty = new Button();
            buttonFindLand = new Button();
            buttonAddpropland = new Button();
            buttonDeleteProperty = new Button();
            buttonEditLand = new Button();
            buttonEditProperty = new Button();
            buttonGenerate = new Button();
            buttonDeleteLand = new Button();
            dataGridView1 = new DataGridView();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            textBox1 = new TextBox();
            tabPage3 = new TabPage();
            textBox2 = new TextBox();
            buttonFileExtract = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(24, 24);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 0;
            buttonSave.Text = "Save data";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(151, 32);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // button2
            // 
            button2.Location = new Point(24, 517);
            button2.Name = "button2";
            button2.Size = new Size(103, 35);
            button2.TabIndex = 2;
            button2.Text = "Test structure";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(24, 53);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(75, 23);
            buttonLoad.TabIndex = 3;
            buttonLoad.Text = "Load data";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new Point(24, 82);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(75, 23);
            buttonNew.TabIndex = 4;
            buttonNew.Text = "New file";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonFindProperty
            // 
            buttonFindProperty.Location = new Point(24, 133);
            buttonFindProperty.Name = "buttonFindProperty";
            buttonFindProperty.Size = new Size(102, 29);
            buttonFindProperty.TabIndex = 5;
            buttonFindProperty.Text = "Find property";
            buttonFindProperty.UseVisualStyleBackColor = true;
            buttonFindProperty.Click += buttonFindProperty_Click;
            // 
            // buttonFindLand
            // 
            buttonFindLand.Location = new Point(24, 168);
            buttonFindLand.Name = "buttonFindLand";
            buttonFindLand.Size = new Size(102, 28);
            buttonFindLand.TabIndex = 6;
            buttonFindLand.Text = "Find land";
            buttonFindLand.UseVisualStyleBackColor = true;
            buttonFindLand.Click += buttonFindLand_Click;
            // 
            // buttonAddpropland
            // 
            buttonAddpropland.Location = new Point(24, 202);
            buttonAddpropland.Name = "buttonAddpropland";
            buttonAddpropland.Size = new Size(102, 26);
            buttonAddpropland.TabIndex = 7;
            buttonAddpropland.Text = "Add prop/land";
            buttonAddpropland.UseVisualStyleBackColor = true;
            buttonAddpropland.Click += buttonAddpropland_Click;
            // 
            // buttonDeleteProperty
            // 
            buttonDeleteProperty.Location = new Point(24, 234);
            buttonDeleteProperty.Name = "buttonDeleteProperty";
            buttonDeleteProperty.Size = new Size(102, 26);
            buttonDeleteProperty.TabIndex = 8;
            buttonDeleteProperty.Text = "Delete property";
            buttonDeleteProperty.UseVisualStyleBackColor = true;
            buttonDeleteProperty.Click += buttonDeleteProperty_Click;
            // 
            // buttonEditLand
            // 
            buttonEditLand.Location = new Point(24, 295);
            buttonEditLand.Name = "buttonEditLand";
            buttonEditLand.Size = new Size(102, 23);
            buttonEditLand.TabIndex = 9;
            buttonEditLand.Text = "Edit land";
            buttonEditLand.UseVisualStyleBackColor = true;
            buttonEditLand.Click += buttonEditLand_Click;
            // 
            // buttonEditProperty
            // 
            buttonEditProperty.Location = new Point(24, 324);
            buttonEditProperty.Name = "buttonEditProperty";
            buttonEditProperty.Size = new Size(102, 25);
            buttonEditProperty.TabIndex = 10;
            buttonEditProperty.Text = "Edit Property";
            buttonEditProperty.UseVisualStyleBackColor = true;
            buttonEditProperty.Click += buttonEditProperty_Click;
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new Point(24, 477);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(103, 34);
            buttonGenerate.TabIndex = 11;
            buttonGenerate.Text = "Generate data";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            // 
            // buttonDeleteLand
            // 
            buttonDeleteLand.Location = new Point(24, 266);
            buttonDeleteLand.Name = "buttonDeleteLand";
            buttonDeleteLand.Size = new Size(102, 23);
            buttonDeleteLand.TabIndex = 12;
            buttonDeleteLand.Text = "Delete land";
            buttonDeleteLand.UseVisualStyleBackColor = true;
            buttonDeleteLand.Click += buttonDeleteLand_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(6, 6);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(1058, 558);
            dataGridView1.TabIndex = 13;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(211, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1078, 598);
            tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1070, 570);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Search";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1070, 570);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Land file";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(6, 6);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(1058, 558);
            textBox1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(textBox2);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1070, 570);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Property file";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(6, 6);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(1058, 558);
            textBox2.TabIndex = 0;
            // 
            // buttonFileExtract
            // 
            buttonFileExtract.Location = new Point(24, 395);
            buttonFileExtract.Name = "buttonFileExtract";
            buttonFileExtract.Size = new Size(102, 27);
            buttonFileExtract.TabIndex = 15;
            buttonFileExtract.Text = "File extract";
            buttonFileExtract.UseVisualStyleBackColor = true;
            buttonFileExtract.Click += buttonFileExtract_Click;
            // 
            // FormBinary
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1301, 622);
            Controls.Add(buttonFileExtract);
            Controls.Add(tabControl1);
            Controls.Add(buttonDeleteLand);
            Controls.Add(buttonGenerate);
            Controls.Add(buttonEditProperty);
            Controls.Add(buttonEditLand);
            Controls.Add(buttonDeleteProperty);
            Controls.Add(buttonAddpropland);
            Controls.Add(buttonFindLand);
            Controls.Add(buttonFindProperty);
            Controls.Add(buttonNew);
            Controls.Add(buttonLoad);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(buttonSave);
            Name = "FormBinary";
            Text = "FormBinary";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSave;
        private Label label1;
        private Button button2;
        private Button buttonLoad;
        private Button buttonNew;
        private Button buttonFindProperty;
        private Button buttonFindLand;
        private Button buttonAddpropland;
        private Button buttonDeleteProperty;
        private Button buttonEditLand;
        private Button buttonEditProperty;
        private Button buttonGenerate;
        private Button buttonDeleteLand;
        private DataGridView dataGridView1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBox1;
        private TabPage tabPage3;
        private TextBox textBox2;
        private Button buttonFileExtract;
    }
}