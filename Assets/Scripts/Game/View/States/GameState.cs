using StatePattern;
using System;
using TowerDefense.Presenter;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace TowerDefense.View.States
{
    public class GameState : IState
    {
        private readonly GameStateSystem _stateSystem;
        private UIDocument _gameUI;

        public GamePresenter GamePresenter { get; private set; }

        public GameState(GameStateSystem stateSystem)
        {
            _stateSystem = stateSystem;
        }

        public void OnEnter()
        {
            if (SceneManager.GetActiveScene().name != "Game")
                SceneManager.LoadScene("Game");            
        }

        public void OnExit()
        {
            GamePresenter = null;
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _stateSystem.StateMachine.MoveToState(_stateSystem.PauseState);
            }

            FindGamePresenter();
            CheckIfGameOver();
            UpdateGameUI(deltaTime);
        }

        private void UpdateGameUI(float deltaTime)
        {
            FindGameUI();
            UpdateGameUI();            
        }

        private void UpdateGameUI()
        {
            if (GamePresenter == null) return;
            if (!GamePresenter.IsGameOver)
            {
                _gameUI.rootVisualElement.Q<Label>("HealthText").text
                = $"Goal health: {GamePresenter.Model.Goal.Health}";

                _gameUI.rootVisualElement.Q<Label>("EnemiesText").text
                    = $"Enemies: {GamePresenter.Model.Enemies.Count}";
            }            
        }

        private void FindGameUI()
        {
            if (!_gameUI)
            {
                var ui = GameObject.Find("GameUI");
                _gameUI = ui.GetComponent<UIDocument>();
            }
        }

        private void CheckIfGameOver()
        {
            if (GamePresenter.IsGameOver)
            {
                _stateSystem.StateMachine.MoveToState(_stateSystem.GameOverState);
            }
        }

        private void FindGamePresenter()
        {
            if (GamePresenter == null)
            {
                GamePresenter = GameObject.FindAnyObjectByType<GamePresenter>();

                if (GamePresenter == null)
                {
                    Debug.LogError("GamePresenter not found");
                }
            }
        }
    }
}

