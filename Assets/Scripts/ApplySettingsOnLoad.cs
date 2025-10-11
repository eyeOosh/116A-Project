using UnityEngine;

public class ApplySettingsOnLoad : MonoBehaviour
{
    void Start()
    {
        if (SettingsManager.Instance != null)
        {
            // Apply saved volume
            AudioListener.volume = SettingsManager.Instance.volume;

            // Apply sensitivity to your FPS controller
            var player = FindObjectOfType<StarterAssets.FirstPersonController>();
            if (player != null)
                player.RotationSpeed = SettingsManager.Instance.sensitivity;
        }
    }
}
