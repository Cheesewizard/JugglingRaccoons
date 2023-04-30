using System.Collections.Generic;
using UnityEngine;

namespace JugglingRaccoons.Core
{
	public class SpawnPointsProvider : MonoBehaviour
	{
		[SerializeField]
		private Transform playerOneSpawnPoint;

		[SerializeField]
		private Transform playerTwoSpawnPoint;
		
		[SerializeField]
		private bool setSpawnPointsOnStart = true;
		
		private void Start()
		{
			if(!setSpawnPointsOnStart) return;
			
			SetSpawnPoints();
		}

		public void SetSpawnPoints()
		{
			var spawnPoints = new List<Transform>()
			{
				playerOneSpawnPoint, playerTwoSpawnPoint
			};
			
			PlayerManager.Instance.SetSpawnPoints(spawnPoints);
		}
	}
}