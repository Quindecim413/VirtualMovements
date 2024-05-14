namespace Приемник
{
    partial class GetLevels
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
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonStartLevels = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownCommandSec = new System.Windows.Forms.NumericUpDown();
            this.buttonSelectChannels = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.channelsTextBox = new System.Windows.Forms.TextBox();
            this.savingTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCommandSec)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCommand.Location = new System.Drawing.Point(13, 162);
            this.textBoxCommand.Multiline = true;
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.ReadOnly = true;
            this.textBoxCommand.Size = new System.Drawing.Size(427, 65);
            this.textBoxCommand.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonStartLevels
            // 
            this.buttonStartLevels.Location = new System.Drawing.Point(342, 39);
            this.buttonStartLevels.Name = "buttonStartLevels";
            this.buttonStartLevels.Size = new System.Drawing.Size(98, 117);
            this.buttonStartLevels.TabIndex = 2;
            this.buttonStartLevels.Text = "Приступить";
            this.buttonStartLevels.UseVisualStyleBackColor = true;
            this.buttonStartLevels.Click += new System.EventHandler(this.buttonStartLevels_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Время набора команды(сек)";
            // 
            // numericUpDownCommandSec
            // 
            this.numericUpDownCommandSec.Location = new System.Drawing.Point(187, 13);
            this.numericUpDownCommandSec.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownCommandSec.Name = "numericUpDownCommandSec";
            this.numericUpDownCommandSec.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCommandSec.TabIndex = 4;
            this.numericUpDownCommandSec.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // buttonSelectChannels
            // 
            this.buttonSelectChannels.Location = new System.Drawing.Point(12, 39);
            this.buttonSelectChannels.Name = "buttonSelectChannels";
            this.buttonSelectChannels.Size = new System.Drawing.Size(324, 33);
            this.buttonSelectChannels.TabIndex = 5;
            this.buttonSelectChannels.Text = "Назначить каналы";
            this.buttonSelectChannels.UseVisualStyleBackColor = true;
            this.buttonSelectChannels.Click += new System.EventHandler(this.buttonSelectChannels_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 6;
            // 
            // channelsTextBox
            // 
            this.channelsTextBox.Location = new System.Drawing.Point(12, 75);
            this.channelsTextBox.Multiline = true;
            this.channelsTextBox.Name = "channelsTextBox";
            this.channelsTextBox.ReadOnly = true;
            this.channelsTextBox.Size = new System.Drawing.Size(324, 81);
            this.channelsTextBox.TabIndex = 7;
            // 
            // savingTimer
            // 
            this.savingTimer.Interval = 300;
            this.savingTimer.Tick += new System.EventHandler(this.savingTimer_Tick);
            // 
            // GetLevels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 240);
            this.Controls.Add(this.channelsTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSelectChannels);
            this.Controls.Add(this.numericUpDownCommandSec);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStartLevels);
            this.Controls.Add(this.textBoxCommand);
            this.Name = "GetLevels";
            this.Text = "Получение уровней";
            this.Shown += new System.EventHandler(this.GetLevels_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCommandSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonStartLevels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownCommandSec;
        private System.Windows.Forms.Button buttonSelectChannels;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox channelsTextBox;
        private System.Windows.Forms.Timer savingTimer;
    }
}