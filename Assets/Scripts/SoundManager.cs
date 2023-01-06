using UnityEngine;

public class SoundManager
{
    public enum ESound
    {
        ButtonClick, AteFood, PowerupShiledPickup, PowerupScoreBoosterPickup, PowerupSpeedUpPickup, DeathVoice, SnakeCollide
    }
    public static void PlaySound(ESound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }
     
    static AudioClip GetAudioClip(ESound sound)
    {
        foreach(GameAsset.SoundAudioClip soundAudioClip in GameAsset.Instance.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Audio Clip not found!: " + sound);
        return null;
    }
}
