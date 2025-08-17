using MapSystem.Model;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MapPresenter))]
public class HexMapDefinition : MapDefinition
{
	[SerializeField] private int _size;
	[SerializeField] private CellType _defaultCellType;

	//stores custom cells
	[SerializeField] private HexCellDefinition[] _customCells = new HexCellDefinition[0];

    //whenever Size is accessed, it returns the current value of _size
    public override int Size => _size;
	public override CellType DefaultCellType => _defaultCellType;
	

	//Hex map is prespawned using custom inspector
	public override bool IsPreSpawned => true;

	public override IEnumerable<ICoordinate> GetAllCoordinates()
	{
		List<ICoordinate> coordinates = new List<ICoordinate>();

        //loop iterates over all possible q values from -size to size
        for (int q = -_size; q <= _size; ++q)
		{			
			for(int r = -_size; r <= _size; ++r)
			{
				//q + r + s = 0
				int s = -q - r;
				//checks if within boundary of size
				if (Mathf.Abs(s) > _size) 
					continue;

				coordinates.Add(new HexPosition(q, r, s));
			}
		}
		return coordinates;
	}

	public override ICoordinateConverter GetCoordinateConverter()
	{
		return new HexPositionConverter();
	}

	public override Dictionary<ICoordinate, CellType> GetCustomCellTypes()
	{
		Dictionary<ICoordinate, CellType> customCellTypes = new Dictionary<ICoordinate, CellType>();
		foreach(var tileDefinition in _customCells)
		{
			customCellTypes.Add(tileDefinition.HexPosition, tileDefinition.CellType);
		}
		return customCellTypes;
		
	}

}
