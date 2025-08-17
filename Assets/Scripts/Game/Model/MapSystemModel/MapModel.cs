using System.Collections.Generic;
using System;
using System.Linq;

namespace MapSystem.Model
{
	public class MapModel
	{
		public int Size { get; private set; }
		public IEnumerable<ICoordinate> AllPositions => _tiles.Keys;
		public IEnumerable<CellModel> AllCells => _tiles.Values;

		private Dictionary<ICoordinate, CellModel> _tiles;

		public MapModel(int size, Dictionary<ICoordinate, CellType> customTiles, IEnumerable<ICoordinate> allCoordinates)
		{
			Size = size;

			//allCoordinates.Count() is dictionary capacity
			_tiles = new Dictionary<ICoordinate, CellModel>(allCoordinates.Count());

			foreach (var pos in allCoordinates)
			{
                //customTiles.ContainsKey(pos) = if dictionary has key of this pos, returns true
				// if true it returns the value, if false it assigns default value
                CellType t = customTiles.ContainsKey(pos) ? customTiles[pos] : CellType.Grass;

				CellModel model;

				//creates CellModel for each wanted cell and adds them to list of cells
				model = new CellModel(pos, t);
				_tiles.Add(pos, model);
			}
		}

		public CellModel GetTile(ICoordinate pos)
		{
			//gets value based on key
			return _tiles.GetValueOrDefault(pos);
		}

		public CellModel FindTileOfType(CellType type)
		{
			//returns first item meeting the condition
			return _tiles.Values.FirstOrDefault(t => t.CellType == type);
		}

		public IEnumerable<CellModel> FindTilesOfType(CellType type)
		{
            //filters a collection based on a specified condition
            return _tiles.Values.Where(t => t.CellType == type);
		}
	}

}