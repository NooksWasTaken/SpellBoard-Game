using UnityEngine;

public class PuzzleRequirementOutcome : MonoBehaviour, IPuzzleOutcome
{
    [SerializeField] private PuzzleRequirement targetRequirement;

    public void Execute()
    {
        if (targetRequirement != null)
            targetRequirement.RegisterCompletion();
    }
}