using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JugglingRaccoons.Core.GameStates
{
    public class GameplayState : AbstractGameState
    {
        [SerializeField]
        private MainMenuState mainMenuState;
        [SerializeField]
        private PlayerInputManager playerInputManager;
        
        public static event Action OnGameplayStateEntered;
        public static event Action OnGameplayStateExited;
        public static event Action<int> OnPlayerWon;
        
        private void OnEnable()
        {
            foreach (var player in PlayerManager.Instance.Players)
            {
                player.BalancingArrowBehaviour.OnBalanceLost += OnPlayerLostBalance;
            }

            // TODO: Get a callback for when the game has finished and they want to go back to the main menu

            playerInputManager.EnableJoining();
            OnGameplayStateEntered?.Invoke();
        }

        private void OnPlayerLostBalance(int playerIndex)
        {
            OnPlayerWon?.Invoke(playerIndex == 0 ? 1 : 0);
        }

        private void OnGameEnded()
        {
            GameStateManager.Instance.ChangeGameState(mainMenuState);
        }

        private void OnDisable()
        {
            foreach (var player in PlayerManager.Instance.Players)
            {
                player.BalancingArrowBehaviour.OnBalanceLost -= OnPlayerLostBalance;
            }

            OnGameplayStateExited?.Invoke();
        }
    }
}