using System;
using JugglingRaccoons.Core;
using JugglingRaccoons.Core.GameStates;
using JugglingRaccoons.Gameplay.Aiming;
using JugglingRaccoons.Gameplay.Juggling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Gameplay.Balancing
{
	public class BalancingBehaviour : MonoBehaviour
	{
		[SerializeField]
		private PlayerInputHandler playerInputHandler;

		[SerializeField]
		private LocalPlayerBehaviour localPlayerBehaviour;

		[SerializeField]
		private Transform seatAndPoleTransform;
		
		[SerializeField, Range(0, 360)]
		private float angleLimit = 45f;

		[SerializeField, Range(0, 360)]
		private float dangerZoneAngle = 25f;

		[SerializeField]
		private float inputStrength = 70f;

		[SerializeField]
		private float defaultUnbalanceAmount = 30f;

		[SerializeField]
		private float maxUnbalanceAmount = 70f;

		[SerializeField]
		private float unbalanceIncrementAmount = 10f;
		
		[SerializeField]
		private float gravityStrength = 100f;

		[SerializeField]
		private float rotationFollowSpeed = 100f;

		[SerializeField]
		private float missedShotImpulseRotation = 10f;

		[SerializeField]
		private float ballCaughtImpulseRotation = 10f;

		private JugglingBehaviour jugglingBehaviour;
		private ShootingBehaviour shootingInput;
		private float currentUnbalanceAmount = 0.0f;
		private float currentRotation = 0.0f;
		private bool hasLostBalance = false;
		private bool inDangerZone = false;
		private bool goingRight = false;
		private bool applyUnbalance = false;

		public event Action<int> OnBalanceLost;
		public event Action<int> OnDangerZoneEnter;
		public event Action<int> OnDangerZoneExit;
		public event Action<int> OnFallingLeft;
		public event Action<int> OnFallingRight;

		private void Start()
		{
			// Juggling events
			jugglingBehaviour = localPlayerBehaviour.JugglingBehaviour;
			if (jugglingBehaviour)
			{
				jugglingBehaviour.OnBallCatched += HandleBallCaught;
				jugglingBehaviour.OnBallThrown += HandleBallThrown;
				jugglingBehaviour.OnMaxBallsReached += HandleMaxballsReached;
			}
			
			// Shooting Events
			shootingInput = localPlayerBehaviour.ShootingBehaviour;
			if (shootingInput)
			{
				shootingInput.OnTargetMissed += HandleTargetMissed;
			}

			Cleanup();
			GameplayState.OnGameplayStateExited += Cleanup;
			GameplayState.OnGameplayStart += Initialize;
			GameplayState.OnPlayerWon += HandlePlayerWon;
		}

		private void Initialize()
		{
			applyUnbalance = true;
		}

		private void Cleanup()
		{
			currentRotation = 0.0f;
			hasLostBalance = false;
			inDangerZone = false;
			goingRight = false;
			transform.rotation = Quaternion.identity;
			currentUnbalanceAmount = defaultUnbalanceAmount;
			seatAndPoleTransform.rotation = Quaternion.identity;
		}
		
		[Button]
		private void ResetRotation() => Initialize();
		
		private void Update()
		{
			if (hasLostBalance || !applyUnbalance) return;
			
			// Only apply the forces if the player hasn't won yet
			if (!playerWon)
			{
				var balanceDirection = Mathf.Sign(currentRotation);
				
				// Apply input rotation
				currentRotation += -playerInputHandler.BalanceValue * inputStrength * Time.deltaTime;
				
				// Apply random unbalance rotation
				var randomRotation = Mathf.PerlinNoise1D(Time.time + playerInputHandler.PlayerId * 100f) * balanceDirection;
				currentRotation += randomRotation * currentUnbalanceAmount * Time.deltaTime;
				
				// Apply Gravity
				float gravityFalloff = 1.0f - Vector2.Dot(seatAndPoleTransform.up, Vector2.up);
				currentRotation += gravityFalloff * gravityStrength * balanceDirection * Time.deltaTime;
			}
			else
			{
				// Wiggle back and forth animation
				currentRotation = Mathf.Sin(Time.time) * 20f;
			}

			// Smoothly move to the target rotation
			var newRotation = Quaternion.Euler(0f, 0f, currentRotation);
			var seatAndPoleRotation = seatAndPoleTransform.rotation;
			seatAndPoleTransform.rotation = Quaternion.RotateTowards(seatAndPoleRotation, newRotation, rotationFollowSpeed * Time.deltaTime);

			var rotation = seatAndPoleRotation.eulerAngles.z;

			// Check which direction the player is going
			if (goingRight)
			{
				// Check if we're now going left
				if (rotation <= 180f)
				{
					goingRight = false;
					OnFallingLeft?.Invoke(playerInputHandler.PlayerId);
				}
			}
			else
			{
				// Check if we're now going right
				if (rotation >= 180f)
				{
					goingRight = true;
					OnFallingRight?.Invoke(playerInputHandler.PlayerId);
				}
			}

			// Check if we're in the danger zone
			if (rotation >= dangerZoneAngle && rotation <= 360f - dangerZoneAngle)
			{
				EnterDangerZone();
			}
			else
			{
				ExitDangerZone();
			}

			// Check if the rotation has exceeded the limit
			if (rotation >= angleLimit && rotation <= 360f - angleLimit)
			{
				BalanceLost();
			}
		}

		private void OnDestroy()
		{
			// Juggling events
			if (jugglingBehaviour)
			{
				jugglingBehaviour.OnBallCatched -= HandleBallCaught;
				jugglingBehaviour.OnBallThrown -= HandleBallThrown;
			}
			
			// Shooting Events
			shootingInput = localPlayerBehaviour.ShootingBehaviour;
			if (shootingInput)
			{
				shootingInput.OnTargetMissed -= HandleTargetMissed;
			}
			
			GameplayState.OnPlayerWon -= HandlePlayerWon;
		}

		private void ApplyUnbalanceImpulse(float amount)
		{
			var balanceDirection = Mathf.Sign(currentRotation);
			currentRotation += Mathf.Abs(amount) * balanceDirection; // Doesn't need delta time since this won't be every frame
		}

		private void BalanceLost()
		{
			hasLostBalance = true;
			Debug.Log("Balance Lost!");
			OnBalanceLost?.Invoke(playerInputHandler.PlayerId);
		}

		private void EnterDangerZone()
		{
			// Return if we've already entered the danger zone
			if (inDangerZone) return;
			inDangerZone = true;

			Debug.Log("Entered DangerZone!");
			OnDangerZoneEnter?.Invoke(playerInputHandler.PlayerId);
		}

		private void ExitDangerZone()
		{
			// If we weren't in the DangerZone, return
			if (!inDangerZone) return;
			inDangerZone = false;
			Debug.Log("Exited DangerZone!");
			OnDangerZoneExit?.Invoke(playerInputHandler.PlayerId);
		}

		private void HandleBallCaught(JuggleBall ball)
		{
			currentUnbalanceAmount = Mathf.Min(currentUnbalanceAmount + unbalanceIncrementAmount, maxUnbalanceAmount);
			ApplyUnbalanceImpulse(ballCaughtImpulseRotation);
		}

		private void HandleBallThrown(JuggleBall ball)
		{
			currentUnbalanceAmount = Mathf.Max(currentUnbalanceAmount - unbalanceIncrementAmount, defaultUnbalanceAmount);
		}

		private void HandlePlayerWon(int playerId)
		{
			currentRotation = 0f;
			applyUnbalance = false;
		}
		
		private void HandleMaxballsReached() => ApplyUnbalanceImpulse(999);

		private void HandleTargetMissed() => ApplyUnbalanceImpulse(missedShotImpulseRotation);
	}
}