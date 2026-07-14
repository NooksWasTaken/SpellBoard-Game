using System.Collections;
using TMPro;
using UnityEngine;

public class MessageFadeUI : MonoBehaviour
{
    public static MessageFadeUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text messageText;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 3f;

    public bool IsDisplaying { get; private set; }

    private Coroutine displayRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    public bool ShowMessage(string message)
    {
        if (IsDisplaying)
            return false;

        if (displayRoutine != null)
            StopCoroutine(displayRoutine);

        displayRoutine = StartCoroutine(DisplayRoutine(message));
        return true;
    }

    private IEnumerator DisplayRoutine(string message)
    {
        IsDisplaying = true;

        canvasGroup.gameObject.SetActive(true);
        messageText.text = message;

        yield return Fade(0f, 1f);

        yield return new WaitForSeconds(displayDuration);

        yield return Fade(1f, 0f);

        canvasGroup.gameObject.SetActive(false);

        displayRoutine = null;
        IsDisplaying = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                from,
                to,
                elapsed / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = to;
    }
}