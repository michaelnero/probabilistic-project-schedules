using System;
using System.Collections.Generic;
using System.Linq;

namespace Planner.ObjectModel
{
    public class ProjectNetwork
    {
        private readonly Dictionary<string, Activity> _map;
        private readonly DependencyMatrix _matrix;

        private IReadOnlyList<string> _graph;

        public ProjectNetwork()
            : this(Array.Empty<Activity>())
        {

        }

        public ProjectNetwork(IEnumerable<Activity> activities)
        {
            _map = new Dictionary<string, Activity>();
            _matrix = new DependencyMatrix();

            AddRange(activities);
        }

        public void AddRange(IEnumerable<Activity> activities)
        {
            foreach (var activity in activities)
            {
                Add(activity);
            }
        }

        public void Add(Activity activity)
        {
            if (!_map.TryAdd(activity.Id, activity))
            {
                throw new ArgumentException($"Activity with ID '{activity.Id}' already exists in the network.", nameof(activity));
            }

            _matrix.Add(activity.Id, activity.Precedents);

            // Clear the cached ordered dependency vector.
            _graph = null;
        }

        public ProjectSchedule Calculate()
        {
            // First, we get an ordered vector for the dependency matrix.
            // The vector is cached so we only have to do this once if the
            // network doesn't change.
            var graph = _graph ??= DependencySolver.Solve(_matrix);

            // This can be empty since we visit precedent activities before
            // dependent activities. We create the ActivityNode instances in the
            // first loop since we're gauranteed to visit activites in the
            // correct order.
            var nodes = new Dictionary<string, ActivityNode>();

            // Do the forward pass through the project network.
            for (var i = 0; i < graph.Count; i++)
            {
                var id = graph[i];

                var activity = _map[id];
                var duration = activity.Duration.Sample();

                // We're gauranteed to have precedent nodes created because of
                // our ordering gaurantee.
                var precedents = activity.Precedents.Select(x => nodes[x]);

                var est = ScheduleCalculations.EarliestStartTime(precedents);
                var eft = ScheduleCalculations.EarliestFinishTime(est, duration);

                nodes[id] = new ActivityNode(activity)
                {
                    Duration = duration,
                    EarliestStart = est,
                    EarliestFinish = eft
                };
            }

            var finish = 0;
            var criticalPath = new List<string>();

            // Do the backwards pass through the network.
            for (var i = graph.Count - 1; i >= 0; i--)
            {
                var id = graph[i];

                var activity = _map[id];
                var node = nodes[id];

                // We're gauranteed to have dependent nodes created because of
                // our ordering gaurantee -- but in reverse of the first loop.
                var dependents = _matrix[id].Select(x => nodes[x]);

                var lft = ScheduleCalculations.LatestFinishTime(node, dependents);
                var lst = ScheduleCalculations.LatestStartTime(lft, node.Duration);

                node.LatestFinish = lft;
                node.LatestStart = lst;

                finish = Math.Max(lft, finish);

                if (ScheduleCalculations.IsCriticalPath(node))
                {
                    criticalPath.Add(id);
                }
            }

            // Since we populated this while moving backwards through the
            // network, the activity IDs will be backwards. So we just need to
            // reverse the list.
            criticalPath.Reverse();

            return new ProjectSchedule(nodes.Values, 0, finish, criticalPath);
        }
    }
}
