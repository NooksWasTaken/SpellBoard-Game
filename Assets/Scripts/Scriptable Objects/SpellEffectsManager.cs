using UnityEngine;

public class SpellEffectsManager : MonoBehaviour
{
    [Header("Spell Detection")]
    public float spellRadius = 5f;        // radius of the spell effect
    public LayerMask puzzleLayer;         // layer to detect puzzles

    private SpellTypingSystem typingSystem;

    private void Start()
    {
        // get the SpellTypingSystem from the same player object
        typingSystem = GetComponent<SpellTypingSystem>();
        if (typingSystem == null)
        {
            Debug.LogError("SpellTypingSystem not found on player!");
        }
    }

    // performs the appropriate spell based on puzzle type
    public void CastSpellEffect(PuzzleType type)
    {
        // check if player actually has the spell before casting
        if (!HasSpell(type))
        {
            Debug.Log("Player does not have this spell.");
            return;
        }

        switch (type)
        {
            case PuzzleType.RESTORE:
                Restore();
                break;

            case PuzzleType.PURIFY:
                Purify();
                break;
        }
    }

    // helper function to check if the spell exists in the player's list
    private bool HasSpell(PuzzleType type)
    {
        if (typingSystem == null) return false;

        foreach (Spell spell in typingSystem.spells)
        {
            if (spell.puzzleType == type)
            {
                return true;
            }
        }

        return false;
    }

    // Unique spell functions
    public void Restore()
    {
        Debug.Log("PLAYER CASTED RESTORE!");

        // custom Restore-specific gameplay logic goes here

        // detect and affect nearby puzzles
        CheckNearbyPuzzles(PuzzleType.RESTORE);
    }

    public void Purify()
    {
        Debug.Log("PLAYER CASTED PURIFY!");

        // custom Purify-specific gameplay logic goes here

        // detect and affect nearby puzzles
        CheckNearbyPuzzles(PuzzleType.PURIFY);
    }

    // helper function to find nearby puzzles of a certain type
    private void CheckNearbyPuzzles(PuzzleType type)
    {
        // draws a sphere to detect for puzzles
        Collider[] hits = Physics.OverlapSphere(transform.position, spellRadius, puzzleLayer);

        // loop through every collider found
        foreach (Collider hit in hits)
        {
            Puzzle puzzle = hit.GetComponent<Puzzle>();
            if (puzzle != null && puzzle.puzzleType == type)
            {
                // trigger puzzle solved event
                puzzle.TrySolvePuzzle();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spellRadius);
    }
}