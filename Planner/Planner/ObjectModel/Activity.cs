using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Planner.ObjectModel
{
    [DebuggerDisplay("Activity = {Id}")]
    public class Activity
    {
        public Activity(string id, string description, ThreePointEstimate duration)
            : this(id, description, duration, Array.Empty<string>())
        {

        }

        public Activity(string id, string description, ThreePointEstimate duration, IEnumerable<string> precedents)
        {
            Id = id;
            Description = description;
            Duration = duration;
            Precedents = new HashSet<string>(precedents);
        }

        public string Id { get; }
        public string Description { get; }
        public ThreePointEstimate Duration { get; }
        public IReadOnlyCollection<string> Precedents { get; }
    }
}
