using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JugglingRaccoons.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JugglingRaccoons.Core
{
	public class PlayerConnectionHandler : MonoBehaviour
	{
		[SerializeField] private List<Transform> spawnPositions = new();

		public void OnPlayerJoined(PlayerInput playerInput)
		{
			// Set the player at it's spawn position
			playerInput.transform.position = spawnPositions[playerInput.playerIndex].transform.position;

			var localPlayer = playerInput.transform.gameObject.GetComponent<LocalPlayerBehaviour>();
			if (localPlayer != null)
			{
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

		public void OnPlayerLeft()
		{
			// May not need this
		}
	}
}