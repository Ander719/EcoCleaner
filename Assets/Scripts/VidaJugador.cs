using UnityEngine;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 9;   // Vida total del jugador
    public int vidaActual;         // Vida actual
    public Slider barraVida;       // Slider de la barra de vida

    private void Start()
    {
        // Inicializamos la vida al comenzar el juego
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    // Método para recibir daño
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

    // Qué pasa cuando la vida llega a 0
    private void Morir()
    {
        Debug.Log("Jugador murió");
        // Aquí puedes poner animación de muerte o reiniciar el nivel
    }
}
