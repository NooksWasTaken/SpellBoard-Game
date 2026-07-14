using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SlideInTransition : MonoBehaviour
{
    public static SlideInTransition Instance { get; private set; }
    public bool IsPlaying { get; private set; }

    [Header("Animation")]
    [SerializeField] private Vector2 startPosition = new Vector2(-2000f, 0f);
    [SerializeField] private Vector2 exitPosition = new Vector2(2000f, 0f);
    [SerializeField] private Vector2 targetPosition = Vector2.zero;
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;
    private Coroutine slideRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPosition;
        gameObject.SetActive(false);
    }

    public void PlayEnterTransition()
    {
        gameObject.SetActive(true);

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        rectTransform.anchoredPosition = startPosition;
        slideRoutine = StartCoroutine(SlideRoutine(startPosition, targetPosition));
    }

    public void PlayExitTransition()
    {
        gameObject.SetActive(true);

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        rectTransform.anchoredPosition = targetPosition;
        slideRoutine = StartCoroutine(SlideRoutine(targetPosition, exitPosition));
    }

    private IEnumerator SlideRoutine(Vector2 from, Vector2 to)
    {
        IsPlaying = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = curve.Evaluate(Mathf.Clamp01(elapsed / duration));

            rectTransform.anchoredPosition = Vector2.Lerp(from, to, t);

            yield return null;
        }

        rectTransform.anchoredPosition = to;

        IsPlaying = false;
        slideRoutine = null;

        if (to == exitPosition)
            gameObject.SetActive(false);
    }

    public void Hide()
    {
        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        rectTransform.anchoredPosition = startPosition;
        gameObject.SetActive(false);

        slideRoutine = null;
    }
}