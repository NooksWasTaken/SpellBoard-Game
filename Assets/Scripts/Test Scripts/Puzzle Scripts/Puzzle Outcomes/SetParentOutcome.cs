using UnityEngine;

public class SetParentOutcome : MonoBehaviour, IPuzzleOutcome
{
    [Header("References")]
    [SerializeField] private Transform child;
    [SerializeField] private Transform newParent;

    [Header("Settings")]
    [SerializeField] private bool worldPositionStays = true;

    public void Execute()
    {
        if (child == null || newParent == null)
        {
            Debug.LogWarning($"{name}: Child or Parent reference is missing.");
            return;
        }

        child.SetParent(newParent, worldPositionStays);
    }
}