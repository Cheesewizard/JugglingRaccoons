using System;
using Cysharp.Threading.Tasks;
using JugglingRaccoons.Gameplay;
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
        [SerializeField]
        private CountdownUI countdownUI;
        [SerializeField]
        private CountdownAudioBehaviour countdownAudio;
        [SerializeField]
        private int countdown;

        public static event Action OnGameplayStateEntered;
        public static event Action OnGameplayStateExited;
        public static event Action OnGameplayStart;
        public static event Action<int> OnPlayerWon;

        private static float timer;
        
        private void OnEnable()
        {
            // TODO: Get a callback for when the game has finished and they want to go back to the main menu
            playerInputManager.onPlayerJoined += HandlePlayerJoined;
            playerInputManager.EnableJoining();
            
            EventSystem.current.SetSelectedGameObject(playAgainButton.gameObject);

            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            
            if (PlayerManager.Instance.Players.Count < 2)
            {
                PlayerManager.Instance.PlayerJoined += OnPlayerJoined;
            }
            else
            {
                StartCountdown();
            }
			
            void OnPlayerJoined(LocalPlayerBehaviour player)
            {
                StartCountdown();
                PlayerManager.Instance.PlayerJoined -= OnPlayerJoined;
            }
            
            OnGameplayStateEntered?.Invoke();
        }

        private async void StartCountdown()
        {
            countdownUI.gameObject.SetActive(true);

            var prevSecond = 0;
            timer = countdown;
            while (timer > 0)
            {
                var seconds = (int)timer;
                if (seconds != prevSecond)
                {
                    countdownAudio.PlayCountdownAudio();
                    prevSecond = seconds;
                }
                countdownUI.UpdateCountdown(seconds, timer - seconds);
                timer -= Time.deltaTime;
                await UniTask.Yield();
            }
            
            countdownAudio.PlayCountdownEndAudio();
            countdownUI.gameObject.SetActive(false);
            OnGameplayStart?.Invoke();
        }

        private void Start()
        {
            playAgainButton.onClick.AddListener(OnPlayAgainPressed);
            menuButton.onClick.AddListener(OnMenuButtonPressed);

            foreach (var player in PlayerManager.Instance.Players)
            {
                player.BalancingBehaviour.OnBalanceLost += OnPlayerLostBalance;
            }
        }

        private void OnPlayAgainPressed()
        {
            OnDisable();
            OnEnable();
        }

        private async void HandlePlayerJoined(PlayerInput player)
        {
            await UniTask.WaitForEndOfFrame();

            PlayerManager.Instance.PlayersInputLookup[player].BalancingBehaviour.OnBalanceLost += OnPlayerLostBalance;
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