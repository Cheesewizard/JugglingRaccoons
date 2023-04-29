using System;
using UnityEngine;

namespace JugglingRaccoons.Core.GameStates
{
    public class MainMenuState : GameState
    {
        public static event Action OnMainMenuStateEntered;
        public static event Action OnMainMenuStateExited;

        [SerializeField]
        private GameplayState gameplayState;

        private void OnEnable()
        {
            OnMainMenuStateEntered?.Invoke();

            // TODO: Add main menu functionality. For now, just move to gameplay state
            GameStateManager.Instance.ChangeGameState(gameplayState);
        }

        private void OnDisable()
        {
            OnMainMenuStateExited?.Invoke();
        }
    }
}