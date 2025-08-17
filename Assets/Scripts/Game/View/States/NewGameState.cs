using StatePattern;
using TowerDefense.Presenter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.View.States
{
    public class NewGameState : IState
    {
        private readonly GameStateSystem _stateSystem;

        public NewGameState(GameStateSystem stateSystem)
        {
            _stateSystem = stateSystem;
        }

        public void OnEnter()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Game");
            loadOperation.completed += (op) => _stateSystem.StateMachine.MoveToState(_stateSystem.GameState);
        }

        public void OnExit()
        {
            
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}

