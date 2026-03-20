using UnityEngine;

public class SpellEffectsManager : MonoBehaviour
{
    [Header("Spell Detection")]
    public float spellRadius = 5f;        // radius of the spell effect
    public LayerMask puzzleLayer;         // layer to detect puzzles

    // performs the appropriate spell based on puzzle type
    public void CastSpellEffect(PuzzleType type)
    {
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
        Debug.Log("TEST TEXT 1");
        // draws a sphere to detect for puzzles
        Collider[] hits = Physics.OverlapSphere(transform.position, spellRadius, puzzleLayer);

        // loop through every collider found
        foreach (Collider hit in hits)
        {
            Puzzle puzzle = hit.GetComponent<Puzzle>();
            if (puzzle != null && puzzle.puzzleType == type)
            {
                // trigger puzzle solved event
                puzzle.OnPuzzleSolved();
                Debug.Log("TEST TEXT 2");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spellRadius);
    }
}