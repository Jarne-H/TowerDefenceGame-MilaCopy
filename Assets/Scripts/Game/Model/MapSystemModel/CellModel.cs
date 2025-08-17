using System;
using TowerDefense.Model;

namespace MapSystem.Model
{
    public class CellModel
    {
        public ICoordinate Position { get; private set; }
        public CellType CellType { get; set; }
        public bool IsEmpty { get; private set; }

        public EventHandler WasClicked;
        public IPlaceableModel PlacedObject { get; private set; }

        public CellModel( ICoordinate pos, CellType type)
        {
            Position = pos;
            CellType = type;
            IsEmpty = true;
        }

        public void InformGameOfClick()
        {
            OnWasClicked();
        }

        protected virtual void OnWasClicked()
        {
            WasClicked.Invoke(this, EventArgs.Empty);
        }

        public void PlacePlaceableObjectModel(IPlaceableModel objectModel)
        {
            PlacedObject = objectModel;
            IsEmpty = false;
        }

        public void RemovePlacedObjectModel()
        {
            PlacedObject = null;
            IsEmpty = true;
        }
    }
}
