using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource SFXSource;
    
    //Music
    public AudioClip lobbyMusic;
    public AudioClip huntMusic;
    
    //Combat
    public AudioClip attackSound;
    public AudioClip hurtSound;

    //Misc
    public AudioClip spawnSound; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        PlayMusic(lobbyMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}