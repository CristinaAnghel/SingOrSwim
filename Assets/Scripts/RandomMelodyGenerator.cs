using System.Runtime.CompilerServices;
using UnityEngine;

public class MelodyNote
{
    private string noteName;
    private float noteOctave;

    public MelodyNote(string name, float octave)
    {
        noteName = name;
        noteOctave = octave;
    }

    public string GetNoteName()
    {
        return noteName;
    }

    public float GetNoteOctave()
    {
        return noteOctave;
    }

    public void SetNoteName(string name)
    {
        noteName = name;
    }

    public void SetNoteOctave(float octave)
    {
        noteOctave = octave;
    }
}

public class RandomMelodyGenerator : MonoBehaviour
{
    private string[] noteNames;
    private float[] octaves;
    private int melodyLength;
    private MelodyNote[] melodyNotes;

    public void Start()
    {
        noteNames = new string[]  { "Do", "Re", "Mi", "Fa", "Sol", "La", "Si" };
        octaves = new float[]  { 0.5f, 1f, 2f };
    }


    // generates a melody with random notes and octaves
    public void GenerateRandomMelodyNote()
    {
        for(int i = 0; i < melodyLength; i++)
        {
            string randomNoteName = noteNames[Random.Range(0, noteNames.Length)];
            float randomOctave = octaves[Random.Range(0, octaves.Length)];
            MelodyNote note = new MelodyNote(randomNoteName, randomOctave);
            melodyNotes[i] = note;
        }
    }


    public void SetMelodyLength(int length)
    {
        melodyLength = length;
        melodyNotes = new MelodyNote[melodyLength];
    }

    public MelodyNote[] GetMelodyNotes()
    {
        return melodyNotes;
    }
}
