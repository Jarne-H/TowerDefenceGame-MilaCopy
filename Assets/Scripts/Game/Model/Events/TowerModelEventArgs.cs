using System;

namespace TowerDefense.Model
{
    public class TowerModelEventArgs : EventArgs
    {
        public TowerModel TowerModel { get; }

        public TowerModelEventArgs(TowerModel towerModel)
        {
            TowerModel = towerModel;
        }
    }
}
