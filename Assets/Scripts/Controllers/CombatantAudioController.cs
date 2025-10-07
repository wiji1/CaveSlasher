using System;
using Controllers;
using UnityEngine;

public class CombatantAudioController<T> : AudioController<T>, ICombatantAudioController<T> where T : Enum
{

    public CombatantAudioController(AudioSource audioSource, string path) : base(audioSource, path) { }

    public void PlaySound(CombatantSound sound)
    {
        var clip = Resources.Load<AudioClip>("Audio/" + _path + "/" + sound.ToString().ToLower());
        if (clip is null) throw new NullReferenceException();
        
        _audioSource.PlayOneShot(clip);
    }
}