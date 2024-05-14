using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Приемник
{
    public class LabelWithValue
    {
        public LabelWithValue(Label namingLabel, Label contentLabel)
        {
            NamingLabel = namingLabel;
            ContentLabel = contentLabel;
        }

        public Label NamingLabel { get; }
        public Label ContentLabel { get; }

        public bool Visible
        {
            get
            {
                return ContentLabel.Visible;
            }
            set
            {
                ContentLabel.Visible = value;
                NamingLabel.Visible = value;
            }
        }
    }
}
