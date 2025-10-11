using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 10f)] public float sensitivity = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("Volume", 1f);
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 5f);
    }

    public void SetVolume(float value)
    {
        volume = value;
        AudioListener.volume = volume;
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
    }
}
