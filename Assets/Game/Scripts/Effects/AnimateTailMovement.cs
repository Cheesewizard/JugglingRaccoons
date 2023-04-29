using UnityEngine;

namespace JugglingRaccoons.Effects
{
	public class AnimateTailMovement : MonoBehaviour
	{
		[SerializeField]
		private GameObject targetSolver;

		[SerializeField]
		private float xOffset;
		
		[SerializeField]
		private float amplitude = 1.0f;

		[SerializeField]
		private float frequency = 2.0f;

		[SerializeField]
		private float speed = 2.0f;

		private Vector3 startPosition;

		private void Start()
		{
			startPosition = targetSolver.transform.position;
		}

		private void Update()
		{
			// Sine wave up down
			var newY = startPosition.y + amplitude * Mathf.Sin(Time.time * frequency * speed); 
			
			targetSolver.transform.position = new Vector3(transform.position.x + xOffset, newY,
					targetSolver.transform.position.z);
		}
	}
}