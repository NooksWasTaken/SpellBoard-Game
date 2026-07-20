using TMPro;
using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public static NoteUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private GameObject hoverText;

    public bool IsNoteOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        panel.SetActive(false);
    }

    public void Open(NoteData note)
    {
        if (note == null)
            return;

        IsNoteOpen = true;

        contentText.text = note.ContentText;
        panel.SetActive(true);
        hoverText.SetActive(false);
    }

    public void Close()
    {
        IsNoteOpen = false;
        panel.SetActive(false);
        hoverText.SetActive(true);
    }

    public void Toggle(NoteData note)
    {
        if (IsNoteOpen)
        {
            Close();
            PickupUI.Instance.ShowPrompt(note.HoverText);
        }
            
        else
        {
            Open(note);
            PickupUI.Instance.HidePrompt();
        }
            
    }
}