using MapSystem.Model;
using StatePattern;
using System;
using System.Collections.Generic;

namespace TowerDefense.Model
{
    public class TowerModel : ModelBase, IPlaceableModel
    {
        public CellModel CellBelow { get; set; }
        public List<CellModel> WatchedCells { get; private set; }

        private float _timer;
        private int _shotsFired, _coolDownTime = 2;

        public EventHandler<EnemyModelEventArgs> TowerShot;

        private List<EnemyModel> _enemies;

        public int ShotsFired
        {
            get => _shotsFired;
            set
            {
                if (_shotsFired == value) return;

                _shotsFired = value;
                OnPropertyChanged();
            }
        }

        public TowerModel(CellModel cellBelow, List<CellModel> roadCells, List<EnemyModel> enemies)
        {
            CellBelow = cellBelow;
            WatchedCells = new List<CellModel>();
            _enemies = enemies;

            foreach (var cell in roadCells)
            {
                if (CellBelow.Position.IsAdjacentTo(cell.Position))
                {
                    WatchedCells.Add(cell);
                }
            }
        }

        public void Update(float deltaTime)
        {
            WatchForEnemies(deltaTime);            
        }

        private void WatchForEnemies(float deltaTime)
        {
            _timer += deltaTime;

            foreach (var cell in WatchedCells)
            {
                foreach (var enemy in _enemies)
                {
                    if ( enemy.IsActive && cell.Position == enemy.CurrentPosition)
                    {
                        if (_timer >= _coolDownTime)
                        {
                            Attack(enemy);
                            _timer = 0;
                            return;
                        }
                    }
                }
            }
        }

        protected virtual void OnTowerShot(EnemyModel enemy)
        {
            TowerShot?.Invoke(this, new EnemyModelEventArgs(enemy));
        }

        public void DestroyPresenter()
        {
            OnDestroySelf();
        }

        public void Attack(EnemyModel target)
        {
            if (target is EnemyModel enemy)
            {
                ShotsFired++;
                OnTowerShot(enemy);
            }
        }
    }
}
