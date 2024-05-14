using System;
using System.Drawing;
using System.Windows.Forms;

namespace Приемник
{
    public partial class ChannelWithRatingDataControl : UserControl
    {
        public ChannelWithRatingDataControl(ChannelsRatingsViewer.ChannelWithRatingData channelWithRatingData)
        {
            InitializeComponent();
            ChannelWithRatingData = channelWithRatingData;

            ChannelUsed = ChannelWithRatingData.Selected;

            channelName.Text = ChannelWithRatingData.Channel.Name;
            windowWidth.Text = ChannelWithRatingData.WindowWidth.ToString();
            distribution.Text = ChannelWithRatingData.AVGDistribution.ToString();
            label1.Click += (s, args) =>
            {
                Clicked(this);
            };
            label2.Click += (s, args) =>
              {
                  Clicked(this);
              };
            label3.Click += (s, args) =>
              {
                  Clicked(this);
              };
            Click += (s, args) =>
              {
                  Clicked(this);
              };
            channelName.Click += (s, args) =>
              {
                  Clicked(this);
              };
            windowWidth.Click += (s, args) =>
              {
                  Clicked(this);
              };
            distribution.Click += (s, args) =>
            {
                Clicked(this);
            };
            checkBox1.Click += (s, args) =>
              {
                  Clicked(this);
              };
        }

        public event Action<ChannelWithRatingDataControl> Clicked;

        private ChannelsRatingsViewer.ChannelWithRatingData ChannelWithRatingData;

        public bool ChannelUsed
        {
            get
            {
                return ChannelWithRatingData.Selected;
            }
            private set
            {
                ChannelWithRatingData.Selected = value;
                
                label1.Enabled = value;
                label2.Enabled = value;
                label3.Enabled = value;

                channelName.Enabled = value;
                windowWidth.Enabled = value;
                distribution.Enabled = value;
            }
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            ChannelUsed = checkBox1.Checked;
        }

        private void ChannelWithRatingDataControl_MouseHover(object sender, EventArgs e)
        {
            BackColor = SystemColors.ActiveCaption;
        }

        private void ChannelWithRatingDataControl_MouseLeave(object sender, EventArgs e)
        {
            BackColor = SystemColors.Control;
        }
    }
}
