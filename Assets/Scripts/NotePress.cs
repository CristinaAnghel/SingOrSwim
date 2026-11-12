using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotePress : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider pitchSlider;


    void Start()
    {
        pitchSlider.gameObject.SetActive(false);
    }

    public void IncreasePitch()
    {
        audioSource.pitch = 1f;
        audioSource.Play();

        audioMixer.SetFloat("MyExposedPitch", pitchSlider.value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IncreasePitch();
        
    }

}
