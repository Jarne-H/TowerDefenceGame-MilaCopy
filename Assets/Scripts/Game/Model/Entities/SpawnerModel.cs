using MapSystem.Model;
using System;
using System.Collections.Generic;

namespace TowerDefense.Model
{
    public class SpawnerModel : ModelBase
    {
        public ICoordinate Position { get; }
        public CellModel Cell { get; }

        public EventHandler<EnemyModelEventArgs> CreatedEnemy;

        public List<EnemyModel> Enemies { get; set; } = new List<EnemyModel>();

        private int _enemyHealth;

        public SpawnerModel(CellModel spawnCell, int enemyHealth, int enemyDamage)
        {
            Position = spawnCell.Position;
            Cell = spawnCell;
            _enemyHealth = enemyHealth;
        }

        public void CreateEnemyModel(List<CellModel> pathToGoal)
        {            
            EnemyModel enemy = new EnemyModel(Cell.Position, new List<CellModel>(pathToGoal), _enemyHealth);
            Enemies.Add(enemy);
            OnCreatedEnemy(enemy);
        }

        protected virtual void OnCreatedEnemy(EnemyModel enemy)
        {
            CreatedEnemy?.Invoke(this, new EnemyModelEventArgs(enemy));
        }
    }
}
