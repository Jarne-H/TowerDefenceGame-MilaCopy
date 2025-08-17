using MapSystem.Model;
using NUnit.Framework;
using StatePattern;
using System.Collections.Generic;

namespace TowerDefense.Model
{
    public class WalkingState : IState
    {
        private readonly EnemyModel _enemy;
        private List<CellModel> _pathToGoal;

        public WalkingState(EnemyModel enemy, List<CellModel> pathToGoal)
        {
            _enemy = enemy;
            _pathToGoal = pathToGoal;
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }

        public void Update(float deltaTime)
        {
            if (_pathToGoal.Count > 1 && _enemy.NextPosition == _pathToGoal[0].Position)
            {
                _enemy.NextPosition = _pathToGoal[1].Position;
                _pathToGoal.RemoveAt(0);
            }
            else if (_pathToGoal.Count == 1 && _enemy.NextPosition == _pathToGoal[0].Position)
            {
                _enemy.StateMachine.MoveToState(new AttackingState(_enemy, 1)); //change the 1?
            }
        }
    }
}
