using System;
using System.Collections.Generic;
using JugglingRaccoons.Effects;
using JugglingRaccoons.Gameplay.BalancingArrow;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Gameplay
{
	public class RagdollEnabler : MonoBehaviour
	{
		[SerializeField]
		private LocalPlayerBehaviour localPlayerBehaviour;

		[SerializeField]
		private AnimatePedalMovement animatePedalMovement;

		[SerializeField]
		private Bopper bopper;

		public Transform[] transforms; // Array of transforms to apply radial force to

		[SerializeField]
		private Transform head;

		[SerializeField]
		private List<Transform> storedPositions; // Array of transforms to apply radial force to

		[SerializeField]
		private float minForce = 30f;

		[SerializeField]
		private float maxForce = 50f;

		[SerializeField]
		private float duration = 3.0f;

		private float timer; // Timer to keep track of the animation duration
		private bool isAnimating; // Flag to indicate if the animation is currently playing


		private Vector3[] targetPositions; // Array to store the target positions for each transform
		private BalancingArrowBehaviour balancingBehaviour;

		private void Start()
		{
			// Set the timer to zero, the isAnimating flag to false, and the target positions to the current positions
			timer = 0.0f;
			isAnimating = false;
			foreach (var position in transforms)
			{
				storedPositions.Add(position);
			}

			targetPositions = new Vector3[transforms.Length];
			for (int i = 0; i < transforms.Length; i++)
			{
				targetPositions[i] = transforms[i].position;
			}

			balancingBehaviour = localPlayerBehaviour.BalancingArrowBehaviour;
			balancingBehaviour.OnBalanceLost += TriggerAnimation;

		}

		private void Update()
		{
			// If the animation is playing
			if (isAnimating)
			{
				// Increment the timer by the elapsed time since the last frame
				timer += Time.deltaTime;

				// Calculate the progress of the animation using the timer and duration
				float progress = Mathf.Clamp01(timer / duration);

				// Loop through each transform in the array
				for (int i = 0; i < transforms.Length; i++)
				{
					// Calculate a random direction vector
					Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;

					// Calculate a random force
					float force = Mathf.Lerp(minForce, maxForce, progress);

					// Calculate the new position for the transform
					Vector3 newPosition = targetPositions[i] + direction * force * Time.deltaTime;

					// Lerp the transform to the new position
					transforms[i].position = Vector3.Lerp(transforms[i].position, newPosition, progress);
				}

				// If the animation is finished
				if (progress >= 1.0f)
				{
					// Stop the animation and reset the timer
					isAnimating = false;
					timer = 0.0f;
				}
			}
		}

		[Button]
		public void TriggerAnimation(int _)
		{
			// Set the target positions to the current positions plus a random direction multiplied by a random force
			for (int i = 0; i < transforms.Length; i++)
			{
				Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
				float force = Random.Range(minForce, maxForce);
				targetPositions[i] = transforms[i].position + direction * force * duration;
			}

			animatePedalMovement.enabled = false;
			bopper.enabled = false;

			// Start the animation
			isAnimating = true;
		}


		[Button]
		public void Reset()
		{
			isAnimating = false;

			for (var i = 0; i < transforms.Length; i++)
			{
				transforms[i].transform.position = storedPositions[i].position;
				transforms[i].transform.rotation = storedPositions[i].rotation;
			}
		}

		private void OnDestroy()
		{
			balancingBehaviour.OnBalanceLost -= TriggerAnimation;
		}
	}
}