using UnityEngine;

public class PlayerLayerController : MonoBehaviour
{
    [SerializeField] private float maxDuration = 15f;

    private int originalLayer;
    private int currentTemporaryLayer;

    private float remainingTime;
    private bool layerActive;

    private void Awake()
    {
        originalLayer = gameObject.layer;
    }

    public void SetLayerTemporarily(int layer, float duration)
    {
        if (!layerActive)
        {
            originalLayer = gameObject.layer;
            currentTemporaryLayer = layer;

            SetLayer(transform, layer);

            remainingTime = Mathf.Min(duration, maxDuration);
            layerActive = true;
            return;
        }

        remainingTime = Mathf.Min(remainingTime + duration, maxDuration);
    }

    private void Update()
    {
        if (!layerActive)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            SetLayer(transform, originalLayer);

            layerActive = false;
            remainingTime = 0f;
        }
    }

    private void SetLayer(Transform target, int layer)
    {
        target.gameObject.layer = layer;

        foreach (Transform child in target)
            SetLayer(child, layer);
    }

    public float RemainingTime => remainingTime;
    public float MaxDuration => maxDuration;
}