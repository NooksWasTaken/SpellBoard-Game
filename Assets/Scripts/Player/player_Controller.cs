using System.Collections;
using UnityEngine;

public class player_Controller : MonoBehaviour
{
    [Header("Player State")]
    public player_State currentState = player_State.Exploring;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private float groundedTimer;

    [Header("Spell Casting UI")]
    public GameObject SpellCastingCanvas;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        SpellCastingCanvas.SetActive(false);
    }

    private void Update()
    {
        //State Toggle
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSpellCasting();
            ToggleSpellCanvas();
        }

        if (controller.isGrounded)
        {
            groundedTimer = 0.2f; //Time window for jumping
        }

        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        //Ground Check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Movement...
        if (currentState == player_State.Exploring)
        {
            Movement();
        }

        //Graity Application
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Movement()
    {
        float x = 0;
        float z = 0;

        //4-Direction Input
        if (Input.GetKey(KeyCode.W)) z = 1;
        else if (Input.GetKey(KeyCode.S)) z = -1;
        else if (Input.GetKey(KeyCode.A)) x = -1;
        else if (Input.GetKey(KeyCode.D)) x = 1;

        Vector3 move = new Vector3(x, 0, z);

        //Move the player based on the moveSpeed
        controller.Move(move * moveSpeed * Time.deltaTime);

        //Jump
        if (Input.GetButtonDown("Jump") && groundedTimer > 0)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
        }
    }

    public void ToggleSpellCasting()
    {
        currentState = (currentState == player_State.Exploring)
            ? player_State.SpellCasting
            : player_State.Exploring;

        Debug.Log($"State: {currentState}");
    }

    private void ToggleSpellCanvas()
    {
        if (currentState == player_State.SpellCasting)
        {
            SpellCastingCanvas.SetActive(true);
        }
        else if (currentState == player_State.Exploring)
        {
            SpellCastingCanvas.SetActive(false);
        }
    }
}