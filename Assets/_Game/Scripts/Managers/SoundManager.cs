using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClips;

    /// <summary>
    /// Plays the audio clip based on the SFXType.
    /// </summary>
    /// <param name="sfxType"></param>
    public void PlaySFX(SFXType sfxType)
    {
        _audioSource.PlayOneShot(_audioClips[(byte)sfxType]);
    }
}
