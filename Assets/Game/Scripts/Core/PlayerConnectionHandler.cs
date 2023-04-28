using System;
using System.Collections.Generic;
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
		}

		public void OnPlayerLeft()
		{
			// May not need this
		}
	}
}