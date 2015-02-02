using System;
using System.Collections.Generic;

namespace SpaceChemSolver.Simulation
{
    public static class Algorithms
    {
        /// <summary>
        /// Performs the breadth first search algorithm.
        /// </summary>
        /// <typeparam name="T">the node type</typeparam>
        /// <param name="start">the start node</param>
        /// <param name="neighbors">method to get neighboring nodes</param>
        /// <returns>all nodes reachable from the start node</returns>
        public static HashSet<T> BFS<T>(T start, Func<T, IEnumerable<T>> neighbors)
        {
            var queue = new Queue<T>();
            var visited = new HashSet<T>();
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();

                foreach(var neighbor in neighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }

        /// <summary>
        /// Checks if two lists are pair-wise equivalent.
        /// </summary>
        /// <typeparam name="T">the list element type</typeparam>
        /// <param name="list1">the first list</param>
        /// <param name="list2">the second list</param>
        /// <returns>true if the lists are pair-wise equivalent, false otherwise</returns>
        public static bool ListEquals<T>(IList<T> list1, IList<T> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
