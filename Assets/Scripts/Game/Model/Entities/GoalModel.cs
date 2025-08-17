

using System;
using System.Diagnostics;

namespace TowerDefense.Model
{
    public class GoalModel : ModelBase, IDamageable
    {
        public int Health { get; private set; }
        public EventHandler GameOver;

        public GoalModel(int health)
        {
            Health = health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;            
            OnPropertyChanged();

            if (Health <= 0)
            {
                OnGameOver();
            }
        }        

        protected virtual void OnGameOver()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }
    }
}
