using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JugglingRaccoons
{
    public class MenuAudioBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioClip navigation;

        [SerializeField]
        private AudioClip selection;

        private void Start()
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("Audio Manager is missing from the scene!");
            }
        }

        public void HandleNavigation()
        {
            if (AudioManager.Instance == null) return;

            AudioManager.Instance.PlayClip(navigation);
        }

        public void HandleSelection()
        {
            if (AudioManager.Instance == null) return;

            AudioManager.Instance.PlayClip(selection);
        }
    }
}
