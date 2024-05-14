using System;
using System.IO;
using System.Linq;

namespace Приемник
{
    public class StoreEEGDataWork
    {
        public double DurationAfter;

        DateTime DurationAfterTime = DateTime.MinValue;

        public Func<bool> RecordDoneCheck;
        DateTime recordDoneTime = DateTime.MinValue;

        public string SaveFileName { get; private set; }

        public StoreEEGDataWork(double[][] beforeData, double durationAfter, string saveFileName, Func<bool> recordComplitionCheck)
        {
            DurationAfter = durationAfter;
            RecordDoneCheck = recordComplitionCheck;
            SaveFileName = saveFileName;
            SaveToFile(beforeData, SaveFileName);
        }

        public event Action<double[][]> OnRecieve;

        public void AddData(double[][] data)
        {
            if (recordDoneTime == DateTime.MinValue)
            {
                if (OnRecieve != null)
                    OnRecieve.Invoke(data);

            }
            SaveToFile(data, SaveFileName);
        }

        private static void SaveToFile(double[][] data, string saveFileName)
        {
            string dirname = Path.GetDirectoryName(saveFileName);
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
            if (!File.Exists(saveFileName))
            {
                File.Create(saveFileName).Close();
            }
            using (StreamWriter sw = new StreamWriter(saveFileName, true))
            {
                foreach (var line in data)
                    sw.WriteLine(String.Join("\t", line.Select(el => el.ToString())));
            }
        }
        bool alreadyDone = false;
        public bool DoneRecording()
        {
            if (alreadyDone)
            {
                return (DateTime.Now - DurationAfterTime).TotalSeconds > DurationAfter;
            }
            else
            {
                alreadyDone = RecordDoneCheck();
                DurationAfterTime = DateTime.Now;
            }
            return false;
            if (!RecordDoneCheck())
            {
                DurationAfterTime = DateTime.Now;
                return false;
            }else return (DateTime.Now - DurationAfterTime).TotalSeconds > DurationAfter;
        }
    }
}
