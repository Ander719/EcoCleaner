using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 9;   // Vida total del jugador
    public int vidaActual;         // Vida actual
    public Slider barraVida;       // Slider de la barra de vida
    public bool estaMuerto = false;

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
    private void Morir()
    {

        estaMuerto = true;

        // Bloquear controles
        Player2D player = GetComponent<Player2D>();
        if (player != null) player.enabled = false;

        // Animaci�n de muerte
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.SetBool("isDead", true);

        // Hacer trigger y detener gravedad para que no caiga
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null) box.isTrigger = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        // ?? Cargar la escena de Game Over despu�s de la animaci�n
        ControladorPuntuacionEnemigo puntaje = FindAnyObjectByType<ControladorPuntuacionEnemigo>();
        ControladorPuntuacion puntajebasura = FindAnyObjectByType<ControladorPuntuacion>();
        if (puntaje != null && puntajebasura != null)
            DatosJuego.puntuacionEnemigo = puntaje.getPuntuacion();
            DatosJuego.puntuacionBasura = puntajebasura.getBasura();

        // Esperamos animación
        float duracionAnimacion = 2.5f;
        Invoke("CargarGameOver", duracionAnimacion);
    }

    // M�todo para cargar la escena de Game Over
    private void CargarGameOver()
    {
        SceneManager.LoadScene("GameOver"); // Aseg�rate que coincide con el nombre de tu escena
    }
}
