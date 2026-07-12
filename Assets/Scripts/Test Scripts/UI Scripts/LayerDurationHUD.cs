using UnityEngine;
using UnityEngine.UI;

public class LayerDurationHUD : MonoBehaviour
{
    public static LayerDurationHUD Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image fillBar;

    private float duration;
    private float timer;
    private bool running;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        panel.SetActive(false);
    }

    private void Update()
    {
        if (!running)
            return;

        timer -= Time.deltaTime;

        fillBar.fillAmount = Mathf.Clamp01(timer / duration);

        if (timer <= 0f)
        {
            running = false;
            panel.SetActive(false);
        }
    }

    public void StartTimer(float time)
    {
        duration = time;
        timer = time;
        running = true;

        fillBar.fillAmount = 1f;
        panel.SetActive(true);
    }

    public void StopTimer()
    {
        running = false;
        fillBar.fillAmount = 0f;
        panel.SetActive(false);
    }
}