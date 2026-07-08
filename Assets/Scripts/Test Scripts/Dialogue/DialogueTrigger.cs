using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogue;

    [Header("Settings")]
    [SerializeField] private bool replayOnEnter = true;

    private bool hasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (DialogueManager.Instance.IsPlaying)
            return;

        if (!replayOnEnter && hasPlayed)
            return;

        Debug.Log("DIALOGUE RUNNING");

        DialogueManager.Instance.StartDialogue(dialogue);

        hasPlayed = true;
    }
}