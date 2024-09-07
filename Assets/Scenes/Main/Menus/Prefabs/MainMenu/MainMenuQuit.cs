#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class MainMenuQuit : MonoBehaviour
{
    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
