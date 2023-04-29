using System.Collections.Generic;
using System.Linq;
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


		private Dictionary<PlayerInput, LocalPlayerBehaviour> playersInputLookup  = new();
		private List<Transform> spawnPoints = new();

		public PlayerManager()
		{
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

		public void OnPlayerJoined(PlayerInput playerInput)
		{
			// Set the player at it's spawn position
			playerInput.transform.position = spawnPoints[playerInput.playerIndex].transform.position;

			var localPlayer = playerInput.transform.gameObject.GetComponent<LocalPlayerBehaviour>();
			if (localPlayer != null)
			{
				Players.Add(localPlayer);
				playersInputLookup.Add(playerInput, localPlayer);

				// Flip the X scale for the ball aimer for the second player
				if (playerInput.playerIndex == 1)
				{
					var pivot = localPlayer.BallAimerPivotTransform;
					if (pivot != null)
					{
						pivot.localScale = new Vector3(-pivot.localScale.x, pivot.localScale.y, pivot.localScale.z);
					}
				}
			}
		}

		private void OnPlayerLeft(PlayerInput playerInput)
		{
			var player = playersInputLookup[playerInput];
			Players.Remove(player);
			playersInputLookup.Remove(playerInput);
		}
	}
}