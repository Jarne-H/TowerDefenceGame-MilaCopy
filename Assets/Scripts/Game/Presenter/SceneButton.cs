using UnityEngine;
using UnityEngine.UIElements;

public class SceneButton : MonoBehaviour
{
    [SerializeField]
    private string ButtonName = "Button-MainMenu";
    [SerializeField]
    private string sceneName = "MainMenu";

    private void Start()
    {
        //find the button by name in UIdocument
        VisualElement element = GetComponent<UIDocument>().rootVisualElement.Q(ButtonName);

        //add SwitchScene method to the button click event
        if (element != null)
        {
            element.RegisterCallback<ClickEvent>(evt => SceneSwitcher.SwitchScene(sceneName));
        }
        else
        {
            Debug.LogWarning($"Button with name '{ButtonName}' not found in the UI document.");
        }
    }
}