using UnityEngine;

public class RemoveVignetteOutcome : MonoBehaviour, IPuzzleOutcome
{
    public void Execute()
    {
        MiasmaVignette.Instance.FadeOut();
    }
}
