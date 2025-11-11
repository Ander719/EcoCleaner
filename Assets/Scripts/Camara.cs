using UnityEngine;

public class CamaraSoloX : MonoBehaviour
{
    public Transform objetivo;   // El jugador
    public float alturaFija = 0f;
    public float suavizado = 5f;

    [Header("Límites del mapa")]
    public float minX = -10f;    // Límite izquierdo
    public float maxX = 10f;     // Límite derecho


    void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 nuevaPos = transform.position;

        // Seguir al jugador en el eje X
        nuevaPos.x = objetivo.position.x;

        // Mantener la altura fija
        nuevaPos.y = alturaFija;

        // Mantener el eje Z igual
        nuevaPos.z = transform.position.z;

        // Limitar dentro de los bordes del mapa
        nuevaPos.x = Mathf.Clamp(nuevaPos.x, minX, maxX);

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, nuevaPos, Time.deltaTime * suavizado);
    }
}
