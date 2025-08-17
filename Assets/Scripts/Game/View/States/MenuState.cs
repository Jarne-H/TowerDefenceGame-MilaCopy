using StatePattern;
using TowerDefense.View.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.View.States
{
    public class MenuState : IState
    {
        private readonly GameStateSystem _stateSystem;

        public MenuState(GameStateSystem stateSystem)
        {
            _stateSystem = stateSystem;
        }

        public void OnEnter()
        {
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Menu");
            sceneLoad.completed += SceneLoad_completed;
        }

        private void SceneLoad_completed(AsyncOperation obj)
        {
            Object.FindFirstObjectByType<MenuUI>().NewGameClicked += MenuUI_NewGameClicked;
        }

        private void MenuUI_NewGameClicked(object sender, System.EventArgs e)
        {
            _stateSystem.StateMachine.MoveToState(_stateSystem.GameState);
        }

        public void OnExit()
        {
            
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}

