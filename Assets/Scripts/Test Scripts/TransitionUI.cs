using System.Collections;
using TMPro;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    public static TransitionUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text messageText;

    [Header("Fade")]
    [SerializeField] private float fadeSpeed = 2f;

    public bool IsTransitionRunning { get; private set; }
    public bool CanTeleportPlayer { get; private set; }

    private Coroutine routine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        messageText.gameObject.SetActive(false);
    }

    public bool ShowMessage(string message)
    {
        if (IsTransitionRunning)
            return false;

        routine = StartCoroutine(ShowRoutine(message));
        return true;
    }

    private IEnumerator ShowRoutine(string message)
    {
        IsTransitionRunning = true;
        CanTeleportPlayer = false;

        yield return new WaitForSeconds(1f);

        yield return Fade(0f, 1f);

        yield return new WaitForSeconds(1f);

        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        messageText.gameObject.SetActive(false);

        CanTeleportPlayer = true;

        yield return Fade(1f, 0f);

        CanTeleportPlayer = false;
        IsTransitionRunning = false;
    }

    private IEnumerator Fade(float start, float end)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Lerp(start, end, t);

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}