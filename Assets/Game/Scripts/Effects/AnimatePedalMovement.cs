using UnityEngine;

namespace JugglingRaccoons.Effects
{
	public class AnimatePedalMovement : MonoBehaviour
	{
		[SerializeField]
		private Transform leftFoot;

		[SerializeField]
		private Transform rightFoot;

		[SerializeField]
		private Transform leftPedal;

		[SerializeField]
		private Transform rightPedal;

		[SerializeField]
		private float speed = 800f;

		[SerializeField]
		private float power = 10;

		// Can be updated from an event listening to movement changes.
		[SerializeField]
		public bool isForwards;

		private void Update()
		{
			// They are reversed
			if (!isForwards)
			{
				transform.rotation *= Quaternion.AngleAxis((speed * power) * Time.deltaTime, Vector3.forward);
			}
			else
			{
				transform.rotation *= Quaternion.AngleAxis((speed * power) * Time.deltaTime, Vector3.back);
			}


			leftFoot.transform.position = leftPedal.transform.position;
			rightFoot.transform.position = rightPedal.transform.position;
		}
	}
}