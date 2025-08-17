using UnityEngine;

public abstract class SceneSwitcher
{
    public static void SwitchScene(string sceneName)
    {
        // Check if the scene name is valid
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            return;
        }
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
