using StatePattern;
using TowerDefense.View.UI;
using UnityEngine;


namespace TowerDefense.View.States
{
    public class PauseState : IState
    {
        private readonly GameStateSystem _stateSystem;
        private PauseUI _pauseUI;

        public PauseState(GameStateSystem stateSystem)
        {
            _stateSystem = stateSystem;
        }

        public void OnEnter()
        {
            _pauseUI = Object.FindFirstObjectByType<PauseUI>();

            _pauseUI.NewGameClicked += PauseUI_NewGameClicked;
            _pauseUI.MenuClicked += PauseUI_MenuClicked;

            _pauseUI.Show();
        }

        public void OnExit()
        {
            _pauseUI.NewGameClicked -= PauseUI_NewGameClicked;
            _pauseUI.MenuClicked -= PauseUI_MenuClicked;
            _pauseUI.Hide();
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _stateSystem.StateMachine.MoveToState(_stateSystem.GameState);
            }
        }

        private void PauseUI_MenuClicked(object sender, System.EventArgs e)
        {
            _stateSystem.StateMachine.MoveToState(_stateSystem.MenuState);
        }

        private void PauseUI_NewGameClicked(object sender, System.EventArgs e)
        {
            _stateSystem.StateMachine.MoveToState(_stateSystem.NewGameState);
        }
    }
}

