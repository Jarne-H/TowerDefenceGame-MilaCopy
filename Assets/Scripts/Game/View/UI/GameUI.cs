using System;
using TowerDefense.Model;
using TowerDefense.Presenter;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefense.View.UI
{
    [RequireComponent(typeof(UIDocument))]

    public class GameUI : MonoBehaviour
    {
        public event EventHandler NewGameClicked, MenuClicked;
        private UIDocument _document;

        private GamePresenter _gamePresenter;

        [SerializeField]
        private GoalPresenter _goalPresenter;

        private Label _healthText, _enemiesText;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();

            _healthText = _document.rootVisualElement.Q<Label>("HealthText");
            _healthText.text = _goalPresenter.Model.Health.ToString();

            _enemiesText = _document.rootVisualElement.Q<Label>("EnemiesText");

            //find goal presenter in scene
            _goalPresenter = FindFirstObjectByType<GoalPresenter>();

        }

        private void Update()
        {
            if (_goalPresenter != null)
            {
                _healthText.text = $"Goal Health: {_goalPresenter.Model.Health}";
                //enemiesText.text = $"Enemies: {_goalPresenter.Model.Enemies.Count}";
            }
            else
            {
                _goalPresenter = FindFirstObjectByType<GoalPresenter>();
            }
        }

        //method to attach to GameModel event of goal
        public void GoalGotAttacked(object sender, GoalModelEventArgs e)
        {
            _healthText.text = $"Goal Health: {e.GoalModel.Health}";
        }

    }
}
