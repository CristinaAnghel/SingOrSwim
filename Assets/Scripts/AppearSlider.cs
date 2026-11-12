using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AppearSlider : MonoBehaviour, IPointerDownHandler
{
    private bool isSliderActive;
    [SerializeField] private Slider pitchSlider;
    [SerializeField] private GameObject sliderBG;
    [SerializeField] private AudioClip arrowPress;
    void Start()
    {
        isSliderActive = false;
        pitchSlider.gameObject.SetActive(false);
        sliderBG.SetActive(false);
    }


    public void CloseSlider()
    {
        pitchSlider.gameObject.SetActive(false);
        sliderBG.SetActive(false);
        isSliderActive = false;
    }

    // appear slider
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isSliderActive)
        {
            PermanentAudio.instance.PlaySFX(arrowPress);
            pitchSlider.gameObject.SetActive(true);
            sliderBG.SetActive(true);
            isSliderActive = true;
        }
        else
        {
            PermanentAudio.instance.PlaySFX(arrowPress);
            CloseSlider();
        }
    }

}
