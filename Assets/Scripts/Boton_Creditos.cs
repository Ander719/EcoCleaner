using UnityEngine;
using UnityEngine.SceneManagement;
public class ControladorCreditos : MonoBehaviour
{
    public void Creditos()
    {
        // Cargar la escena principal del juego
        SceneManager.LoadScene("Creditos");
    }
}
