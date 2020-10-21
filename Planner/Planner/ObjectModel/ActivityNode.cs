namespace Planner.ObjectModel
{
    public class ActivityNode
    {
        public ActivityNode(Activity activity)
        {
            Activity = activity;
        }

        public Activity Activity { get; }

        public int Duration { get; internal set; }
        public int EarliestStart { get; internal set; }
        public int EarliestFinish { get; internal set; }
        public int LatestStart { get; internal set; }
        public int LatestFinish { get; internal set; }
    }
}
