using System;
using UnityEngine;

namespace Controllers
{
    public class AudioController<T> : IAudioController<T> where T : Enum
    {
        protected readonly AudioSource _audioSource;
        protected readonly string _path;

        public AudioController(AudioSource audioSource, string path)
        {
            _audioSource = audioSource;
            _path = path;
        }
        
        public void PlaySound(T sound)
        {
            var clip = GetClip(sound);
            if (clip is null) throw new NullReferenceException();
            
            _audioSource.PlayOneShot(clip);
        }

        public void PlayContinuous(T sound)
        {
            var clip = GetClip(sound);
            if (clip is null)
            {
                MonoBehaviour.print("Clip is null for sound: " + sound);
                throw new NullReferenceException();
            }
            
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void Pause()
        {
            _audioSource.Stop();
        }

        public void Resume()
        {
            _audioSource.Play();
        }

        private AudioClip GetClip(T sound)
        {
            return Resources.Load<AudioClip>("Audio/" + _path + "/" + sound.ToString().ToLower());
        }
    }
}