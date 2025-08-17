using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefense.View.UI
{
    [RequireComponent(typeof(UIDocument))]

    public class MenuUI : MonoBehaviour
    {
        public event EventHandler NewGameClicked;
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            Button newGameBtn = _document.rootVisualElement.Q<Button>("NewGameButton");
            newGameBtn.clicked += NewGameBtn_clicked;
        }

        private void NewGameBtn_clicked()
        {
            NewGameClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
