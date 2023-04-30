using System;
using JugglingRaccoons.Core;
using JugglingRaccoons.Core.GameStates;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JugglingRaccoons.Gameplay
{
    public class GameplayUIBahaviour : MonoBehaviour
    {
        [SerializeField]
        private Button playAgainButton;
        [SerializeField]
        private Button menuButton;
        [SerializeField]
        private TextMeshProUGUI playerWinText;
        [SerializeField]
        private TextMeshProUGUI playerJoinText;

        private void Awake()
        {
            Initialize();
            GameplayState.OnGameplayStateEntered += Initialize;
            PlayerManager.Instance.PlayerJoined += HandlePlayerJoined;
        }

        private void Initialize()
        {
            playerWinText.gameObject.SetActive(false);
            playAgainButton.gameObject.SetActive(false);
            menuButton.gameObject.SetActive(false);
            playerJoinText.gameObject.SetActive(PlayerManager.Instance.Players.Count < 2);
        }
        
        private void HandlePlayerJoined(LocalPlayerBehaviour player)
        {
            playerJoinText.gameObject.SetActive(PlayerManager.Instance.Players.Count < 2);
        }

        private void OnEnable()
        {
            GameplayState.OnPlayerWon += OnPlayerWon;
        }

        private void OnPlayerWon(int playerId)
        {
            playerWinText.text = $"Player {playerId + 1} wins!";
            playAgainButton.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);
            playerWinText.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            GameplayState.OnPlayerWon -= OnPlayerWon;
        }
    }
}