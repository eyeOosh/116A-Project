using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsBackButton : MonoBehaviour
{
    // Call this method on the Button's OnClick event
    public void GoBackToMainMenu()
    {
        // Save current settings
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SaveSettings();
        }
        else
        {
            Debug.LogWarning("SettingsManager not found! Settings not saved.");
        }

        // Unlock and show the cursor before loading the Main Menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load Main Menu scene by name (make sure scene is in Build Settings)
        SceneManager.LoadScene("MainMenuScene");
    }
}
