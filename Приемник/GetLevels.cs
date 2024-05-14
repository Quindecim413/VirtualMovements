using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Приемник
{
    public partial class GetLevels : Form
    {
        MainForm form;
        private SelectChannelsForm.Channel[] Channels;

        DateTime startedLeveling = DateTime.Now;
        TimeSpan timeout;

        List<double>[] channelsPowerData;
        List<double>[] channelsRestData;
        List<double>[] channelsPowerLeft;
        List<double>[] channelsPowerRight;

        List<double[]> recievedVals = new List<double[]>();

        public GetLevels(MainForm form)
        {
            this.form = form;
            InitializeComponent();

            channelsPowerData = new List<double>[29];
            channelsRestData = new List<double>[29];
            channelsPowerLeft = new List<double>[29];
            channelsPowerRight = new List<double>[29];
            for (int i = 0; i < 29; i++)
            {
                channelsPowerData[i] = new List<double>();
                channelsRestData[i] = new List<double>();
                channelsPowerLeft[i] = new List<double>();
                channelsPowerRight[i] = new List<double>();
            }
            
            form.OnDisconnected += () =>
            {
                if (state == State.DoesNothing)
                {
                    buttonStartLevels.Enabled = false;
                }
            };
            form.OnConnected += () => {
                buttonStartLevels.Enabled = true;
            };
            form.CommandsRepresenationController.Show(CommandsRepresenationController.Mode.OnlyRequiredCommand);
        }

        /// <summary>
        /// Clears up buffers of all recived values
        /// </summary>
        public void ClearData()
        {
            for(int i = 0; i < channelsPowerData.Length; i++)
            {
                channelsPowerData[i].Clear();
                channelsRestData[i].Clear();
                channelsPowerLeft[i].Clear();
                channelsPowerRight[i].Clear();
            }

            recievedVals.Clear();
        }

        #region Events
        /// <summary>
        /// When new portion of EEG is recieved it is saved in one of buffers: 
        /// channelsPowerData, channelsRestData, channelsPowerLeftData, channelsPowerRightData
        /// </summary>
        /// <param name="channels"></param>
        private void Form_OnDataFormed(AnalizeFramePerChannel[] channels)
        {
            if (state != State.PerfomsCounting)
                return;

            List<double>[] selectedList = null;
            switch (command)
            {
                case EduCommand.Power:
                    selectedList = channelsPowerData;
                    break;
                case EduCommand.Rest:
                    selectedList = channelsRestData;
                    break;
                case EduCommand.PowerLeft:
                    selectedList = channelsPowerLeft;
                    break;
                case EduCommand.PowerRight:
                    selectedList = channelsPowerRight;
                    break;
            }

            if (selectedList != null)
            {
                for (int i = 0; i < 29; i++)
                {
                    if (Channels[i].Status != SelectChannelsForm.Channel.SelectionStatus.NotSelected)
                    {
                        selectedList[i].Add(channels[i].Power);
                    }
                    else
                    {
                        selectedList[i].Add(0);
                    }
                }
            }
        }

        /// <summary>
        /// Shows up form where user can select channels to analise
        /// </summary>
        private void buttonSelectChannels_Click(object sender, EventArgs e)
        {
            SelectChannelsForm f = new SelectChannelsForm(this, Channels);
            this.Enabled = false;
            f.FormClosed += (s, ev) =>
            {
                this.Enabled = true;

                var left = String.Join(",", Channels.Where((el) => el.Status == SelectChannelsForm.Channel.SelectionStatus.Left)
                .Select((el) => el.Name).Cast<string>());

                var right = String.Join(",", Channels.Where((el) => el.Status == SelectChannelsForm.Channel.SelectionStatus.Right)
                .Select((el) => el.Name).Cast<string>());
                channelsTextBox.Text = "Left: " + left + "\r\nRight: " + right;
            };

            f.Show();
        }

        private void GetLevels_Shown(object sender, EventArgs e)
        {
            SetCommand(EduCommand.NoCommand);
            SetState(State.DoesNothing);

            form.OnDataFormed += Form_OnDataFormed;

            Channels = new SelectChannelsForm.Channel[SelectChannelsForm.Channel.Names.Length];
            for (int i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new SelectChannelsForm.Channel(SelectChannelsForm.Channel.Names[i], i, SelectChannelsForm.Channel.SelectionStatus.NotSelected);
            }

            if (!form.Connected)
            {
                buttonStartLevels.Enabled = false;
            }
        }

        #endregion

        #region states controls
        /// <summary>
        /// States of the form
        /// </summary>
        enum State
        {
            DoesNothing, PerfomsCounting, Saving, DoneCounting
        }

        /// <summary>
        /// States of education process
        /// </summary>
        enum EduCommand
        {
            Rest, Power, NoCommand, EduDone, PowerLeft, PowerRight
        }

        EduCommand command;
        DateTime lastCommandChange = DateTime.Now;
        /// <summary>
        /// Shows userfriendly representation of education command that should be performed right now
        /// </summary>
        /// <param name="command"></param>
        void SetCommand(EduCommand command)
        {
            if (this.command != command)
            {
                lastCommandChange = DateTime.Now;
                
            }
            this.command = command;
            switch (command)
            {
                case EduCommand.Rest:
                    textBoxCommand.Text = "Отдыхайте";
                    break;
                case EduCommand.Power:
                    textBoxCommand.Text = "Напрягитесь";
                    break;
                case EduCommand.PowerLeft:
                    textBoxCommand.Text = "Напрягите Лево";
                    break;
                case EduCommand.PowerRight:
                    textBoxCommand.Text = "Напрягите Право";
                    break;
                case EduCommand.NoCommand:
                    textBoxCommand.Text = "Приступайте к рассчёту";
                    break;
                case EduCommand.EduDone:
                    textBoxCommand.Text = "";
                    break;
            }
        }

        State state;

        void SetState(State state)
        {
            
            switch (state)
            {
                case State.DoesNothing:
                    ///This state represents situation when form waits user to set setting and start analisis
                    ///
                    Invoke(new Action(() =>
                    {
                        buttonStartLevels.Text = "Приступить";
                        numericUpDownCommandSec.Enabled = true;
                        timer1.Enabled = false;
                        savingTimer.Enabled = false;
                        buttonSelectChannels.Enabled = true;
                        startedLeveling = DateTime.Now;
                        form.ParamsChangesLocked = false;
                        ClearData();
                        this.state = state;
                        SetCommand(EduCommand.NoCommand);
                    }));
                    break;
                case State.PerfomsCounting:
                    ///This state represents situation when analisis already started and not done yet.
                    ///
                    var l = Channels.Where(el => el.Status == SelectChannelsForm.Channel.SelectionStatus.Left).Count();
                    var r = Channels.Where(el => el.Status == SelectChannelsForm.Channel.SelectionStatus.Right).Count();
                    if(l == 0 && r != 0)
                    {
                        MessageBox.Show("Выберите хотя бы один левый канал");
                        return;
                    }
                    else if(l != 0 && r == 0)
                    {
                        MessageBox.Show("Выберите хотя бы один правый канал");
                        return;
                    }
                    else if (l == 0 && r == 0)
                    {
                        MessageBox.Show("Выберите хотя бы один левый и правый канал");
                        return;
                    }
                    this.state = state;
                    ClearData();
                    form.ParamsChangesLocked = true;
                    buttonStartLevels.Text = "Прервать";
                    numericUpDownCommandSec.Enabled = false;

                    DateTime startedTime = DateTime.Now;
                    form.AddStoreEEGDataWork(GetSaveCommandFileName(EduCommand.Rest, startedTime), 
                        ()=> DateTime.Now - startedLeveling > timeout, null);
                    form.CommandsRepresenationController.Show(CommandsRepresenationController.Mode.OnlyRequiredCommand);
                    
                    SetCommand(EduCommand.Rest);
                    startedLeveling = DateTime.Now;
                    timer1.Enabled = true;
                    buttonSelectChannels.Enabled = false;
                    break;
                case State.Saving:
                    ///This state represents situation when analisis already done but not all intermidiate analisis data saved yet.
                    ///
                    this.Invoke(new Action(() =>
                    {
                        savingTimer.Enabled = true;
                        buttonStartLevels.Text = "Сохранение.";
                        buttonSelectChannels.Enabled = false;
                        timer1.Enabled = false;
                        savingTimer.Enabled = false;
                    }));
                    
                    SibmitResults();
                    SetCommand(EduCommand.EduDone);

                    break;
                case State.DoneCounting:
                    ///This state represents situation when analisis already done but all intermidiate analisis data saved yet.
                    ///Its only left to close the form.
                    this.Invoke(new Action(() =>
                    {
                        buttonStartLevels.Enabled = false;
                        form.ParamsChangesLocked = false;
                        buttonSelectChannels.Enabled = true;
                        buttonStartLevels.Text = "Расчёт окончен. Закрыть";
                        buttonStartLevels.Enabled = true;
                        numericUpDownCommandSec.Enabled = true;
                        timer1.Enabled = false;
                        savingTimer.Enabled = false;
                        this.Text = "Получение уровней";

                        ClearData();
                    }));
                    
                    this.state = state;
                    break;
            }
        }

        DateTime startLeveling = DateTime.Now;
        string clientIdentifier = "";

        private void buttonStartLevels_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case State.DoesNothing:
                    timeout = new TimeSpan(0, 0, (int)numericUpDownCommandSec.Value);
                    startLeveling = DateTime.Now;
                    clientIdentifier = form.ClientIdentifier;
                    SetState(State.PerfomsCounting);
                    break;
                case State.PerfomsCounting:
                    SetState(State.DoesNothing);
                    break;
                case State.DoneCounting:
                    Close();
                    break;
            }
        }
        #endregion
        
        #region loggin

        private string GetSaveCommandFileName(EduCommand command, DateTime commandCreatedTime)
        {
            string time = startLeveling.ToString("dd.MM.yy_HH.mm.ss");

            string timeCommand = commandCreatedTime.ToString("dd.MM.yy_HH.mm.ss");

            string saveDirName = "./levels_records/" + clientIdentifier + "/" + "levels" + "_" + time + "/";

            string saveFileName = saveDirName + "/" + command.ToString() + "_" + timeCommand + ".txt";
            return saveFileName;
        }

        ///// <summary>
        ///// Saves signal of command performed
        ///// </summary>
        ///// <param name="command">command performed by user</param>
        ///// <param name="commandCreatedTime">time when user started to perform command</param>
        ///// <param name="recievedVals">user's EEG recieved when he was performing command</param>
        //void SaveDataRecieved(EduCommand command, DateTime commandCreatedTime, List<double[]> recievedVals)
        //{
        //    if (command == EduCommand.Rest || command == EduCommand.Power || command == EduCommand.PowerLeft || command == EduCommand.PowerRight)
        //    {
        //        string identifier = clientIdentifier;

        //        DirectoryInfo dirInfo = new DirectoryInfo("./levels_records");
        //        if (!dirInfo.Exists)
        //        {
        //            dirInfo.Create();
        //        }

        //        dirInfo = new DirectoryInfo("./levels_records/" + identifier);
        //        if (!dirInfo.Exists)
        //        {
        //            dirInfo.Create();
        //        }

        //        string time = startLeveling.ToString("dd.MM.yy_HH.mm.ss");

        //        string timeCommand = commandCreatedTime.ToString("dd.MM.yy_HH.mm.ss");

        //        string saveDirName = "./levels_records/" + identifier + "/" + "levels" + "_" + time + "/";
        //        Directory.CreateDirectory(saveDirName);
        //        string saveFileName = saveDirName + "/" + command.ToString() + "_" + timeCommand + ".txt";
        //        using (StreamWriter sw = new StreamWriter(saveFileName, true))
        //        {
        //            foreach(var line in recievedVals)
        //            {
        //                sw.WriteLine(String.Join("\t", line.Select(el => el.ToString())));
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Saves intermidiate results of signal quality analisis when analisis fails
        /// </summary>
        /// <param name="powers_l">Spectral energies of all selected channels when user was straining his hands</param>
        /// <param name="rests_l">Spectral energies of all selected channels when user was relaxing his hands</param>
        /// <param name="powers_left_l">Spectral energies of all selected channels when user was straining only his left hand</param>
        /// <param name="powers_right_l">Spectral energies of all selected channels when user was straining only his right hand</param>
        /// <param name="powers_medians">Expected value of spectral energies of all selected channels when user was straining his hands</param>
        /// <param name="powers_sigmas">Quadratic distribution value of spectral energies of all selected channels when user was straining his hands</param>
        /// <param name="rests_medians">Expected value of spectral energies of all selected channels when user was relaxing his hands</param>
        /// <param name="rests_sigmas">Quadratic disribution of spectral energies of all selected channels when user was relaxing his hands</param>
        void SaveFailureResults(double[][] powers_l, double[][] rests_l, double[][] powers_left_l, double[][] powers_right_l,
            double[] powers_medians, double[] powers_sigmas, double[] rests_medians, double[] rests_sigmas)
        {
            string identifier = clientIdentifier;

            DirectoryInfo dirInfo = new DirectoryInfo("./levels_records");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            dirInfo = new DirectoryInfo("./levels_records/" + identifier);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string time = startLeveling.ToString("dd.MM.yy_HH.mm.ss");

            string saveDirName = "./levels_records/" + identifier + "/" + "levels" + "_" + time + "/";
            Directory.CreateDirectory(saveDirName);
            using (StreamWriter sw = new StreamWriter(saveDirName + "powers_list.txt", false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < powers_l[0].Length; i++)
                {
                    sw.WriteLine(String.Join("\t", powers_l.Select((arr) => arr[i].ToString())));
                }
            }
            using (StreamWriter sw = new StreamWriter(saveDirName + "rests_list.txt", false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < rests_l[0].Length; i++)
                {
                    sw.WriteLine(String.Join("\t", rests_l.Select((arr) => arr[i].ToString())));
                }
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "powers_medians.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", powers_medians.Select((el) => el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "rests_medians.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", rests_medians.Select((el) => el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "powers_sigmas.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", powers_sigmas.Select((el) => el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "rests_sigmas.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", rests_sigmas.Select(el => el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "channels_statuses.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", Channels.Select((el) => el.Status.ToString())));
            }
        }

        /// <summary>
        /// Saves intermidiate results of signal quality analisis
        /// </summary>
        /// <param name="powers_l">Spectral energies of all selected channels when user was straining his hands</param>
        /// <param name="rests_l">Spectral energies of all selected channels when user was relaxing his hands</param>
        /// <param name="powers_left_l">Spectral energies of all selected channels when user was straining only his left hand</param>
        /// <param name="powers_right_l">Spectral energies of all selected channels when user was straining only his right hand</param>
        /// <param name="powers_medians">Expected value of spectral energies of all selected channels when user was straining his hands</param>
        /// <param name="powers_sigmas">Quadratic distribution value of spectral energies of all selected channels when user was straining his hands</param>
        /// <param name="rests_medians">Expected value of spectral energies of all selected channels when user was relaxing his hands</param>
        /// <param name="rests_sigmas">Quadratic disribution of spectral energies of all selected channels when user was relaxing his hands</param>
        /// <param name="matches">matrix of success paiwise identifications</param>
        /// <param name="best_left_ind">index of left channel selected as most appropriate</param>
        /// <param name="best_right_ind">index of right channel selected as most appropriate</param>
        /// <param name="cutOffL">cutoff level of left channel</param>
        /// <param name="cutOffR">cutoff level of right channel</param>
        void SaveResults(double[][] powers_l, double[][] rests_l, double[][] powers_left_l, double[][] powers_right_l,
            double[]powers_medians, double[] powers_sigmas, double[] rests_medians, double[] rests_sigmas,
            int[,]matches, int best_left_ind, int best_right_ind, double cutOffL, double cutOffR)
        {
            string identifier = clientIdentifier;

            DirectoryInfo dirInfo = new DirectoryInfo("./levels_records");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            dirInfo = new DirectoryInfo("./levels_records/" + identifier);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string time = startLeveling.ToString("dd.MM.yy_HH.mm.ss");

            string saveDirName = "./levels_records/" + identifier + "/" + "levels" + "_" + time +"/";
            Directory.CreateDirectory(saveDirName);
            using (StreamWriter sw = new StreamWriter(saveDirName+"powers_list.txt", false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < powers_l[0].Length; i++) {
                    sw.WriteLine(String.Join("\t", powers_l.Select((arr) => arr[i].ToString())));
                }
            }
            using (StreamWriter sw = new StreamWriter(saveDirName + "rests_list.txt", false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < rests_l[0].Length; i++)
                {
                    sw.WriteLine(String.Join("\t", rests_l.Select((arr) => arr[i].ToString())));
                }
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "powers_medians.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", powers_medians.Select((el)=>el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "rests_medians.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", rests_medians.Select((el)=>el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "powers_sigmas.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", powers_sigmas.Select((el)=>el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "rests_sigmas.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", rests_sigmas.Select(el=>el.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "channels_statuses.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(String.Join("\t", Channels.Select((el)=>el.Status.ToString())));
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "matches_left_right_total.txt", false, System.Text.Encoding.Default))
            {
                for(int i = 0; i < matches.GetLength(0); i++)
                {
                    var l = new int[matches.GetLength(1)];
                    for(int j = 0; j < matches.GetLength(1); j++)
                    {
                        l[j] = matches[i, j];
                    }
                    sw.WriteLine(String.Join("\t", l.Select((el) => el.ToString())));
                }
            }

            using (StreamWriter sw = new StreamWriter(saveDirName + "results.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine("best_left_index");
                sw.WriteLine(best_left_ind);
                sw.WriteLine(rests_medians[best_left_ind] - powers_medians[best_left_ind] < 0? "Inverted": "NonInverted");
                sw.WriteLine("best_right_index");
                sw.WriteLine(best_right_ind);
                sw.WriteLine(rests_medians[best_right_ind] - powers_medians[best_right_ind] < 0 ? "Inverted" : "NonInverted");
                sw.WriteLine("CutOffLeft");
                sw.WriteLine(cutOffL);
                sw.WriteLine("CutOffRight");
                sw.WriteLine(cutOffR);
            }
        }

        #endregion

        #region submitting results
        /// <summary>
        /// evaluates properties of signal
        /// </summary>
        /// <param name="data">analizing signal</param>
        /// <param name="median">Median of passed signal</param>
        /// <param name="sigma">Quadratic deviation of passed signal</param>
        private static void CountCharacteristics(double[] data, out double median, out double sigma)
        {
            median = data.Aggregate(0d, (sum, el) => sum + el) / data.Length;
            double m = median;
            sigma = Math.Sqrt(data.Aggregate(0d, (sum, el) => sum + (el - m) * (el - m)) / data.Length);
        }

        /// <summary>
        /// Performs data channels quality computations, shows intermidiate results to user to consume best options 
        /// and finally selects best channels and saves intermediate results
        /// </summary>
        void SibmitResults()
        {
            Thread t = new Thread(() =>
            {
                double[][] powers_l = new double[29][];
                double[] powers_medians = new double[29];
                double[] powers_sigmas = new double[29];

                double[][] rests_l = new double[29][];
                double[] rests_medians = new double[29];
                double[] rests_sigmas = new double[29];

                double[][] powers_left_l = new double[29][];
                double[] powers_left_medians = new double[29];
                double[] powers_left_sigmas = new double[29];

                double[][] powers_right_l = new double[29][];
                double[] powers_right_medians = new double[29];
                double[] powers_right_sigmas = new double[29];

                for (int i = 0; i < 29; i++)
                {
                    powers_l[i] = new double[channelsPowerData[i].Count];
                    for (int j = 0; j < powers_l[i].Length; j++)
                    {
                        powers_l[i][j] = channelsPowerData[i][j];
                    }
                    CountCharacteristics(powers_l[i], out powers_medians[i], out powers_sigmas[i]);

                    rests_l[i] = new double[channelsRestData[i].Count];
                    for (int j = 0; j < rests_l[i].Length; j++)
                    {
                        rests_l[i][j] = channelsRestData[i][j];
                    }
                    CountCharacteristics(rests_l[i], out rests_medians[i], out rests_sigmas[i]);


                    powers_left_l[i] = new double[channelsPowerLeft[i].Count];
                    for (int j = 0; j < powers_left_l[i].Length; j++)
                    {
                        powers_left_l[i][j] = channelsPowerLeft[i][j];
                    }
                    CountCharacteristics(powers_left_l[i], out powers_left_medians[i], out powers_left_sigmas[i]);

                    powers_right_l[i] = new double[channelsPowerRight[i].Count];
                    for (int j = 0; j < powers_right_l[i].Length; j++)
                    {
                        powers_right_l[i][j] = channelsPowerRight[i][j];
                    }
                    CountCharacteristics(powers_right_l[i], out powers_right_medians[i], out powers_right_sigmas[i]);
                }

                Invoke(new Action(() =>
                {
                    var leftChannelsData = new List<ChannelsRatingsViewer.ChannelWithRatingData>();
                    var rightChannelsData = new List<ChannelsRatingsViewer.ChannelWithRatingData>();

                    for (int i = 0; i < 29; i++)
                    {
                        if (Channels[i].Status == SelectChannelsForm.Channel.SelectionStatus.NotSelected)
                            continue;

                        List<ChannelsRatingsViewer.ChannelWithRatingData> list =
                            Channels[i].Status == SelectChannelsForm.Channel.SelectionStatus.Left ? leftChannelsData : rightChannelsData;

                        list.Add(new ChannelsRatingsViewer.ChannelWithRatingData()
                        {
                            AVGDistribution = rests_sigmas[i] + powers_sigmas[i],
                            Ind = i,
                            WindowWidth = rests_medians[i] - powers_medians[i],
                            Channel = Channels[i],
                            Selected = true
                        });
                    }

                    ChannelsRatingsViewer channelsSelect = new ChannelsRatingsViewer(leftChannelsData, rightChannelsData);
                    channelsSelect.ShowDialog();


                    double[] channelsRatings = new double[29];

                    for (int i = 0; i < 29; i++)
                    {
                        channelsRatings[i] = rests_medians[i] - rests_sigmas[i] - powers_medians[i] - powers_sigmas[i];
                    }

                    int[] indsLeft = leftChannelsData.Where((el)=>el.Selected).Select((el)=>el.Ind).ToArray();
                    ind = 0;
                    int[] indsRight = rightChannelsData.Where((el) => el.Selected).Select((el) => el.Ind).ToArray();

                    //здесь надо проверить, что у нас прошло хотя бы по одному электроду слева и справа, иначе кидаем ошибку
                    if (indsLeft.Length == 0 || indsRight.Length == 0)
                    {
                        string errorMessage = "";
                        if (indsLeft.Length == 0 & indsRight.Length != 0)
                        {
                            errorMessage += "Все левые каналы не прошли проверку качества сигнала.";
                        }
                        else if (indsLeft.Length != 0 && indsRight.Length == 0)
                        {
                            errorMessage += "Все правые каналы не прошли проверку качества сигнала.";
                        }
                        else
                        {
                            errorMessage += "Все левые и правые каналы не прошли проверку качества сигнала.";
                        }

                        MessageBox.Show(errorMessage, "Не удалось рассчитать каналы");

                        SetState(State.DoesNothing);

                        SaveFailureResults(powers_l, rests_l, powers_left_l, powers_right_l,
                        powers_medians, powers_sigmas, rests_medians, rests_sigmas);

                        return;
                    }
                    //расчитываем отчечки для каждого канала

                    double[] cutOffsLeft = indsLeft.Select(elInd => (rests_medians[elInd] + powers_medians[elInd]) / 2).ToArray();
                    double[] cutOffsRight = indsRight.Select(elInd => (rests_medians[elInd] + powers_medians[elInd]) / 2).ToArray();

                    int[,] matches = new int[indsLeft.Length, indsRight.Length];

                    for (int i = 0; i < indsLeft.Length; i++)
                    {
                        for (int j = 0; j < indsRight.Length; j++)
                        {
                            for (int k = 0; k < powers_left_l[i].Length; k++)
                            {
                                int leftInd = indsLeft[i];
                                int rightInd = indsRight[j];
                                //bool leftPowered = powers_left_l[leftInd][k] < cutOffsLeft[i];
                                //bool rightNonPowered = powers_left_l[rightInd][k] >= cutOffsRight[j];

                                bool leftInverted = rests_medians[leftInd] - powers_medians[leftInd] < 0;
                                bool rightInverted = rests_medians[rightInd] - powers_medians[rightInd] < 0;

                                MyuRitmAnalizer.MoveCommand com = MyuRitmAnalizer.Evaluate(powers_left_l[leftInd][k], powers_left_l[rightInd][k], cutOffsLeft[i], cutOffsRight[j],
                                    leftInverted, rightInverted);

                                matches[i, j] += com == MyuRitmAnalizer.MoveCommand.Left ? 1 : 0; //leftPowered & rightNonPowered ? 1 : 0;
                            }

                            for (int k = 0; k < powers_right_l[j].Length; k++)
                            {
                                int leftInd = indsLeft[i];
                                int rightInd = indsRight[j];
                                
                                //bool leftNonPowered = powers_right_l[leftInd][k] > cutOffsLeft[i];
                                //bool rightPowered = powers_right_l[rightInd][k] <= cutOffsRight[j];
                                //matches[i, j] += leftNonPowered & rightPowered ? 1 : 0;

                                bool leftInverted = rests_medians[leftInd] - powers_medians[leftInd] < 0;
                                bool rightInverted = rests_medians[rightInd] - powers_medians[rightInd] < 0;

                                MyuRitmAnalizer.MoveCommand com = MyuRitmAnalizer.Evaluate(powers_right_l[leftInd][k], powers_right_l[rightInd][k], cutOffsLeft[i], cutOffsRight[j],
                                    leftInverted, rightInverted);

                                matches[i, j] += com == MyuRitmAnalizer.MoveCommand.Right ? 1 : 0;
                            }
                        }
                    }

                    int best_left_ind = 0;
                    int best_right_ind = 0;
                    for (int i = 0; i < matches.GetLength(0); i++)
                    {
                        for (int j = 0; j < matches.GetLength(1); j++)
                        {
                            if (matches[i, j] > matches[best_left_ind, best_right_ind])
                            {
                                best_left_ind = i;
                                best_right_ind = j;
                            }
                        }
                    }

                    int[,] globalMatches = new int[29, 29];
                    for (int i = 0; i < indsLeft.Length; i++)
                    {
                        for (int j = 0; j < indsRight.Length; j++)
                        {
                            globalMatches[indsLeft[i], indsRight[j]] = matches[i, j];
                        }
                    }

                    int bestLeftChannel = indsLeft[best_left_ind];
                    int bestRightChannel = indsRight[best_right_ind];

                    double cutOffL = cutOffsLeft[best_left_ind];
                    double cutOffR = cutOffsRight[best_right_ind];


                    SaveResults(powers_l, rests_l, powers_left_l, powers_right_l,
                        powers_medians, powers_sigmas, rests_medians, rests_sigmas,
                        globalMatches, bestLeftChannel, bestRightChannel, cutOffL, cutOffR);
                    form.CutOffLeft = (int)cutOffL;
                    form.CutOffRight = (int)cutOffR;
                    form.ChannelLeftInd = bestLeftChannel;
                    form.ChannelRightInd = bestRightChannel;
                    form.LeftInverted = rests_medians[bestLeftChannel] - powers_medians[bestLeftChannel] < 0;
                    form.RightInverted = rests_medians[bestRightChannel] - powers_medians[bestRightChannel] < 0;

                    MessageBox.Show($"Расчёт окончен! Выбраны каналы:\r\n{Channels[bestLeftChannel].Name}(Лево {(form.LeftInverted? "Инвертировано": "Не инвертировано")}) \r\n{Channels[bestRightChannel].Name }(Право {(form.RightInverted? "Инвертировано": "Не инвертировано")})");
                    SetState(State.DoneCounting);
                }));
            });

            t.Start();
        }
        #endregion

        #region timer events
        /// <summary>
        /// Represents time left before next action been taken by user
        /// </summary>
        void SetTimeLeft(int secLeft)
        {
            SetCommand(command);
            textBoxCommand.Text += "(ещё " + secLeft + " сек.)";
        }

        /// <summary>
        /// Checks state of current levels computation process and changes current state of form if required 
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            if (now - startedLeveling <= timeout)
            {
                SetCommand(EduCommand.Rest);
                SetTimeLeft((int)(timeout - (now - startedLeveling)).TotalSeconds);
                form.CommandsRepresenationController.Progress = (now - startedLeveling).TotalSeconds / timeout.TotalSeconds;
                form.CommandsRepresenationController.RequiredCommand =  MyuRitmAnalizer.MoveCommand.Stop;
            }
            else if (now - startedLeveling <= timeout + timeout)
            {
                if(command!=EduCommand.Power)
                {
                    DateTime startedTime = DateTime.Now;
                    form.AddStoreEEGDataWork(GetSaveCommandFileName(EduCommand.Power, startedTime),
                        () => command!=EduCommand.Power, null);
                }
                SetCommand(EduCommand.Power);
                SetTimeLeft((int)(timeout.TotalSeconds * 2 - (now - startedLeveling).TotalSeconds));

                form.CommandsRepresenationController.Progress = (now - (startedLeveling + timeout)).TotalSeconds / timeout.TotalSeconds;
                form.CommandsRepresenationController.RequiredCommand = MyuRitmAnalizer.MoveCommand.Forward;
            }
            else if(now - startedLeveling <= timeout + timeout + timeout)
            {
                if (command != EduCommand.PowerLeft)
                {
                    DateTime startedTime = DateTime.Now;
                    form.AddStoreEEGDataWork(GetSaveCommandFileName(EduCommand.PowerLeft, startedTime),
                        () => command != EduCommand.PowerLeft, null);
                }
                SetCommand(EduCommand.PowerLeft);
                SetTimeLeft((int)(timeout.TotalSeconds * 3 - (now - startedLeveling).TotalSeconds));
                form.CommandsRepresenationController.Progress = (now - (startedLeveling + timeout + timeout)).TotalSeconds / timeout.TotalSeconds;
                form.CommandsRepresenationController.RequiredCommand = MyuRitmAnalizer.MoveCommand.Left;
            }
            else if(now - startedLeveling <= timeout + timeout + timeout + timeout)
            {
                if (command != EduCommand.PowerRight)
                {
                    DateTime startedTime = DateTime.Now;
                    form.AddStoreEEGDataWork(GetSaveCommandFileName(EduCommand.PowerRight, startedTime),
                        () => command != EduCommand.PowerRight, null);
                }
                SetCommand(EduCommand.PowerRight);
                SetTimeLeft((int)(timeout.TotalSeconds * 4 - (now - startedLeveling).TotalSeconds));

                form.CommandsRepresenationController.Progress = (now - (startedLeveling + timeout + timeout + timeout)).TotalSeconds / timeout.TotalSeconds;
                form.CommandsRepresenationController.RequiredCommand = MyuRitmAnalizer.MoveCommand.Right;
            }
            else
            {
                //завершаем
                form.CommandsRepresenationController.Progress = 1;
                form.CommandsRepresenationController.RequiredCommand = MyuRitmAnalizer.MoveCommand.None;
                form.CommandsRepresenationController.Show(CommandsRepresenationController.Mode.OnlyRequiredCommand);
                SetState(State.Saving);
            }
        }

        /// <summary>
        /// Timer responsible for dots animation on "saving" button
        /// </summary>
        int ind = 0;
        private void savingTimer_Tick(object sender, EventArgs e)
        {
            ind++;
            this.Text = "Получение уровней. Сохранение.";
            buttonStartLevels.Text = "Сохранение.";
            ind %= 4;
            for (int i = 0; i < ind; i++)
            {
                this.Text += ".";
                buttonStartLevels.Text += ".";
            }
        }
        #endregion

    }
}
