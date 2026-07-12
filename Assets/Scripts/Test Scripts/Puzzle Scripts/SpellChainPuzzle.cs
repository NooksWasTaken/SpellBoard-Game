using UnityEngine;

public class SpellChainPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Spell Chain")]
    [SerializeField] private PlayerSpells[] spellChain;         // spells required to complete the puzzle, follow a strict order depending on which element was assigned first

    [Header("Puzzle Settings")]
    [SerializeField] private MonoBehaviour[] outcomeObjects;    // objects to be affected after the puzzle is solved
    [SerializeField] private bool replayable;
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private string solvedLayer = "PuzzleSolved";

    [Header("Puzzle Requirement")]
    [SerializeField] private PuzzleRequirement requirement;

    private IPuzzleOutcome[] outcomes;
    private PuzzleVisual visual;
    private bool solved;
    private int currentSpellIndex;

    private void Awake()
    {
        visual = GetComponent<PuzzleVisual>();

        outcomes = new IPuzzleOutcome[outcomeObjects.Length];

        for (int i = 0; i < outcomeObjects.Length; i++)
        {
            outcomes[i] = outcomeObjects[i] as IPuzzleOutcome;

            if (outcomes[i] == null)
            {
                Debug.LogError($"{outcomeObjects[i].name} does not have the IPuzzleOutcome interface");
            }
        }
    }

    public PuzzleResult TrySolve(PlayerSpells spell)
    {
        if (requirement != null && !requirement.IsUnlocked)
        {
            Debug.Log("This puzzle is locked.");
            return PuzzleResult.Failed;
        }

        if (solved && !replayable)
            return PuzzleResult.Failed;

        if (spellChain == null || spellChain.Length == 0)
        {
            Debug.LogWarning($"{name} has no spell chain assigned.");
            return PuzzleResult.Failed;
        }

        PlayerSpells requiredSpell = spellChain[currentSpellIndex];

        if (spell != requiredSpell)
        {
            Debug.Log("Wrong spell! Chain reset.");

            currentSpellIndex = 0;
            return PuzzleResult.Failed;
        }

        currentSpellIndex++;

        Debug.Log($"Correct! ({currentSpellIndex}/{spellChain.Length})");

        if (currentSpellIndex >= spellChain.Length)
        {
            Debug.Log("Spell chain completed!");

            solved = true;

            if (!replayable)
            {
                gameObject.layer = LayerMask.NameToLayer(solvedLayer);
            }

            foreach (var outcome in outcomes)
            {
                outcome?.Execute();
            }

            if (replayable)
            {
                solved = false;
                currentSpellIndex = 0;
            }

            return PuzzleResult.Solved;
        }

        return PuzzleResult.Progress;
    }

    public void Select()
    {
        visual.Select();
    }

    public void Deselect()
    {
        visual.Deselect();

        if (!solved)
            ResetChain();
    }

    public float GetTimeLimit()
    {
        return timeLimit;
    }

    public int GetCurrentSpellIndex()
    {
        return currentSpellIndex;
    }

    public int GetSpellCount()
    {
        return spellChain.Length;
    }

    public PlayerSpells GetCurrentRequiredSpell()
    {
        if (currentSpellIndex >= spellChain.Length)
            return null;

        return spellChain[currentSpellIndex];
    }

    public void ResetChain()
    {
        currentSpellIndex = 0;
        solved = false;
    }
}