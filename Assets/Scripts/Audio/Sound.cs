using UnityEngine;


[System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;

    private AudioSource audioSource;

    public void Initialize(AudioSource source)
    {
        this.audioSource = source;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

    public void Play()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    public void Pause()
    {
        if (audioSource != null)
            audioSource.Pause();
    }

    public void UnPause()
    {
        if (audioSource != null)
            audioSource.UnPause();
    }

    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}

public enum SoundType
{
    ButtonClick,
    AteFood,
    PowerupShiledPickup,
    PowerupScoreBoosterPickup,
    PowerupSpeedUpPickup,
    Death,
    SnakeCollide
}
