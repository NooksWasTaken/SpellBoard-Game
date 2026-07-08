using System.Collections;
using UnityEngine;

public class DisableObjectOutcome : MonoBehaviour, IPuzzleOutcome
{
    [Header("Target (Leave empty to disable this object)")]
    [SerializeField] private GameObject target;

    [SerializeField] private float delay = 0f;

    public void Execute()
    {
        GameObject objectToDisable = target != null ? target : gameObject;

        if (delay <= 0f)
        {
            objectToDisable.SetActive(false);
        }
        else
        {
            StartCoroutine(DisableAfterDelay(objectToDisable));
        }
    }

    private IEnumerator DisableAfterDelay(GameObject objectToDisable)
    {
        yield return new WaitForSeconds(delay);
        objectToDisable.SetActive(false);
    }
}