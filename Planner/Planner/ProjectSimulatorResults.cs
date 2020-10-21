using System.Collections.Generic;

namespace Planner
{
    public class ProjectSimulatorResults
    {
        public ProjectSimulatorResults(int scheduleMin, int scheduleMax, IReadOnlyDictionary<int, int> histogram)
        {
            ScheduleMin = scheduleMin;
            ScheduleMax = scheduleMax;
            Histogram = histogram;
        }

        public int ScheduleMin { get; }
        public int ScheduleMax { get; }
        public IReadOnlyDictionary<int, int> Histogram { get; }
    }
}