using System;
using JugglingRaccoons.Core;
using UnityEngine;

namespace JugglingRaccoons.Gameplay.Aiming
{
    public class ShootingInput : MonoBehaviour
    {

        [SerializeField]
        private PlayerInputHandler playerInputHandler;

        [SerializeField]
        private float minTargetDistance = 20f;

        [SerializeField]
        private float maxTargetDistance = 40f;

        [SerializeField]
        private float targetReductionPerBall = 5f;

        public event Action OnTargetHit;
        public event Action OnTargetMissed;

        private ShootingPointSpawner shootingPointSpawner;
        private float currentTargetDistance;

        private void Awake()
        {
            shootingPointSpawner = GetComponent<ShootingPointSpawner>();
            if (shootingPointSpawner != null)
            {
                currentTargetDistance = maxTargetDistance;
            }
        }

        private void Update()
        {
            if (playerInputHandler == null || !shootingPointSpawner.isActiveAndEnabled) return;

            bool isWithinTarget = shootingPointSpawner.CheckIfBetweenPoints(shootingPointSpawner.arrowPointer.transform.rotation.z); // Change this to whatever target angle is supposed to be
            if (playerInputHandler.ThrowActionPressed)
            {
                if (isWithinTarget)
                {
                    Debug.Log($"Player {playerInputHandler.PlayerId + 1} hit the target!");
                    DecrementTargetDistance();
                    OnTargetHit?.Invoke();
                }
                else
                {
                    Debug.Log($"Player {playerInputHandler.PlayerId + 1} missed the target!");
                    OnTargetMissed?.Invoke();
                }

                shootingPointSpawner.ClearTargetZone();
                shootingPointSpawner.Spawn(currentTargetDistance);
            }
        }

        private void DecrementTargetDistance()
        {
            currentTargetDistance -= targetReductionPerBall;
            currentTargetDistance = Mathf.Max(currentTargetDistance, minTargetDistance);
        }

        // TODO: Call this when you receive a ball
        private void IncrementTargetDistance()
        {
            currentTargetDistance += targetReductionPerBall;
            currentTargetDistance = Mathf.Min(currentTargetDistance, maxTargetDistance);
        }
    }
}