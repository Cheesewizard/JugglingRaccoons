using System;
using JugglingRaccoons.Core.GameStates;
using UnityEngine;
using TMPro;

namespace JugglingRaccoons.Gameplay
{
    public class GameplayUIBahaviour : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerWinText;

        private void Awake()
        {
            playerWinText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameplayState.OnPlayerWon += OnPlayerWon;
        }

        private void OnPlayerWon(int playerId)
        {
            playerWinText.text = $"Player {playerId} wins!";
            playerWinText.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            GameplayState.OnPlayerWon -= OnPlayerWon;
        }
    }
}