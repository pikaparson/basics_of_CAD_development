namespace orsapr
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.SeatLengthTextBox = new System.Windows.Forms.TextBox();
            this.SeatWidthTextBox = new System.Windows.Forms.TextBox();
            this.SeatThicknessTextBox = new System.Windows.Forms.TextBox();
            this.LegsHeightTextBox = new System.Windows.Forms.TextBox();
            this.LegsLengthAndWidthTextBox = new System.Windows.Forms.TextBox();
            this.SeatLengthLabel = new System.Windows.Forms.Label();
            this.LegsLengthLabel = new System.Windows.Forms.Label();
            this.SeatThicknessLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SeatWidthLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.WarningsGroupBox = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SeatParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SeatFormsСomboBox = new System.Windows.Forms.ComboBox();
            this.LegsParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.LegsParametersComboBox = new System.Windows.Forms.ComboBox();
            this.WarningsGroupBox.SuspendLayout();
            this.SeatParametersGroupBox.SuspendLayout();
            this.LegsParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(221, 391);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Построить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Build_Button_Click);
            // 
            // SeatLengthTextBox
            // 
            this.SeatLengthTextBox.BackColor = System.Drawing.Color.White;
            this.SeatLengthTextBox.Location = new System.Drawing.Point(225, 49);
            this.SeatLengthTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.SeatLengthTextBox.Name = "SeatLengthTextBox";
            this.SeatLengthTextBox.Size = new System.Drawing.Size(112, 22);
            this.SeatLengthTextBox.TabIndex = 1;
            this.SeatLengthTextBox.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.SeatLengthTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_OnlyDigitKeyPress);
            // 
            // SeatWidthTextBox
            // 
            this.SeatWidthTextBox.Location = new System.Drawing.Point(225, 75);
            this.SeatWidthTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.SeatWidthTextBox.Name = "SeatWidthTextBox";
            this.SeatWidthTextBox.Size = new System.Drawing.Size(112, 22);
            this.SeatWidthTextBox.TabIndex = 2;
            this.SeatWidthTextBox.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.SeatWidthTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_OnlyDigitKeyPress);
            // 
            // SeatThicknessTextBox
            // 
            this.SeatThicknessTextBox.BackColor = System.Drawing.Color.White;
            this.SeatThicknessTextBox.Location = new System.Drawing.Point(225, 101);
            this.SeatThicknessTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.SeatThicknessTextBox.Name = "SeatThicknessTextBox";
            this.SeatThicknessTextBox.Size = new System.Drawing.Size(112, 22);
            this.SeatThicknessTextBox.TabIndex = 3;
            this.SeatThicknessTextBox.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.SeatThicknessTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_OnlyDigitKeyPress);
            // 
            // LegsHeightTextBox
            // 
            this.LegsHeightTextBox.BackColor = System.Drawing.Color.White;
            this.LegsHeightTextBox.Location = new System.Drawing.Point(225, 53);
            this.LegsHeightTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.LegsHeightTextBox.Name = "LegsHeightTextBox";
            this.LegsHeightTextBox.Size = new System.Drawing.Size(112, 22);
            this.LegsHeightTextBox.TabIndex = 4;
            this.LegsHeightTextBox.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.LegsHeightTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_OnlyDigitKeyPress);
            // 
            // LegsLengthAndWidthTextBox
            // 
            this.LegsLengthAndWidthTextBox.Location = new System.Drawing.Point(225, 79);
            this.LegsLengthAndWidthTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.LegsLengthAndWidthTextBox.Name = "LegsLengthAndWidthTextBox";
            this.LegsLengthAndWidthTextBox.Size = new System.Drawing.Size(112, 22);
            this.LegsLengthAndWidthTextBox.TabIndex = 5;
            this.LegsLengthAndWidthTextBox.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.LegsLengthAndWidthTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_OnlyDigitKeyPress);
            // 
            // SeatLengthLabel
            // 
            this.SeatLengthLabel.AutoSize = true;
            this.SeatLengthLabel.Location = new System.Drawing.Point(77, 53);
            this.SeatLengthLabel.Name = "SeatLengthLabel";
            this.SeatLengthLabel.Size = new System.Drawing.Size(48, 16);
            this.SeatLengthLabel.TabIndex = 6;
            this.SeatLengthLabel.Text = "Длина";
            // 
            // LegsLengthLabel
            // 
            this.LegsLengthLabel.AutoSize = true;
            this.LegsLengthLabel.Location = new System.Drawing.Point(77, 82);
            this.LegsLengthLabel.Name = "LegsLengthLabel";
            this.LegsLengthLabel.Size = new System.Drawing.Size(112, 16);
            this.LegsLengthLabel.TabIndex = 7;
            this.LegsLengthLabel.Text = "Ширина и длина";
            // 
            // SeatThicknessLabel
            // 
            this.SeatThicknessLabel.AutoSize = true;
            this.SeatThicknessLabel.Location = new System.Drawing.Point(77, 105);
            this.SeatThicknessLabel.Name = "SeatThicknessLabel";
            this.SeatThicknessLabel.Size = new System.Drawing.Size(65, 16);
            this.SeatThicknessLabel.TabIndex = 8;
            this.SeatThicknessLabel.Text = "Толщина";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(77, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Высота";
            // 
            // SeatWidthLabel
            // 
            this.SeatWidthLabel.AutoSize = true;
            this.SeatWidthLabel.Location = new System.Drawing.Point(77, 79);
            this.SeatWidthLabel.Name = "SeatWidthLabel";
            this.SeatWidthLabel.Size = new System.Drawing.Size(58, 16);
            this.SeatWidthLabel.TabIndex = 10;
            this.SeatWidthLabel.Text = "Ширина";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "от 300 до 400 мм";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(344, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "от 300 до 600 мм";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(344, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 16);
            this.label8.TabIndex = 13;
            this.label8.Text = "от 20 до 35 мм";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(344, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 16);
            this.label9.TabIndex = 14;
            this.label9.Text = "от 300 до 400 мм";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(344, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 16);
            this.label10.TabIndex = 15;
            this.label10.Text = "от 25 до 35 мм";
            // 
            // WarningsGroupBox
            // 
            this.WarningsGroupBox.Controls.Add(this.label11);
            this.WarningsGroupBox.Location = new System.Drawing.Point(9, 281);
            this.WarningsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.WarningsGroupBox.MaximumSize = new System.Drawing.Size(556, 105);
            this.WarningsGroupBox.MinimumSize = new System.Drawing.Size(556, 105);
            this.WarningsGroupBox.Name = "WarningsGroupBox";
            this.WarningsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.WarningsGroupBox.Size = new System.Drawing.Size(556, 105);
            this.WarningsGroupBox.TabIndex = 17;
            this.WarningsGroupBox.TabStop = false;
            this.WarningsGroupBox.Text = "Предупреждения";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 16);
            this.label11.TabIndex = 0;
            // 
            // SeatParametersGroupBox
            // 
            this.SeatParametersGroupBox.Controls.Add(this.label12);
            this.SeatParametersGroupBox.Controls.Add(this.SeatFormsСomboBox);
            this.SeatParametersGroupBox.Controls.Add(this.SeatLengthLabel);
            this.SeatParametersGroupBox.Controls.Add(this.SeatLengthTextBox);
            this.SeatParametersGroupBox.Controls.Add(this.SeatWidthTextBox);
            this.SeatParametersGroupBox.Controls.Add(this.SeatThicknessTextBox);
            this.SeatParametersGroupBox.Controls.Add(this.label8);
            this.SeatParametersGroupBox.Controls.Add(this.SeatThicknessLabel);
            this.SeatParametersGroupBox.Controls.Add(this.label7);
            this.SeatParametersGroupBox.Controls.Add(this.SeatWidthLabel);
            this.SeatParametersGroupBox.Controls.Add(this.label6);
            this.SeatParametersGroupBox.Location = new System.Drawing.Point(9, 15);
            this.SeatParametersGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.SeatParametersGroupBox.Name = "SeatParametersGroupBox";
            this.SeatParametersGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.SeatParametersGroupBox.Size = new System.Drawing.Size(556, 135);
            this.SeatParametersGroupBox.TabIndex = 18;
            this.SeatParametersGroupBox.TabStop = false;
            this.SeatParametersGroupBox.Text = "Параметры сиденья";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(77, 27);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 16);
            this.label12.TabIndex = 19;
            this.label12.Text = "Форма";
            // 
            // SeatFormsСomboBox
            // 
            this.SeatFormsСomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SeatFormsСomboBox.FormattingEnabled = true;
            this.SeatFormsСomboBox.Items.AddRange(new object[] {
            "Прямоугольная",
            "Круглая"});
            this.SeatFormsСomboBox.Location = new System.Drawing.Point(225, 23);
            this.SeatFormsСomboBox.Margin = new System.Windows.Forms.Padding(4);
            this.SeatFormsСomboBox.Name = "SeatFormsСomboBox";
            this.SeatFormsСomboBox.Size = new System.Drawing.Size(243, 24);
            this.SeatFormsСomboBox.TabIndex = 19;
            this.SeatFormsСomboBox.SelectedIndexChanged += new System.EventHandler(this.SeatTypeComboBox_SelectedIndexChanged);
            // 
            // LegsParametersGroupBox
            // 
            this.LegsParametersGroupBox.Controls.Add(this.label13);
            this.LegsParametersGroupBox.Controls.Add(this.LegsParametersComboBox);
            this.LegsParametersGroupBox.Controls.Add(this.label9);
            this.LegsParametersGroupBox.Controls.Add(this.LegsHeightTextBox);
            this.LegsParametersGroupBox.Controls.Add(this.LegsLengthAndWidthTextBox);
            this.LegsParametersGroupBox.Controls.Add(this.label10);
            this.LegsParametersGroupBox.Controls.Add(this.LegsLengthLabel);
            this.LegsParametersGroupBox.Controls.Add(this.label4);
            this.LegsParametersGroupBox.Location = new System.Drawing.Point(9, 159);
            this.LegsParametersGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.LegsParametersGroupBox.Name = "LegsParametersGroupBox";
            this.LegsParametersGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.LegsParametersGroupBox.Size = new System.Drawing.Size(556, 114);
            this.LegsParametersGroupBox.TabIndex = 20;
            this.LegsParametersGroupBox.TabStop = false;
            this.LegsParametersGroupBox.Text = "Параметры ножек";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(77, 31);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 16);
            this.label13.TabIndex = 17;
            this.label13.Text = "Форма";
            // 
            // LegsParametersComboBox
            // 
            this.LegsParametersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LegsParametersComboBox.FormattingEnabled = true;
            this.LegsParametersComboBox.Items.AddRange(new object[] {
            "Квадратные",
            "Круглые"});
            this.LegsParametersComboBox.Location = new System.Drawing.Point(224, 27);
            this.LegsParametersComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.LegsParametersComboBox.Name = "LegsParametersComboBox";
            this.LegsParametersComboBox.Size = new System.Drawing.Size(243, 24);
            this.LegsParametersComboBox.TabIndex = 16;
            this.LegsParametersComboBox.SelectedIndexChanged += new System.EventHandler(this.LegsTypeComboBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 422);
            this.Controls.Add(this.LegsParametersGroupBox);
            this.Controls.Add(this.SeatParametersGroupBox);
            this.Controls.Add(this.WarningsGroupBox);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.MaximumSize = new System.Drawing.Size(597, 469);
            this.MinimumSize = new System.Drawing.Size(597, 469);
            this.Name = "MainForm";
            this.Text = "Табурет";
            this.WarningsGroupBox.ResumeLayout(false);
            this.WarningsGroupBox.PerformLayout();
            this.SeatParametersGroupBox.ResumeLayout(false);
            this.SeatParametersGroupBox.PerformLayout();
            this.LegsParametersGroupBox.ResumeLayout(false);
            this.LegsParametersGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SeatLengthTextBox;
        private System.Windows.Forms.TextBox SeatWidthTextBox;
        private System.Windows.Forms.TextBox SeatThicknessTextBox;
        private System.Windows.Forms.TextBox LegsHeightTextBox;
        private System.Windows.Forms.TextBox LegsLengthAndWidthTextBox;
        private System.Windows.Forms.Label SeatLengthLabel;
        private System.Windows.Forms.Label LegsLengthLabel;
        private System.Windows.Forms.Label SeatThicknessLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SeatWidthLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox WarningsGroupBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox SeatParametersGroupBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox SeatFormsСomboBox;
        private System.Windows.Forms.GroupBox LegsParametersGroupBox;
        private System.Windows.Forms.ComboBox LegsParametersComboBox;
        private System.Windows.Forms.Label label13;
    }
}

