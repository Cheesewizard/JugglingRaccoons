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

        [SerializeField, Range(0f, 20f)]
        private float maxMissedShotUnbalanceValue = 20f;

        [SerializeField, Range(0f, 20f)]
        private float minMissedShotUnbalanceValue = 10f;

        [SerializeField, Range(0f, 10f)]
        private float missedShotAngleIncrement = 2f;

        private float currentMissedShotUnbalanceValue = 0.0f;

        private void OnEnable()
        {
            currentMissedShotUnbalanceValue = minMissedShotUnbalanceValue;
            if (shootingInput != null)
            {
                shootingInput.OnTargetMissed += ApplyUnbalanceForce;
                shootingInput.OnTargetHit += DecrementMissedShotUnbalanceValue;
            }
        }

        private void ApplyUnbalanceForce()
        {
            if (balancingArrowBehaviour != null)
            {
                var randNum = Random.Range(-1f, 1f);
                var sign = randNum / Mathf.Abs(randNum);
                balancingArrowBehaviour.ApplyRotationForce(currentMissedShotUnbalanceValue * sign);
            }
        }

        private void OnDisable()
        {
            if (shootingInput != null)
            {
                shootingInput.OnTargetMissed -= ApplyUnbalanceForce;
                shootingInput.OnTargetHit -= DecrementMissedShotUnbalanceValue;
            }
        }

        // TODO: Call this when the player receives another ball
        private void IncrementMissedShotUnbalanceValue()
        {
            currentMissedShotUnbalanceValue += missedShotAngleIncrement;
            currentMissedShotUnbalanceValue = Mathf.Min(currentMissedShotUnbalanceValue, maxMissedShotUnbalanceValue);
        }

        private void DecrementMissedShotUnbalanceValue()
        {
            currentMissedShotUnbalanceValue -= missedShotAngleIncrement;
            currentMissedShotUnbalanceValue = Mathf.Max(currentMissedShotUnbalanceValue, minMissedShotUnbalanceValue);
        }
    }
}