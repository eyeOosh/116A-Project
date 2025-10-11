using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuInitializer : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    void OnEnable()
    {
        if (SettingsManager.Instance != null)
        {
            // Set sliders to saved values
            volumeSlider.value = SettingsManager.Instance.volume;
            sensitivitySlider.value = SettingsManager.Instance.sensitivity;
        }
    }
}
