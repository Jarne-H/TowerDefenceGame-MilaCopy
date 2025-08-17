

using System.Collections.Generic;

public interface IPathfindingStrategy<T>
{
    List<T> FindPath(T start, T end);
}
