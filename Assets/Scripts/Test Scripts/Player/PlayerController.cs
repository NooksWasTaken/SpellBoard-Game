using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Move,
        Casting
    }

    [Header("State")]
    [SerializeField] private PlayerState currentState = PlayerState.Move;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Player References")]
    [SerializeField] private PuzzleSelector puzzleSelector;
    public GameObject HUDCanvas;
    public TypingManager typingManager;

    private CharacterController controller;
    private Vector3 velocity;

    public PlayerState CurrentState => currentState;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        HUDCanvas.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (NoteUI.Instance != null && NoteUI.Instance.IsNoteOpen)
                return;

            ToggleState();
        }

        switch (currentState)
        {
            case PlayerState.Move:
                MoveState();
                break;

            case PlayerState.Casting:
                CastingState();
                break;
        }
    }

    private void ToggleState()
    {
        if (currentState == PlayerState.Move && SpellJournal.Instance != null && SpellJournal.Instance.IsOpen)
            return;

        switch (currentState)
        {
            case PlayerState.Move:
                ChangeState(PlayerState.Casting);
                break;

            case PlayerState.Casting:
                ChangeState(PlayerState.Move);
                break;
        }
    }

    private void ChangeState(PlayerState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case PlayerState.Move:
                puzzleSelector.DeselectCurrentPuzzle();
                SpellChainHUD.Instance.Hide();
                HUDCanvas.gameObject.SetActive(false);

                CastIconToggle.Instance.ToggleIcon();
                CameraController.Instance.ZoomOut();
                CameraController.Instance.DisableMouseFollow();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case PlayerState.Casting:
                HUDCanvas.gameObject.SetActive(true);
                typingManager.ShowWaitingUI();
                PickupUI.Instance.HidePrompt();

                CastIconToggle.Instance.ToggleIcon();
                CameraController.Instance.ZoomIn();
                CameraController.Instance.EnableMouseFollow();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                typingManager.SubmitFocus();
                break;
        }
    }

    // public methods to use for switching states (used for cases outside this class)
    public void SetMoveState()
    {
        ChangeState(PlayerState.Move);
    }

    private void MoveState()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void CastingState()
    {
        SpellHUD.Instance.Refresh();
    }
}