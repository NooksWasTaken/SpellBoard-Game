using System.Collections;
using UnityEngine;

public class MiasmaObstacle : MonoBehaviour
{
    [Header("Return Point")]
    [SerializeField] private Transform returnPoint;

    [Header("Dialogue")]
    [SerializeField] private DialogueData blockedDialogue;

    [Header("Transition")]
    [SerializeField] private string blockedMessage;

    [Header("Effects")]
    [SerializeField] private ParticleSystem failEffect;

    public DialogueData BlockedDialogue => blockedDialogue;

    private int playerLayer;
    private int playerNoCollideLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        playerNoCollideLayer = LayerMask.NameToLayer("PlayerNoCollide");
    }

    private void OnTriggerStay(Collider other)
    {
        if (TransitionUI.Instance == null ||
            TransitionUI.Instance.IsTransitionRunning)
            return;

        if (other.gameObject.layer == playerNoCollideLayer)
            return;

        if (other.gameObject.layer == playerLayer)
        {
            StartCoroutine(BlockPlayer(other));
        }
    }

    private IEnumerator BlockPlayer(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        PlayerAnimController animController = other.GetComponent<PlayerAnimController>();
        CharacterController controller = other.GetComponent<CharacterController>();

        if (playerController == null ||
            animController == null ||
            controller == null)
            yield break;

        if (!TransitionUI.Instance.ShowMessage(blockedMessage))
            yield break;

        playerController.enabled = false;

        if (failEffect != null)
        {
            ParticleSystem effect = Instantiate(failEffect, other.transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }

        animController.ForceIdle();

        while (TransitionUI.Instance.IsTransitionRunning)
        {
            if (TransitionUI.Instance.CanTeleportPlayer)
                break;

            yield return null;
        }

        controller.enabled = false;
        other.transform.position = returnPoint.position;
        controller.enabled = true;

        while (TransitionUI.Instance.IsTransitionRunning)
            yield return null;

        playerController.enabled = true;
    }

    public DialogueData GetBlockedDialogue()
    {
        return blockedDialogue;
    }
}