using UnityEngine;

public class PlayerLayerOutcome : MonoBehaviour, IPuzzleOutcome
{
    [SerializeField] private PlayerLayerController player;
    [SerializeField] private int targetLayer;
    [SerializeField] private float duration = 3f;

    private void Awake()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerLayerController>();
    }

    public void Execute()
    {
        if (player != null)
            player.SetLayerTemporarily(targetLayer, duration);

        LayerDurationHUD.Instance?.StartTimer(duration);
    }
}