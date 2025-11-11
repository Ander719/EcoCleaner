using UnityEngine;
using UnityEngine.SceneManagement;
public class ControladorOpciones : MonoBehaviour
{
    public void Opciones()
    {
        // Cargar la escena principal del juego
        SceneManager.LoadScene("Opciones");
    }
}
