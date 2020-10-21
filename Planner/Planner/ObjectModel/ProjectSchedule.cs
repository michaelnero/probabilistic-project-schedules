using System.Collections.Generic;

namespace Planner.ObjectModel
{
    public class ProjectSchedule
    {
        public ProjectSchedule(
            IEnumerable<ActivityNode> activityNodes,
            int startTime,
            int finishTime,
            IReadOnlyList<string> criticalPath)
        {
            ActivityNodes = new List<ActivityNode>(activityNodes);
            StartTime = startTime;
            FinishTime = finishTime;
            CriticalPath = criticalPath;
        }

        public IReadOnlyList<ActivityNode> ActivityNodes { get; }
        public int StartTime { get; }
        public int FinishTime { get; }
        public IReadOnlyList<string> CriticalPath { get; }
    }
}
