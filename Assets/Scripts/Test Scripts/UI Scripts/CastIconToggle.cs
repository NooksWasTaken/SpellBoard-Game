using UnityEngine;

public class CastIconToggle : MonoBehaviour
{
    public static CastIconToggle Instance { get; private set; }

    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject JournalIcon;

    private bool isToggled = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        JournalIcon.SetActive(isToggled);
    }

    public void ToggleIcon()
    {
        isToggled = !isToggled;
        JournalIcon.SetActive(isToggled);
    }
}
