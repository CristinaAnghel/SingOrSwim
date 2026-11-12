using UnityEngine;
using UnityEngine.EventSystems;

public class NoteDropSlot : MonoBehaviour
{
    public string noteName;
    public float noteOctave;
    public bool isOccupied;
    public AudioSource audioSource;

    public void Start()
    {
        ResetData();
    }

    public string GetNoteName()
    {
        return noteName;
    }

    public void SetNoteName(string name)
    {
        noteName = name;
    }

    public float GetNoteOctave()
    {
        return noteOctave;
    }

    public void SetNoteOctave(float octave)
    {
        noteOctave = octave;
    }

    public bool GetIsOccupied()
    {
        return isOccupied;
    }

    public void SetIsOccupied(bool occupied)
    {
        isOccupied = occupied;
    }

    public void SetAudioSource(AudioSource source)
    {
        audioSource = source;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void ResetData()
    {
        noteName = "";
        noteOctave = -1f;
        isOccupied = false;
        audioSource = null;
    }
}
