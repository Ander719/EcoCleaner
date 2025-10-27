using UnityEngine;
using UnityEngine.SceneManagement;
public class ControladorVolver : MonoBehaviour
{
    public void volver()
    {
        // Cargar la escena principal del juego
        SceneManager.LoadScene("Menu");
    }
}
