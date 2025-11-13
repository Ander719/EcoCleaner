using UnityEngine;
using TMPro;

public class ControladorPuntuacion : MonoBehaviour
{
    public TextMeshProUGUI textoBasura;

    void Start()
    {
        ActualizarTexto();
    }

    public void IncrementarPuntuacion(int cantidad)
    {
        // Usamos GameManager para persistir la puntuación
        GameManager.instance.puntuacion += cantidad;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (textoBasura != null)
            textoBasura.text = "X" + GameManager.instance.puntuacion;
    }

    public int GetBasura()
    {
        return GameManager.instance.puntuacion;
    }
}
