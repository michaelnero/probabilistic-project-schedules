using System;
using System.Collections.Generic;

namespace Planner
{
    // https://github.com/PrismLibrary/Prism/blob/master/src/Prism.Core/Modularity/ModuleDependencySolver.cs

    public static class DependencySolver
    {
        public static IReadOnlyList<string> Solve(DependencyMatrix matrix)
        {
            var skip = new List<string>();

            while (skip.Count < matrix.Count)
            {
                var leaves = FindLeaves(matrix, skip);

                if ((0 == leaves.Count) && (skip.Count < matrix.Count))
                {
                    throw new Exception("Cyclic dependency found.");
                }

                skip.AddRange(leaves);
            }

            skip.Reverse();

            if (skip.Count > matrix.Count)
            {
                throw new Exception("Dependencies exist for items not known to the solver.");
            }

            return skip;
        }

        private static List<string> FindLeaves(DependencyMatrix matrix, List<string> skip)
        {
            var result = new List<string>();

            foreach (var precedent in matrix.Keys)
            {
                if (skip.Contains(precedent))
                {
                    continue;
                }

                var count = 0;

                foreach (var dependent in matrix[precedent])
                {
                    if (skip.Contains(dependent))
                    {
                        continue;
                    }

                    count++;
                }

                if (0 == count)
                {
                    result.Add(precedent);
                }
            }

            return result;
        }
    }
}