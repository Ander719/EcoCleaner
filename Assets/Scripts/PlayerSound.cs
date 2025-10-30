using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoRecibirDano;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoAtaque;

    public void playSaltar()
    {
        audioSource.PlayOneShot(sonidoSaltar);
    }
    public void playRecibirDano()
    {
        audioSource.PlayOneShot(sonidoRecibirDano);
    }
    public void playMuerte()
    {
        audioSource.PlayOneShot(sonidoMuerte);
    }
    public void playAtaque()
    {
        audioSource.PlayOneShot(sonidoAtaque);
    }
}
