using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent (typeof(Outline))]
public class NoteInteraction : MonoBehaviour, IInteractable
{
    [Header("Player Reference")]
    private PlayerController playerController;

    [Header("Note Data")]
    [SerializeField] private NoteData note;

    private Outline outline;

    private bool playerInside;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        outline.enabled = false;
    }
    
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (!playerInside)
            return;

        if (playerController != null && playerController.CurrentState == PlayerController.PlayerState.Casting)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (note == null || NoteUI.Instance == null)
            return;

        NoteUI.Instance.Toggle(note);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        playerController = other.GetComponent<PlayerController>();

        playerInside = true;

        PickupUI.Instance.ShowPrompt(note.HoverText);
        outline.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        playerInside = false;

        playerController = null;

        PickupUI.Instance.HidePrompt();
        outline.enabled = false;

        if (NoteUI.Instance != null && NoteUI.Instance.IsNoteOpen)
            NoteUI.Instance.Close();
    }
}