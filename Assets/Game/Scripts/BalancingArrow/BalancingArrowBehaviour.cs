using System;
using Game.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

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
	private float gravityStrength = 0.1f;

	[SerializeField]
	private float noiseBias = 0.04f;

	private PlayerInputHandler playerInputHandler;
	private float balanceInput = 0.0f;
	private float currentRotation = 0.0f;
	private bool hasLostBalance = false;
	private bool inDangerZone = false;
	private float previousRotation = 0.0f;

	private event Action OnBalanceLost;
	private event Action OnDangerZoneEnter;
	private event Action OnDangerZoneExit;

	private void Awake() => playerInputHandler = transform.parent.GetComponent<PlayerInputHandler>();

	private void OnEnable()
	{
		playerInputHandler.BalanceAction.performed += UpdateBalanceInput;
		playerInputHandler.BalanceAction.canceled += ResetBalanceInput;
	}

	private void Update()
	{
		if (hasLostBalance) return;

		currentRotation += balanceInput;

		var randomForce = ((Mathf.PerlinNoise1D(Time.time) + noiseBias) * 2 - 1f) * defaultUnbalanceAmount;
		ApplyRotationForce(randomForce);
		ApplyGravity();
	}

	private void LateUpdate()
	{
		if (hasLostBalance) return;

		// Smoothly move to the target rotation
		var lerpedRotation = Mathf.Lerp(previousRotation, currentRotation, 0.5f);
		previousRotation = currentRotation;
		var newRotation = Quaternion.Euler(0, 0, lerpedRotation);
		transform.rotation = newRotation;

		var rotation = transform.rotation.eulerAngles.z;

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

	private void OnDisable()
	{
		playerInputHandler.BalanceAction.performed -= UpdateBalanceInput;
		playerInputHandler.BalanceAction.canceled -= ResetBalanceInput;
	}

	private void UpdateBalanceInput(InputAction.CallbackContext obj) => balanceInput = -obj.ReadValue<float>() * inputStrength;

	private void ResetBalanceInput(InputAction.CallbackContext obj) => balanceInput = 0f;

	private void ApplyRotationForce(float force) => currentRotation += force;

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
		OnBalanceLost?.Invoke();
	}

	private void EnterDangerZone()
	{
		// Return if we've already entered the danger zone
		if (inDangerZone) return;
		inDangerZone = true;

		Debug.Log("Entered DangerZone!");
		OnDangerZoneEnter?.Invoke();
	}

	private void ExitDangerZone()
	{
		// If we weren't in the DangerZone, return
		if (!inDangerZone) return;
		inDangerZone = false;
		Debug.Log("Exited DangerZone!");
		OnDangerZoneExit?.Invoke();
	}

	[Button]
	private void ResetArrow()
	{
		hasLostBalance = false;
		inDangerZone = false;
		currentRotation = 0.0f;
	}
}