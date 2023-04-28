using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
	public class JugglingManager : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private GameObject ballPrefab;
		[SerializeField]
		private Transform leftHand;
		[SerializeField]
		private Transform rightHand;

		[Header("Config")]
		private int startingBallsCount = 3;

		private HashSet<JuggleBall> balls = new ();
		
		private void Awake()
		{
			for (int i = 0; i < startingBallsCount; i++)
			{
				var ball = Instantiate(ballPrefab).GetComponent<JuggleBall>();
				if (!ball)
				{
					Debug.LogError("No ballz bro");
					return;
				}
				
				balls.Add(ball);
				var step = 1f / (startingBallsCount);
				var halfStep = step / 2;
				ball.SetInitialJugglePosition(i * step + halfStep);
			}
		}

		private void Update()
		{
			foreach (var ball in balls)
			{
				ball.Move(leftHand.position, rightHand.position);
			}
		}

		[Button(ButtonSizes.Medium, DrawResult = false)]
		private void AddBall()
		{
			var ball = Instantiate(ballPrefab).GetComponent<JuggleBall>();
			if (!ball)
			{
				Debug.LogError("No ballz bro");
				return;
			}
				
			balls.Add(ball);
			ball.SetInitialJugglePosition(0);
			
			//TODO: recompute correct step for all balls to be distanced
		}
		
		public static Vector3 GetParabolicPoint(Vector3 start, Vector3 end, float height, float t)
		{
			var y = Mathf.Sin(Mathf.PI * t) * height;
			return Vector3.Lerp(start, end, t) + Vector3.up * y;
		} 
	}
}