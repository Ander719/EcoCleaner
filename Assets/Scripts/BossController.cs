using System;
using UnityEngine;
public class BossController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public int startDirection = 1;
    public bool stayOnEdge = true;

    [Header("Detección")]
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRender;
    private int direction;
    private bool isGrounded;
    private bool canMove = true;

    private float halfWidth;
    private float halfHeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        direction = startDirection;

        halfWidth = spriteRender.bounds.extents.x;
        halfHeight = spriteRender.bounds.extents.y;

        IgnorarColision();
        UpdateFlip();
    }

    private void Update()
    {
        CheckGround();
        animator.SetBool("IsWalking", isGrounded);
    }

    private void FixedUpdate()
    {
        if (canMove && isGrounded) HandleMovement();
    }

    private void HandleMovement()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        animator.SetBool("IsWalking", true);

        // Detectar pared o borde
        Vector2 frontPos = (Vector2)transform.position + Vector2.right * direction * (halfWidth + 0.05f);
        bool wallHit = Physics2D.Raycast(frontPos, Vector2.right * direction, 0.1f, groundLayer);

        bool edgeDetected = false;
        if (stayOnEdge)
        {
            Vector2 edgeCheck = frontPos + Vector2.down * (halfHeight + 0.05f);
            edgeDetected = !Physics2D.Raycast(edgeCheck, Vector2.down, 0.1f, groundLayer);
        }

        // Si choca con pared o está al borde, girar
        if (wallHit || edgeDetected)
            Flip();
    }

    private void CheckGround()
    {
        // Raycast hacia abajo para saber si está tocando el suelo
        Vector2 origin = (Vector2)transform.position + Vector2.down * (halfHeight + 0.05f);
        isGrounded = Physics2D.Raycast(origin, Vector2.down, 0.1f, groundLayer);
    }
    private void IgnorarColision()
    {
        Collider2D bossCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = GameObject.FindWithTag("Jugador").GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(bossCollider, playerCollider);
    }
    
    public void SetCanMove(bool value)
    {
        canMove = value;

        if (!value)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }
    }

    private void Flip()
    {
        direction *= -1;
        UpdateFlip();
    }

    private void UpdateFlip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction == 1 ? -1 : 1); //logica al reves por el sprite
        transform.localScale = scale;
    }

}
