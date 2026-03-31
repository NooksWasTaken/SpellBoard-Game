using UnityEngine;

public class TornScroll : MonoBehaviour, IInteractable
{
    GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void Interact()
    {
        gameManager.restoreTornScroll += 1;
        Debug.Log("torn scroll +1");
        Destroy(gameObject);
    }
}
