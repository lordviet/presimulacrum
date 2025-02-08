using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    [SerializeField] InputAction quit;

    private void OnEnable()
    {
        quit.Enable();
    }

    void FixedUpdate()
    {
        if (quit.IsPressed())
        {
            Debug.Log("Quitting game now!");
            Application.Quit();
        }
    }
}
