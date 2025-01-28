using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 75;
    }

    public void Exit() => Application.Quit();
}
