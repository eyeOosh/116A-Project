using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // For new Input System

public class ReturnToMainMenuOnTab : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            // Unlock and show the cursor before loading the menu
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Load the Main Menu scene (by name)
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
