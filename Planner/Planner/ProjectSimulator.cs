using System;
using System.Collections.Generic;
using Planner.ObjectModel;

namespace Planner
{
    public class ProjectSimulator
    {
        private readonly ProjectNetwork _network;

        public ProjectSimulator(ProjectNetwork network)
        {
            _network = network;
        }

        public ProjectSimulatorResults Simulate(int iterations)
        {
            if (iterations <= 0) throw new ArgumentOutOfRangeException(nameof(iterations));

            int min = 0, max = 0;
            var histogram = new Dictionary<int, int>();

            for (int i = 0; i < iterations; i++)
            {
                var schedule = _network.Calculate();

                min = Math.Min(min, schedule.FinishTime);
                max = Math.Max(max, schedule.FinishTime);

                histogram.TryGetValue(schedule.FinishTime, out var count);
                histogram[schedule.FinishTime] = count + 1;
            }

            return new ProjectSimulatorResults(min, max, new SortedDictionary<int, int>(histogram));
        }
    }
}