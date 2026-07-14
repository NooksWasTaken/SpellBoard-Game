using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionOutcome : MonoBehaviour, IPuzzleOutcome
{
    [Header("Scene")]
    [SerializeField] private string sceneName;

    [Header("Transition")]
    [SerializeField] private float transitionDelay = 0f;

    [Header("Message")]
    [SerializeField] private CanvasGroup messageGroup;
    [SerializeField] private float messageDelay = 0f;
    [SerializeField] private float textFadeDuration = 1f;
    [SerializeField] private float textDisplayDuration = 2f;

    private bool running;

    private void Awake()
    {
        if (messageGroup != null)
        {
            messageGroup.alpha = 0f;
            messageGroup.gameObject.SetActive(false);
        }
    }

    public void Execute()
    {
        if (running)
            return;

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        running = true;

        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
            player.enabled = false;

        if (transitionDelay > 0f)
            yield return new WaitForSeconds(transitionDelay);

        if (SlideInTransition.Instance != null)
        {
            SlideInTransition.Instance.PlayEnterTransition();

            while (SlideInTransition.Instance.IsPlaying)
                yield return null;
        }

        if (messageDelay > 0f)
            yield return new WaitForSeconds(messageDelay);

        if (messageGroup != null)
        {
            messageGroup.gameObject.SetActive(true);

            float elapsed = 0f;

            while (elapsed < textFadeDuration)
            {
                elapsed += Time.deltaTime;

                messageGroup.alpha = Mathf.Lerp(
                    0f,
                    1f,
                    elapsed / textFadeDuration);

                yield return null;
            }

            messageGroup.alpha = 1f;

            yield return new WaitForSeconds(textDisplayDuration);

            elapsed = 0f;

            while (elapsed < textFadeDuration)
            {
                elapsed += Time.deltaTime;

                messageGroup.alpha = Mathf.Lerp(
                    1f,
                    0f,
                    elapsed / textFadeDuration);

                yield return null;
            }

            messageGroup.alpha = 0f;
            messageGroup.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(sceneName);
    }
}