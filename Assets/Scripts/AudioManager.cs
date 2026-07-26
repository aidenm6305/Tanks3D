using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip moveClip;
    public AudioClip rotateClip;
    public AudioClip shootClip;
    public AudioClip reloadClip;

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

    public void PlayMove(AudioSource source)
    {
        if (source != null && moveClip != null && !source.isPlaying)
        {
            source.pitch = .85f;
            source.clip = moveClip;
            source.Play();
        }
    }

    public void StopMove(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }

    public void PlayRotate(AudioSource source)
    {
        if (source != null && rotateClip != null && !source.isPlaying)
        {
            source.pitch = .85f;
            source.clip = rotateClip;
            source.Play();
        }
    }

    public void StopRotate(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }

    public void PlayShoot(AudioSource source)
    {
        if (source != null && shootClip != null)
        {
            source.pitch = Random.Range(0.7f, .9f);
            source.PlayOneShot(shootClip);
        }
    }

    public void PlayReload(AudioSource source)
    {
        if (source != null && reloadClip != null)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(reloadClip);
        }
    }
}
