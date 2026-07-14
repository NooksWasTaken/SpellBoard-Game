using System.Collections;
using UnityEngine;

public class MiasmaVignette : MonoBehaviour
{
    public static MiasmaVignette Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        canvasGroup.alpha = 0f;
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine(canvasGroup.alpha, 1f));
    }

    public void FadeOut()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        yield return FadeRoutine(canvasGroup.alpha, 0f);

        gameObject.SetActive(false);
    }

    private IEnumerator FadeRoutine(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = end;
        fadeRoutine = null;
    }
}