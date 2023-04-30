using System;
using JugglingRaccoons.Gameplay;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JugglingRaccoons.Core.GameStates
{
    public class MainMenuState : AbstractGameState
    {
        [SerializeField]
        private GameplayState gameplayState;
        [SerializeField]
        private TutorialState tutorialState;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button tutorialButton;
        
        public static event Action OnMainMenuStateEntered;
        public static event Action OnMainMenuStateExited;
        
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);

            OnMainMenuStateEntered?.Invoke();
        }

        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonPressed);
            tutorialButton.onClick.AddListener(OnTutorialButtonPressed);
            PlayerManager.Instance.PlayerJoined += HandlePlayerJoined;
        }

        private void HandlePlayerJoined(LocalPlayerBehaviour obj)
        {
            if (PlayerManager.Instance.Players.Count == 1)
            {
                // If the first player joined
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }
        }

        private void OnStartButtonPressed()
        {
            // TODO: animate curtains
            GameStateManager.Instance.ChangeGameState(gameplayState);
        }

        private void OnTutorialButtonPressed()
        {
            // TODO: animate curtains
            GameStateManager.Instance.ChangeGameState(tutorialState);
        }

        private void OnDisable()
        {
            OnMainMenuStateExited?.Invoke();
        }
    }
}