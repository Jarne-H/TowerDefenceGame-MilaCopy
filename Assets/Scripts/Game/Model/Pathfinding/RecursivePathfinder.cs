using System.Collections.Generic;
using MapSystem.Model;

namespace Pathfinding
{
    public class RecursivePathfinder<T> : IPathfindingStrategy<T> where T : CellModel
    {
        private readonly IEnumerable<T> _allRoadCells;

        public RecursivePathfinder(IEnumerable<T> allRoadCells)
        {
            _allRoadCells = allRoadCells;
        } 

        public List<T> FindPath(T start, T end)
        {
            List<T> path = new List<T>();
            FindPathRecursive(start, end, path);
            return path;
        }

        public bool FindPathRecursive(T current, T end, List<T> path)
        {            
            path.Add(current);

            if (current.Position.IsAdjacentTo(end.Position))
            {
                return true;
            }

            foreach (T cell in _allRoadCells)
            {
                if (cell.Position.IsAdjacentTo(current.Position) && !path.Contains(cell)) 
                {
                    if (FindPathRecursive(cell, end, path))
                    {
                        return true;
                    }
                }
            }

            path.Remove(current);
            return false;
        }
    }
}
