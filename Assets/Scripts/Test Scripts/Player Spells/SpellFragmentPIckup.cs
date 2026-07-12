using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpellFragmentPickup : MonoBehaviour
{
    [Header("Fragment")]
    [SerializeField] private SpellFragment fragment;

    [Header("Visual")]
    [SerializeField] private Outline outline;

    private PlayerFragmentManager playerBoard;
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

        if (playerController != null &&
            playerController.CurrentState == PlayerController.PlayerState.Casting)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerBoard != null)
            {
                playerBoard.AddFragment(fragment);

                Debug.Log($"Picked up fragment: {fragment.fragmentName}");

                PickupUI.Instance.ShowNotification(fragment.fragmentName);
                PickupUI.Instance.HidePrompt();

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerFragmentManager board = other.GetComponent<PlayerFragmentManager>();
        playerController = other.GetComponent<PlayerController>();

        if (board == null)
            return;

        PickupUI.Instance.ShowPrompt(fragment.fragmentName);

        playerBoard = board;
        playerInRange = true;

        if (outline != null &&
            playerController.CurrentState != PlayerController.PlayerState.Casting)
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (outline != null &&
            playerController.CurrentState == PlayerController.PlayerState.Casting)
        {
            outline.enabled = false;
            PickupUI.Instance.HidePrompt();
        }
        else if (outline != null &&
                 playerController.CurrentState != PlayerController.PlayerState.Casting)
        {
            PickupUI.Instance.ShowPrompt(fragment.fragmentName);
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerFragmentManager board = other.GetComponent<PlayerFragmentManager>();

        if (board != playerBoard)
            return;

        PickupUI.Instance.HidePrompt();

        playerInRange = false;
        playerBoard = null;

        if (outline != null)
            outline.enabled = false;
    }
}