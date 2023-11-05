namespace Cadastre
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
            buttonTest = new Button();
            buttonSave = new Button();
            buttonLoad = new Button();
            buttonNew = new Button();
            buttonFindProperty = new Button();
            buttonFindLand = new Button();
            dataGridView1 = new DataGridView();
            buttonGenerate = new Button();
            buttonFindAll = new Button();
            buttonInsert = new Button();
            buttonEdit = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // buttonTest
            // 
            buttonTest.Location = new Point(32, 512);
            buttonTest.Name = "buttonTest";
            buttonTest.Size = new Size(124, 42);
            buttonTest.TabIndex = 0;
            buttonTest.Text = "Test structure";
            buttonTest.UseVisualStyleBackColor = true;
            buttonTest.Click += buttonTest_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(47, 12);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(93, 23);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save data";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(47, 41);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(93, 23);
            buttonLoad.TabIndex = 2;
            buttonLoad.Text = "Load data";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new Point(47, 70);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(93, 23);
            buttonNew.TabIndex = 3;
            buttonNew.Text = "New Tree";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonFindProperty
            // 
            buttonFindProperty.Location = new Point(47, 144);
            buttonFindProperty.Name = "buttonFindProperty";
            buttonFindProperty.Size = new Size(109, 34);
            buttonFindProperty.TabIndex = 4;
            buttonFindProperty.Text = "Find property";
            buttonFindProperty.UseVisualStyleBackColor = true;
            buttonFindProperty.Click += buttonFindProperty_Click;
            // 
            // buttonFindLand
            // 
            buttonFindLand.Location = new Point(47, 184);
            buttonFindLand.Name = "buttonFindLand";
            buttonFindLand.Size = new Size(109, 31);
            buttonFindLand.TabIndex = 5;
            buttonFindLand.Text = "Find land";
            buttonFindLand.UseVisualStyleBackColor = true;
            buttonFindLand.Click += buttonFindLand_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(311, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(732, 542);
            dataGridView1.TabIndex = 6;
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new Point(32, 472);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(124, 34);
            buttonGenerate.TabIndex = 7;
            buttonGenerate.Text = "Generate Data";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            // 
            // buttonFindAll
            // 
            buttonFindAll.Location = new Point(47, 221);
            buttonFindAll.Name = "buttonFindAll";
            buttonFindAll.Size = new Size(109, 35);
            buttonFindAll.TabIndex = 8;
            buttonFindAll.Text = "Find all";
            buttonFindAll.UseVisualStyleBackColor = true;
            buttonFindAll.Click += buttonFindAll_Click;
            // 
            // buttonInsert
            // 
            buttonInsert.Location = new Point(47, 262);
            buttonInsert.Name = "buttonInsert";
            buttonInsert.Size = new Size(109, 33);
            buttonInsert.TabIndex = 9;
            buttonInsert.Text = "Insert land/prop";
            buttonInsert.UseVisualStyleBackColor = true;
            buttonInsert.Click += buttonInsert_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(47, 301);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(109, 29);
            buttonEdit.TabIndex = 10;
            buttonEdit.Text = "Edit item";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += buttonEdit_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(47, 336);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(109, 31);
            buttonDelete.TabIndex = 11;
            buttonDelete.Text = "Delete item";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1055, 566);
            Controls.Add(buttonDelete);
            Controls.Add(buttonEdit);
            Controls.Add(buttonInsert);
            Controls.Add(buttonFindAll);
            Controls.Add(buttonGenerate);
            Controls.Add(dataGridView1);
            Controls.Add(buttonFindLand);
            Controls.Add(buttonFindProperty);
            Controls.Add(buttonNew);
            Controls.Add(buttonLoad);
            Controls.Add(buttonSave);
            Controls.Add(buttonTest);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonTest;
        private Button buttonSave;
        private Button buttonLoad;
        private Button buttonNew;
        private Button buttonFindProperty;
        private Button buttonFindLand;
        private DataGridView dataGridView1;
        private Button buttonGenerate;
        private Button buttonFindAll;
        private Button buttonInsert;
        private Button buttonEdit;
        private Button buttonDelete;
    }
}