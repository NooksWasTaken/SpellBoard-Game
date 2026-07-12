using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpellPickups : MonoBehaviour
{
    [Header("Spell")]
    [SerializeField] public PlayerSpells spell;

    [Header("Visual")]
    [SerializeField] private Outline outline;

    private PlayerSpellBoard playerBoard;
    private PlayerController playerController;
    private bool playerInRange;

    private void Awake()
    {
        if (outline == null)
            outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (playerController != null && playerController.CurrentState == PlayerController.PlayerState.Casting)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerBoard != null && playerBoard.AddSpell(spell))
            {
                Debug.Log($"Learned spell: {spell.spellName}");
                PickupUI.Instance.ShowNotification(spell.spellName);
                PickupUI.Instance.HidePrompt();

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerSpellBoard board = other.GetComponent<PlayerSpellBoard>();
        playerController = other.GetComponent<PlayerController>();

        if (board == null)
            return;

        PickupUI.Instance.ShowPrompt(spell.spellName);

        playerBoard = board;
        playerInRange = true;

        if (outline != null && playerController.CurrentState != PlayerController.PlayerState.Casting)
            outline.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (outline != null && playerController.CurrentState == PlayerController.PlayerState.Casting)
        {
            outline.enabled = false;
            PickupUI.Instance.HidePrompt();
        }  
        else if (outline != null && playerController.CurrentState != PlayerController.PlayerState.Casting)
        {
            PickupUI.Instance.ShowPrompt(spell.spellName);
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerSpellBoard board = other.GetComponent<PlayerSpellBoard>();

        if (board != playerBoard)
            return;

        PickupUI.Instance.HidePrompt();

        playerInRange = false;
        playerBoard = null;

        if (outline != null)
            outline.enabled = false;
    }
}