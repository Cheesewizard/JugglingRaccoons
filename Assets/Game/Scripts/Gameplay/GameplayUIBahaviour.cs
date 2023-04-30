using System;
using JugglingRaccoons.Core.GameStates;
using UnityEngine;
using TMPro;
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

        private void Awake()
        {
            playerWinText.gameObject.SetActive(false);
            playAgainButton.gameObject.SetActive(false);
            menuButton.gameObject.SetActive(false);
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