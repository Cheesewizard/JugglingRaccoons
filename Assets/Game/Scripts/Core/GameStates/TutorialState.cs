using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JugglingRaccoons.Core.GameStates
{
	public class TutorialState : AbstractGameState
	{
		[SerializeField]
		private Button startButton;
		[SerializeField]
		private GameplayState gameplayState;
		
		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(startButton.gameObject);
		}

		private void Awake()
		{
			startButton.onClick.AddListener(OnStartButtonPressed);
		}

		private void OnStartButtonPressed()
		{
			GameStateManager.Instance.ChangeGameState(gameplayState);
		}
	}
}