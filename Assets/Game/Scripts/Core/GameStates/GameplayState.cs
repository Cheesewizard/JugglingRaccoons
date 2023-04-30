using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JugglingRaccoons.Core.GameStates
{
    public class GameplayState : AbstractGameState
    {
        [SerializeField]
        private MainMenuState mainMenuState;
        [SerializeField]
        private PlayerInputManager playerInputManager;
        [SerializeField]
        private Button playAgainButton;
        [SerializeField]
        private Button menuButton;
        
        public static event Action OnGameplayStateEntered;
        public static event Action OnGameplayStateExited;
        public static event Action<int> OnPlayerWon;

        private void OnEnable()
        {
            // TODO: Get a callback for when the game has finished and they want to go back to the main menu
            playerInputManager.onPlayerJoined += HandlePlayerJoined;
            playerInputManager.EnableJoining();
            
            EventSystem.current.SetSelectedGameObject(playAgainButton.gameObject);

            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            OnGameplayStateEntered?.Invoke();
        }

        private void Start()
        {
            playAgainButton.onClick.AddListener(OnPlayAgainPressed);
            menuButton.onClick.AddListener(OnMenuButtonPressed);
        }

        private void OnPlayAgainPressed()
        {
            OnDisable();
            OnEnable();
        }

        private async void HandlePlayerJoined(PlayerInput player)
        {
            await UniTask.WaitForEndOfFrame();

            PlayerManager.Instance.PlayersInputLookup[player].BalancingArrowBehaviour.OnBalanceLost += OnPlayerLostBalance;
        }

        private void OnPlayerLostBalance(int playerIndex)
        {
            OnPlayerWon?.Invoke(playerIndex == 0 ? 1 : 0);
        }

        private void OnMenuButtonPressed()
        {
            GameStateManager.Instance.ChangeGameState(mainMenuState);
        }

        private void OnDisable()
        {
            OnGameplayStateExited?.Invoke();
        }
    }
}