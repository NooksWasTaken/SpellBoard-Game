using UnityEngine;
using UnityEngine.UI;

public class SpellJournal : MonoBehaviour
{
    public static SpellJournal Instance { get; private set; }

    [SerializeField] private PlayerSpellBoard spellBoard;
    [SerializeField] private PlayerFragmentManager fragmentManager;

    [Header("Scroll View")]
    [SerializeField] private Transform content;
    [SerializeField] private SpellEntryUI spellEntryPrefab;
    [SerializeField] private FragmentEntryUI fragmentEntryPrefab;

    [Header("Other HUD References")]
    [SerializeField] private CanvasGroup pickUpHUD;
    [SerializeField] private GameObject journalUI;
    [SerializeField] private GameObject CastIcon;

    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    private bool isOpen;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        isOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (NoteUI.Instance != null && NoteUI.Instance.IsNoteOpen)
                return;

            ToggleJournal();
        }
    }

    private void ToggleJournal()
    {
        if (!isOpen && playerController.CurrentState == PlayerController.PlayerState.Casting)
            return;

        isOpen = !isOpen;

        journalUI.SetActive(isOpen);
        CastIcon.SetActive(!isOpen);

        pickUpHUD.alpha = isOpen ? 0f : 1f;
        pickUpHUD.interactable = !isOpen;
        pickUpHUD.blocksRaycasts = !isOpen;
        
        UIAlertManager.Instance.SetJournalAlert(false);
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        if (isOpen)
            Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        // display all currently spels
        foreach (PlayerSpells spell in spellBoard.GetSpells())
        {
            SpellEntryUI entry = Instantiate(spellEntryPrefab, content);
            entry.Initialize(spell);
        }

        // display all current fragments
        foreach (var progress in fragmentManager.GetFragmentProgress())
        {
            FragmentEntryUI entry = Instantiate(fragmentEntryPrefab, content);
            entry.Initialize(progress);
        }
    }


}