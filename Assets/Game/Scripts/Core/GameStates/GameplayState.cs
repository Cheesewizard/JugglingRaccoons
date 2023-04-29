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
            OnGameplayStateEntered?.Invoke();

            // TODO: Get a callback for a player losing balance so we can activate the win state
            // TODO: Get a callback for when the game has finished and they want to go back to the main menu
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
            OnGameplayStateExited?.Invoke();
        }
    }
}