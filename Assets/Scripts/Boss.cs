using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Animator animator;


    public float speed = 2f;
    public int startDirection = 1;
    public bool stayOnEdge = true;

    private bool isAttacking = false;

    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    private bool isGrounded;

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

        if (rigidBody.linearVelocity.x < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left,halfWidth + 0.11f, LayerMask.GetMask("Ground")))
            {
                FlipObject();
            }
            else if (stayOnEdge && !Physics2D.Raycast(leftPos,Vector2.down,halfHeight + 0.1f,LayerMask.GetMask("Ground")))
            {
                FlipObject();
            }
            
        }
        else if (rigidBody.linearVelocity.x > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right,halfWidth + 0.11f, LayerMask.GetMask("Ground")))
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
    public void StartAttack(GameObject player)
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsWalking", false);

        rigidBody.linearVelocity = Vector2.zero; // se queda quieto al atacar

        Debug.Log("Atacando al jugador...");
        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack()
    {
        // Espera a que termine la animación del ataque
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }
}
