using System.Collections;
using UnityEngine;

public class EntrySceneFadeOut : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimController animController;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (animController != null)
            animController.enabled = false;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        if (playerController != null)
            playerController.enabled = true;

        if (animController != null)
            animController.enabled = true;
    }
}