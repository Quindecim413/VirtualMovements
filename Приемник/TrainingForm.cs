using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Приемник
{
    public partial class TrainingForm : Form
    {
        MainForm mainForm;
        string recordSubDir;
        string baseRecordDir = "";

        public TrainingForm(MainForm form)
        {
            this.mainForm = form;
            InitializeComponent();
            this.Focus();


            if (!form.Connected)
            {
                buttonStartTrain.Enabled = false;
            }

            form.OnDisconnected += () =>
            {
                if (state == TrainState.Stopped)
                {
                    buttonStartTrain.Enabled = false;
                }
            };
            form.OnConnected += () => {
                buttonStartTrain.Enabled = true;
            };
            form.OnCommandFormed += RecieveCommand;
            form.CommandsRepresenationController.Show(CommandsRepresenationController.Mode.Everything);
        }

        /// <summary>
        /// Logs message to textbox on form
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            Invoke((Action)(() => {
                textBoxLog.Text = message + "\r\n" + textBoxLog.Text;
            }));
        }

        #region Events
        public void RecieveData(double[][] data)
        {
            if (state != TrainState.Started)
                return;
            for (int i = 0; i < data.Length; i++)
                recievedVals.Add(data[i]);
        }
        public void RecieveCommand(MyuRitmAnalizer.MoveCommand command)
        {
            recievedCommand = command;
        }

        private void buttonStartTrain_Click(object sender, EventArgs e)
        {
            if (state == TrainState.Started)
            {
                timer1.Enabled = false;
                state = TrainState.Stopped;
                recievedVals.Clear();
                program.Clear();
                commandNumber = -1;
                buttonStartTrain.Text = "Приступить";
                mainForm.ParamsChangesLocked = false;
            }
            else
            {
                buttonStartTrain.Text = "Прервать";
                state = TrainState.Started;
                recievedVals.Clear();
                program.Clear();
                commandVerify = (int)numericUpDownVerifyCommand.Value;
                commandTimeout = (int)numericUpDownTimeForCommand.Value;
                trainStarted = DateTime.Now;
                commandStarted = trainStarted;
                commandVerificationStarted = trainStarted;
                commandNumber = -1;
                try
                {
                    var prog = GetProgramm();
                    foreach (var p in prog)
                    {
                        switch (p)
                        {
                            case 1:
                                program.Enqueue(MyuRitmAnalizer.MoveCommand.Left);
                                break;
                            case 2:
                                program.Enqueue(MyuRitmAnalizer.MoveCommand.Right);
                                break;
                            case 3:
                                program.Enqueue(MyuRitmAnalizer.MoveCommand.Forward);
                                break;
                            case 0:
                                program.Enqueue(MyuRitmAnalizer.MoveCommand.Stop);
                                break;
                            default:
                                throw new Exception("invalid command " + p);
                        }
                    }
                    totalCommands = prog.Count;
                    SelectNextCommand();
                    commands.Clear();

                    mainForm.CommandsRepresenationController.Show(CommandsRepresenationController.Mode.Everything);

                    DateTime dt = DateTime.Now;
                    recordSubDir = dt.ToString("dd.MM.yy_HH.mm.ss");
                    baseRecordDir = GetBaseWriteDirPath();
                    timer1.Enabled = true;
                    mainForm.ParamsChangesLocked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при старте тренировки: " + ex.Message + "\r\n" + ex.StackTrace);
                    buttonStartTrain.Text = "Приступить";
                }
            }
        }

        #endregion

        #region Program initialization
        private void buttonCommandsRandomize_Click(object sender, EventArgs e)
        {
            List<int> vals = new List<int>();
            int confLights = 4;
            for (int i = 0; i < (int)numericUpDownTotalCommands.Value / confLights; i++)
            {
                for (int j = 0; j < confLights; j++)
                    vals.Add(j);
            }
            Random rand = new Random();
            List<int> opt = new List<int>();
            for (int j = 0; j < confLights; j++)
            {
                opt.Add(j);
            }

            for (int i = 0; i < numericUpDownTotalCommands.Value % confLights; i++)
            {
                int ind = rand.Next(0, opt.Count);
                vals.Add(opt[ind]);
                opt.RemoveAt(ind);
            }

            Shuffle(vals);

            string res = "";
            foreach (int val in vals)
            {
                res += val;
            }

            textBoxProgram.Text = res;
        }
        Random rand = new Random();
        public void Shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        List<double[]> recievedVals = new List<double[]>();

        public static string RemoveWhitespace(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
        /// <summary>
        /// Parses text representation of user program
        /// </summary>
        /// <returns></returns>
        List<int> GetProgramm()
        {
            List<int> res = new List<int>();
            string prog = RemoveWhitespace(textBoxProgram.Text);

            for (int i = 0; i < prog.Length; i++)
            {
                res.Add(int.Parse(prog[i].ToString()));
            }

            return res;
        }
        #endregion

        #region commands controll
        enum TrainState
        {
            Started, Stopped
        }
        TrainState state = TrainState.Stopped;
        Queue<MyuRitmAnalizer.MoveCommand> program = new Queue<MyuRitmAnalizer.MoveCommand>();
        

        public string CommandToStr(MyuRitmAnalizer.MoveCommand command)
        {
            return MainForm.MoveComandToString(command);
        }

        DateTime trainStarted;
        DateTime commandStarted;
        DateTime commandVerificationStarted;
        int commandTimeout;
        int commandVerify;
        MyuRitmAnalizer.MoveCommand command;
        MyuRitmAnalizer.MoveCommand recievedCommand;
        int commandNumber = -1;
        int totalCommands = 0;
        void SelectNextCommand()
        {
            commandNumber++;
            if (program.Count == 0)
                return;
            command = program.Dequeue();
            Log("Selecting new Command: " + command.ToString());
        }

        bool Done()
        {
            return commandNumber >= totalCommands;
        }

        /// <summary>
        /// Queue of commands that user should perform in current training
        /// </summary>
        Queue<Tuple<MyuRitmAnalizer.MoveCommand, bool>> commands = new Queue<Tuple<MyuRitmAnalizer.MoveCommand, bool>>();

        /// <summary>
        /// Main timer event where command performed by user verified
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (state != TrainState.Started)
                return;
            DateTime now = DateTime.Now;
            labelTimePassedTotal.Text = ((int)(10 * (now - trainStarted).TotalSeconds) / 10).ToString();
            labelCommandTime.Text = ((int)(10 * (now - commandStarted).TotalSeconds) / 10).ToString();
            labelVerificationTime.Text = ((int)(10 * (now - commandVerificationStarted).TotalSeconds) / 10).ToString();

            if ((now - commandStarted).TotalSeconds > commandTimeout)
            {
                Log(command.ToString() + " timed out");
                commandStarted = now;
                commandVerificationStarted = now;

                var data = recievedVals.ToArray();
                Task.Run(() => { WriteData(false, command, data); });
                recievedVals.Clear();

                commands.Enqueue(new Tuple<MyuRitmAnalizer.MoveCommand, bool>(command, false));
                SelectNextCommand();
                mainForm.CommandsRepresenationController.FailCount++;
            }
            else
            {
                mainForm.CommandsRepresenationController.Progress = (now - commandStarted).TotalSeconds / commandTimeout;
                if (recievedCommand != command)
                {
                    Log("Recieved and generated commands conflict. \r\n Recieved: " + recievedCommand.ToString() + " Generated: " + command.ToString());
                    commandVerificationStarted = now;
                }
                else if ((now - commandVerificationStarted).TotalSeconds >= commandVerify)
                {
                    Log("Recieved command verified. Command" + recievedCommand.ToString());

                    var data = recievedVals.ToArray();
                    Task.Run(() => { WriteData(true, command, data); });
                    recievedVals.Clear();

                    commandStarted = now;
                    commands.Enqueue(new Tuple<MyuRitmAnalizer.MoveCommand, bool>(command, true));
                    SelectNextCommand();
                    commandVerificationStarted = now;
                    mainForm.CommandsRepresenationController.SuccessCount++;
                }
                else
                {
                    Log("Recieved command not verified yet. Command " + recievedCommand.ToString() + ". \r\nVerified time: " + (now - commandStarted).TotalSeconds + ". Required: " + commandVerify);
                }
            }

            mainForm.CommandsRepresenationController.RequiredCommand = command;

            labelCommandNumber.Text = (commandNumber + 1).ToString();
            labelAction.Text = CommandToStr(command);
            if (Done())
            {
                Log("Done research");
                state = TrainState.Stopped;
                mainForm.ParamsChangesLocked = false;
                timer1.Enabled = false;
                string results = "";
                var commandsDoneList = commands.ToArray();
                int successCount = 0;
                for(int i = 0; i < commandsDoneList.Length; i++)
                {
                    successCount += (commandsDoneList[i].Item2 ? 1 : 0);
                    results += "#" + (i + 1) + ". " + commandsDoneList[i].Item1 + " : " +
                        (commandsDoneList[i].Item2 ? "OK" : "Error") + "\r\n";
                }

                SaveCommands();
                buttonStartTrain.Text = "Приступить";
                MessageBox.Show($"Обследование завершено.\r\nУспешно {successCount} из {commandsDoneList.Length}.\r\nРезультаты:\r\n{results}");
            }
        }
        #endregion

        #region saving results
        /// <summary>
        /// Evaluates directory where to save training results
        /// </summary>
        /// <returns></returns>
        public string GetBaseWriteDirPath()
        {
            string identifier = mainForm.ClientIdentifier;

            string baseDir = "./researches/" + identifier + "/" + recordSubDir;

            DirectoryInfo dirInfo = new DirectoryInfo(baseDir);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            return baseDir;
        }

        /// <summary>
        /// Writes overall log of person trining results
        /// </summary>
        public void SaveCommands()
        {
            string eduFileName = baseRecordDir + "/Log.txt";
            
            using (StreamWriter sw = new StreamWriter(eduFileName, false, System.Text.Encoding.Default))
            {

                for (; commands.Count != 0; )
                {
                    Tuple<MyuRitmAnalizer.MoveCommand, bool> commands_pair = commands.Dequeue();
                    
                    string line = "Required: "+ commands_pair.Item1 +
                        ". Recieved: "+ commands_pair.Item2 +
                        ". Result: " + (commands_pair.Item2 ? "OK": "Error");
                    
                    sw.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Writes intermediate results of every command made by user
        /// </summary>
        void WriteData(bool success, MyuRitmAnalizer.MoveCommand command, double[][]data)
        {
            DateTime now = DateTime.Now; ;
            string time = now.ToString("dd.MM.yy_HH.mm.ss");

            string eduFileName = baseRecordDir + "/" + command + "_command_" + time;

            if (!success)
                eduFileName += "__TIMEOUT";

            eduFileName += ".dat";
            using (StreamWriter sw = new StreamWriter(eduFileName, false, System.Text.Encoding.Default))
            {

                for (int i = 0; i < data.Length; i++)
                {
                    string line = "";
                    foreach (var val in data[i])
                    {
                        line += val.ToString() + "\t";
                    }
                    sw.WriteLine(line);
                }
            }
        }

        private void TrainingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.OnCommandFormed -= RecieveCommand;
        }
        #endregion
    }
}
