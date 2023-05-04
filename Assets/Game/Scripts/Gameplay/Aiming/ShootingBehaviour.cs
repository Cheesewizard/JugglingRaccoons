using System;
using JugglingRaccoons.Core;
using JugglingRaccoons.Core.GameStates;
using Quack.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Gameplay.Aiming
{
    public class ShootingBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputHandler playerInputHandler;

        [SerializeField]
        private ShootingVisual shootingVisual;

        [SerializeField]
        private float minValidRange = 20f;

        [SerializeField]
        private float maxValidRange = 40f;

        [SerializeField, Range(0, 45)]
        float edgeMargin = 10f;
        
        [SerializeField]
        private float angleStepPerBall = 5f;

        [SerializeField]
        private float arrowSpeed = 1f;

        public event Action OnTargetHit;
        public event Action OnTargetMissed;
        
        private RemapValue arrowRemapper => new(0, arrowSpeed, 0, 180);

        private bool ignoreInput = true;
        private float currentArrowAngle;
        private float currentTargetRange;
        private float firstRangeAngle;
        private float secondRangeAngle;
        
        private void Awake()
        {
            Cleanup();
            GameplayState.OnGameplayStateExited += Cleanup;
            GameplayState.OnGameplayStart += Initialize;
        }

        private void Initialize()
        {
            shootingVisual.gameObject.SetActive(true);
            currentTargetRange = maxValidRange;
            shootingVisual.ClearTargetZone();
            GenerateAngle();
            shootingVisual.UpdateRangeVisual(firstRangeAngle, secondRangeAngle);
        }

        private void Cleanup()
        {
            shootingVisual.ClearTargetZone();
            shootingVisual.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (playerInputHandler == null || !shootingVisual.isActiveAndEnabled) return;
            
            if (playerInputHandler.ThrowActionPressed)
            {
                if (ValidateShotAngle(currentArrowAngle))
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
                
                shootingVisual.ClearTargetZone();
                //TODO: put a delay
                GenerateAngle();
                shootingVisual.UpdateRangeVisual(firstRangeAngle, secondRangeAngle);
            }
            
            currentArrowAngle = arrowRemapper.Evaluate(Mathf.PingPong(Time.time, arrowSpeed));
            shootingVisual.UpdateArrowVisual(RemapAngle(currentArrowAngle));
        }

        private void DecrementTargetDistance()
        {
            currentTargetRange -= angleStepPerBall;
            currentTargetRange = Mathf.Max(currentTargetRange, minValidRange);
        }

        // TODO: Call this when you receive a ball
        private void IncrementTargetDistance()
        {
            currentTargetRange += angleStepPerBall;
            currentTargetRange = Mathf.Min(currentTargetRange, maxValidRange);
        }

        private bool ValidateShotAngle(float shotAngle)
        {
            Debug.Log($"FIRST: {firstRangeAngle} SHOT: {shotAngle} SECOND: {secondRangeAngle}");
            return shotAngle > firstRangeAngle && shotAngle < secondRangeAngle || shotAngle < firstRangeAngle && shotAngle > secondRangeAngle;
        }
		
        [Button(ButtonSizes.Medium, DrawResult = false)]
        private void GenerateAngle()
        {
            firstRangeAngle = Random.Range(edgeMargin, 180 - edgeMargin);
            secondRangeAngle = firstRangeAngle + currentTargetRange;
            secondRangeAngle = secondRangeAngle < 180 - edgeMargin ? secondRangeAngle : secondRangeAngle - currentTargetRange * 2;
        }
        
        public static float RemapAngle(float angle, bool flip = false, RemapStrategy remapStrategy = RemapStrategy.None)
        {
            var remappedAngle = angle - 90 * (flip ? -1 : 1);
            switch (remapStrategy)
            {
                case RemapStrategy.Deg2Rad:
                    remappedAngle *= Mathf.Deg2Rad;
                    break;
                case RemapStrategy.Rad2Deg:
                    remappedAngle *= Mathf.Rad2Deg;
                    break;
            }
            return remappedAngle;
        }
    }
    
    public enum RemapStrategy
    {
        None,
        Deg2Rad,
        Rad2Deg,
    }
}