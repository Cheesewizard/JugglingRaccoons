using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Effects
{
	public class Bopper : MonoBehaviour
	{
		[SerializeField]
		private Transform target;

		[SerializeField]
		private float xOffset;

		[SerializeField]
		private bool randomInitialValue = true;
		
		[SerializeField, Range(0,1.5f), HideIf(nameof(randomInitialValue))]
		private float initialValue;
		
		[SerializeField]
		private float amplitude = 1.0f;

		[SerializeField]
		private float frequency = 2.0f;

		[SerializeField]
		private float speed = 2.0f;
		
		private float timeOffset;
		private Vector3 startPosition;

		private void Start()
		{
			timeOffset = initialValue;
			if (randomInitialValue)
			{
				timeOffset = Random.Range(0f, 1.5f);
			}
			
			startPosition = transform.localPosition;
		}

		private void Update()
		{
			// Sine wave up down
			var newY = startPosition.y + amplitude * Mathf.Sin((Time.time + timeOffset) * frequency * speed);
			target.transform.localPosition = new Vector3(transform.localPosition.x + xOffset, newY,
					target.transform.localPosition.z);
		}
	}
}