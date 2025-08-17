using MapSystem.Model;
using System;
using System.Linq;
using UnityEngine;

public class MapPresenter : MonoBehaviour
{
	[SerializeField]
	private CellPresenter[] _cellPrefabs;

	public MapModel Model { get; private set; }
	[SerializeField]
	private MapDefinition _mapDefinition;


	public void Awake()
	{
		Model = new MapModel(_mapDefinition.Size, _mapDefinition.GetCustomCellTypes(), _mapDefinition.GetAllCoordinates());
		ICoordinateConverter converter = _mapDefinition.GetCoordinateConverter();

		if (_mapDefinition.IsPreSpawned) //prespawned -> link models to presenters
		{
			foreach (var cell in transform.GetComponentsInChildren<CellPresenter>())
			{
				ICoordinate pos = converter.ConvertVector3ToCoordinate(cell.transform.localPosition);
				cell.SetModel(Model.GetTile(pos));

			}
		}
		else //Not prespawned, we need to spawn the prefabs
		{
			foreach (var cell in Model.AllCells)
			{
				SpawnCell(cell, converter);
			}
		}
	}

	public void SpawnCell(CellModel model, ICoordinateConverter converter)
	{
		Vector3 position = converter.ConvertCoordinateToVector3(model.Position);

		GameObject prefab = _cellPrefabs.FirstOrDefault(p => p.CellType == model.CellType)?.gameObject;
		if (prefab != null)
		{
			GameObject instance = Instantiate(prefab, transform);
			instance.transform.localPosition = position;
			instance.name = $"{prefab.name}-{model.Position}";
		}

	}

}

