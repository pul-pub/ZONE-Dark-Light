using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GUIHandler handlerGUI;
    [SerializeField] private Player player;

    private void Awake()
    {
        handlerGUI.Initialization();
        player.Initialization();

        Destroy(gameObject);
    }
}
