using System;
using System.Collections.Generic;

namespace IdentityProvider.Infrastructure.LatestAdditions
{
    public static class GraphHeper
    {
        /// <summary>
        ///     Sorts a directed acyclic graph...
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dependencies">Dependency: Item2 depends on Item1.</param>
        public static void TopologicalSort<T>(List<T> list, IEnumerable<SortDependency<T>> dependencies)
        {
            var dependenciesByDependent = new Dictionary<T, List<T>>();

            foreach (var relation in dependencies)
            {
                List<T> group;

                if (!dependenciesByDependent.TryGetValue(relation.Dependency, out group))
                {
                    group = new List<T>();
                    dependenciesByDependent.Add(relation.Dependency, group);
                }

                group.Add(relation.Key);
            }

            var result = new List<T>();
            var givenList = new HashSet<T>(list);
            var processed = new HashSet<T>();
            var analysisStarted = new List<T>();

            foreach (var element in list)
                AddDependenciesBeforeElement(element, result, givenList, dependenciesByDependent, processed,
                    analysisStarted);

            list.Clear();

            list.AddRange(result);
        }

        private static void AddDependenciesBeforeElement<T>(T element, List<T> result, HashSet<T> givenList,
            Dictionary<T, List<T>> dependencies, HashSet<T> processed, List<T> analysisStarted)
        {
            if (!processed.Contains(element) && givenList.Contains(element))
            {
                if (analysisStarted.Contains(element))
                {
                    var circularReferenceIndex = analysisStarted.IndexOf(element);
                    throw new Exception(
                        $"Circular dependency detected on elements:\r\n{string.Join(",\r\n", analysisStarted.GetRange(circularReferenceIndex, analysisStarted.Count - circularReferenceIndex))}.");
                }

                analysisStarted.Add(element);

                if (dependencies.ContainsKey(element))
                    foreach (var dependency in dependencies[element])
                        AddDependenciesBeforeElement(dependency, result, givenList, dependencies, processed,
                            analysisStarted);

                analysisStarted.RemoveAt(analysisStarted.Count - 1);

                processed.Add(element);

                result.Add(element);
            }
        }

        public class SortDependency<T>
        {
            public T Key { get; set; }
            public T Dependency { get; set; }
        }
    }
}