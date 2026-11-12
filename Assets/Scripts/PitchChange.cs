using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PitchChange : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioMixer audioMixer;
    [SerializeField] private Slider pitchSlider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void IncreasePitch()
    {
        audioSource.pitch = 1f;
        audioSource.Play();

        audioMixer.SetFloat("MyExposedPitch", pitchSlider.value);
    }

}
