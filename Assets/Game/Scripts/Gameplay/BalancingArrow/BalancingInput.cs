using JugglingRaccoons.Gameplay.Aiming;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Gameplay.BalancingArrow
{
    public class BalancingInput : MonoBehaviour
    {
        [SerializeField]
        private ShootingInput shootingInput;

        [SerializeField]
        private BalancingArrowBehaviour balancingArrowBehaviour;

        [SerializeField, Range(0f, 10f)]
        private float missedShotUnbalanceValue = 5f;

        private void OnEnable()
        {
            if (shootingInput != null)
            {
                shootingInput.OnTargetMissed += OnTargetMissed;
            }
        }

        private void OnTargetMissed()
        {
            if (balancingArrowBehaviour != null)
            {
                balancingArrowBehaviour.ApplyRotationForce(Random.Range(-missedShotUnbalanceValue, missedShotUnbalanceValue));
            }
        }

        private void OnDisable()
        {
            if (shootingInput != null)
            {
                shootingInput.OnTargetMissed -= OnTargetMissed;
            }
        }
    }
}