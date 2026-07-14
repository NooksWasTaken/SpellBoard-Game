using System.Collections;
using UnityEngine;

public class MaterialSwitchOuutcome : MonoBehaviour, IPuzzleOutcome
{
    [Header("Target")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Materials")]
    [SerializeField] private Material startMaterial;
    [SerializeField] private Material endMaterial;

    [Header("Animation")]
    [SerializeField] private float duration = 1f;

    private Material runtimeMaterial;
    private Coroutine lerpRoutine;

    private void Awake()
    {
        if (targetRenderer == null || startMaterial == null || endMaterial == null)
            return;

        runtimeMaterial = targetRenderer.material;

        ApplyMaterial(startMaterial);
    }

    public void Execute()
    {
        if (runtimeMaterial == null)
            return;

        if (lerpRoutine != null)
            StopCoroutine(lerpRoutine);

        lerpRoutine = StartCoroutine(LerpRoutine());
    }

    private IEnumerator LerpRoutine()
    {
        float elapsed = 0f;

        Color startColor = GetColor(startMaterial);
        Color endColor = GetColor(endMaterial);

        Color startEmission = GetEmission(startMaterial);
        Color endEmission = GetEmission(endMaterial);

        float startSmoothness = GetFloat(startMaterial, "_Smoothness");
        float endSmoothness = GetFloat(endMaterial, "_Smoothness");

        float startMetallic = GetFloat(startMaterial, "_Metallic");
        float endMetallic = GetFloat(endMaterial, "_Metallic");

        runtimeMaterial.EnableKeyword("_EMISSION");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            SetColor(Color.Lerp(startColor, endColor, t));
            runtimeMaterial.SetColor("_EmissionColor",
                Color.Lerp(startEmission, endEmission, t));

            SetFloat("_Smoothness",
                Mathf.Lerp(startSmoothness, endSmoothness, t));

            SetFloat("_Metallic",
                Mathf.Lerp(startMetallic, endMetallic, t));

            yield return null;
        }

        ApplyMaterial(endMaterial);

        lerpRoutine = null;
    }

    private void ApplyMaterial(Material source)
    {
        SetColor(GetColor(source));

        runtimeMaterial.EnableKeyword("_EMISSION");
        runtimeMaterial.SetColor("_EmissionColor", GetEmission(source));

        SetFloat("_Smoothness", GetFloat(source, "_Smoothness"));
        SetFloat("_Metallic", GetFloat(source, "_Metallic"));
    }

    private Color GetColor(Material mat)
    {
        if (mat.HasProperty("_BaseColor"))
            return mat.GetColor("_BaseColor");

        return mat.color;
    }

    private void SetColor(Color color)
    {
        if (runtimeMaterial.HasProperty("_BaseColor"))
            runtimeMaterial.SetColor("_BaseColor", color);
        else
            runtimeMaterial.color = color;
    }

    private Color GetEmission(Material mat)
    {
        if (mat.HasProperty("_EmissionColor"))
            return mat.GetColor("_EmissionColor");

        return Color.black;
    }

    private float GetFloat(Material mat, string property)
    {
        return mat.HasProperty(property) ? mat.GetFloat(property) : 0f;
    }

    private void SetFloat(string property, float value)
    {
        if (runtimeMaterial.HasProperty(property))
            runtimeMaterial.SetFloat(property, value);
    }
}