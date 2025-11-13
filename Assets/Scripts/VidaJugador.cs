using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 9;   // Vida total del jugador
    [HideInInspector] public int vidaActual;         // Vida actual
    public Slider barraVida;       // Slider de la barra de vida
    [HideInInspector] public bool estaMuerto = false;

    private void Start()
    {
        // Inicializamos la vida al comenzar el juego
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    // M�todo para recibir da�o
    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        ActualizarBarra();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // Actualiza la barra en pantalla
    private void ActualizarBarra()
    {
        if (barraVida != null)
            barraVida.value = (float)vidaActual / vidaMaxima;
    }

    // Qu� pasa cuando la vida llega a 0
    public void Morir()
    {
        estaMuerto = true;

        // Bloquear controles
        Player2D player = GetComponent<Player2D>();
        if (player != null)
            player.enabled = false;

        // Animación de muerte
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("isDead", true);

        // rb y collider
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (rb != null && box != null)
        {
            // Mantener gravedad para que caiga si estaba en el aire
            rb.gravityScale = 2f;

            // Congelar movimiento horizontal y rotación
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            // Definir el radio de detección
            float radioDeDeteccion = 5f;
            Vector2 posicion = transform.position;

            Collider2D[] enemigosCercanos = Physics2D.OverlapCircleAll(posicion, radioDeDeteccion);

            foreach (var enemigo in enemigosCercanos)
            {
                if (enemigo.CompareTag("Enemigo"))
                    Physics2D.IgnoreCollision(enemigo, box);
            }
        }

        // Guardar puntuaciones
        ControladorPuntuacionEnemigo puntaje = FindAnyObjectByType<ControladorPuntuacionEnemigo>();
        ControladorPuntuacion puntajebasura = FindAnyObjectByType<ControladorPuntuacion>();
        if (puntaje != null && puntajebasura != null)
        {
            DatosJuego.puntuacionEnemigo = puntaje.GetPuntuacion();
            DatosJuego.puntuacionBasura = puntajebasura.GetBasura();
        }

        // Esperar animación antes de cargar GameOver
        float duracionAnimacion = 2.5f;
        Invoke(nameof(CargarGameOver), duracionAnimacion);
    }

    // M�todo para cargar la escena de Game Over
    private void CargarGameOver()
    {
        SceneManager.LoadScene("GameOver"); // Aseg�rate que coincide con el nombre de tu escena
    }
}
