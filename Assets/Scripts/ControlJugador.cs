using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player2D : MonoBehaviour
{
    public PlayerSound playerSound;

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public int vidaP = 10;

    [Header("Ataque")]
    public float duracionAtaque = 0.3f;

    [Header("Daño")]
    public float duracionDanio = 0.5f;
    public float retroceso = 3f;
    public float reboteAlEnemigo = 8f;

    [Header("Altura mínima para morir")]
    public float alturaMinima = -0.8f;

    [Header("Efecto Vino")]
    public float velocidadReducida = 2f;
    private float velocidadOriginal;

    [Header("HUD Vino")]
    public GameObject panelVino;
    public TextMeshProUGUI cuentaAtras;
    private float tiempoVino = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool enSuelo = true;
    private bool atacando = false;
    private bool recibiendoDanio = false;
    private BoxCollider2D boxCollider;
    private bool controlesInvertidos = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        velocidadOriginal = velocidad;
    }

    void Update()
    {
        VidaJugador vida = GetComponent<VidaJugador>();

        // Chequear altura mínima
        if (vida != null && !vida.estaMuerto && transform.position.y < alturaMinima)
        {
            vida.Morir();
            Vector3 pos = transform.position;
            pos.y = alturaMinima;
            transform.position = pos;
            return;
        }

        // Bloquear movimiento si recibe daño o muere
        if (recibiendoDanio || atacando || (vida != null && vida.estaMuerto)) return;

        // Movimiento horizontal
        float moverHorizontal = Input.GetAxis("Horizontal");
        if (controlesInvertidos)
            moverHorizontal *= -1;

        rb.linearVelocity = new Vector2(moverHorizontal * velocidad, rb.linearVelocity.y);

        // Girar sprite
        if (moverHorizontal < 0) transform.localScale = new Vector3(-3, 3, 3);
        else if (moverHorizontal > 0) transform.localScale = new Vector3(3, 3, 3);

        // Animación caminar
        animator.SetBool("isWalking", Mathf.Abs(moverHorizontal) > 0.1f && enSuelo);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Intento de salto: enSuelo = {enSuelo}, Velocidad actual Y = {rb.linearVelocity.y}");
        }

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Debug.Log("✅ SALTO EJECUTADO");
            playerSound.playSaltar();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
            animator.SetBool("isJumping", true);
            Debug.Log($"Velocidad después del salto: {rb.linearVelocity}");
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.Return) && !atacando && enSuelo)
        {
            playerSound.playAtaque();
            StartCoroutine(AtacarCoroutine());
        }
    }

   private void OnCollisionEnter2D(Collision2D collision)
{
    // ✅ Detectar suelo incluso con CompositeCollider2D
    if (
        collision.collider.CompareTag("Suelo") ||
        (collision.collider.transform.parent != null && collision.collider.transform.parent.CompareTag("Suelo")) ||
        collision.collider.CompareTag("Enemigo")
    )
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
    if (
        collision.collider.CompareTag("Suelo") ||
        (collision.collider.transform.parent != null && collision.collider.transform.parent.CompareTag("Suelo"))
    )
        enSuelo = false;
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vino"))
        {
            VidaJugador vida = GetComponent<VidaJugador>();
            if (vida != null)
            {
                vida.RecibirDanio(1);
            }
            StartCoroutine(EfectoVino());
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Enemigo") && !recibiendoDanio)
        {
            StartCoroutine(RecibirDanio(null));
        }
        if (other.CompareTag("Item"))
        {
            playerSound.playRecoger();
        }
        if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene("ander");
        }
    }
    private IEnumerator EfectoVino()
    {
        controlesInvertidos = true;
        velocidad = velocidadReducida;

        // Activar HUD
        if (panelVino != null)
            panelVino.SetActive(true);

        float tiempoRestante = tiempoVino;

        while (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            if (cuentaAtras != null)
                cuentaAtras.text = tiempoRestante.ToString("F1");  // Ej: 9.5

            yield return null;
        }

        // Restaurar valores
        controlesInvertidos = false;
        velocidad = velocidadOriginal;

        // Apagar HUD
        if (panelVino != null)
            panelVino.SetActive(false);
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
            vida.RecibirDanio(3);
        }

        yield return new WaitForSeconds(duracionDanio);
        animator.SetBool("isDamage", false);
        recibiendoDanio = false;

        if (vidaP < 1)
        {
            yield return new WaitForSeconds(5);
            playerSound.playMuerte();
            SceneManager.LoadScene("GameOver");
        }
    }
}
