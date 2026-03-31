using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRadius = 2f;
    public LayerMask interactLayer;

    private player_Controller playerController;

    void Start()
    {
        playerController = GetComponent<player_Controller>();
    }

    void Update()
    {
        // only allow interaction in Exploring state, don't do anything otherwise
        if (playerController.currentState != player_State.Exploring)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // cast a sphere, interact only if an object possesses a script using an interface
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius, interactLayer);

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
                return; // interact with first valid one
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}