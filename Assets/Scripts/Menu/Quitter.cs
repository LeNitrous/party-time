using UnityEngine;

public class Quitter : MonoBehaviour
{
    public void Quit()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
}
