using UnityEngine;
using TMPro;
public class ControladorPuntuacion : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuacion;
    private int puntuacion = 0;
    public void IncrementarPuntuacion(int cantidad)
    {
        puntuacion += cantidad;
        textoPuntuacion.text = "X" + puntuacion;
    }
}
