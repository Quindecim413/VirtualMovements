namespace Приемник
{
    partial class ChannelWithRatingDataControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.channelName = new System.Windows.Forms.Label();
            this.windowWidth = new System.Windows.Forms.Label();
            this.distribution = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя канала";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ширина окна";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Среднее распределение";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(4, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // channelName
            // 
            this.channelName.AutoSize = true;
            this.channelName.Location = new System.Drawing.Point(25, 27);
            this.channelName.Name = "channelName";
            this.channelName.Size = new System.Drawing.Size(29, 13);
            this.channelName.TabIndex = 4;
            this.channelName.Text = "Имя";
            // 
            // windowWidth
            // 
            this.windowWidth.AutoSize = true;
            this.windowWidth.Location = new System.Drawing.Point(25, 62);
            this.windowWidth.Name = "windowWidth";
            this.windowWidth.Size = new System.Drawing.Size(13, 13);
            this.windowWidth.TabIndex = 5;
            this.windowWidth.Text = "0";
            // 
            // distribution
            // 
            this.distribution.AutoSize = true;
            this.distribution.Location = new System.Drawing.Point(25, 97);
            this.distribution.Name = "distribution";
            this.distribution.Size = new System.Drawing.Size(13, 13);
            this.distribution.TabIndex = 6;
            this.distribution.Text = "0";
            // 
            // ChannelWithRatingDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.distribution);
            this.Controls.Add(this.windowWidth);
            this.Controls.Add(this.channelName);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ChannelWithRatingDataControl";
            this.Size = new System.Drawing.Size(172, 121);
            this.MouseLeave += new System.EventHandler(this.ChannelWithRatingDataControl_MouseLeave);
            this.MouseHover += new System.EventHandler(this.ChannelWithRatingDataControl_MouseHover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label channelName;
        private System.Windows.Forms.Label windowWidth;
        private System.Windows.Forms.Label distribution;
    }
}
