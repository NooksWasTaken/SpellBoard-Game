using System.Collections;
using UnityEngine;

public class DisableObjectOutcome : MonoBehaviour, IPuzzleOutcome
{
    [SerializeField] private GameObject[] targetObjects;
    [SerializeField] private float disableDelay = 0f;

    public void Execute()
    {
        StartCoroutine(DisableRoutine());
    }

    private IEnumerator DisableRoutine()
    {
        if (disableDelay > 0f)
            yield return new WaitForSeconds(disableDelay);

        if (targetObjects == null || targetObjects.Length == 0)
        {
            gameObject.SetActive(false);
            yield break;
        }

        foreach (GameObject target in targetObjects)
        {
            if (target != null)
            {
                target.SetActive(false);
            }
        }
    }
}