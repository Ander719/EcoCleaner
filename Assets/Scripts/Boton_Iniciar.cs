using UnityEngine;
using UnityEngine.SceneManagement;
public class ControladorMenu : MonoBehaviour
{
    public void IniciarJuego()
    {
        // Cargar la escena principal del juego
        SceneManager.LoadScene("marcos");
    }
}
