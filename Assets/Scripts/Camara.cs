using UnityEngine;

public class CamaraSoloX : MonoBehaviour
{
    public Transform objetivo;   // Jugador
    public float alturaFija = 0f;
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 nuevaPos = transform.position;

        // Seguir solo eje X
        nuevaPos.x = objetivo.position.x;

        // Mantener Y fija
        nuevaPos.y = alturaFija;

        // Mantener Z
        nuevaPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, nuevaPos, Time.deltaTime * suavizado);
    }
}
