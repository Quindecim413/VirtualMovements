namespace Приемник
{
    partial class TrainingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainingForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownTotalCommands = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTimeForCommand = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonCommandsRandomize = new System.Windows.Forms.Button();
            this.textBoxProgram = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonStartTrain = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.labelTimePassedTotal = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCommandNumber = new System.Windows.Forms.Label();
            this.labelVerifyCommand = new System.Windows.Forms.Label();
            this.numericUpDownVerifyCommand = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelCommandTime = new System.Windows.Forms.Label();
            this.labelAction = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelVerificationTime = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTotalCommands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeForCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVerifyCommand)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(390, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Кол-во команд";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Время на выполнение команды";
            // 
            // numericUpDownTotalCommands
            // 
            this.numericUpDownTotalCommands.Location = new System.Drawing.Point(416, 25);
            this.numericUpDownTotalCommands.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownTotalCommands.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTotalCommands.Name = "numericUpDownTotalCommands";
            this.numericUpDownTotalCommands.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownTotalCommands.TabIndex = 2;
            this.numericUpDownTotalCommands.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownTimeForCommand
            // 
            this.numericUpDownTimeForCommand.Location = new System.Drawing.Point(317, 98);
            this.numericUpDownTimeForCommand.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTimeForCommand.Name = "numericUpDownTimeForCommand";
            this.numericUpDownTimeForCommand.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownTimeForCommand.TabIndex = 3;
            this.numericUpDownTimeForCommand.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonCommandsRandomize
            // 
            this.buttonCommandsRandomize.Location = new System.Drawing.Point(302, 22);
            this.buttonCommandsRandomize.Name = "buttonCommandsRandomize";
            this.buttonCommandsRandomize.Size = new System.Drawing.Size(93, 23);
            this.buttonCommandsRandomize.TabIndex = 4;
            this.buttonCommandsRandomize.Text = "Зарандомить";
            this.buttonCommandsRandomize.UseVisualStyleBackColor = true;
            this.buttonCommandsRandomize.Click += new System.EventHandler(this.buttonCommandsRandomize_Click);
            // 
            // textBoxProgram
            // 
            this.textBoxProgram.Location = new System.Drawing.Point(123, 24);
            this.textBoxProgram.Name = "textBoxProgram";
            this.textBoxProgram.Size = new System.Drawing.Size(173, 20);
            this.textBoxProgram.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(17, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Программа";
            // 
            // buttonStartTrain
            // 
            this.buttonStartTrain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartTrain.Location = new System.Drawing.Point(20, 152);
            this.buttonStartTrain.Name = "buttonStartTrain";
            this.buttonStartTrain.Size = new System.Drawing.Size(452, 34);
            this.buttonStartTrain.TabIndex = 9;
            this.buttonStartTrain.Text = "Приступить";
            this.buttonStartTrain.UseVisualStyleBackColor = true;
            this.buttonStartTrain.Click += new System.EventHandler(this.buttonStartTrain_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 201);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Прошло всего";
            // 
            // labelTimePassedTotal
            // 
            this.labelTimePassedTotal.AutoSize = true;
            this.labelTimePassedTotal.Location = new System.Drawing.Point(130, 201);
            this.labelTimePassedTotal.Name = "labelTimePassedTotal";
            this.labelTimePassedTotal.Size = new System.Drawing.Size(13, 13);
            this.labelTimePassedTotal.TabIndex = 11;
            this.labelTimePassedTotal.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "# команды";
            // 
            // labelCommandNumber
            // 
            this.labelCommandNumber.AutoSize = true;
            this.labelCommandNumber.Location = new System.Drawing.Point(130, 293);
            this.labelCommandNumber.Name = "labelCommandNumber";
            this.labelCommandNumber.Size = new System.Drawing.Size(16, 13);
            this.labelCommandNumber.TabIndex = 13;
            this.labelCommandNumber.Text = "-1";
            // 
            // labelVerifyCommand
            // 
            this.labelVerifyCommand.AutoSize = true;
            this.labelVerifyCommand.Location = new System.Drawing.Point(17, 128);
            this.labelVerifyCommand.Name = "labelVerifyCommand";
            this.labelVerifyCommand.Size = new System.Drawing.Size(228, 13);
            this.labelVerifyCommand.TabIndex = 14;
            this.labelVerifyCommand.Text = "Время непрерывного исполнения команды";
            // 
            // numericUpDownVerifyCommand
            // 
            this.numericUpDownVerifyCommand.Location = new System.Drawing.Point(317, 126);
            this.numericUpDownVerifyCommand.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVerifyCommand.Name = "numericUpDownVerifyCommand";
            this.numericUpDownVerifyCommand.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownVerifyCommand.TabIndex = 15;
            this.numericUpDownVerifyCommand.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Действие";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Время команды";
            // 
            // labelCommandTime
            // 
            this.labelCommandTime.AutoSize = true;
            this.labelCommandTime.Location = new System.Drawing.Point(130, 230);
            this.labelCommandTime.Name = "labelCommandTime";
            this.labelCommandTime.Size = new System.Drawing.Size(13, 13);
            this.labelCommandTime.TabIndex = 18;
            this.labelCommandTime.Text = "0";
            // 
            // labelAction
            // 
            this.labelAction.AutoSize = true;
            this.labelAction.Location = new System.Drawing.Point(130, 317);
            this.labelAction.Name = "labelAction";
            this.labelAction.Size = new System.Drawing.Size(48, 13);
            this.labelAction.TabIndex = 19;
            this.labelAction.Text = "NoMove";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 258);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Подтверждение";
            // 
            // labelVerificationTime
            // 
            this.labelVerificationTime.AutoSize = true;
            this.labelVerificationTime.Location = new System.Drawing.Point(130, 258);
            this.labelVerificationTime.Name = "labelVerificationTime";
            this.labelVerificationTime.Size = new System.Drawing.Size(13, 13);
            this.labelVerificationTime.TabIndex = 21;
            this.labelVerificationTime.Text = "0";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(215, 192);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(257, 155);
            this.textBoxLog.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(470, 39);
            this.label4.TabIndex = 23;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // TrainingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 354);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.labelVerificationTime);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelAction);
            this.Controls.Add(this.labelCommandTime);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDownVerifyCommand);
            this.Controls.Add(this.labelVerifyCommand);
            this.Controls.Add(this.labelCommandNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelTimePassedTotal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonStartTrain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxProgram);
            this.Controls.Add(this.buttonCommandsRandomize);
            this.Controls.Add(this.numericUpDownTimeForCommand);
            this.Controls.Add(this.numericUpDownTotalCommands);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TrainingForm";
            this.Text = "TrainingForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrainingForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTotalCommands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeForCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVerifyCommand)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownTotalCommands;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeForCommand;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonCommandsRandomize;
        private System.Windows.Forms.TextBox textBoxProgram;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonStartTrain;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTimePassedTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelCommandNumber;
        private System.Windows.Forms.Label labelVerifyCommand;
        private System.Windows.Forms.NumericUpDown numericUpDownVerifyCommand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelCommandTime;
        private System.Windows.Forms.Label labelAction;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelVerificationTime;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label label4;
    }
}