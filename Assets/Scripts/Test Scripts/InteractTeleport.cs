using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractTeleport : MonoBehaviour, IInteractable
{
    [Header("Teleport")]
    [SerializeField] private Transform targetPosition;

    [Header("Transition")]
    [SerializeField] private float teleportDelay = 1f;

    [Header("Interact Text")]
    [SerializeField] private string hoverText = "Leave Library";

    private CharacterController characterController;
    private PlayerController playerController;

    private bool playerInside;
    private bool isTeleporting;

    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (!playerInside || isTeleporting)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (characterController == null ||
            playerController == null ||
            targetPosition == null)
            return;

        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        isTeleporting = true;

        playerInside = false;

        PickupUI.Instance.HidePrompt();

        playerController.enabled = false;

        SlideInTransition.Instance.PlayEnterTransition();

        while (SlideInTransition.Instance.IsPlaying)
            yield return null;

        characterController.enabled = false;
        characterController.transform.position = targetPosition.position;
        characterController.enabled = true;

        yield return new WaitForSeconds(teleportDelay);

        SlideInTransition.Instance.PlayExitTransition();

        while (SlideInTransition.Instance.IsPlaying)
            yield return null;

        playerController.enabled = true;

        isTeleporting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
            return;

        characterController = other.GetComponent<CharacterController>();
        playerController = other.GetComponent<PlayerController>();

        if (characterController == null || playerController == null)
            return;

        playerInside = true;

        PickupUI.Instance.ShowPrompt(hoverText);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
            return;

        playerInside = false;

        characterController = null;
        playerController = null;

        PickupUI.Instance.HidePrompt();
    }
}