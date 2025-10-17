using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Ataque")]
    public float duracionAtaque = 0.3f; // duración del ataque en segundos, ajustar según animación

    private Rigidbody2D rb;
    private Animator animator;
    private bool enSuelo = true;
    private bool atacando = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento horizontal
        float moverHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moverHorizontal * velocidad, rb.linearVelocity.y);

        // Girar sprite según dirección
        if (moverHorizontal < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (moverHorizontal > 0) transform.localScale = new Vector3(1, 1, 1);

        // Animación caminar (solo si está en el suelo y no está atacando)
        animator.SetBool("isWalking", Mathf.Abs(moverHorizontal) > 0.1f && enSuelo && !atacando);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
            animator.SetBool("isJumping", true);
        }

        // Ataque con Enter
        if (Input.GetKeyDown(KeyCode.Return) && !atacando)
        {
            StartCoroutine(AtacarCoroutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Suelo"))
        {
            enSuelo = true;
            animator.SetBool("isJumping", false); // detener salto
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }

    // Corutina que controla el ataque
    private System.Collections.IEnumerator AtacarCoroutine()
    {
        atacando = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(duracionAtaque); // duración exacta del ataque
        animator.SetBool("isAttacking", false);
        atacando = false;
    }
}
