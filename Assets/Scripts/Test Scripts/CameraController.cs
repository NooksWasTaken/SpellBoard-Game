using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player;
    [SerializeField] private Transform followTarget;

    [Header("Zoom")]
    [SerializeField] private float zoomedFOV = 30f;
    [SerializeField] private float zoomSmoothTime = 0.15f;

    [Header("Mouse Follow")]
    //[SerializeField][Range(0f, 1f)] private float mouseInfluence = 0.5f;
    [SerializeField] private float followSmoothTime = 0.1f;
    [SerializeField] private float maxOffset = 3f;

    private float defaultFOV;
    private float targetFOV;
    private float zoomVelocity;

    private Vector3 followVelocity;
    private bool followMouse;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (cinemachineCamera == null)
            cinemachineCamera = GetComponent<CinemachineCamera>();

        defaultFOV = cinemachineCamera.Lens.FieldOfView;
        targetFOV = defaultFOV;
    }

    private void Update()
    {
        UpdateZoom();
        UpdateFollow();
    }

    private void UpdateZoom()
    {
        LensSettings lens = cinemachineCamera.Lens;

        lens.FieldOfView = Mathf.SmoothDamp(
            lens.FieldOfView,
            targetFOV,
            ref zoomVelocity,
            zoomSmoothTime);

        cinemachineCamera.Lens = lens;
    }

    private void UpdateFollow()
    {
        Vector3 targetPosition = player.position;

        if (followMouse)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Plane plane = new Plane(Vector3.forward, player.position);

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 offset = hitPoint - player.position;

                // Clamp how far mouse can pull camera
                offset = Vector3.ClampMagnitude(offset, maxOffset);

                targetPosition = player.position + offset;

                // LOCK Z (no depth movement)
                targetPosition.z = player.position.z;
            }
        }

        followTarget.position = Vector3.SmoothDamp(
            followTarget.position,
            targetPosition,
            ref followVelocity,
            followSmoothTime);
    }

    public void ZoomIn()
    {
        targetFOV = zoomedFOV;
    }

    public void ZoomOut()
    {
        targetFOV = defaultFOV;
    }

    public void SetZoom(float fov)
    {
        targetFOV = fov;
    }

    public void EnableMouseFollow()
    {
        followMouse = true;
    }

    public void DisableMouseFollow()
    {
        followMouse = false;
    }
}