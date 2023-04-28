using JugglingRaccoons.Core.JugglingRaccoons.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JugglingRaccoons.Core
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerInputHandler : MonoBehaviour
	{
		public float BalanceValue { get; private set; } = 0.0f;
		public bool ThrowAction { get; private set; } = false;

		private PlayerInput playerInput;
		private InputManager inputManager;
		private InputAction balanceAction;
		private InputAction throwAction;

		private void Awake() => inputManager = InputServiceLocator.GetPlayerInput();

		private void Update()
		{
			if (playerInput == null)
			{
				playerInput = GetComponent<PlayerInput>();
				var gameplayActionMap = inputManager.Gameplay;
				balanceAction = playerInput.actions[gameplayActionMap.Balance.name];
				throwAction = playerInput.actions[gameplayActionMap.Balance.name];
			}

			// Balance
			if (balanceAction != null)
			{
				BalanceValue = balanceAction.ReadValue<float>();
				var deadZone = 0.01f;
				BalanceValue = BalanceValue <= deadZone && BalanceValue >= -deadZone ? 0f : BalanceValue;
			}

			// Throw
			if (throwAction != null)
			{
				ThrowAction = throwAction.triggered;
			}
		}
	}
}