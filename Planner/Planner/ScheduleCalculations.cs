using System;
using System.Collections.Generic;
using Planner.ObjectModel;

namespace Planner
{
    public static class ScheduleCalculations
    {
        public static int EarliestStartTime(IEnumerable<ActivityNode> precedents)
        {
            var est = 0;

            // Not using Linq's Max since precedents can be empty.
            foreach (var precedent in precedents)
            {
                est = Math.Max(precedent.EarliestFinish, est);
            }

            return est;
        }

        public static int EarliestFinishTime(int earliestStartTime, int duration)
        {
            return earliestStartTime + duration;
        }

        public static int LatestStartTime(int latestFinishTime, int duration)
        {
            return latestFinishTime - duration;
        }

        public static int LatestFinishTime(ActivityNode node, IEnumerable<ActivityNode> dependents)
        {
            var any = false;
            var eft = int.MaxValue;

            foreach (var dependent in dependents)
            {
                eft = Math.Min(dependent.LatestStart, eft);
                any = true;
            }

            return any ? eft : node.EarliestFinish;
        }

        public static bool IsCriticalPath(ActivityNode node)
        {
            return 0 == (node.LatestFinish - node.EarliestFinish);
        }
    }
}
