using UnityEngine;

public class UIAlertManager : MonoBehaviour
{
    public static UIAlertManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup journalAlert;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetJournalAlert(false);
    }

    public void SetJournalAlert(bool visible)
    {
        if (journalAlert == null) return;

        journalAlert.alpha = visible ? 1f : 0f;
    }
}