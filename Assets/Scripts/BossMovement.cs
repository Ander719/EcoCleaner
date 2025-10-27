using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int startDirection = 1;

    private int currentDirection;
    private float halfWidth;
    private Vector2 movement;

    [HideInInspector] private bool isAttacking = false;

    private void Start()
    {
        halfWidth = spriteRenderer.bounds.extents.x;
        currentDirection = startDirection;
        spriteRenderer.flipX = currentDirection != -1;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Jugador"))
        {
            AttackPlayer();
        }
    }
    private void FixedUpdate()
    {
        if (!isAttacking)
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
        if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")) && rigidBody.linearVelocity.x < 0)
        {
            currentDirection *= -1;
            spriteRenderer.flipX = true;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")) && rigidBody.linearVelocity.x > 0)
        {
            currentDirection = -1;
            spriteRenderer.flipX = false;
        }
    }
    private void AttackPlayer()
    {
        // Iniciar animación de ataque
        animator.SetBool("IsAttacking", true); // Activar animación de ataque
        animator.SetBool("IsWalking", false);  // Asegurarse de que no camine durante el ataque

        // Detener movimiento durante el ataque
        isAttacking = true;

        // Aquí puedes agregar la lógica para hacer daño al jugador
        // Si el ataque implica daño directo, por ejemplo:
        // other.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

        // Esperar a que termine la animación de ataque para reanudar el movimiento
        StartCoroutine(ResumeMovementAfterAttack());
    }

    // Coroutine para reanudar el movimiento después de la animación de ataque
    private IEnumerator ResumeMovementAfterAttack()
    {
        // Esperamos a que termine la animación de ataque
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Reanudar el movimiento y permitir caminar
        isAttacking = false;

        // Después del ataque, podemos permitir que el boss camine otra vez
        animator.SetBool("IsAttacking", false);
    }
}
