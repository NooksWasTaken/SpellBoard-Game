using UnityEngine;

// note to me: rename this class to just NormalPuzzle
public class FirePuzzle : MonoBehaviour, IPuzzle
{
    [SerializeField] private PlayerSpells requiredSpell;
    [SerializeField] private float timeLimit = 5f;

    private PuzzleVisual visual;
    private bool solved;

    private void Awake()
    {
        visual = GetComponent<PuzzleVisual>();
    }

    public PuzzleResult TrySolve(PlayerSpells spell)
    {
        if (solved)
            return PuzzleResult.Failed;

        if (spell != requiredSpell)
        {
            Debug.Log("Wrong spell.");
            return PuzzleResult.Failed;
        }

        solved = true;
        gameObject.SetActive(false);

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
}