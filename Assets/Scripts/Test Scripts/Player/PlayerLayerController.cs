using System.Collections;
using UnityEngine;

public class PlayerLayerController : MonoBehaviour
{
    private Coroutine layerRoutine;

    public void SetLayerTemporarily(int layer, float duration)
    {
        if (layerRoutine != null)
            StopCoroutine(layerRoutine);

        layerRoutine = StartCoroutine(ChangeLayer(layer, duration));
    }

    private IEnumerator ChangeLayer(int layer, float duration)
    {
        int originalLayer = gameObject.layer;

        SetLayer(transform, layer);

        yield return new WaitForSeconds(duration);

        SetLayer(transform, originalLayer);

        layerRoutine = null;
    }

    private void SetLayer(Transform target, int layer)
    {
        target.gameObject.layer = layer;

        foreach (Transform child in target)
            SetLayer(child, layer);
    }
}