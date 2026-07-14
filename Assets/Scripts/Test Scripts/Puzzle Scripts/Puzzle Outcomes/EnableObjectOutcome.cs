using System.Collections;
using UnityEngine;

public class EnableObjectOutcome : MonoBehaviour, IPuzzleOutcome
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float enableDelay = 0f;

    private Coroutine enableRoutine;

    private void Awake()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    public void Execute()
    {
        if (targetObject == null)
        {
            Debug.LogWarning($"{name}: No target object assigned.");
            return;
        }

        if (enableRoutine != null)
            StopCoroutine(enableRoutine);

        enableRoutine = StartCoroutine(EnableRoutine());
    }

    private IEnumerator EnableRoutine()
    {
        if (enableDelay > 0f)
            yield return new WaitForSeconds(enableDelay);

        targetObject.SetActive(true);

        enableRoutine = null;
    }
}