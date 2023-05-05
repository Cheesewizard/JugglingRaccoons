using UnityEngine;

namespace JugglingRaccoons.Gameplay.Aiming
{
	public class ShootingVisual : MonoBehaviour
	{
		[SerializeField]
		private Transform arrowHinge;

		[SerializeField]
		private GameObject targetZonePrefab;
		
		private GameObject spawnedTargetZoneOne;
		private GameObject spawnedTargetZoneTwo;

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
		
		public void UpdateRangeVisual(float firstAngle, float secondAngle)
		{
			var flip = transform.lossyScale.x < 0;
			var firstAngleRemap = ShootingBehaviour.RemapAngle(firstAngle, flip, RemapStrategy.Deg2Rad);
			var secondAngleRemap = ShootingBehaviour.RemapAngle(secondAngle, flip, RemapStrategy.Deg2Rad);
			var firstPosition = transform.position + new Vector3(Mathf.Cos(firstAngleRemap), Mathf.Sin(firstAngleRemap));
			var secondPosition = transform.position + new Vector3(Mathf.Cos(secondAngleRemap), Mathf.Sin(secondAngleRemap));
			spawnedTargetZoneOne = Instantiate(targetZonePrefab, firstPosition, Quaternion.Euler(0, 0, ShootingBehaviour.RemapAngle(firstAngle)));
			spawnedTargetZoneTwo = Instantiate(targetZonePrefab, secondPosition, Quaternion.Euler(0, 0, ShootingBehaviour.RemapAngle(secondAngle)));
			spawnedTargetZoneOne.transform.SetParent(transform);
			spawnedTargetZoneTwo.transform.SetParent(transform);
		}

		public void UpdateArrowVisual(float arrowAngle)
		{
			arrowHinge.transform.rotation = Quaternion.Euler(0, 0, arrowAngle);
		}
		
		public void ClearTargetZone()
		{
			Destroy(spawnedTargetZoneOne);
			Destroy(spawnedTargetZoneTwo);
		}
	}
}