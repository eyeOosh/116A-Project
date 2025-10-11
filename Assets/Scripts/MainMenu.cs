using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the next scene (arena)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Lock and hide the cursor for FPS gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game.");
        Application.Quit();
    }
}
