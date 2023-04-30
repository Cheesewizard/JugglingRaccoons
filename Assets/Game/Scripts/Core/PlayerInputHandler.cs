using JugglingRaccoons.Core.GameStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JugglingRaccoons.Core
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerInputHandler : MonoBehaviour
	{
		public float BalanceValue { get; private set; } = 0.0f;
		public bool ThrowActionPressed { get; private set; } = false;

		public int PlayerId { get; private set; } = 0;

		private PlayerInput playerInput;
		private InputManager inputManager;
		private InputAction balanceAction;
		private InputAction throwAction;

		private void Awake()
		{
			inputManager = InputServiceLocator.GetPlayerInput();
			GameplayState.OnGameplayStateEntered += HandleEnteredGameplayState;
			GameplayState.OnPlayerWon += HandlePlayerWon;
		}

		private void HandleEnteredGameplayState()
		{
			balanceAction.Enable();
			throwAction.Enable();
		}

		private void HandlePlayerWon(int obj)
		{
			balanceAction.Disable();
			throwAction.Disable();
		}

		private void Update()
		{
			if (playerInput == null)
			{
				playerInput = GetComponent<PlayerInput>();
				PlayerId = playerInput.playerIndex;
				var gameplayActionMap = inputManager.Gameplay;
				balanceAction = playerInput.actions[gameplayActionMap.Balance.name];
				throwAction = playerInput.actions[gameplayActionMap.Throw.name];
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
				ThrowActionPressed = throwAction.WasPressedThisFrame();
			}
		}
	}
}