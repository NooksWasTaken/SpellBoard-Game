using System.Collections;
using TMPro;
using UnityEngine;

public class PickupUI : MonoBehaviour
{
    public static PickupUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptText;

    [SerializeField] private GameObject notificationPanel;
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

        promptPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void ShowPrompt(string itemName)
    {
        promptPanel.SetActive(true);
        promptText.text = $"[E] {itemName}";
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
    }

    public void ShowNotification(string itemName)
    {
        if (notificationRoutine != null)
        {
            StopCoroutine(notificationRoutine);
            notificationRoutine = null;
        }

        notificationPanel.SetActive(false);

        notificationRoutine = StartCoroutine(NotificationRoutine(itemName));
    }

    private IEnumerator NotificationRoutine(string itemName)
    {
        notificationPanel.SetActive(true);
        notificationText.text = $"Picked up {itemName}!";

        yield return new WaitForSeconds(notificationDuration);

        notificationPanel.SetActive(false);
        notificationRoutine = null;
    }
}