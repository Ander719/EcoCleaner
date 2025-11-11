using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoPuntuacion;
    [SerializeField] private TextMeshProUGUI textoBasura;
    [SerializeField] private TextMeshProUGUI textoPuntuacionFinal;

    private int puntuacionObjetivo = 0;
    private float puntuacionActual = 0;

    private int basuraObjetivo = 0;
    private float basuraActual = 0;

    private int puntuacionFinalObjetivo = 0;
    private float puntuacionFinalActual = 0;

    private float velocidad = 50f;   // Velocidad del conteo

    // Fases: 1 = puntuación, 2 = basura, 3 = final
    private int fase = 1;

    void Start()
    {
        puntuacionObjetivo = DatosJuego.puntuacionEnemigo;
        basuraObjetivo = DatosJuego.puntuacionBasura;
        puntuacionFinalObjetivo = puntuacionObjetivo * basuraObjetivo;
        velocidad *= puntuacionFinalObjetivo < 1000 ? 100f : 300f;
    }

    void Update()
    {
        switch (fase)
        {
            case 1:
                ContarPuntuacion();
                break;
            case 2:
                ContarBasura();
                break;
            case 3:
                ContarPuntuacionFinal();
                break;
        }
    }

    void ContarPuntuacion()
    {
        puntuacionActual += velocidad * Time.deltaTime;

        if (puntuacionActual >= puntuacionObjetivo)
        {
            puntuacionActual = puntuacionObjetivo;
            fase = 2;   // pasa a basura
        }

        textoPuntuacion.text = "Puntuación: " + ((int)puntuacionActual).ToString("D5");
    }

    void ContarBasura()
    {
        basuraActual += velocidad * Time.deltaTime;

        if (basuraActual >= basuraObjetivo)
        {
            basuraActual = basuraObjetivo;
            fase = 3;   // pasa a puntuación final
        }

        textoBasura.text = "Basura: " + ((int)basuraActual).ToString("D2");
    }

    void ContarPuntuacionFinal()
    {
        puntuacionFinalActual += velocidad * Time.deltaTime;

        if (puntuacionFinalActual >= puntuacionFinalObjetivo)
        {
            puntuacionFinalActual = puntuacionFinalObjetivo;
        }

        textoPuntuacionFinal.text = "Puntuación Final: " + ((int)puntuacionFinalActual).ToString("D5");
    }
}
