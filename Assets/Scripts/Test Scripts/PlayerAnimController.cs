using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField] private ParticleSystem castingParticles;

    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public PlayerController player;

    private int facingDirection = 1;
    private bool wasCasting;

    private void Update()
    {
        FlipSprite();
        UpdateSpriteAnim();

        bool isCasting = player.CurrentState == PlayerController.PlayerState.Casting;

        if (isCasting != wasCasting)
        {
            wasCasting = isCasting;

            if (castingParticles != null)
            {
                if (isCasting)
                    castingParticles.Play();
                else
                    castingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    private void FlipSprite()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0f && facingDirection == -1 && player.CurrentState != PlayerController.PlayerState.Casting)
        {
            Flip();
        }
        else if (horizontal < 0f && facingDirection == 1 && player.CurrentState != PlayerController.PlayerState.Casting)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        spriteRenderer.transform.Rotate(0f, -180f, 0f);
    }

    private void UpdateSpriteAnim()
    {
        bool isMoving = false;

        if (player.CurrentState == PlayerController.PlayerState.Move)
        {
            isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f;
        }

        anim.SetBool("idle", player.CurrentState == PlayerController.PlayerState.Move && !isMoving);
        anim.SetBool("move", player.CurrentState == PlayerController.PlayerState.Move && isMoving);
        anim.SetBool("casting", player.CurrentState == PlayerController.PlayerState.Casting);
    }

    public void ForceIdle()
    {
        anim.SetBool("move", false);
        anim.SetBool("casting", false);
        anim.SetBool("idle", true);
    }
}