using System.Collections;
using TMPro;
using UnityEngine;

public class PickupUI : MonoBehaviour
{
    public static PickupUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private CanvasGroup promptGroup;
    [SerializeField] private TMP_Text promptText;

    [SerializeField] private CanvasGroup notificationGroup;
    [SerializeField] private TMP_Text notificationText;

    [SerializeField] private float notificationDuration = 2f;

    private Coroutine notificationRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetCanvasGroup(promptGroup, false);
        SetCanvasGroup(notificationGroup, false);
    }

    private void SetCanvasGroup(CanvasGroup group, bool visible)
    {
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }

    public void ShowPrompt(string itemName)
    {
        promptText.text = $"[E] {itemName}";
        SetCanvasGroup(promptGroup, true);
    }

    public void HidePrompt()
    {
        SetCanvasGroup(promptGroup, false);
    }

    public void ShowNotification(string itemName)
    {
        if (notificationRoutine != null)
            StopCoroutine(notificationRoutine);

        notificationRoutine = StartCoroutine(NotificationRoutine(itemName));
    }

    private IEnumerator NotificationRoutine(string itemName)
    {
        notificationText.text = $"Picked up {itemName}!";

        SetCanvasGroup(notificationGroup, true);

        yield return new WaitForSeconds(notificationDuration);

        SetCanvasGroup(notificationGroup, false);
        notificationRoutine = null;
    }
}