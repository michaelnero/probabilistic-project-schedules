using System;
using Accord.Statistics.Distributions.Univariate;

namespace Planner.ObjectModel
{
    public class ThreePointEstimate
    {
        private GeneralizedBetaDistribution _distribution;

        public ThreePointEstimate(int min, int mode, int max)
        {
            Min = min;
            Mode = mode;
            Max = max;
        }

        public int Min { get; }
        public int Mode { get; }
        public int Max { get; }

        public int Sample()
        {
            if (null == _distribution)
            {
                _distribution = GeneralizedBetaDistribution.Vose(Min, Max, Mode);
            }

            var value = (int)Math.Round(_distribution.Generate());
            return value;
        }

        public static implicit operator ThreePointEstimate((int min, int mode, int max) duration)
        {
            return new ThreePointEstimate(duration.min, duration.mode, duration.max);
        }
    }
}
