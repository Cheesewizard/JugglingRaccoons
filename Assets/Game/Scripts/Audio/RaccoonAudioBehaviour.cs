using JugglingRaccoons.Gameplay;
using JugglingRaccoons.Gameplay.Aiming;
using JugglingRaccoons.Gameplay.BalancingArrow;
using JugglingRaccoons.Gameplay.Juggling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Audio
{
    public class RaccoonAudioBehaviour : MonoBehaviour
    {
        [SerializeField, Required]
        private LocalPlayerBehaviour localPlayerBehaviour;

        [SerializeField]
        private AudioClip deathClip;

        [SerializeField]
        private AudioClip throwClip;

        [SerializeField]
        private AudioClip wheelSqueak;

        [SerializeField]
        private AudioClip throwFailed;

        [SerializeField]
        private AudioClip targetHitClip;

        private JugglingBehaviour jugglingBehaviour;
        private BalancingArrowBehaviour balancingArrowBehaviour;
        private ShootingInput shootingInput;

        private void Awake()
        {
            jugglingBehaviour = localPlayerBehaviour.JugglingBehaviour;
            balancingArrowBehaviour = localPlayerBehaviour.BalancingArrowBehaviour;
            shootingInput = localPlayerBehaviour.ShootingInput;
        }

        private void OnEnable()
        {
            jugglingBehaviour.OnBallThrown += HandleBallThrown;
            balancingArrowBehaviour.OnBalanceLost += HandleDeath;
            balancingArrowBehaviour.OnFallingLeft += HandleWheelSqueak;
            balancingArrowBehaviour.OnFallingRight += HandleWheelSqueak;
            shootingInput.OnTargetMissed += HandleThrowFailed;
            shootingInput.OnTargetHit += HandleOnTargetHit;
        }

        private void Start()
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("Audio Manager is missing from the scene!");
            }
        }

        private void HandleBallThrown(JuggleBall ball)
        {
            if (AudioManager.Instance == null) return;
            
            AudioManager.Instance.PlayClip(throwClip, true);
        }

        private void HandleDeath(int playerId)
        {
            if (AudioManager.Instance == null) return;
            
            AudioManager.Instance.PlayClip(deathClip, true);
        }

        private void HandleWheelSqueak(int playerId)
        {
            if (AudioManager.Instance == null) return;

            AudioManager.Instance.PlayClip(wheelSqueak, true, 0.1f);
        }

        private void HandleThrowFailed()
        {
            if (AudioManager.Instance == null) return;
            
            AudioManager.Instance.PlayClip(throwFailed, false, 0.3f);
        }

        private void HandleOnTargetHit()
        {
            if (AudioManager.Instance == null) return;
            
            AudioManager.Instance.PlayClip(targetHitClip,false, 0.3f);
        }

        private void OnDisable()
        {
            jugglingBehaviour.OnBallThrown -= HandleBallThrown;
            balancingArrowBehaviour.OnBalanceLost -= HandleDeath;
            balancingArrowBehaviour.OnFallingLeft -= HandleWheelSqueak;
            balancingArrowBehaviour.OnFallingRight -= HandleWheelSqueak;
            shootingInput.OnTargetMissed -= HandleThrowFailed;
            shootingInput.OnTargetHit -= HandleOnTargetHit;
        }
    }
}
