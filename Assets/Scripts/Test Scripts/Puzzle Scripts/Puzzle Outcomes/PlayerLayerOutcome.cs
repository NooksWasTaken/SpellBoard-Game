using UnityEngine;

public class PlayerLayerOutcome : MonoBehaviour, IPuzzleOutcome
{
    [SerializeField] private PlayerLayerController player;
    [SerializeField] private int targetLayer;
    [SerializeField] private float duration = 3f;

    public void Execute()
    {
        if (player != null)
            player.SetLayerTemporarily(targetLayer, duration);

        LayerDurationHUD.Instance?.StartTimer(duration);
    }
}