using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Приемник
{
    public class CommandsRepresenationController
    {
        public CommandsRepresenationController(LabelWithValue evaluatedCommandLabel, LabelWithValue requiredCommandLabel,
            ProgressBar commandTotalTimeAllowedProgressBar, LabelWithValue successCountLabel, LabelWithValue failureCountLabel,
            LastCommandAnimationController lastCommandAnimationController, Panel chartContainer, Panel commandsContainer, Chart controlFlowChart)
        {
            EvaluatedCommandLabel = evaluatedCommandLabel;
            RequiredCommandLabel = requiredCommandLabel;
            CommandTotalTimeAllowedProgressBar = commandTotalTimeAllowedProgressBar;
            SuccessCountLabel = successCountLabel;
            FailureCountLabel = failureCountLabel;
            LastCommandAnimationController = lastCommandAnimationController;
            ChartContainer = chartContainer;
            CommandsContainer = commandsContainer;
            ControlFlowChart = controlFlowChart;
        }

        private LabelWithValue EvaluatedCommandLabel { get; }
        private LabelWithValue RequiredCommandLabel { get; }
        private ProgressBar CommandTotalTimeAllowedProgressBar { get; }
        private LabelWithValue SuccessCountLabel { get; }
        private LabelWithValue FailureCountLabel { get; }
        private LastCommandAnimationController LastCommandAnimationController { get; }
        private Panel ChartContainer { get; }
        private Panel CommandsContainer { get; }
        private Chart ControlFlowChart { get; }

        public void Show(Mode mode)
        {
            /*OnlyEvaluatedCommand,
            OnlyRequiredCommand,
            BothCommandsWithoutCommandResults,
            Everything*/
            int chartContainerEverything = 260;
            int chartEverything = 230;
            //int commandsContainerEverything = 0;

            int chartContainerOnlyRequiredCommand = 480;
            int chartOnlyRequiredCommand = 440;
            //int commandsContainerRequiredCommand = -100;

            int chartContainerOnlyEvaluatedCommand = 480;
            int chartOnlyEvaluatedCommand = 440;
            //int commandsContainerEvaluatedCommand = 0;

            int chartContainerBothCommandsWithoutCommandResults = 360;
            int chartBothCommandsWithoutCommandResults = 325;
            //int commandsContainerBothCommandsWithoutCommandResults = 0;

            switch (mode)
            {
                case Mode.OnlyEvaluatedCommand:
                    //CommandsContainer.Top = commandsContainerEvaluatedCommand;
                    ChartContainer.Visible = true;
                    ControlFlowChart.Height = chartOnlyEvaluatedCommand;
                    ChartContainer.Height = chartContainerOnlyEvaluatedCommand;

                    EvaluatedCommandLabel.Visible = true;
                    RequiredCommandLabel.Visible = false;
                    CommandTotalTimeAllowedProgressBar.Visible = false;

                    SuccessCountLabel.Visible = false;
                    FailureCountLabel.Visible = false;
                    LastCommandAnimationController.Visible = false;

                    FailCount = 0;
                    SuccessCount = 0;
                    LastCommandAnimationController.ShowInProgress();
                    break;
                case Mode.OnlyRequiredCommand:
                    //CommandsContainer.Top = commandsContainerRequiredCommand;
                    ChartContainer.Visible = false;
                    ControlFlowChart.Height = chartOnlyRequiredCommand;
                    ChartContainer.Height = chartContainerOnlyRequiredCommand;

                    EvaluatedCommandLabel.Visible = false;
                    RequiredCommandLabel.Visible = true;
                    CommandTotalTimeAllowedProgressBar.Visible = true;

                    SuccessCountLabel.Visible = false;
                    FailureCountLabel.Visible = false;
                    LastCommandAnimationController.Visible = false;

                    FailCount = 0;
                    SuccessCount = 0;
                    LastCommandAnimationController.ShowInProgress();
                    break;
                case Mode.BothCommandsWithoutCommandResults:
                    //CommandsContainer.Top = commandsContainerBothCommandsWithoutCommandResults;
                    ChartContainer.Visible = true;
                    ControlFlowChart.Height = chartBothCommandsWithoutCommandResults;
                    ChartContainer.Height = chartContainerBothCommandsWithoutCommandResults;

                    EvaluatedCommandLabel.Visible = true;
                    RequiredCommandLabel.Visible = true;
                    CommandTotalTimeAllowedProgressBar.Visible = true;

                    SuccessCountLabel.Visible = false;
                    FailureCountLabel.Visible = false;
                    LastCommandAnimationController.Visible = false;

                    FailCount = 0;
                    SuccessCount = 0;
                    LastCommandAnimationController.ShowInProgress();
                    break;
                case Mode.Everything:
                    //CommandsContainer.Top = commandsContainerEverything;
                    ChartContainer.Visible = true;
                    ControlFlowChart.Height = chartEverything;
                    ChartContainer.Height = chartContainerEverything;

                    EvaluatedCommandLabel.Visible = true;
                    RequiredCommandLabel.Visible = true;
                    CommandTotalTimeAllowedProgressBar.Visible = true;

                    SuccessCountLabel.Visible = true;
                    FailureCountLabel.Visible = true;
                    LastCommandAnimationController.Visible = true;

                    FailCount = 0;
                    SuccessCount = 0;
                    LastCommandAnimationController.ShowInProgress();
                    break;
            }
        }
        public enum Mode
        {
            OnlyEvaluatedCommand,
            OnlyRequiredCommand,
            BothCommandsWithoutCommandResults,
            Everything
        }

        public static string MoveComandToString(MyuRitmAnalizer.MoveCommand command)
        {
            switch (command)
            {
                case MyuRitmAnalizer.MoveCommand.Left:
                    return "Лево (1)";
                case MyuRitmAnalizer.MoveCommand.Forward:
                    return "Вперед (3)";
                case MyuRitmAnalizer.MoveCommand.Right:
                    return "Вправо (2)";
                case MyuRitmAnalizer.MoveCommand.Stop:
                    return "Отдых (0)";
                default:
                    return "Ничего";
            }
        }
        MyuRitmAnalizer.MoveCommand lastEvaluatedCommand = MyuRitmAnalizer.MoveCommand.None;
        public MyuRitmAnalizer.MoveCommand EvaluatedCommand
        {
            get
            {
                return lastEvaluatedCommand;
            }
            set
            {
                lastEvaluatedCommand = value;
                EvaluatedCommandLabel.ContentLabel.Invoke(new Action(() => {
                    EvaluatedCommandLabel.ContentLabel.Text = MoveComandToString(value);
                }));
            }
        }

        MyuRitmAnalizer.MoveCommand lastRequiredCommand = MyuRitmAnalizer.MoveCommand.None;
        public MyuRitmAnalizer.MoveCommand RequiredCommand
        {
            get
            {
                return lastRequiredCommand;
            }
            set
            {
                lastRequiredCommand = value;
                RequiredCommandLabel.ContentLabel.Invoke(new Action(() => {
                    RequiredCommandLabel.ContentLabel.Text = MoveComandToString(value);
                }));
            }
        }

        double progress = 0;
        public double Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value < 0 ? 0 : Math.Min(1, value);
                CommandTotalTimeAllowedProgressBar.Value = (int)Math.Min(progress * 101, 100);
            }
        }

        int successCount = 0;
        public int SuccessCount
        {
            get
            {
                return successCount;
            }
            set
            {
                if (value != successCount)
                {
                    LastCommandAnimationController?.ShowSuccess();
                }
                successCount = value;
                SuccessCountLabel.ContentLabel.Invoke(new Action(() =>
                {
                    SuccessCountLabel.ContentLabel.Text = successCount.ToString();
                }));
            }
        }

        int failCount = 0;
        public int FailCount
        {
            get
            {
                return failCount;
            }
            set
            {
                if(value != failCount)
                {
                    LastCommandAnimationController?.ShowFailure();
                }
                failCount = value;
                FailureCountLabel.ContentLabel.Invoke(new Action(() =>
                {
                    FailureCountLabel.ContentLabel.Text = failCount.ToString();
                }));
            }
        }
    }
}
