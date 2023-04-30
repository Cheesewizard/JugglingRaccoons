using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Gameplay.Expressions
{
	public class Blinking : MonoBehaviour
	{
		[SerializeField]
		private float minBlinkInterval = 1f;

		[SerializeField]
		private float maxBlinkInterval = 1f;

		[SerializeField]
		private float blinkDuration = 0.1f;

		[SerializeField]
		private float timeUntilNextBlink;

		public event Action<float> OnBlink;

		private void Start()
		{
			timeUntilNextBlink = Random.Range(minBlinkInterval, maxBlinkInterval);
		}

		private void Update()
		{
			timeUntilNextBlink -= Time.deltaTime;

			if (timeUntilNextBlink <= 0.0f)
			{
				OnBlink?.Invoke(blinkDuration);

				// Reset timeUntilNextBlink to a random value
				timeUntilNextBlink = Random.Range(minBlinkInterval, maxBlinkInterval);
			}
		}
	}
}