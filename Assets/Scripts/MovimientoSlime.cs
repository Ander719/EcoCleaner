using UnityEngine;
using System.Collections;

public class BloqueMovil : MonoBehaviour
{
    [Header("Movimiento")]
    public Vector3 puntoA;
    public Vector3 puntoB;
    public float velocidad = 2f;
    public float tiempoEspera = 1f;

    [Header("Ataque")]
    public float duracionAtaque = 0.5f; // Tiempo que dura la animación

    private bool moviendoHaciaB = true;
    private bool esperando = false;
    private Animator animator;
    private bool atacando = false;

    void Start()
    {
        transform.position = puntoA;
        animator = GetComponent<Animator>();
        ActualizarDireccionVisual();
    }

    void Update()
    {
        if (esperando || atacando) return;

        Vector3 objetivo = moviendoHaciaB ? puntoB : puntoA;

        // Movimiento
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);

        // Animación caminar
        bool estaMoviendo = Vector3.Distance(transform.position, objetivo) > 0.01f;
        if (animator != null)
            animator.SetBool("isWalking", estaMoviendo);

        // Cambio de dirección al llegar
        if (transform.position == objetivo)
        {
            StartCoroutine(EsperarYCambiarDireccion());
        }
    }

    private IEnumerator EsperarYCambiarDireccion()
    {
        esperando = true;

        if (animator != null)
            animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(tiempoEspera);

        moviendoHaciaB = !moviendoHaciaB;
        ActualizarDireccionVisual();

        esperando = false;
    }

    private void ActualizarDireccionVisual()
    {
        if (moviendoHaciaB && puntoB.x > puntoA.x)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moviendoHaciaB && puntoB.x < puntoA.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (!moviendoHaciaB && puntoB.x > puntoA.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    // 🔥 Detecta colisión con el jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Jugador") && !atacando)
        {
            StartCoroutine(Atacar());
        }
    }

    private IEnumerator Atacar()
    {
        atacando = true;

        if (animator != null)
            animator.SetBool("isAttack", true);

        yield return new WaitForSeconds(duracionAtaque);

        if (animator != null)
            animator.SetBool("isAttack", false);

        atacando = false;
    }
}
