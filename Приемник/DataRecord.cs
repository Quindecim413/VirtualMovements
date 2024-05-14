using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Приемник
{
    public class DataRecord
    {
        public class ChannelRecord
        {
            #region fourier data
            public List<Tuple<double, double>> FourierValues { get; } = new List<Tuple<double, double>>();
            public List<Tuple<double, double, double>> FourierComplexValues { get; } = new List<Tuple<double, double, double>>();
            #endregion

            public void AddRealFourierValue(double freq, double fourierValue)
            {
                FourierValues.Add(new Tuple<double, double>(freq, fourierValue));
            }
            public void AddComplexFourierValue(double freq, double fourierValueX, double fourierValueY)
            {
                FourierComplexValues.Add(new Tuple<double, double, double>(freq, fourierValueX, fourierValueY));
            }

            /// <summary>
            /// Spectral power of fourier signal
            /// </summary>
            public double Power
            {
                get;
                set;
            } = double.MinValue;


            public double[] Data
            {
                get;
                set;
            } = new double[0];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelA">Left channel to analize</param>
        /// <param name="channelB">Right channel to analize</param>
        public DataRecord(int channelA, int channelB)
        {
            this.Channels.Add(channelA, new ChannelRecord());
            this.Channels.Add(channelB, new ChannelRecord());
        }

        public Dictionary<int, ChannelRecord> Channels { get; } = new Dictionary<int, ChannelRecord>();

        /// <summary>
        /// Methods saved its contents into passed baseDir
        /// </summary>
        /// <param name="baseDir">name of directory where to save data</param>
        public void WriteToDir(string baseDir)
        {
            foreach(var ch in Channels)
            {
                string channelBase = baseDir + ch.Key + "/";

                DirectoryInfo dirInfo = new DirectoryInfo(channelBase);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                using (StreamWriter sw = new StreamWriter(channelBase+"power.txt", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(ch.Value.Power);
                }

                using (StreamWriter sw = new StreamWriter(channelBase + "fourier.dat", false, System.Text.Encoding.Default))
                {
                    foreach (var d in ch.Value.FourierValues)
                    {
                        sw.WriteLine(d.Item1 + " " + d.Item2);
                    }
                }
                using (StreamWriter sw = new StreamWriter(channelBase + "fourier_complex.dat", false, System.Text.Encoding.Default))
                {
                    foreach (var d in ch.Value.FourierComplexValues)
                    {
                        sw.WriteLine(d.Item1 + " " + d.Item2 + " " + d.Item3);
                    }
                }

                using (StreamWriter sw = new StreamWriter(channelBase + "data.dat", false, System.Text.Encoding.Default))
                {
                    foreach (var d in ch.Value.Data)
                    {
                        sw.WriteLine(d);
                    }
                }
            }
            
        }
    }
}
