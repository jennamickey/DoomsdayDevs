
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource loopingSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip roundstart;
    [SerializeField] private AudioClip gunshot;
    [SerializeField] private AudioClip zombies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        loopingSource.clip = zombies;
        loopingSource.loop = true;
        loopingSource.playOnAwake = false;
    }

    public void PlayRoundStart()
    {
        effectsSource.PlayOneShot(roundstart);
    }

    public void PlayGunshot()
    {
        effectsSource.PlayOneShot(gunshot);
    }

    public void PlayZombieSounds()
    {
        if (!loopingSource.isPlaying)
        {
            loopingSource.UnPause();

            if (!loopingSource.isPlaying)
                loopingSource.Play();
        }
    }

    public void Stopzombies()
    {
        loopingSource.Stop();
    }
}
