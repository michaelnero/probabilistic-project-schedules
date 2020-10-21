using System.Collections.Generic;

namespace Planner
{
    // https://github.com/PrismLibrary/Prism/blob/master/src/Prism.Core/Modularity/ModuleDependencySolver.cs

    public class DependencyMatrix
    {
        private readonly Dictionary<string, HashSet<string>> _matrix;

        public DependencyMatrix()
        {
            _matrix = new Dictionary<string, HashSet<string>>();
        }

        public IReadOnlyCollection<string> this[string key] => _matrix[key];
        public IEnumerable<string> Keys => _matrix.Keys;
        public int Count => _matrix.Count;

        public void Add(string item, IEnumerable<string> precedents)
        {
            if (!_matrix.ContainsKey(item))
            {
                _matrix.Add(item, new HashSet<string>());
            }

            foreach (var predecessor in precedents)
            {
                if (!_matrix.TryGetValue(predecessor, out var set))
                {
                    _matrix[predecessor] = set = new HashSet<string>();
                }

                set.Add(item);
            }
        }
    }
}