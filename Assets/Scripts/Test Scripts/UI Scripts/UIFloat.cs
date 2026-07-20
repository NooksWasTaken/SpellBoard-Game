using UnityEngine;

public class UIFloat : MonoBehaviour
{
    [SerializeField] private float amplitude = 10f;
    [SerializeField] private float speed = 2f;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        rectTransform.anchoredPosition = startPosition + Vector2.up * offset;
    }
}