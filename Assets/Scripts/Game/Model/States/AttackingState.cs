

using StatePattern;
using System;
using TowerDefense.Model;

public class AttackingState : IState
{
    private float _attackingTime, _timer;
    private readonly EnemyModel _enemy;

    public AttackingState(EnemyModel attacker, float attackingTime)
    {
        _attackingTime = attackingTime;
        _enemy = attacker;
    }

    public void OnEnter()
    {
        _timer = _attackingTime;
    }

    public void OnExit()
    {
        _timer = 0;
    }

    public void Update(float deltaTime)
    {
        _timer += deltaTime;

        if (_timer >= _attackingTime)
        {
            _timer = 0;
            _enemy.Attack();
        }
    }
}
