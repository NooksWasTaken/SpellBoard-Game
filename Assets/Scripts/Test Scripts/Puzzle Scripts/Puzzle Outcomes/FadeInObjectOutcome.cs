using System.Collections;
using UnityEngine;

public class FadeInObjectOutcome : MonoBehaviour, IPuzzleOutcome
{
    [Header("Target")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Fade")]
    [SerializeField] private float duration = 1f;

    private Material runtimeMaterial;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (targetRenderer == null)
            return;

        runtimeMaterial = targetRenderer.material;

        SetAlpha(0f);

        targetRenderer.gameObject.SetActive(false);
    }

    public void Execute()
    {
        if (runtimeMaterial == null)
            return;

        targetRenderer.gameObject.SetActive(true);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        SetAlpha(0f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            SetAlpha(t);

            yield return null;
        }

        SetAlpha(1f);

        fadeRoutine = null;
    }

    private void SetAlpha(float alpha)
    {
        if (runtimeMaterial.HasProperty("_BaseColor"))
        {
            Color color = runtimeMaterial.GetColor("_BaseColor");
            color.a = alpha;
            runtimeMaterial.SetColor("_BaseColor", color);
        }
        else
        {
            Color color = runtimeMaterial.color;
            color.a = alpha;
            runtimeMaterial.color = color;
        }
    }
}