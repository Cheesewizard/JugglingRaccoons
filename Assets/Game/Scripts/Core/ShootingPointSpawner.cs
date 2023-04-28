using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Core
{
	public class ShootingPointSpawner : MonoBehaviour
	{
		public GameObject arrowPointer;

		public GameObject targetZonePrefab;
		public SpriteRenderer spriteCircle;

		public float minAngle;
		public float maxAngle;

		public float targetDistance = 20f;
		public float edgeMargin = 10f;

		private GameObject spawnedTargetZoneOne;
		private GameObject spawnedTargetZoneTwo;

		private float circleRadius;
		private float currentScale;

		private void Start()
		{
			circleRadius = (spriteCircle.bounds.size.x) / 2f;
			Spawn(targetDistance);
		}

		public void Spawn(float distance)
		{
			SpawnAtRandomLocation(distance);
		}

		private void SpawnAtRandomLocation(float targetDistance)
		{
			var randomAngle = Random.Range(minAngle - edgeMargin, maxAngle + edgeMargin);

			var randomPointOnCircumference = new Vector2(
				circleRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad),
				circleRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad)
			);

			var circleCenter = targetZonePrefab.transform.position;
			var spawnPosition = (Vector2) circleCenter + randomPointOnCircumference;

			var angle = Mathf.Atan2(spawnPosition.y - circleCenter.y, spawnPosition.x - circleCenter.x) * Mathf.Rad2Deg;
			spawnedTargetZoneOne = Instantiate(targetZonePrefab, spawnPosition, Quaternion.Euler(0f, 0f, angle));

			var secondAngle = angle - targetDistance * ((angle > 0) ? 1 : -1);
			randomPointOnCircumference = new Vector2(
				circleRadius * Mathf.Cos(secondAngle * Mathf.Deg2Rad),
				circleRadius * Mathf.Sin(secondAngle * Mathf.Deg2Rad)
			);

			spawnPosition = (Vector2) circleCenter + randomPointOnCircumference;

			angle = Mathf.Atan2(spawnPosition.y - circleCenter.y, spawnPosition.x - circleCenter.x) * Mathf.Rad2Deg;
			spawnedTargetZoneTwo = Instantiate(targetZonePrefab, spawnPosition,
				Quaternion.Euler(0f, 0f, angle));
		}

		public bool CheckIfBetweenPoints(float targetAngle)
		{
			var zoneOneAngle = spawnedTargetZoneOne.transform.rotation.z;
			var zoneTwoAngle = spawnedTargetZoneTwo.transform.rotation.z;

			// Check positive range and negative
			return targetAngle >= Mathf.Min(zoneOneAngle, zoneTwoAngle) &&
			       targetAngle <= Mathf.Max(zoneOneAngle, zoneTwoAngle);
		}

		public void ClearTargetZone()
		{
			DestroyImmediate(spawnedTargetZoneOne);
			DestroyImmediate(spawnedTargetZoneTwo);
		}

		[Button]
		private void Reset()
		{
			ClearTargetZone();
			Spawn(targetDistance);
		}
	}
}