using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int puntos = 100;    // cantidad a sumar
    private ControladorPuntuacionEnemigo controlador;

    void Start()
    {
        controlador = FindAnyObjectByType<ControladorPuntuacionEnemigo>();
    }

    public void Morir()
    {
        if (controlador != null)
        {
            controlador.IncrementarPuntuacion(puntos);
        }

        Destroy(gameObject);
    }
}
