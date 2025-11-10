using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public Image[] corazones;       // Tus 3 imágenes de corazones
    public Sprite corazonLlena;     // Sprite de corazón lleno
    public Sprite corazonVacio;     // Sprite de corazón vacío
    public int vidaMaxima = 3;      // Máximo de corazones

    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarUI();
    }

    // Llamar esto cuando recibas daño
    public void RecibirDaño(int dano)
    {
        vidaActual -= dano;
        if (vidaActual < 0) vidaActual = 0;
        ActualizarUI();
    }

    // Actualiza los sprites de los corazones
    private void ActualizarUI()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].sprite = i < vidaActual ? corazonLlena : corazonVacio;
        }
    }
    // Llama a esto para recuperar corazones

}
