using UnityEngine;

public class SpellPuzzle : MonoBehaviour, IPuzzle, IPuzzleOutcome
{
    [Header("Puzzle Settings")]
    [SerializeField] private PlayerSpells requiredSpell;            // spell needed to solve the puzzle
    [SerializeField] private MonoBehaviour[] outcomeObjects;        // objects to be affected after the puzzle is solved
    [SerializeField] private float timeLimit = 5f;                  // time given before solving expires
    [SerializeField] private bool replayable;                       // determines if the puzzle can be repeated
    [SerializeField] private string solvedLayer = "PuzzleSolved";

    private IPuzzleOutcome[] outcomes;
    private PuzzleVisual visual;
    private bool solved;

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
        if (solved && !replayable)
            return PuzzleResult.Failed;

        if (spell != requiredSpell)
        {
            Debug.Log("Wrong spell.");
            return PuzzleResult.Failed;
        }

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
        }

        return PuzzleResult.Solved;
    }

    public PlayerSpells GetCurrentRequiredSpell()
    {
        return requiredSpell;
    }

    public void Select()
    {
        Debug.Log("Fire Selected");
        visual.Select();
    }

    public void Deselect()
    {
        Debug.Log("Fire Deselected");
        visual.Deselect();
    }

    public float GetTimeLimit()
    {
        return timeLimit;
    }

    public void Execute()
    { }
}