using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompareMelodies : MonoBehaviour
{
    [SerializeField] private RandomMelodyGenerator melodyGenerator;
    
    [SerializeField] private NoteDropSlot[] slots;
    [SerializeField] private TMP_Text[] scoreText;
    private int[] occupiedSlots;
    private int currentPlayedSlot;
    private int foundAllNotesCounter;

    [SerializeField] private Animator[] scoreAnimator;
    [SerializeField] private TMP_Text finalAccuracy;
    private MelodyNote[] mermaidMelody;
    private MelodyNote[] pirateMelody;
    private float melodyAccuracyScore;
    private int melodyAddedScore;

    [SerializeField] private AudioSource[] mermaidSource;
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource[] pirateSource;
    private float timeBetweenNotes;
    private float timeBetweenRounds;

    AudioSource temporaryNote;

    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button singButton;

    [SerializeField] private Animator[] pirateHearts;
    [SerializeField] private Animator[] mermaidHearts;
    private int pirateTimesWon;
    private int mermaidTimesWon;

    [SerializeField] private Animator mermaidAnimator;
    [SerializeField] private Animator pirateAnimator;

    [SerializeField] private GameObject currentBackground;
    [SerializeField] private Sprite[] backgroundOptions;
    private int currentBackgroundIndex;

    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioClip breakHeartSound;
    [SerializeField] private AudioClip buttonPress;

    [SerializeField] private GameObject[] noteColliders;
    public void Awake()
    {
        melodyAccuracyScore = 0;
        melodyAddedScore = 0;
        pirateTimesWon = 0;
        mermaidTimesWon = 0;
        timeBetweenNotes = 1.5f;
        currentBackground.GetComponent<SpriteRenderer>().sprite = backgroundOptions[0];
        currentBackgroundIndex = 0;
        for(int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].enabled = false;
        }
        occupiedSlots = new int[slots.Length];
        currentPlayedSlot = -1;
        foundAllNotesCounter = 0;
        finalAccuracy.enabled = false;
    }


    public void Start()
    {
        StartRound();
    }


    public void GetPirateMelody()
    {
        MelodyNote[] bufferPirateMelody = new MelodyNote[slots.Length];
        AudioSource[] bufferPirateSource = new AudioSource[slots.Length];
        pirateSource = new AudioSource[slots.Length];

        // checks the occupied slots to get the pirate melody and marks the occupied slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetIsOccupied())
            {
                MelodyNote note = new MelodyNote(slots[i].GetNoteName(), slots[i].GetNoteOctave());
                bufferPirateMelody[i] = note;
                bufferPirateSource[i] = slots[i].GetAudioSource();
                occupiedSlots[i] = 1;
            }
        }

        int actualMelodyLength = 0;

        for (int i = 0; i < bufferPirateMelody.Length; i++)
        {
            if(bufferPirateMelody[i] != null)
            {
                actualMelodyLength++;
            }
        }

        pirateMelody = new MelodyNote[actualMelodyLength];
        int pirateMelodyIndex = 0;

        for (int i = 0; i < bufferPirateMelody.Length; i++)
        {
            if (bufferPirateMelody[i] != null)
            {
                pirateMelody[pirateMelodyIndex] = bufferPirateMelody[i];
                pirateSource[pirateMelodyIndex] = bufferPirateSource[i];
                pirateMelodyIndex++;
            }
        }

    }


    // checks if the note is adjacent to the reference note in the musical scale
    public bool IsAdjacentNote(MelodyNote note, MelodyNote refNote)
    {
        if(refNote.GetNoteName() == "Do" && (note.GetNoteName() == "Re" || note.GetNoteName() == "Si"))
        {
            return true;
        }
        else if (refNote.GetNoteName() == "Re" && (note.GetNoteName() == "Mi" || note.GetNoteName() == "Do"))
        {
            return true;
        }
        else if (refNote.GetNoteName() == "Mi" && (note.GetNoteName() == "Fa" || note.GetNoteName() == "Re"))
        {
            return true;
        }
        else if (refNote.GetNoteName() == "Fa" && (note.GetNoteName() == "Sol" || note.GetNoteName() == "Mi"))
        {
            return true;
        }
        else if (refNote.GetNoteName() == "Sol" && (note.GetNoteName() == "La" || note.GetNoteName() == "Fa"))
        {
            return true;
        }
        else if (refNote.GetNoteName() == "La" && (note.GetNoteName() == "Si" || note.GetNoteName() == "Sol"))
        {
            return true;
        }
        else if (refNote.GetNoteName() == "Si" && (note.GetNoteName() == "Do" || note.GetNoteName() == "La"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private int GetNextOccupiedSlot()
    {
        bool foundNext = false;
        for (int i = currentPlayedSlot + 1; i < occupiedSlots.Length && !foundNext; i++)
        {
            if(occupiedSlots[i] == 1)
            {
                currentPlayedSlot = i;
                foundAllNotesCounter++;
                return i;
            }
        }
        return -1;
    }


    public void DeactivateColliders()
    {
        for(int i = 0; i < noteColliders.Length; i++)
        {
            noteColliders[i].GetComponent<Collider2D>().enabled = false;
        }
    }


    public void ActivateColliders()
    {
        for (int i = 0; i < noteColliders.Length; i++)
        {
            noteColliders[i].GetComponent<Collider2D>().enabled = true;
        }
    }


    public void Compare()
    {
        int length = mermaidMelody.Length;

        if (mermaidMelody.Length < pirateMelody.Length)
        {
            melodyAddedScore -= (pirateMelody.Length - mermaidMelody.Length) * 5;
            length = mermaidMelody.Length;
        }

        if (mermaidMelody.Length > pirateMelody.Length)
        {
            melodyAddedScore -= (mermaidMelody.Length - pirateMelody.Length) * 5;
            length = pirateMelody.Length;
        }

        for (int i = 0; i < length; i++)
        {
            Debug.Log("Comparing note " + i + ": Mermaid Note: " + mermaidMelody[i].GetNoteName() + " with octave: " + mermaidMelody[i].GetNoteOctave() + " vs Pirate Note: " + pirateMelody[i].GetNoteName() + " with octave: " + pirateMelody[i].GetNoteOctave());
            if (pirateMelody[i] != null)
            {
                if (mermaidMelody[i].GetNoteName() == pirateMelody[i].GetNoteName() && mermaidMelody[i].GetNoteOctave() == pirateMelody[i].GetNoteOctave())
                {
                    melodyAddedScore += 10;
                    if (foundAllNotesCounter < pirateMelody.Length)
                        currentPlayedSlot = GetNextOccupiedSlot();
                    Debug.Log(currentPlayedSlot);
                    scoreText[currentPlayedSlot].text = "+10";
                    Debug.Log("Exact match! Added 10 points.");
                }
                else if (mermaidMelody[i].GetNoteName() == pirateMelody[i].GetNoteName() && mermaidMelody[i].GetNoteOctave() != pirateMelody[i].GetNoteOctave())
                {
                    melodyAddedScore += 7;
                    if (foundAllNotesCounter < pirateMelody.Length)
                        currentPlayedSlot = GetNextOccupiedSlot();
                    Debug.Log(currentPlayedSlot);
                    scoreText[currentPlayedSlot].text = "+7";
                    Debug.Log("Same note name but different octave. Added 7 points.");
                }
                else if (mermaidMelody[i].GetNoteName() != pirateMelody[i].GetNoteName() && mermaidMelody[i].GetNoteOctave() == pirateMelody[i].GetNoteOctave())
                {
                    melodyAddedScore += 4;
                    if (foundAllNotesCounter < pirateMelody.Length)
                        currentPlayedSlot = GetNextOccupiedSlot();
                    Debug.Log(currentPlayedSlot);
                    scoreText[currentPlayedSlot].text = "+4";
                    Debug.Log("Same octave but different note name. Added 4 points.");
                }
                else
                {
                    if (foundAllNotesCounter < pirateMelody.Length)
                        currentPlayedSlot = GetNextOccupiedSlot();
                    scoreText[currentPlayedSlot].text = "+0";
                }

                if (IsAdjacentNote(pirateMelody[i], mermaidMelody[i]))
                {
                    melodyAddedScore += 2;
                    Debug.Log("The notes are adjacent. Added 2 points.");
                }
            }

            Debug.Log("Current melody added score: " + melodyAddedScore);
        }

        Debug.Log("Final melody accuracy score: " + melodyAddedScore);

        melodyAccuracyScore = (float)melodyAddedScore / (float)(mermaidMelody.Length);
        float displayedAccuracy = (float)melodyAccuracyScore * 10.0f;
        if(displayedAccuracy < 0)
        {
            displayedAccuracy = 0;
        }

        if(displayedAccuracy > 100)
        {
            displayedAccuracy = 100;
        }
        finalAccuracy.text = "Accuracy: " + displayedAccuracy.ToString() + "%";

        Debug.Log("Final melody accuracy percentage: " + melodyAccuracyScore);
    }


    public void ClearPartiture()
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("MusicNote");
        GameObject[] occupiedSlots = GameObject.FindGameObjectsWithTag("NoteDropSlot");

        for (int i = 0; i < notes.Length; i++)
        {
            Destroy(notes[i]);
        }

        for(int i = 0; i < occupiedSlots.Length; i++)
        {
            if (occupiedSlots[i].GetComponent<NoteDropSlot>().GetIsOccupied())
            {
                occupiedSlots[i].GetComponent<NoteDropSlot>().ResetData();
                occupiedSlots[i].GetComponent<NoteDropSlot>().SetIsOccupied(false);
            }
        }
    }


    public void CloseSliders()
    {
        GameObject[] sliders = GameObject.FindGameObjectsWithTag("GetSliderArrow");
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].GetComponent<AppearSlider>().CloseSlider();
        }
    }


    private bool GameOngoing()
    {
        if(pirateTimesWon >= 3 || mermaidTimesWon >= 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void StartRound()
    {
        ClearPartiture();
        currentBackground.GetComponent<SpriteRenderer>().sprite = backgroundOptions[currentBackgroundIndex];
        finalAccuracy.enabled = false;
        for(int i = 0; i < scoreAnimator.Length; i++)
        {
            scoreText[i].enabled = true;
            scoreAnimator[i].SetTrigger("reset");
        }

        for(int i = 0; i < occupiedSlots.Length; i++)
        {
            occupiedSlots[i] = 0;
        }

        // resets the score and generates a new melody if the game is still going
        if (GameOngoing())
        {
            melodyAccuracyScore = 0;
            melodyAddedScore = 0;
            melodyGenerator.SetMelodyLength(Random.Range(4, 6));
            melodyGenerator.GenerateRandomMelodyNote();
            mermaidMelody = melodyGenerator.GetMelodyNotes();
            StartCoroutine(PlayMermaidSong());
            mermaidAnimator.SetTrigger("isSinging");
        }
        else
        {
            if(pirateTimesWon >= 3)
            {
                pirateAnimator.SetTrigger("hasWonGame");
                mermaidAnimator.SetTrigger("hasLostGame");
                Invoke("LoadWinScene", 3f);
            }
            else
            {
                pirateAnimator.SetTrigger("hasLostGame");
                mermaidAnimator.SetTrigger("hasWonGame");
                Invoke("LoadLoseScene", 3f);
            }
        }
    }


    public void LoadWinScene()
    {
        SceneManager.LoadSceneAsync(2);
    }


    public void LoadLoseScene()
    {
        SceneManager.LoadSceneAsync(3);
    }


    public void PlayMermaidSongAgain()
    {
        StartCoroutine(PlayMermaidSong());
        mermaidAnimator.SetTrigger("isSinging");
        PermanentAudio.instance.PlaySFX(buttonPress);
    }


    public AudioSource GetTemporaryNote(string noteName)
    {
        switch (noteName)
        {
            case "Do":
                return Instantiate(mermaidSource[0], transform.position, Quaternion.identity);
            case "Re":
                return Instantiate(mermaidSource[1], transform.position, Quaternion.identity);
            case "Mi":
                return Instantiate(mermaidSource[2], transform.position, Quaternion.identity);
            case "Fa":
                return Instantiate(mermaidSource[3], transform.position, Quaternion.identity);
            case "Sol":
                return Instantiate(mermaidSource[4], transform.position, Quaternion.identity);
            case "La":
                return Instantiate(mermaidSource[5], transform.position, Quaternion.identity);
            case "Si":
                return Instantiate(mermaidSource[6], transform.position, Quaternion.identity);
            default:
                return null;
        }
    }


    // plays the mermaid melody with a delay between each note and disables the buttons and colliders while the melody is playing
    public IEnumerator PlayMermaidSong()
    {
        for (int i = 0; i < mermaidMelody.Length; i++)
        {
            playAgainButton.interactable = false;
            singButton.interactable = false;
            DeactivateColliders();
            temporaryNote = GetTemporaryNote(mermaidMelody[i].GetNoteName());
            Debug.Log("Mermaid Melody Note " + i + ": " + mermaidMelody[i].GetNoteName() + " with octave: " + mermaidMelody[i].GetNoteOctave());
            audioMixer.SetFloat("MyExposedPitch", mermaidMelody[i].GetNoteOctave());

            temporaryNote.Play();
            Destroy(temporaryNote.gameObject, temporaryNote.clip.length);

            yield return new WaitForSeconds(timeBetweenNotes);

            playAgainButton.interactable = true;
            singButton.interactable = true;
            ActivateColliders();
        }

        mermaidAnimator.SetTrigger("stopSinging");
    }


    public void PlayRound()
    {
        // gets the pirate melody
        PermanentAudio.instance.PlaySFX(buttonPress);
        GetPirateMelody();
        currentPlayedSlot = -1;
        foundAllNotesCounter = 0;
        
        // compares the melodies
        Compare();
        CloseSliders();

        // plays the pirate melody
        currentPlayedSlot = -1;
        foundAllNotesCounter = 0;
        StartCoroutine(PlayPirateSong());

        timeBetweenRounds = pirateMelody.Length * timeBetweenNotes;
        pirateAnimator.SetTrigger("isSinging");

        for(int i = 0; i < scoreText.Length; i++)
        {
            Debug.Log("Score text " + i + ": " + scoreText[i].text);
        }

        Invoke("DisplayFinalAccuracy", timeBetweenRounds);

    }


    public void DisplayFinalAccuracy()
    {
        finalAccuracy.enabled = true;
        PermanentAudio.instance.PlaySFX(scoreSound);
        Invoke("DestroyHeart", 1.5f);
    }


    // plays the pirate melody with a delay between each note and disables the buttons and colliders while the melody is playing
    public IEnumerator PlayPirateSong()
    {
        for (int i = 0; i < pirateMelody.Length; i++)
        {
            playAgainButton.interactable = false;
            singButton.interactable = false;
            DeactivateColliders();
            if (pirateMelody[i] != null)
            {
                AudioSource source = pirateSource[i];
                audioMixer.SetFloat("MyExposedPitch", pirateMelody[i].GetNoteOctave());
                source.Play();
                if (foundAllNotesCounter < pirateMelody.Length)
                {
                    currentPlayedSlot = GetNextOccupiedSlot();
                    scoreText[currentPlayedSlot].enabled = true;
                    scoreAnimator[currentPlayedSlot].SetTrigger("play");
                    PermanentAudio.instance.PlaySFX(scoreSound);
                }

                scoreAnimator[currentPlayedSlot].SetTrigger("isGone");
                yield return new WaitForSeconds(timeBetweenNotes);
                scoreAnimator[currentPlayedSlot].SetTrigger("reset");
                playAgainButton.interactable = true;
                singButton.interactable = true;
                ActivateColliders();
            }
        }

        pirateAnimator.SetTrigger("stopSinging");

    }


    public void DestroyHeart()
    {
        if (melodyAccuracyScore >= 6.0f)
        {
            pirateTimesWon++;

            Debug.Log($"Attempting to break mermaid heart at index: {mermaidHearts.Length - pirateTimesWon}");

            if (pirateTimesWon <= 3)
            {
                mermaidHearts[pirateTimesWon - 1].SetTrigger("isBroken");
                PermanentAudio.instance.PlaySFX(breakHeartSound);
                mermaidAnimator.SetTrigger("hasLostRound");
                pirateAnimator.SetTrigger("hasWonRound");
            }

            Invoke("ResetPirateWonAnimations", 1.5f);
        }
        else
        {
            mermaidTimesWon++;

            Debug.Log($"Attempting to break pirate heart at index: {pirateHearts.Length - mermaidTimesWon}");

            if (mermaidTimesWon <= 3)
            {
                pirateHearts[pirateHearts.Length - mermaidTimesWon].SetTrigger("isBroken");
                PermanentAudio.instance.PlaySFX(breakHeartSound);
                mermaidAnimator.SetTrigger("hasWonRound");
                pirateAnimator.SetTrigger("hasLostRound");
            }

            Invoke("ResetMermaidWonAnimations", 1.5f);
        }
    }


    public void ResetPirateWonAnimations()
    {
        mermaidAnimator.SetTrigger("stopHasLost");
        pirateAnimator.SetTrigger("stopHasWon");

        currentBackgroundIndex++;
        if (currentBackgroundIndex >= backgroundOptions.Length)
        {
            currentBackgroundIndex = 4;
        }
        Invoke("StartRound", 2f);
    }


    public void ResetMermaidWonAnimations()
    {
        mermaidAnimator.SetTrigger("stopHasWon");
        pirateAnimator.SetTrigger("stopHasLost");

        currentBackgroundIndex++;
        if(currentBackgroundIndex >= backgroundOptions.Length)
        {
            currentBackgroundIndex = 4;
        }
        Invoke("StartRound", 2f);
    }
}
