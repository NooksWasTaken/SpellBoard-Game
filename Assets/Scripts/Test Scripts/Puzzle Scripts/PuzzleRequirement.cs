using UnityEngine;

public class PuzzleRequirement : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private int requiredCompletions = 3;

    [Header("Locked Object")]
    [SerializeField] private GameObject targetObject;

    [SerializeField] private string lockedLayer = "Default";
    [SerializeField] private string unlockedLayer = "Puzzle";

    private int currentCompletions;

    public bool IsUnlocked => currentCompletions >= requiredCompletions;

    private void Start()
    {
        if (targetObject == null)
            targetObject = gameObject;

        SetLayer(targetObject, LayerMask.NameToLayer(lockedLayer));
    }

    public void RegisterCompletion()
    {
        if (IsUnlocked)
            return;

        currentCompletions++;

        Debug.Log($"Puzzle Progress: {currentCompletions}/{requiredCompletions}");

        if (IsUnlocked)
        {
            Debug.Log("Main puzzle unlocked!");

            SetLayer(targetObject, LayerMask.NameToLayer(unlockedLayer));
        }
    }

    private void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }
}