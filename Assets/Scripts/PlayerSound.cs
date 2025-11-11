using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoRecibirDano;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoRecogerItem;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void playSaltar()
    {
        if (audioManager == null || !audioManager.audioDesactivado())
            audioSource.PlayOneShot(sonidoSaltar);
    }

    public void playRecibirDano()
    {
        if (audioManager == null || !audioManager.audioDesactivado())
            audioSource.PlayOneShot(sonidoRecibirDano);
    }

    public void playMuerte()
    {
        if (audioManager == null || !audioManager.audioDesactivado())
            audioSource.PlayOneShot(sonidoMuerte);
    }

    public void playAtaque()
    {
        if (audioManager == null || !audioManager.audioDesactivado())
            audioSource.PlayOneShot(sonidoAtaque);
    }

    public void playRecoger()
    {
        if (audioManager == null || !audioManager.audioDesactivado())
            audioSource.PlayOneShot(sonidoRecogerItem);
    }
}
