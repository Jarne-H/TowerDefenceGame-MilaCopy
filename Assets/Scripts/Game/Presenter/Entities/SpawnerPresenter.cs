using System;
using System.Collections.Generic;
using System.ComponentModel;
using TowerDefense.Model;
using UnityEngine;

namespace TowerDefense.Presenter
{
    public class SpawnerPresenter : PresenterBase<SpawnerModel>
    {
        private SpawnerModel _model;
        private HexPositionConverter _hexConverter = new HexPositionConverter();

        public GameObject EnemyPrefab { get; set; }

        public List<EnemyPresenter> EnemyPresenters { get; set; } = new List<EnemyPresenter>();

        public new SpawnerModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                _model.CreatedEnemy += Created_Enemy;
            }
        }

        private void SpawnEnemy(EnemyModel model)
        {
            var spawnPos = _hexConverter.ConvertCoordinateToVector3(Model.Position);

            var enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);

            if (enemy.TryGetComponent<EnemyPresenter>(out EnemyPresenter presenter))
            {
                presenter.Model = model;
                model.AttackGoal += presenter.Attack_Goal;
                EnemyPresenters.Add(presenter);
            }
        }

        protected virtual void Created_Enemy(object sender, EnemyModelEventArgs e)
        {
            SpawnEnemy(e.Enemy);
        }

        protected override void Destroy_Self(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
