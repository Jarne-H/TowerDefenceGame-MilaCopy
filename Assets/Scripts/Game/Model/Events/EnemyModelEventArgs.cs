using System;
using System.Collections.Generic;

namespace TowerDefense.Model
{
    public class EnemyModelEventArgs : EventArgs
    {
        public EnemyModel Enemy { get; }

        public EnemyModelEventArgs(EnemyModel enemy)
        {
            Enemy = enemy;
        }
    }
}
