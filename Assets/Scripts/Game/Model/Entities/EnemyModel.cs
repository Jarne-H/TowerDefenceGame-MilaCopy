using MapSystem.Model;
using StatePattern;
using System;
using System.Collections.Generic;

namespace TowerDefense.Model
{
    public class EnemyModel : ModelBase//, IDamageable ???
    {
        private ICoordinate _position;

        public bool IsActive { get; set; } = true;

        private int _health;

        public StateMachine StateMachine { get; private set; }

        public EventHandler AttackGoal;

        public ICoordinate NextPosition
        {
            get => _position;
            set
            {
                if (_position == value) return;

                _position = value;
                OnPropertyChanged();
            }
        }

        public ICoordinate CurrentPosition { get; set; }

        public bool IsAllowedToMove => CurrentPosition == NextPosition;

        public EnemyModel(ICoordinate position, List<CellModel> pathToGoal, int health)
        {            
            NextPosition = position;
            CurrentPosition = position;
            _health = health;
            StateMachine = new StateMachine(new WalkingState(this, pathToGoal));
        }

        public void SelfDestruct()
        {
            IsActive = false;
            OnDestroySelf();
        }

        public void Attack()
        {
            OnAttackGoal();
        }

        protected virtual void OnAttackGoal()
        {
            AttackGoal?.Invoke(this, EventArgs.Empty);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                SelfDestruct();
            }
        }
    }
}
