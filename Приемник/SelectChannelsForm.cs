using System.Drawing;
using System.Windows.Forms;

namespace Приемник
{
    public partial class SelectChannelsForm : Form
    {
        public class Channel
        {
            /// <summary>
            /// Collection of appropriate Channels names
            /// </summary>
            public static readonly string[] Names = new string[]
            {
                "1. FP1-A1",
                "2. F3-A1",
                "3. C3-A1",
                "4. P3-A1",
                "5. O1-A1",
                "6. F7-A1",
                "7. T3-A1",
                "8. T5-A1",
                "9. FZ-A1",
                "10. PZ-A1",
                "11. A1-A2",
                "12. FP2-A2",
                "13. F4-A2",
                "14. C4-A2",
                "15. P4-A2",
                "16. O2-A2",
                "17. F8-A2",
                "18. T4-A2",
                "19. T6-A2",
                "20. FPZ-A2",
                "21. CZ-A2",
                "22. OZ-A2",
                "23. E1",
                "24. E2",
                "25. E3",
                "26. E4",
                "27. Не используется",
                "28. Дыхание",
                "29. Служебный канал"
            };

            /// <summary>
            /// Enum defines where electrod is placed on head of user
            /// </summary>
            public enum SelectionStatus
            {
                Left,
                Right,
                NotSelected
            }
            /// <summary>
            /// name of current electrod
            /// </summary>
            public readonly string Name;

            public SelectionStatus Status;
            /// <summary>
            /// relative index
            /// </summary>
            public readonly int Index;
            public Channel(string name, int index, SelectionStatus status)
            {
                Name = name;
                Index = index;
                Status = status;
            }
        }
        public Channel[] Channels { get; private set; }
        public SelectChannelsForm(Form parentForm, Channel[] channels)
        {
            InitializeComponent();

            int posY = 0;
            for(int i = 0; i < channels.Length; i++)
            {
                var c = new ChannelSelection(channels[i]);
                c.Location = new Point(0, posY);
                posY += c.Height;
                panel1.Controls.Add(c);
            }
            Channels = channels;
        }
    }
}
