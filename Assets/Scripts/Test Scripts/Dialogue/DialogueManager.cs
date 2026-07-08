using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private Coroutine dialogueRoutine;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        if (dialogue == null || dialogue.lines.Count == 0)
            return;

        // Stop any dialogue currently running
        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        currentDialogue = dialogue;
        currentLineIndex = 0;

        IsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueRoutine = StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        while (currentLineIndex < currentDialogue.lines.Count)
        {
            DialogueLine line = currentDialogue.lines[currentLineIndex];

            yield return StartCoroutine(TypeLine(line));

            currentLineIndex++;
        }

        EndDialogue();
    }

    private IEnumerator TypeLine(DialogueLine line)
    {
        dialogueText.text = line.text;

        dialogueText.ForceMeshUpdate();

        dialogueText.maxVisibleCharacters = 0;

        int totalCharacters = dialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            if (i < totalCharacters)
            {
                char c = dialogueText.textInfo.characterInfo[i].character;

                float delay = 1f / Mathf.Max(line.typingSpeed, 1f);

                switch (c)
                {
                    case '.':
                        delay *= 8f;
                        break;

                    case ',':
                        delay *= 4f;
                        break;

                    case '!':
                    case '?':
                        delay *= 6f;
                        break;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        yield return new WaitForSeconds(line.displayTime);
    }

    private void EndDialogue()
    {
        IsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueRoutine = null;
    }
}