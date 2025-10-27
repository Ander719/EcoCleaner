using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Nombre de la escena a cargar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu jugador tenga el tag "Player"
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
