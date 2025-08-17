using MapSystem.Model;
using System.Collections.Generic;
using UnityEngine;

public class GridMapDefinition: MapDefinition
{
	[SerializeField] private int _size;
	[SerializeField] private CellType _defaultCellType;

	[SerializeField] private GridPosition[] _roadCoordinates;


	public override int Size => _size;
	public override CellType DefaultCellType => _defaultCellType;

	public override bool IsPreSpawned => false;

	public override IEnumerable<ICoordinate> GetAllCoordinates()
	{
		List<ICoordinate> coordinates = new List<ICoordinate>();

		for (int column = 0; column < _size; ++column)
		{
			for (int row = 0; row < _size; ++row)
			{
				coordinates.Add(new GridPosition(column, row));
			}
		}
		return coordinates;
	}

	public override ICoordinateConverter GetCoordinateConverter()
	{
		return new GridPositionConverter(_size);
	}

	public override Dictionary<ICoordinate, CellType> GetCustomCellTypes()
	{
		Dictionary<ICoordinate, CellType> customCellTypes = new Dictionary<ICoordinate, CellType>();
		foreach (var coord in _roadCoordinates)
		{
			customCellTypes.Add(coord, CellType.Road);
		}
		return customCellTypes;

	}
}
