using UnityEngine;
using TMPro;

public class ControladorPuntuacion : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuacion;

    void Start()
    {
        ActualizarTexto();
    }

    public void IncrementarPuntuacion(int cantidad)
    {
        GameManager.instance.puntuacion += cantidad;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        textoPuntuacion.text = "X" + GameManager.instance.puntuacion;
    }

    public int getBasura()
    {
        return GameManager.instance.puntuacion;
    }
}
