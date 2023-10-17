using UnityEngine;
using System;

public class GameAsset : MonoBehaviour
{
    public static GameAsset Instance;
    public GameObject m_SnakeBody;
    public SoundAudioClip[] soundAudioClipArray;    
   
    private void Awake()
    {
        Instance = this;
    }

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.ESoundType sound;
        public AudioClip audioClip;
    }
}
