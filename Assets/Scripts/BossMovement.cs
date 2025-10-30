using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int startDirection = 1;
    [SerializeField] private bool stayOnEdge = true;

    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    private bool isGrounded;
    

    [HideInInspector] private bool isAttacking = false;
    [HideInInspector] private bool hasAttacked = false;

    private void Start()
    {
        halfWidth = spriteRenderer.bounds.extents.x;
        halfHeight = spriteRenderer.bounds.extents.y;
        currentDirection = startDirection;
        spriteRenderer.flipX = currentDirection != -1;
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
        HandlePlayerInteraction();
        if (!isAttacking || !isGrounded)
        {
            HandleMovement();
            SetDirection();
        }
    }
    private void HandleMovement(){
        movement.x = speed * currentDirection;
        movement.y = rigidBody.linearVelocityY;
        rigidBody.linearVelocity = movement;
        // Set IsWalking segun el movimiento
        if (movement.x != 0)
        {
            animator.SetBool("IsWalking", true);  // Set walking
        }
        else
        {
            animator.SetBool("IsWalking", false); // Set idle
        }
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

        if (rigidBody.linearVelocity.x < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left,halfWidth + 0.11f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1;
                spriteRenderer.flipX = true;
            }
            else if (stayOnEdge && !Physics2D.Raycast(leftPos,Vector2.down,halfHeight + 0.1f,LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1;
                spriteRenderer.flipX = true;
            }
            
        }
        else if (rigidBody.linearVelocity.x > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right,halfWidth + 0.11f, LayerMask.GetMask("Ground")))
            {
                currentDirection = -1;
                spriteRenderer.flipX = false;
            }
            else if (stayOnEdge && !Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1;
                spriteRenderer.flipX = false;
            }
        }
    }
    private void HandlePlayerInteraction()
    {
        // Detectamos la colisión con el jugador solo una vez
        if (hasAttacked) return;  // Si ya atacó, no repetimos el ataque

        Collider2D playerCollider = Physics2D.OverlapBox(transform.position, new Vector2(halfWidth * 2, halfHeight * 2), 0, LayerMask.GetMask("Jugador"));

        if (playerCollider != null)
        {
            if (playerCollider.CompareTag("Jugador"))
            {
                // Activar el ataque una vez
                AttackPlayer(playerCollider.gameObject);
            }
        }
    }
    private void AttackPlayer(GameObject player)
    {
        if (isAttacking) return;  // Evitar ataques si ya está en ataque

        // Iniciar animación de ataque
        animator.SetBool("IsAttacking", true);  // Activar animación de ataque
        animator.SetBool("IsWalking", false);   // Asegurarse de que no camine durante el ataque

        // Detener movimiento durante el ataque
        isAttacking = true;
        hasAttacked = true;  // Marcar que ya ha atacado

        // Aquí puedes agregar la lógica para hacer daño al jugador
        // Ejemplo: player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

        // Esperar a que termine la animación del ataque y luego reiniciar
        StartCoroutine(ResumeMovementAfterAttack());
    }
    private IEnumerator ResumeMovementAfterAttack()
    {
        // Esperamos a que termine la animación de ataque
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Reanudar el movimiento
        isAttacking = false;
        animator.SetBool("IsAttacking", false);  // Desactivar animación de ataque

        // Permitir que el boss se mueva otra vez
        hasAttacked = false;  // Permitir futuros ataques
    }
}
