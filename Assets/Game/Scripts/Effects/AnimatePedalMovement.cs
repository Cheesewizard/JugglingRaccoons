using System;
using JugglingRaccoons.Gameplay;
using Sirenix.OdinInspector;
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

		[SerializeField, Required]
		private LocalPlayerBehaviour localPlayerBehaviour;

		private void OnEnable()
		{
			localPlayerBehaviour.BalancingArrowBehaviour.OnFallingLeft += OnFallingLeft;
			localPlayerBehaviour.BalancingArrowBehaviour.OnFallingRight += OnFallingRight;
		}

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

		private void OnFallingLeft(int playerId)
		{
			isForwards = playerId != 0;
		}

		private void OnFallingRight(int playerId)
		{
			isForwards = playerId == 0;
		}
	}
}