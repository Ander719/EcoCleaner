using UnityEngine;
using TMPro;

public class ControladorPuntuacionEnemigo : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuacion;
    private int puntuacion = 0;

    public void IncrementarPuntuacion(int cantidad)
    {
        puntuacion += cantidad;
        textoPuntuacion.text = puntuacion.ToString("D5");
    }
}
