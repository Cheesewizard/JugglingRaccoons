using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JugglingRaccoons.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JugglingRaccoons.Core
{
	public class PlayerManager
	{
		// Rider magic :sparkles:
		public static PlayerManager Instance => instance ??= new PlayerManager();
		private static PlayerManager instance;

		public List<LocalPlayerBehaviour> Players { get; private set; } = new();
		public Dictionary<PlayerInput, LocalPlayerBehaviour> PlayersInputLookup { get; } = new();

		private List<Transform> spawnPoints = new();

		public event Action<LocalPlayerBehaviour> PlayerJoined;
		public event Action<LocalPlayerBehaviour> PlayerLeft;

		public PlayerManager()
		{
			SubscribeToInputManager();
		}

		private async void SubscribeToInputManager()
		{
			// Thanks Unity for making me do this
			await UniTask.WaitUntil(() => PlayerInputManager.instance);
			
			PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
			PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
		}

		public void SetSpawnPoints(List<Transform> spawnPoints)
		{
			this.spawnPoints = spawnPoints;
		}

		public bool TryGetOpponent(LocalPlayerBehaviour player, out LocalPlayerBehaviour opponent)
		{
			opponent = null;
			if (Players.Count == 0) return false;

			opponent = Players.FirstOrDefault(other => other != player);
			return opponent != null;
		}

		private void OnPlayerJoined(PlayerInput playerInput)
		{
			// Set the player at it's spawn position
			playerInput.transform.position = spawnPoints[playerInput.playerIndex].transform.position;

			var localPlayer = playerInput.transform.gameObject.GetComponent<LocalPlayerBehaviour>();
			if (localPlayer != null)
			{
				Players.Add(localPlayer);
				PlayersInputLookup.Add(playerInput, localPlayer);

				// Flip the X scale for the ball aimer for the second player
				if (playerInput.playerIndex == 1)
				{
					var scale = localPlayer.transform.localScale;
					localPlayer.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
				}
				
				PlayerJoined?.Invoke(localPlayer);
			}
		}

		private void OnPlayerLeft(PlayerInput playerInput)
		{
			var player = PlayersInputLookup[playerInput];
			Players.Remove(player);
			PlayersInputLookup.Remove(playerInput);
			PlayerLeft?.Invoke(player);
		}
	}
}