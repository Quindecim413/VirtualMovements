namespace Приемник
{
    partial class ChannelsRatingsViewer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.leftChannelsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.rightChannelsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // leftChannelsPanel
            // 
            this.leftChannelsPanel.AutoScroll = true;
            this.leftChannelsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftChannelsPanel.Location = new System.Drawing.Point(0, 0);
            this.leftChannelsPanel.Name = "leftChannelsPanel";
            this.leftChannelsPanel.Size = new System.Drawing.Size(224, 414);
            this.leftChannelsPanel.TabIndex = 0;
            // 
            // rightChannelsPanel
            // 
            this.rightChannelsPanel.AutoScroll = true;
            this.rightChannelsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightChannelsPanel.Location = new System.Drawing.Point(635, 0);
            this.rightChannelsPanel.Name = "rightChannelsPanel";
            this.rightChannelsPanel.Size = new System.Drawing.Size(227, 414);
            this.rightChannelsPanel.TabIndex = 1;
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(231, 0);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.Name = "Series3";
            series4.ChartArea = "ChartArea1";
            series4.Name = "Series4";
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Series.Add(series3);
            this.chart.Series.Add(series4);
            this.chart.Size = new System.Drawing.Size(398, 402);
            this.chart.TabIndex = 2;
            // 
            // ChannelsRatingsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 414);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.rightChannelsPanel);
            this.Controls.Add(this.leftChannelsPanel);
            this.Name = "ChannelsRatingsViewer";
            this.Text = "ChannelsRatingsViewer";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel leftChannelsPanel;
        private System.Windows.Forms.FlowLayoutPanel rightChannelsPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    }
}