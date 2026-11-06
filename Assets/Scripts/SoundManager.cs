using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audio;                                 // Hace sonar el AudioClip correspondiente de soundSO a través de un método.
    [SerializeField] SoundCollection soundSO;

    void Awake()
    {
        DontDestroyOnLoad(this);
        audio = GetComponent<AudioSource>();

    }

    void OnEnable()
    {
        soundSO.OnPlaySound += PlaySound;                      // Suscripción y desuscripción al evento de soundSO.
    }

    void OnDisable()
    {
        soundSO.OnPlaySound -= PlaySound;
    }

    void PlaySound(AudioClip clip)                             // Método para ejecutar el sonido.
    {
        audio.PlayOneShot(clip);
    }
}
