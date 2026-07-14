using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MiasmaVignetteTrigger : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (MiasmaVignette.Instance != null)
            MiasmaVignette.Instance.FadeIn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (MiasmaVignette.Instance != null)
            MiasmaVignette.Instance.FadeOut();
    }
}