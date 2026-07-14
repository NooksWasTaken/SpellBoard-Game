using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MessageTrigger : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string message;

    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
            return;

        if (MessageFadeUI.Instance == null)
            return;

        if (MessageFadeUI.Instance.ShowMessage(message))
        {
            gameObject.SetActive(false);
        }
    }
}