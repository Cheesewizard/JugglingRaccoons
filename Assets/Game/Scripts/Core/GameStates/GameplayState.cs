using System;
using UnityEngine;

namespace JugglingRaccoons.Core.GameStates
{
    public class GameplayState : AbstractGameState
    {
        public static event Action OnGameplayStateEntered;
        public static event Action OnGameplayStateExited;
        public static event Action<int> OnPlayerWon;

        [SerializeField]
        private MainMenuState mainMenuState;

        private void OnEnable()
        {
            foreach (var player in PlayerManager.Instance.Players)
            {
                player.BalancingArrowBehaviour.OnBalanceLost += OnPlayerLostBalance;
            }

            // TODO: Get a callback for when the game has finished and they want to go back to the main menu

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