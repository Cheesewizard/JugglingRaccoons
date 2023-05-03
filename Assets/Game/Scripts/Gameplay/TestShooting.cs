using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Gameplay
{
	public class TestShooting : MonoBehaviour
	{
		[SerializeField]
		float validAngle = 20f;
		[SerializeField]
		float edgeMargin = 10f;
		[SerializeField]
		bool flip;
		
		private float firstAngle;
		private float secondAngle;
		private float shotAngle;

		private void OnDrawGizmos()
		{
			var firstAngleRad = RemapAngle(firstAngle, flip, RemapStrategy.Deg2Rad);
			var secondAngleRad = RemapAngle(secondAngle, flip, RemapStrategy.Deg2Rad);
			var shotAngleRad = RemapAngle(shotAngle, flip, RemapStrategy.Deg2Rad);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(firstAngleRad), Mathf.Sin(firstAngleRad)));
			Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(secondAngleRad), Mathf.Sin(secondAngleRad)));
			Gizmos.color = ValidateShotAngle() ? Color.green : Color.red;
			Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(shotAngleRad), Mathf.Sin(shotAngleRad)));
		}

		private float RemapAngle(float angle, bool flip = false, RemapStrategy remapStrategy = RemapStrategy.None)
		{
			var remappedAngle = angle - 90 * (flip ? -1 : 1);
			switch (remapStrategy)
			{
				case RemapStrategy.Deg2Rad:
					remappedAngle *= Mathf.Deg2Rad;
					break;
				case RemapStrategy.Rad2Deg:
					remappedAngle *= Mathf.Rad2Deg;
					break;
			}
			return remappedAngle;
		}

		private bool ValidateShotAngle()
		{
			return shotAngle > firstAngle && shotAngle < secondAngle || shotAngle < firstAngle && shotAngle > secondAngle;
		}
		
		[Button(ButtonSizes.Medium, DrawResult = false)]
		private void GenerateAngle()
		{
			firstAngle = Random.Range(edgeMargin, 180 - edgeMargin);
			secondAngle = firstAngle + validAngle;
			secondAngle = secondAngle < 180 - edgeMargin ? secondAngle : secondAngle - validAngle * 2;
		}
		
		[Button(ButtonSizes.Medium, DrawResult = false)]
		private void TestShot()
		{
			shotAngle = Random.Range(0, 180);
			Debug.Log($"SHOT: {shotAngle} START: {firstAngle} END: {secondAngle}");
		}

		private enum RemapStrategy
		{
			None,
			Deg2Rad,
			Rad2Deg,
		}
	}
}