using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string practiceRange = "MainScene";
    [SerializeField] private string robotRange = "RobotRange";
    [SerializeField] private string transition = "TransitionScreen";
    public void PlayGame()
    {
        // Load the next scene (arena)
        transitionScript.scene = practiceRange;
        SceneManager.LoadScene(transition);

        // Lock and hide the cursor for FPS gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayRobot()
    {
        transitionScript.scene = robotRange;
        SceneManager.LoadScene(transition);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game.");
        Application.Quit();
    }
}
