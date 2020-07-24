using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioClip backgroundTheme = default;
    [SerializeField] AudioClip victoryTheme = default;
    [SerializeField] AudioClip defeatTheme = default;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = backgroundTheme;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void Victory()
    {
        musicSource.Pause();
        musicSource.loop = false;
        musicSource.clip = victoryTheme;
        musicSource.Play();
    }
    
    public void Defeat()
    {
        musicSource.Pause();
        musicSource.loop = false;
        musicSource.clip = defeatTheme;
        musicSource.Play();
    }
}
