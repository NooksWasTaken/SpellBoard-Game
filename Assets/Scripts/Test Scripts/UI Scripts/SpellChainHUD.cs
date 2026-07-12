using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellChainHUD : MonoBehaviour
{
    public static SpellChainHUD Instance { get; private set; }

    [SerializeField] private GameObject root;
    [SerializeField] private Image fillBar;

    [Header("Animation")]
    [SerializeField] private float fillSpeed = 6f;

    private Coroutine fillRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        root.SetActive(false);
        fillBar.fillAmount = 0f;
    }

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillBar.fillAmount = 0f;
    }

    public void UpdateProgress(int current, int total)
    {
        Show();

        float targetFill = total <= 0 ? 0f : (float)current / total;

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillRoutine = StartCoroutine(AnimateFill(targetFill));
    }

    public void ResetProgress()
    {
        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillBar.fillAmount = 0f;
    }

    private IEnumerator AnimateFill(float target)
    {
        while (!Mathf.Approximately(fillBar.fillAmount, target))
        {
            fillBar.fillAmount = Mathf.MoveTowards(
                fillBar.fillAmount,
                target,
                fillSpeed * Time.deltaTime);

            yield return null;
        }

        fillBar.fillAmount = target;
    }
}