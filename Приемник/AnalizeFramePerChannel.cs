using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Приемник
{
    public class AnalizeFramePerChannel
    {
        /// <summary>
        /// EEG signal
        /// </summary>
        public readonly double[] Data;

        private int MinFreq;
        /// <summary>
        /// Element of EEG signal which is related to MinFreq
        /// </summary>
        public int MinFreqElem => (int)(MinFreq * Duration);


        private int MaxFreq;
        /// <summary>
        /// Element of EEG signal which is related to MaxFreq
        /// </summary>
        public int MaxFreqElem => (int)(MaxFreq * Duration);

        public double Duration;
        public AnalizeFramePerChannel(double[] data, int minFreq, int maxFreq, double duration)
        {
            Data = data;
            MinFreq = minFreq;
            MaxFreq = maxFreq;
            Duration = duration;
        }

        double power = 0;
        double[] fourier;
        alglib.complex[] fourierComplex;
        /// <summary>
        /// Sprectral power of EEG data strored
        /// </summary>
        public double Power
        {
            get
            {
                if (fourier == null)
                {
                    computeParams();
                }
                return power;
            }
        }
        /// <summary>
        /// Collection of complex fourier elements of EEG data strored
        /// </summary>
        public alglib.complex[] FourierComplexElements
        {
            get
            {
                if(fourier == null)
                {
                    computeParams();
                }
                return fourierComplex;
            }
        }
        /// <summary>
        /// Collection of real fourier elements of EEG signal stored
        /// </summary>
        public double[] FourierElements
        {
            get
            {
                if(fourier == null)
                {
                    computeParams();
                }
                return fourier;
            }
        }

        /// <summary>
        /// Evaluates all required properties of EEG signal stored. Called once for one object
        /// </summary>
        private void computeParams()
        {
            ComputeFourier(Data, MinFreq, MaxFreq, Duration, out fourier, out fourierComplex);
            power = fourier.Aggregate(0d, (sum, el) => sum + el);
        }

        /// <summary>
        /// Evaluates all required properties of EEG signal stored.
        /// </summary>
        /// <param name="data">Egg signal</param>
        /// <param name="minFreq">minimum frequency of </param>
        /// <param name="maxFreq"></param>
        /// <param name="duration"></param>
        /// <param name="fourier"></param>
        /// <param name="fourierComplex"></param>
        private static void ComputeFourier(double[] data, int minFreq, int maxFreq, double duration, out double[] fourier, out alglib.complex[] fourierComplex)
        {
            fourierComplex = new alglib.complex[data.Length];
            alglib.xparams prms = new alglib.xparams(0);
            alglib.fftr1d(data, data.Length, out fourierComplex, prms);

            double[] freqs = new double[fourierComplex.Length];

            for (int freq = (int)(minFreq * duration), i=0; freq <= maxFreq * duration; freq++, i++)
            {
                freqs[i] = Math.Sqrt(fourierComplex[freq].x * fourierComplex[freq].x
                    + fourierComplex[freq].y * fourierComplex[freq].y) / duration;
            }
            fourier = freqs;
        }

    }
}
