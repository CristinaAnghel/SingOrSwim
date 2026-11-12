using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class DragNote : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string noteName;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AudioClip notePlace;

    private float noteOctave;
    private GameObject currentSlot;

    public void Start()
    {
        currentSlot = null;
    }


    // gets position of mouse when dragging the note
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPos = eventData.position;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
        newPosition.z = 0;
        transform.position = newPosition;
    }


    public void PlayNote()
    {
        audioSource.pitch = 1f;
        audioMixer.SetFloat("MyExposedPitch", noteOctave);

        audioSource.Play();
    }


    public float GetNoteOctave()
    {
        return noteOctave;
    }


    public void SetNoteOctave(float octave)
    {
        noteOctave = octave;
    }


    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // reset the current slot if the note is being dragged again
        if (currentSlot != null)
        {
            currentSlot.GetComponent<NoteDropSlot>().ResetData();
            currentSlot.GetComponent<NoteDropSlot>().SetIsOccupied(false);
            currentSlot = null;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("NoteDropSlot");
        float closestDistance = Mathf.Infinity;

        // finds the closest note slot to the position of the dragged note
        foreach(GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if(distance < closestDistance && !slot.GetComponent<NoteDropSlot>().GetIsOccupied())
            {
                closestDistance = distance;
                currentSlot = slot;
            }
        }


        // snaps to the closest empty note slot
        if(currentSlot != null && closestDistance < 1f)
        {
            PermanentAudio.instance.PlaySFX(notePlace);
            transform.position = currentSlot.transform.position;
            currentSlot.GetComponent<NoteDropSlot>().SetNoteName(noteName);
            currentSlot.GetComponent<NoteDropSlot>().SetNoteOctave(noteOctave);
            currentSlot.GetComponent<NoteDropSlot>().SetIsOccupied(true);
            currentSlot.GetComponent<NoteDropSlot>().SetAudioSource(audioSource);
        }
        else
        {
            Object.Destroy(this.gameObject);
        }

    }
}
