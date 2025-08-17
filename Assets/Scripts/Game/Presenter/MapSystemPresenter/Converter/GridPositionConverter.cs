
using MapSystem.Model;
using UnityEngine;

public class GridPositionConverter : ICoordinateConverter
{
	private readonly float _cellSize;
	private readonly int _gridSize;

	public GridPositionConverter(int gridSize, float cellSize =1)
	{
		_gridSize = gridSize;
		_cellSize = cellSize;
	}

	public Vector3 ConvertCoordinateToVector3(ICoordinate coordinate)
	{
		GridPosition gridCoord = (GridPosition)coordinate;


		float halfGridOffset = -0.5f * _gridSize*_cellSize;


		float x = (gridCoord.Column+0.5f) * _cellSize;
		float z = (gridCoord.Row+0.5f) * _cellSize;

		Vector3 position = new Vector3(x, 0f, z);
		position.x += halfGridOffset;
		position.z += halfGridOffset;
		return position;
	}

	public ICoordinate ConvertVector3ToCoordinate(Vector3 position)
	{
		float halfGridOffset = -0.5f * _gridSize * _cellSize;
		position.x -= halfGridOffset;
		position.z -= halfGridOffset;


		int col = Mathf.RoundToInt(position.x / _cellSize - 0.5f);
		int row = Mathf.RoundToInt(position.z / _cellSize - 0.5f);
		return new GridPosition(col, row);
	}
}
