using UnityEngine;

namespace JugglingRaccoons
{
    public class CountdownAudioBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioClip countdownSfx;

        [SerializeField]
        private AudioClip countdownEndSfx;

        private void Start()
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("Audio Manager is missing from the scene!");
            }
        }

        public void PlayCountdownAudio()
        {
            if (AudioManager.Instance == null) return;

            AudioManager.Instance.PlayClip(countdownSfx, volume: 0.45f);
        }

        public void PlayCountdownEndAudio()
        {
            if (AudioManager.Instance == null) return;

            AudioManager.Instance.PlayClip(countdownEndSfx, volume: 0.8f);
        }
    }
}
