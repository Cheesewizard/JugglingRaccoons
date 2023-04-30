using System;
using JugglingRaccoons.Core;
using JugglingRaccoons.Core.GameStates;
using JugglingRaccoons.Gameplay.Juggling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Gameplay.BalancingArrow
{
	public class BalancingArrowBehaviour : MonoBehaviour
	{
		[SerializeField, Range(0, 360)]
		private float angleLimit = 45f;

		[SerializeField, Range(0, 360)]
		private float dangerZoneAngle = 35f;

		[SerializeField]
		private float inputStrength = 0.5f;

		[SerializeField]
		private float defaultUnbalanceAmount = 0.3f;

		[SerializeField]
		private float maxUnbalanceAmount = 0.6f;

		[SerializeField]
		private float gravityStrength = 0.1f;

		[SerializeField]
		private float noiseBias = 0.04f;

		[SerializeField]
		private float rotationFollowSpeed = 10f;

		[SerializeField]
		private float unbalanceIncrementAmount = 0.1f;

		[SerializeField]
		private PlayerInputHandler playerInputHandler;

		[SerializeField]
		private LocalPlayerBehaviour localPlayerBehaviour;

		private float currentRotation = 0.0f;
		private bool hasLostBalance = false;
		private bool inDangerZone = false;
		private float currentUnbalanceAmount = 0.0f;
		private JugglingBehaviour jugglingBehaviour;
		private bool goingRight = false;
		private bool playerWon = false;

		public event Action<int> OnBalanceLost;
		public event Action<int> OnDangerZoneEnter;
		public event Action<int> OnDangerZoneExit;
		public event Action<int> OnFallingLeft;
		public event Action<int> OnFallingRight;

		private void Start()
		{
			jugglingBehaviour = localPlayerBehaviour.JugglingBehaviour;
			if (jugglingBehaviour)
			{
				jugglingBehaviour.OnBallCatched += IncreaseUnbalanceAmount;
				jugglingBehaviour.OnBallThrown += DecreaseUnbalanceAmount;
			}

			GameplayState.OnPlayerWon += HandlePlayerWon;

			Initialize();
			GameplayState.OnGameplayStateEntered += Initialize;
		}

		public void Initialize()
		{
			currentRotation = 0.0f;
			hasLostBalance = false;
			inDangerZone = false;
			goingRight = false;
			playerWon = false;
			transform.rotation = Quaternion.identity;
			currentUnbalanceAmount = defaultUnbalanceAmount;
		}
		
		private void Update()
		{
			if (hasLostBalance) return;

			currentRotation += -playerInputHandler.BalanceValue * inputStrength;

			// Only apply the forces if the player hasn't won yet
			if (!playerWon)
			{
				var randomForce = ((Mathf.PerlinNoise1D(Time.time + playerInputHandler.PlayerId * 100f) + noiseBias) * 2 - 1f) * currentUnbalanceAmount;
				ApplyRotationForce(randomForce);
				ApplyGravity();
			}
		}

		private void LateUpdate()
		{
			if (hasLostBalance) return;

			// Smoothly move to the target rotation
			var newRotation = Quaternion.Euler(0, 0, currentRotation);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationFollowSpeed * Time.deltaTime);

			var rotation = transform.rotation.eulerAngles.z;

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
			if (jugglingBehaviour)
			{
				jugglingBehaviour.OnBallCatched -= IncreaseUnbalanceAmount;
				jugglingBehaviour.OnBallThrown -= DecreaseUnbalanceAmount;
			}
			
			GameplayState.OnPlayerWon -= HandlePlayerWon;
		}

		// This can be used for adding a rotation value (degrees)
		public void ApplyRotationForce(float force) => currentRotation += force;

		private void ApplyGravity()
		{
			float gravityAffectiveness = 1.0f - Vector2.Dot(transform.up, Vector2.up);

			var balanceDirection = currentRotation / Mathf.Abs(currentRotation);
			ApplyRotationForce(gravityAffectiveness * gravityStrength * balanceDirection);
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

		[Button]
		private void ResetArrow()
		{
			hasLostBalance = false;
			inDangerZone = false;
			playerWon = false;
			currentRotation = 0.0f;
		}

		private void IncreaseUnbalanceAmount(JuggleBall ball)
		{
			currentUnbalanceAmount += unbalanceIncrementAmount;
			currentUnbalanceAmount = Mathf.Min(currentUnbalanceAmount, maxUnbalanceAmount);
		}

		private void DecreaseUnbalanceAmount(JuggleBall ball)
		{
			currentUnbalanceAmount -= unbalanceIncrementAmount;
			currentUnbalanceAmount = Mathf.Max(currentUnbalanceAmount, defaultUnbalanceAmount);
		}

		private void HandlePlayerWon(int playerId)
		{
			currentRotation = 0f;
			playerWon = true;
		}
	}
}