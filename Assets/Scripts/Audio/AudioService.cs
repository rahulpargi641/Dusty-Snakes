using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoSingletonGeneric<AudioService>
{
    [SerializeField] List<Sound> sounds = new List<Sound>();

    private Dictionary<SoundType, Sound> soundDictionary = new Dictionary<SoundType, Sound>();

    protected override void Awake()
    {
        base.Awake();

        foreach (Sound sound in sounds)
        {
            sound.Initialize(gameObject.AddComponent<AudioSource>());
            soundDictionary[sound.soundType] = sound;
        }
    }

    public void PlaySound(SoundType soundType)
    {
        if (soundDictionary.ContainsKey(soundType))
            soundDictionary[soundType].Play();
        else
            Debug.LogWarning("Sound not found: " + soundType);
    }

    public void StopSound(SoundType soundType)
    {
        if (soundDictionary.ContainsKey(soundType))
            soundDictionary[soundType].Stop();
        else
            Debug.LogWarning("Sound not found: " + soundType);
    }
}
