using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    public int puntos = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            ControladorPuntuacion controlador = FindAnyObjectByType<ControladorPuntuacion>();
            if (controlador != null)
            {
                controlador.IncrementarPuntuacion(puntos);
            }

            Destroy(gameObject);
        }
    }
}
