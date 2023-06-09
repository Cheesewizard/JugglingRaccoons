using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource sfxSource;
        
        [SerializeField]
        private AudioSource rawSfxSource;

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);
        }

        public void PlayTrack(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlayClip(AudioClip clip, bool useRandomPitch = false, float volume = 1f)
        {
            if (!useRandomPitch)
            {
                rawSfxSource.PlayOneShot(clip, volume);
                return;
            }
            
            sfxSource.pitch = useRandomPitch ? Random.Range(0.8f, 1.2f) : 1f;
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}