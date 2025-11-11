using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Melee")]
    public float postAttackDelay = 0.5f;      // 500 ms después de cada ataque
    public float impactDelay = 0.25f;         // tiempo desde que comienza la animación hasta que hace daño (ajustar)
    public int meleeDamage = 2;

    [Header("Movimiento")]
    public float speed = 2f;
    public int startDirection = 1;
    public bool stayOnEdge = true;

    [Header("Vida")]
    public float vidaMax = 3;

    // Estado interno
    private bool isAttacking = false;

    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    private bool isGrounded;
    private float vida;

    // Propiedad pública si la usas en otras partes
    public bool IsAttacking => isAttacking;

    private void Start()
    {
        halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
        currentDirection = startDirection;

        // Aseguramos que siempre mira hacia currentDirection, sin depender del sprite base
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (currentDirection == 1 ? -1 : 1); //logica al reves por el sprite
        transform.localScale = scale;

        vida = vidaMax;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Suelo"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking || isGrounded)
        {
            HandleMovement();
            SetDirection();
        }
    }
    private void HandleMovement()
    {
        movement.x = speed * currentDirection;
        movement.y = rb.linearVelocityY;
        rb.linearVelocity = movement;

        // Activar animación de caminar
        animator.SetBool("IsWalking", movement.x != 0);
    }
    private void SetDirection()
    {
        if (isAttacking) return;
        if (!isGrounded) return;

        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;

        // Ajustamos las posiciones para las verificaciones de la pared
        rightPos.x += halfWidth;
        leftPos.x -= halfWidth;

        if (rb.linearVelocity.x < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.11f, LayerMask.GetMask("Ground")))
            {
                FlipObject();
            }
            else if (stayOnEdge && !Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                FlipObject();
            }

        }
        else if (rb.linearVelocity.x > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.11f, LayerMask.GetMask("Ground")))
            {
                FlipObject();
            }
            else if (stayOnEdge && !Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                FlipObject();
            }
        }
    }
    private void FlipObject()
    {
        currentDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (currentDirection == 1 ? -1 : 1); //logica al reves por el sprite
        transform.localScale = scale;
    }
}
