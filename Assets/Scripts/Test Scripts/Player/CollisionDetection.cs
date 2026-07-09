using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private DialogueData blockedDialogue;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        MiasmaObstacle miasma = hit.collider.GetComponentInParent<MiasmaObstacle>();

        if (miasma == null)
            return;

        if (DialogueManager.Instance.IsPlaying)
            return;

        DialogueManager.Instance.StartDialogue(miasma.BlockedDialogue);
    }
}
