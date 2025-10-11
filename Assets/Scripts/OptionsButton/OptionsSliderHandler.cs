using UnityEngine;
using UnityEngine.UI;

public class OptionsSliderHandler : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    void Start()
    {
        // Initialize slider values from SettingsManager
        if (SettingsManager.Instance != null)
        {
            volumeSlider.value = SettingsManager.Instance.volume;
            sensitivitySlider.value = SettingsManager.Instance.sensitivity;
        }

        // Add listeners to update SettingsManager
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    void UpdateVolume(float value)
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetVolume(value);
    }

    void UpdateSensitivity(float value)
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetSensitivity(value);
    }
}
