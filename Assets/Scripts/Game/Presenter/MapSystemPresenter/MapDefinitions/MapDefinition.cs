using MapSystem.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class MapDefinition : MonoBehaviour
{
	public abstract int Size { get; }
	public abstract bool IsPreSpawned { get; } 
	public abstract CellType DefaultCellType { get; }

	//specifies methods needed in subclasses
	public abstract IEnumerable<ICoordinate> GetAllCoordinates();
	public abstract Dictionary<ICoordinate, CellType> GetCustomCellTypes();
	public abstract ICoordinateConverter GetCoordinateConverter();
}
