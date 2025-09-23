using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class RotationSpeedController : MonoBehaviour
{
    public FirstPersonController playerController; // your FPS controller
    public Slider rotationSlider; // your UI slider

    private bool sliderVisible = false;

    void Start()
    {
        rotationSlider.gameObject.SetActive(false);
        rotationSlider.value = playerController.RotationSpeed;
        rotationSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void Update()
    {
        #if ENABLE_INPUT_SYSTEM
        // Toggle slider on Tab press
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            sliderVisible = !sliderVisible;
            rotationSlider.gameObject.SetActive(sliderVisible);

            if (sliderVisible)
            {
                // Unlock and show the mouse cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Stop FPS camera rotation
                playerController.RotationSpeed = 0f;
            }
            else
            {
                // Lock and hide the cursor again
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Restore previous rotation speed from slider
                playerController.RotationSpeed = rotationSlider.value;
            }
        }
        #endif
    }

    void OnSliderChanged(float value)
    {
        if (!sliderVisible) return;
        playerController.RotationSpeed = value;
    }
}
