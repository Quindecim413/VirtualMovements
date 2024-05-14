using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Приемник
{
    public partial class ChannelsRatingsViewer : Form
    {
        public class ChannelWithRatingData
        {
            public SelectChannelsForm.Channel Channel;
            public double WindowWidth;
            public double AVGDistribution;
            public int Ind;
            public bool Selected { get; set; }
        }

        public List<ChannelWithRatingData> leftChannels = new List<ChannelWithRatingData>();
        public List<ChannelWithRatingData> rightChannels = new List<ChannelWithRatingData>();

        public ChannelsRatingsViewer(List<ChannelWithRatingData> leftChannels, List<ChannelWithRatingData> rightChannels)
        {
            InitializeComponent();

            this.leftChannels.AddRange(leftChannels);
            this.rightChannels.AddRange(rightChannels);

            this.leftChannels.Sort((el1, el2) => Math.Abs(el2.WindowWidth) > Math.Abs(el1.WindowWidth) ? +1 
                                                 : Math.Abs(el2.WindowWidth) == Math.Abs(el1.WindowWidth) ? 0: -1);
            this.rightChannels.Sort((el1, el2) => Math.Abs(el2.WindowWidth) > Math.Abs(el1.WindowWidth) ? +1
                                                 : Math.Abs(el2.WindowWidth) == Math.Abs(el1.WindowWidth) ? 0 : -1);

            ShowChannelsToUser();
			
            
        }

        private void ClickedChannel(ChannelWithRatingData clickedChannel, bool left)
        {
            chart.Series[2].Points.Clear();
            chart.Series[3].Points.Clear();
            Thread t = new Thread(() =>
            {
                Thread.Sleep(250);
                Invoke(new Action(() => {
                    if (left)
                    {
                        chart.Series[2].Points.AddXY(clickedChannel.AVGDistribution, clickedChannel.WindowWidth);
                    }
                    else
                    {
                        chart.Series[3].Points.AddXY(clickedChannel.AVGDistribution, clickedChannel.WindowWidth);
                    }
                }));
            });
            t.Start();
        }

        public ChannelsRatingsViewer()
        {
            InitializeComponent();
            leftChannels = new List<ChannelWithRatingData>();
            rightChannels = new List<ChannelWithRatingData>();
            Random rand = new Random();

            for(int i = 0; i < 5; i++)
            {
                leftChannels.Add(new ChannelWithRatingData()
                {
                    Channel = new SelectChannelsForm.Channel(SelectChannelsForm.Channel.Names[i], i, SelectChannelsForm.Channel.SelectionStatus.Left),
                    WindowWidth = rand.Next(100, 1500),
                    AVGDistribution = rand.Next(20, 1000),
                    Selected = true,
                    Ind = i
                });
            }

            for (int i = 15; i < 22; i++)
            {
                rightChannels.Add(new ChannelWithRatingData()
                {
                    Channel = new SelectChannelsForm.Channel(SelectChannelsForm.Channel.Names[i], i, SelectChannelsForm.Channel.SelectionStatus.Right),
                    WindowWidth = rand.Next(100, 1500),
                    AVGDistribution = rand.Next(20, 1000),
                    Selected = true,
                    Ind = i
                });
            }

            ShowChannelsToUser();
        }

        /// <summary>
        /// Shows channels with short quality info to user
        /// </summary>
        public void ShowChannelsToUser()
        {
            foreach (var left in leftChannels) {
                var control = new ChannelWithRatingDataControl(left);
                control.Clicked += (e) =>
                  {
                      ClickedChannel(left, true);
                  };
                leftChannelsPanel.Controls.Add(control); 
            }
            foreach(var right in rightChannels)
            {
                var control = new ChannelWithRatingDataControl(right);
                control.Clicked += (e) =>
                {
                    ClickedChannel(right, false);
                };
                rightChannelsPanel.Controls.Add(control);
            }


			chart.Series[0].ChartType = SeriesChartType.FastPoint;
			chart.Series[0].Color = Color.Red;

			chart.Series[1].ChartType = SeriesChartType.FastPoint;
			chart.Series[1].Color = Color.Blue;

			chart.Series[2].ChartType = SeriesChartType.Bubble;
			chart.Series[2].Color = Color.Red;

			chart.Series[3].ChartType = SeriesChartType.Bubble;
			chart.Series[3].Color = Color.Blue;

			chart.ChartAreas[0].AxisX.LabelStyle.Format = "#.##";

			foreach (var channel in leftChannels)
			{
				var point = new DataPoint(channel.AVGDistribution, channel.WindowWidth);
				chart.Series[0].Points.Add(point);
				var ann = new TextAnnotation();
				ann.Text = channel.Channel.Name.ToString();
				ann.AnchorDataPoint = point;

				chart.Annotations.Add(ann);
			}
			foreach (var channel in rightChannels)
			{
				var point = new DataPoint(channel.AVGDistribution, channel.WindowWidth);
				chart.Series[1].Points.Add(point);
				var ann = new TextAnnotation();
				ann.Text = channel.Channel.Name.ToString();
				ann.AnchorDataPoint = point;

				chart.Annotations.Add(ann);
			}
		}
    }
}
