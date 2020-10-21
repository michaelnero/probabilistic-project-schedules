using Planner.ObjectModel;

namespace Planner
{
    class Program
    {
        static void Main(string[] args)
        {
            var activities = new[]
            {
                new Activity("A", "A", (17, 29, 47)),
                new Activity("B", "B", (6, 12, 24), new[] { "A" }),
                new Activity("C", "C", (16, 19, 28), new[] { "A" }),
                new Activity("D", "D", (13, 16, 19), new[] { "B" }),
                new Activity("E", "E", (2, 5, 14), new[] { "C" }),
                new Activity("F", "F", (2, 5, 8), new[] { "D", "E" })
            };

            var network = new ProjectNetwork(activities);
            var simulator = new ProjectSimulator(network);

            var results = simulator.Simulate(100_000);
        }
    }
}