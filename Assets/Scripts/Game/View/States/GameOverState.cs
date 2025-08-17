using StatePattern;
using TowerDefense.Presenter;
using TowerDefense.View.UI;
using UnityEngine;


namespace TowerDefense.View.States
{
    public class GameOverState : IState
    {
        private readonly GameStateSystem _stateSystem;
        private GamePresenter _gamePresenter;
        private GameOverUI _gameOverUI;

        public GameOverState(GameStateSystem stateSystem)
        {
            _stateSystem = stateSystem;
        }

        public void OnEnter()
        {
            FindGamePresenter();
            _gameOverUI = Object.FindFirstObjectByType<GameOverUI>();

            _gameOverUI.NewGameClicked += GameOverUI_NewGameClicked;
            _gameOverUI.MenuClicked += GameOverUI_MenuClicked;
            _gameOverUI.WatchReplayClicked += GameOverUI_WatchReplayClicked;

            _gameOverUI.Show();
        }

        public void OnExit()
        {
            _gameOverUI.NewGameClicked -= GameOverUI_NewGameClicked;
            _gameOverUI.MenuClicked -= GameOverUI_MenuClicked;
            _gameOverUI.WatchReplayClicked -= GameOverUI_WatchReplayClicked;
            _gameOverUI.Hide();
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _stateSystem.StateMachine.MoveToState(_stateSystem.MenuState);
            }
        }

        private void GameOverUI_MenuClicked(object sender, System.EventArgs e)
        {
            _stateSystem.StateMachine.MoveToState(_stateSystem.MenuState);
        }

        private void GameOverUI_NewGameClicked(object sender, System.EventArgs e)
        {
            _stateSystem.StateMachine.MoveToState(_stateSystem.NewGameState);
        }

        private void GameOverUI_WatchReplayClicked(object sender, System.EventArgs e)
        {
            _gamePresenter.StartReplay();
            _stateSystem.StateMachine.MoveToState(_stateSystem.NewGameState);
        }

        private void FindGamePresenter()
        {
            if (_gamePresenter == null)
            {
                _gamePresenter = GameObject.FindAnyObjectByType<GamePresenter>();

                if (_gamePresenter == null)
                {
                    Debug.LogError("GamePresenter not found");
                }
            }
        }
    }
}

