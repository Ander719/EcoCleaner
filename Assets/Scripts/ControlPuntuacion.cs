using UnityEngine;
using TMPro;

public class ControladorPuntuacion : MonoBehaviour
{
    public TextMeshProUGUI textoBasura;

    private int puntuacion = 0;

    void Start()
    {
        ActualizarTexto();
    }

    public void IncrementarPuntuacion(int cantidad)
    {
        puntuacion += cantidad;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        textoBasura.text = "X" + puntuacion;
    }

    public int GetBasura()
    {
        return puntuacion;
    }
}
