using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceAmbient
        ;

    private void Awake()
    {
        // Ako već postoji instanca – uništi duplikat
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //audioSourceMusic = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!audioSourceMusic.isPlaying)
        {
            audioSourceMusic.loop = true;
            audioSourceMusic.Play();
        }
    }

    // Optional helpers
    public void PlayMusic()
    {
        if (!audioSourceMusic.isPlaying)
            audioSourceMusic.Play();
    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }

    public void SetVolume(float value)
    {
        audioSourceMusic.volume = value;
    }
}
