using UnityEngine;
public class BossController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public int startDirection = 1;
    public bool stayOnEdge = true;

    [Header("Detección")]
    public float detectionRange = 4f;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    private int direction;
    private bool isGrounded;
    private bool canMove = true;

    private float halfWidth;
    private float halfHeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = startDirection;

        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;

        UpdateFlip();
    }

    private void FixedUpdate()
    {
        CheckGround();

        if (canMove)
            HandleMovement();

        DetectPlayer();
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

    private void DetectPlayer()
    {
        // Raycast hacia la dirección actual
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * direction, detectionRange, playerLayer);

        if (hit.collider != null)
        {
            BossAttack attack = GetComponent<BossAttack>();
            if (attack != null && !attack.IsAttacking)
                attack.StartAttackCycle(hit.collider.transform);
        }
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

    // Gizmos para debug visual
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 dir = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + dir * detectionRange);
    }
}
