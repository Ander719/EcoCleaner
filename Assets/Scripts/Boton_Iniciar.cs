using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    public void IniciarJuego()
    {
        // 1. REINICIAR DATOS: Llama a la función del GameManager para poner la puntuación a cero.
        // Se asegura de que el GameManager exista antes de intentar resetear.
        if (GameManager.instance != null)
        {
            GameManager.instance.ResetearDatos();
        }

        // 2. Cargar la escena principal del juego
        SceneManager.LoadScene("Nivel1");
    }
}