using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform objetivo;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 2f, -10f);

    [Header("Suavizado")]
    public float suavizado = 5f;

    [Header("Altura mínima")]
    public float alturaMinima = 0f;

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Posición deseada
        Vector3 posicionDeseada = objetivo.position + offset;

        // Limitar altura mínima
        if (posicionDeseada.y < alturaMinima)
            posicionDeseada.y = alturaMinima;

        // Suavizado con Lerp
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}
