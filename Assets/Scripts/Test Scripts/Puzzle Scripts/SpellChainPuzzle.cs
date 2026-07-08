using UnityEngine;

public class SpellChainPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Spell Chain")]
    [SerializeField] private PlayerSpells[] spellChain;

    [Header("Puzzle")]
    [SerializeField] private float timeLimit = 10f;

    private PuzzleVisual visual;
    private bool solved;
    private int currentSpellIndex;

    private void Awake()
    {
        visual = GetComponent<PuzzleVisual>();
    }

    public PuzzleResult TrySolve(PlayerSpells spell)
    {
        if (solved)
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
            solved = true;

            Debug.Log("Spell chain completed!");

            // Replace this with your own puzzle completion logic.
            gameObject.SetActive(false);

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
    }
}