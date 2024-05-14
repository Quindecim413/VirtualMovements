using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Приемник
{
    public partial class ChannelSelection : UserControl
    {


        public SelectChannelsForm.Channel Channel;

        public ChannelSelection(SelectChannelsForm.Channel channel)
        {
            InitializeComponent();
            Channel = channel;
            groupBox.Text = Channel.Name;
            switch (Channel.Status)
            {
                case SelectChannelsForm.Channel.SelectionStatus.Left:
                    radioButton1.Checked = true;
                    break;
                case SelectChannelsForm.Channel.SelectionStatus.NotSelected:
                    radioButton2.Checked = true;
                    break;
                case SelectChannelsForm.Channel.SelectionStatus.Right:
                    radioButton3.Checked = true;
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Channel.Status = SelectChannelsForm.Channel.SelectionStatus.Left;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                Channel.Status = SelectChannelsForm.Channel.SelectionStatus.NotSelected;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                Channel.Status = SelectChannelsForm.Channel.SelectionStatus.Right;
        }
    }
}
