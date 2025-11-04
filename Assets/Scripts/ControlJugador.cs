using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player2D : MonoBehaviour
{
    public PlayerSound playerSound;

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Ataque")]
    public float duracionAtaque = 0.3f;

    [Header("Daño")]
    public float duracionDanio = 0.5f;
    public float retroceso = 3f;
    public float reboteAlEnemigo = 8f; // rebote hacia arriba si cae encima del enemigo

    [Header("Altura mínima para morir")]
    public float alturaMinima = -0.8f; // El jugador no puede caer por debajo de esta altura

    private Rigidbody2D rb;
    private Animator animator;
    private bool enSuelo = true;
    private bool atacando = false;
    private bool recibiendoDanio = false;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() { }

    void Update()
    {
        VidaJugador vida = GetComponent<VidaJugador>();

        // Chequear altura mínima siempre
        if (vida != null && !vida.estaMuerto && transform.position.y < alturaMinima)
        {
            vida.Morir();

            // Mantener al jugador en Y mínima
            Vector3 pos = transform.position;
            pos.y = alturaMinima;
            transform.position = pos;

            // Salimos del Update para no procesar movimiento
            return;
        }

        // Si está recibiendo daño, atacando o muerto, no puede moverse ni saltar
        if (recibiendoDanio || atacando || (vida != null && vida.estaMuerto)) return;

        // Movimiento horizontal
        float moverHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moverHorizontal * velocidad, rb.linearVelocity.y);

        // Girar sprite según dirección
        if (moverHorizontal < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (moverHorizontal > 0) transform.localScale = new Vector3(1, 1, 1);

        // Animación caminar
        animator.SetBool("isWalking", Mathf.Abs(moverHorizontal) > 0.1f && enSuelo);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            playerSound.playSaltar();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
            animator.SetBool("isJumping", true);
        }

        // Ataque con Enter
        if (Input.GetKeyDown(KeyCode.Return) && !atacando && enSuelo)
        {
            playerSound.playAtaque();
            StartCoroutine(AtacarCoroutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Suelo
        if (collision.collider.CompareTag("Suelo") || collision.collider.CompareTag("Enemigo"))
        {
            enSuelo = true;
            animator.SetBool("isJumping", false);
        }

        // Enemigo
        if (collision.collider.CompareTag("Enemigo"))
        {
            if (atacando)
            {
                Enemigo e = collision.gameObject.GetComponent<Enemigo>();
                if (e != null) e.Morir();
                else Destroy(collision.gameObject);
            }
            else if (!recibiendoDanio)
            {
                if (transform.position.y > collision.transform.position.y + 0.3f)
                {
                    playerSound.playSaltar();
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, reboteAlEnemigo);
                }
                else
                {
                    playerSound.playRecibirDano();
                    StartCoroutine(RecibirDanio(collision));
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Suelo"))
            enSuelo = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo") && !recibiendoDanio)
        {
            StartCoroutine(RecibirDanio(null));
        }
        if (other.CompareTag("Item"))
        {
            playerSound.playRecoger();
        }
    }

    private System.Collections.IEnumerator AtacarCoroutine()
    {
        atacando = true;
        animator.SetBool("isAttacking", true);
        boxCollider.offset = new Vector2(0.3f, 0f);
        boxCollider.size = new Vector2(0.4f, 0.4f);

        yield return new WaitForSeconds(duracionAtaque);

        animator.SetBool("isAttacking", false);
        atacando = false;
        boxCollider.offset = new Vector2(0f, 0f);
        boxCollider.size = new Vector2(0.3f, 0.4f);
    }

    private System.Collections.IEnumerator RecibirDanio(Collision2D col)
    {
        VidaJugador vida = GetComponent<VidaJugador>();
        if (vida != null && vida.estaMuerto)
            yield break;

        recibiendoDanio = true;
        animator.SetBool("isDamage", true);

        if (col != null)
        {
            Vector2 direccion = (transform.position - col.transform.position).normalized;
            rb.linearVelocity = new Vector2(direccion.x * retroceso, rb.linearVelocity.y + 2f);
        }
        else
        {
            rb.linearVelocity = new Vector2(-transform.localScale.x * retroceso, rb.linearVelocity.y + 2f);
        }

        if (vida != null)
        {
            vida.RecibirDanio(10);
        }

        yield return new WaitForSeconds(duracionDanio);

        animator.SetBool("isDamage", false);
        recibiendoDanio = false;
    }
}
