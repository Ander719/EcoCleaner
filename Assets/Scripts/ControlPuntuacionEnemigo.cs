// ControladorPuntuacionEnemigo.cs
using UnityEngine;
using TMPro;

public class ControladorPuntuacionEnemigo : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuacion;

    void Start()
    {
        ActualizarTexto();
    }

    public void IncrementarPuntuacion(int cantidad)
    {
        GameManager.instance.puntuacionEnemigos += cantidad;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (textoPuntuacion != null)
            textoPuntuacion.text = "" + GameManager.instance.puntuacionEnemigos;
    }

    public int getPuntuacion()
    {
        return GameManager.instance.puntuacionEnemigos;
    }
}
