using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteSpawn : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioClip arrowPress;

    public void OnPointerDown(PointerEventData eventData)
    {
        PermanentAudio.instance.PlaySFX(arrowPress);

        GameObject newNote = Instantiate(notePrefab, transform.position, Quaternion.identity);

        DragNote dragNote = newNote.GetComponent<DragNote>();

        dragNote.SetNoteOctave(slider.value);

    }

}
