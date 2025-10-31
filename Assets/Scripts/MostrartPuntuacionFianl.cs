using TMPro;
using UnityEngine;

public class MostrarPuntuacionFinal : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuacion;
    public TextMeshProUGUI textoBasura;

    void Start()
    {
        textoPuntuacion.text = DatosJuego.puntuacionEnemigo.ToString("D5");
        textoBasura.text = DatosJuego.puntuacionBasura.ToString("D2");
    }
}
