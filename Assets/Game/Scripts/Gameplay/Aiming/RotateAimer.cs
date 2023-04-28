using Quack.Utils;
using UnityEngine;

namespace JugglingRaccoons.Gameplay.Aiming
{
	public class RotateAimer : MonoBehaviour
	{
		public float minAngle;
		public float maxAngle;

		public GameObject targetGameObject;

		public float speed = 1f;

		private RemapValue remapper => new(0, speed, minAngle, maxAngle);

		private void Update()
		{
			targetGameObject.transform.rotation = Quaternion.Euler(0, 0,
				remapper.Evaluate(Mathf.PingPong(Time.time, speed)));
		}

		public void Reset()
		{
			targetGameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}
}