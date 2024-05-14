using System;
using System.Windows.Forms;
using System.Net;
using NetManager;
using EEG;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace Приемник
{
    public partial class MainForm : Form
    {
        #region fields

        int samplingFrequency = 0;
        int channelA = 0, channelB = 0;

        int frequencyLeft = 0, frequencyRight = 0;

        int channelCount = 0;

        public GetLevels getLevelsForm;

        double durationStart = 0, durationProcess = 1, durationFinish = 0;
        public CommandsRepresenationController CommandsRepresenationController { get; }
        private LastCommandAnimationController LastCommandAnimationController { get; }

        public double DurationBefore
        {
            get
            {
                return durationStart;
            }
        }
        public double DurationAfter
        {
            get
            {
                return durationFinish;
            }
        }

        public bool LeftInverted
        {
            get
            {
                return leftInvertedCheckBox.Checked;
            }
            set
            {
                leftInvertedCheckBox.Checked = value;
            }
        }
        public bool RightInverted
        {
            get
            {
                return rightInvertedCheckBox.Checked;
            }
            set
            {
                rightInvertedCheckBox.Checked = value;
            }
        }

        double windDelta = 0.2;

        int showingElements = 50;
        List<double[]> bufferedData = new List<double[]>();

        ArrayList Storage;

        public string ClientIdentifier
        {
            get;
            private set;

        }

        /// <summary>
        /// Index of left analing channel.
        /// Automatically updates values on form.
        /// </summary>
        public int ChannelLeftInd
        {
            get
            {
                return channelA;
            }
            set
            {
                channelA = value;
                comboBoxLeftChannel.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Index of right analing channel.
        /// Automatically updates values on form.
        /// </summary>
        public int ChannelRightInd
        {
            get
            {
                return channelB;
            }
            set
            {
                channelB = value;
                comboBoxRightChannel.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Property which controls cutoff level for right channel
        /// </summary>
        int valL = 0;
        public int CutOffLeft
        {
            get
            {
                return valL;
            }
            set
            {
                valL = value;
                Invoke((Action)(() =>
                {
                    numericUpDownCutOffL.Value = valL;
                }));
            }
        }

        /// <summary>
        /// Property which controls cutoff level for right channel
        /// </summary>
        int valR = 0;
        public int CutOffRight
        {
            get
            {
                return valR;
            }
            set
            {
                valR = value;
                Invoke((Action)(() =>
                {
                    numericUpDownCutOffR.Value = valR;
                }));
            }
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();

            NMClient = new NMClient(this);
            NMClient.OnDeleteClient += new EventHandler<EventClientArgs>(NMClient_OnDeleteClient);
            NMClient.OnError += new EventHandler<EventMsgArgs>(NMClient_OnError);
            NMClient.OnNewClient += new EventHandler<EventClientArgs>(NMClient_OnNewClient);
            NMClient.OnReseive += new EventHandler<EventClientMsgArgs>(NMClient_OnReseive);
            NMClient.OnStop += new EventHandler(NMClient_OnStop);

            LastCommandAnimationController = new LastCommandAnimationController(lastCommandAnimation);

            CommandsRepresenationController = new CommandsRepresenationController(
                new LabelWithValue(label8, evaluatedCommandLabel), 
                new LabelWithValue(label15, requiredCommandLabel), CommandTimeProgressBar,
                new LabelWithValue(label12, successCountlabel), 
                new LabelWithValue(label18, failureCountlabel), 
                LastCommandAnimationController,
                panel1, panel2, chart4);
        }

        #region Networking
        void NMClient_OnStop(object sender, EventArgs e)
        {
            btnConnect.Text = "Подключить";
            chClients.Enabled = true;
            chClients.Items.Clear();
            if (OnDisconnected != null)
                OnDisconnected.Invoke();
        }   
        
        void NMClient_OnReseive(object sender, EventClientMsgArgs e)
        {
            int N = BitConverter.ToInt32(e.Msg, 0);//первые четыре байта переводим в число
            if (N == 6)
            {
                Frame RAW = new Frame(e.Msg);

                double[][] data_out = new double[24][];

                for (int row = 0; row < 24; row++)
                {
                    double[] data = new double[29];
                    for (int column = 0; column < 29; column++)
                    {
                        data[column] = RAW.Data[column * 24 + row];
                    }

                    data_out[row] = data;

                    Storage.Add(data);

                    
                    if (Storage.Count > (durationProcess + windDelta) * samplingFrequency)
                    {
                        Storage.RemoveRange(0, Storage.Count - (int)((durationProcess + windDelta) * samplingFrequency));
                    }
                }
                SendToTraining(data_out);

                if (this.InvokeRequired)
                    Invoke(new Action(() =>
                    {
                        SendDataToStoreEEGDataWorks(data_out);
                    }));
                else
                    SendDataToStoreEEGDataWorks(data_out);
            }
        }
        void NMClient_OnNewClient(object sender, EventClientArgs e)
        {
            chClients.Items.Add(new ClientAddress(e.ClientId, e.Name));
        }
        void NMClient_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void NMClient_OnDeleteClient(object sender, EventClientArgs e)
        {
            ClientAddress Cl = new ClientAddress(e.ClientId, e.Name);
            int I = chClients.Items.Count - 1;
            while ((I >= 0) && (Cl.ToString() != chClients.Items[I].ToString()))
                I--;
            if (I >= 0)
                chClients.Items.RemoveAt(I);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
            CommandsRepresenationController.Show(CommandsRepresenationController.Mode.OnlyEvaluatedCommand);
        }

        private NMClient NMClient;

        private delegate void delegate_SetEnabled(Control Control, bool Enabled);
        private void SetEnabled(Control Control, bool Enabled)
        {
            if (Control.InvokeRequired)
            {
                delegate_SetEnabled E = new delegate_SetEnabled(SetEnabled);
                Control.Invoke(E, new object[] { Control, Enabled });
            }
            else
                Control.Enabled = Enabled;
        }

        #endregion

        #region Events
        private void Button1_Click(object sender, EventArgs e)
        {
            samplingFrequency = (int)numericUpDown3.Value;
            numericUpDown6.Maximum = (samplingFrequency / 2);
            numericUpDown7.Maximum = (samplingFrequency / 2);

            frequencyLeft = (int)numericUpDown6.Value;
            frequencyRight = (int)numericUpDown7.Value;
            if (frequencyLeft > frequencyRight)
            {
                int temp = frequencyLeft;
                frequencyLeft = frequencyRight;
                frequencyRight = temp;
            }

            channelCount = (int)numericUpDown1.Value;

            durationStart = (double)numericUpDown2.Value;
            durationProcess = (double)numericUpDown8.Value;

            windDelta = (double)numericUpDownWindDelta.Value;

            durationFinish = (double)numericUpDown12.Value;

            Storage = new ArrayList();

            if (textBoxIdentifier.Text.Length == 0)
            {
                DateTime dt = DateTime.Now;
                ClientIdentifier = dt.ToString("yyyyMMddHHmm");
                textBoxIdentifier.Text = ClientIdentifier;
            }
            else
            {
                ClientIdentifier = textBoxIdentifier.Text;
            }
            timer2.Interval = (int)(windDelta * 1000);
        }

        private void rightInvertedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void leftInvertedCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// main event where charts and input signal info have been updated on MainForm
        /// </summary>
        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (Storage.Count >= samplingFrequency * (durationProcess + windDelta))
            {
                int s_t_d = (int)(samplingFrequency * durationProcess);
                
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();

                double[][] data_per_channel_raw = new double[29][];
                for(int i = 0; i < 29; i++)
                {
                    data_per_channel_raw[i] = new double[s_t_d];
                }

                for (int i = 0; i < s_t_d; i++)
                {
                    var rec = (double[])Storage[Storage.Count - s_t_d + i];

                    for(int j = 0; j < 29; j++)
                    {
                        data_per_channel_raw[j][i] = rec[j];
                    }

                    chart1.Series[0].Points.AddY(data_per_channel_raw[channelA][i]);
                    chart1.Series[1].Points.AddY(data_per_channel_raw[channelB][i]);
                }

                AnalizeFramePerChannel[] DataPerChannel = new AnalizeFramePerChannel[29];
                for(int i = 0; i < 29; i++)
                {
                    DataPerChannel[i] = new AnalizeFramePerChannel(data_per_channel_raw[i], frequencyLeft, frequencyRight, durationProcess);
                }
                
                chart3.Series[0].Points.Clear();
                chart3.Series[1].Points.Clear();
                chart3.Series[2].Points.Clear();
                chart3.Series[3].Points.Clear();

                var channelsToShow = new AnalizeFramePerChannel[]
                {
                    DataPerChannel[channelA],
                    DataPerChannel[channelB]
                };
                
                for(int k = 0; k < 2; k++)
                {
                    var ch = channelsToShow[k];
                    for(int freq = ch.MinFreqElem; freq < ch.MaxFreqElem; freq++)
                    {
                        double F = freq / ch.Duration;
                        double _x = ch.FourierComplexElements[freq].x / ch.Duration;
                        double _y = ch.FourierComplexElements[freq].y / ch.Duration;
                        chart3.Series[2*k].Points.AddXY(F, _x);
                        chart3.Series[2 * k + 1].Points.AddXY(F, _y);
                        int chInd = k == 0 ? channelA : channelB;
                    }
                }

                chart2.Series[0].Points.Clear();
                chart2.Series[1].Points.Clear();

                for(int fr = channelsToShow[0].MinFreqElem; 
                    fr < channelsToShow[0].MaxFreqElem; fr++)
                {
                    double F = fr / durationProcess;
                    chart2.Series[0].Points.AddXY(F, channelsToShow[0].FourierElements[fr]);
                }

                for (int fr = channelsToShow[1].MinFreqElem;
                    fr < channelsToShow[1].MaxFreqElem; fr++)
                {
                    double F = fr / durationProcess;
                    chart2.Series[1].Points.AddXY(F, channelsToShow[1].FourierElements[fr]);
                }

                double powerA = DataPerChannel[channelA].Power;
                double powerB = DataPerChannel[channelB].Power;

                AnnounceNewData(DataPerChannel);



                MyuRitmAnalizer.MoveCommand com = MyuRitmAnalizer.Evaluate(DataPerChannel[channelA].Power, DataPerChannel[channelB].Power, 
                    CutOffLeft, CutOffRight, LeftInverted, RightInverted);



                //if (powerA >= CutOffLeft && powerB >= CutOffRight)
                //{
                //    com = MoveCommand.NoMove;
                //}
                //else if (powerA < CutOffLeft && powerB >= CutOffRight)
                //{
                //    com = MoveCommand.Left;
                //}
                //else if (powerA >= CutOffLeft && powerB < CutOffRight)
                //{
                //    com = MoveCommand.Right;
                //}
                //else
                //{
                //    com = MoveCommand.Up;
                //}

                label6.Text = powerA.ToString();
                label7.Text = powerB.ToString();
                
                AnnounceNewCommand(com);
                
                if(showingElements == 1)
                {
                    //страница Управление
                    //отображаем столбики с линиями отсечками
                    chart4.Series[0].Points.Clear();
                    chart4.Series[1].Points.Clear();
                    chart4.Series[2].Points.Clear();
                    chart4.Series[3].Points.Clear();

                    chart4.Series[0].Points.AddXY(1, powerA);
                    chart4.Series[0].LegendText = powerA.ToString();

                    chart4.Series[1].Points.AddXY(2, powerB);
                    chart4.Series[1].LegendText = powerB.ToString();

                    chart4.Series[2].Points.AddXY(0, CutOffLeft);
                    chart4.Series[2].Points.AddXY(1.5, CutOffLeft);
                    chart4.Series[2].LegendText = "CutOffLeft " + CutOffLeft.ToString();

                    chart4.Series[3].Points.AddXY(1.5, CutOffRight);
                    chart4.Series[3].Points.AddXY(3, CutOffRight);
                    chart4.Series[3].LegendText = "CutOffRight " + CutOffRight.ToString();
                }
                else
                {
                    //страница Управление
                    //отображаем линии
                    chart4.Series[0].Points.Add(powerA);
                    chart4.Series[0].LegendText = powerA.ToString();
                    
                    chart4.Series[1].Points.Add(powerB);
                    chart4.Series[1].LegendText = powerB.ToString();

                    chart4.Series[2].Points.Add(CutOffLeft);
                    chart4.Series[2].LegendText = "CutOffLeft " + CutOffLeft.ToString();

                    chart4.Series[3].Points.Add(CutOffRight);
                    chart4.Series[3].LegendText = "CutOffRight " + CutOffRight.ToString();

                    while (chart4.Series[0].Points.Count > showingElements)
                    {
                        chart4.Series[0].Points.RemoveAt(0);
                        chart4.Series[1].Points.RemoveAt(0);
                        chart4.Series[2].Points.RemoveAt(0);
                        chart4.Series[3].Points.RemoveAt(0);
                    }

                    chart4.ChartAreas[0].RecalculateAxesScale();
                    
                }

                evaluatedCommandLabel.Text = MoveComandToString(com);

                int[] addresses = new int[chClients.CheckedItems.Count];

                for (int j = 0; j < chClients.CheckedItems.Count; j++)
                    addresses[j] = (chClients.CheckedItems[j] as ClientAddress).Id;

                Frame d = new Frame();
                d.Data[0] = (short)com;

                NMClient.SendData(addresses, d.GetBytes());

                Thread.Sleep(4);
            }
        }

        /// <summary>
        /// shows up CountLevels form and disables controlls preventing opening one more CountLevels form or Training
        /// </summary>
        private void buttonCountLevels_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[3];
            buttonDoTrain.Enabled = false;
            buttonCountLevels.Enabled = false;

            getLevelsForm = new GetLevels(this);
            getLevelsForm.Show();

            getLevelsForm.FormClosed += (s, ev) => {
                buttonDoTrain.Enabled = true;
                buttonCountLevels.Enabled = true;
                getLevelsForm = null;
                ParamsChangesLocked = false;
                CommandsRepresenationController.Show(CommandsRepresenationController.Mode.OnlyEvaluatedCommand);
            };
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownWindDelta.Maximum = numericUpDown8.Value;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!NMClient.Running)
            {
                NMClient.IPServer = IPAddress.Parse(tbIP.Text);
                NMClient.Port = Int32.Parse(tbPort.Text);
                NMClient.Name = tbName.Text;
                NMClient.RunClient();
                btnConnect.Text = "Отключить";
                if (OnConnected != null)
                    OnConnected.Invoke();
            }
            else
            {
                NMClient.StopClient();
                if (OnDisconnected != null)
                    OnDisconnected.Invoke();
            }
        }

        public event Action OnConnected;
        public event Action OnDisconnected;

        public bool Connected => NMClient.Running;

        bool _changesLocked = false;
        public bool ParamsChangesLocked
        {
            get
            {
                return _changesLocked;
            }
            set
            {
                groupBox2.Enabled = !value;
                groupBox3.Enabled = !value;
                button1.Enabled = !value;
                textBoxIdentifier.Enabled = !value;
                _changesLocked = value;
            }
        }

        public delegate void DataFormedDelegate(AnalizeFramePerChannel[] channels);
        public event DataFormedDelegate OnDataFormed;
        public event Action<MyuRitmAnalizer.MoveCommand> OnCommandFormed;

        private void AnnounceNewCommand(MyuRitmAnalizer.MoveCommand command)
        {
            if (OnCommandFormed != null)
            {
                OnCommandFormed.Invoke(command);
            }
        }

        private void AnnounceNewData(AnalizeFramePerChannel[] channels)
        {
            if (OnDataFormed != null)
            {
                OnDataFormed.Invoke(channels);
            }
        }

        //public void SendToTraining(MoveCommand command)
        //{
        //    if (trainingForm == null)
        //        return;
            
        //    trainingForm.RecieveCommand(command);
        //}

        public void SendToTraining(double[][]data)
        {
            if (trainingForm != null)
                trainingForm.RecieveData(data);

        }

        TrainingForm trainingForm;
        private void buttonDoTrain_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[3];
            buttonDoTrain.Enabled = false;
            buttonCountLevels.Enabled = false;

            trainingForm = new TrainingForm(this);
            trainingForm.Show();

            
            trainingForm.FormClosed += (s, ev) =>
            {
                buttonDoTrain.Enabled = true;
                buttonCountLevels.Enabled = true;
                ParamsChangesLocked = false;
                CommandsRepresenationController.Show(CommandsRepresenationController.Mode.OnlyEvaluatedCommand);
            };
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NMClient.Running)
            {
                NMClient.StopClient();
                if (OnDisconnected != null)
                    OnDisconnected.Invoke();
            }
        }

        /// <summary>
        /// Shows and hides channels powers chart
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.chart4.Visible = !checkBox1.Checked;
        }

        /// <summary>
        /// Sets number of elements to show on channel power's chart
        /// </summary>
        private void numericUpDownShowingElements_ValueChanged(object sender, EventArgs e)
        {
            int showingElements = (int)numericUpDownShowingElements.Value;

            if (showingElements == 1)
            {
                chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                chart4.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                for (int i = 0; i < 4; i++)
                {
                    chart4.Series[i].Points.Clear();
                }
            }
            else
            {
                chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart4.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }
            this.showingElements = showingElements;
        }

        private void numericUpDownCutOffL_ValueChanged(object sender, EventArgs e)
        {
            CutOffLeft = (int)numericUpDownCutOffL.Value;
        }

        private void numericUpDownCutOffR_ValueChanged(object sender, EventArgs e)
        {
            CutOffRight = (int)numericUpDownCutOffR.Value;
        }

        private void comboBoxRightChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChannelRightInd = comboBoxRightChannel.SelectedIndex;
        }

        private void comboBoxLeftChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChannelLeftInd = comboBoxLeftChannel.SelectedIndex;
        }

        #endregion

        #region utils
        /// <summary>
        /// Enum flag which represents what command performs or should be performed based of context
        /// </summary>
        public enum MoveCommand
        {
            Left, Up, Right, NoMove, None
        }

        /// <summary>
        /// Transforms MoveCommand enum to human readable notation
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
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

        DateTime lastUpdate;
        private void lastCommandAnimatatedIconTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            LastCommandAnimationController.Update(now - lastUpdate);
            lastUpdate = now;
        }

        

        private void PushToBuffer(double[][] vals)
        {
            bufferedData.AddRange(vals);
            int removeElementsCount = bufferedData.Count - (int)(samplingFrequency * durationStart);
            if (removeElementsCount > 10000)
                bufferedData.RemoveRange(0, removeElementsCount);
        }

        private void SendDataToStoreEEGDataWorks(double[][] data)
        {
            PushToBuffer(data);
            List<StoreEEGDataWork> removeWaitingList = new List<StoreEEGDataWork>();
            foreach(var store in storeEEGDataWorks)
            {
                if (!store.DoneRecording())
                {
                    store.AddData(data);
                }
                else
                {
                    removeWaitingList.Add(store);
                }
            }

            foreach (var store in removeWaitingList)
                storeEEGDataWorks.Remove(store);
        }

        private List<StoreEEGDataWork> storeEEGDataWorks = new List<StoreEEGDataWork>();
        public void AddStoreEEGDataWork(string saveFileName, Func<bool> recordComplitionCheck, Action<double[][]>OnRecieve)
        {
            int getElements = (int)(samplingFrequency * DurationBefore);
            int startElement = 0;
            if(getElements > bufferedData.Count)
            {
                startElement = bufferedData.Count - 1 - getElements;
            }
            else
            {
                getElements = bufferedData.Count;
            }

            var s = new StoreEEGDataWork(bufferedData.GetRange(startElement, getElements).ToArray(), 
                DurationAfter, saveFileName, recordComplitionCheck);

            s.OnRecieve += OnRecieve;
            storeEEGDataWorks.Add(s);
        }

        #endregion
    }
}
