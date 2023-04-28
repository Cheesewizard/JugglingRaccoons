using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Core
{
	public class ShootingPointSpawner : MonoBehaviour
	{
		public GameObject targetZone;
		public SpriteRenderer spriteCircle;

		public float minAngle;
		public float maxAngle;

		private GameObject spawnedTargetZone;
		private float circleRadius;
		private float currentScale;

		private void Start()
		{
			circleRadius = (spriteCircle.bounds.size.x) / 2f;
			Spawn();
		}

		public void Spawn()
		{
			var randomAngle = Random.Range(minAngle, maxAngle) * Mathf.PI / 180f;
			var randomPointOnCircumference = new Vector2(
				circleRadius * Mathf.Cos(randomAngle),
				circleRadius * Mathf.Sin(randomAngle)
			);

			var circleCenter = targetZone.transform.position;
			var spawnPosition = (Vector2) circleCenter + randomPointOnCircumference;

			var angle = Mathf.Atan2(spawnPosition.y - circleCenter.y, spawnPosition.x - circleCenter.x) * Mathf.Rad2Deg;
			spawnedTargetZone = Instantiate(targetZone, spawnPosition, Quaternion.Euler(0f, 0f, angle));
		}


		public void ClearTargetZone()
		{
			DestroyImmediate(spawnedTargetZone);
		}

		[Button]
		public void SetZoneSize(float size)
		{
			var localScale = spawnedTargetZone.transform.localScale;
			localScale.y = size;
		}

		[Button]
		private void Reset()
		{
			ClearTargetZone();
			Spawn();
		}
	}
}