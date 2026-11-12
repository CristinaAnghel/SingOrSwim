using UnityEngine;
using UnityEngine.UI;

public class SliderSnap : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // snaps the slider to the specified values
    void Update()
    {
        if (slider.value < 1.0f)
        {
            slider.value = 0.5f;
        }
        else if (slider.value >= 1.0f && slider.value < 1.5f)
        {
            slider.value = 1.0f;
        }
        else if (slider.value >= 1.5f && slider.value < 2.0f)
        {
            slider.value = 2.0f;
        }
    }
}
