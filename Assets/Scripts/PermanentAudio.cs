using UnityEngine;

public class PermanentAudio : MonoBehaviour
{
    public static PermanentAudio instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (!musicSource.isPlaying) musicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
