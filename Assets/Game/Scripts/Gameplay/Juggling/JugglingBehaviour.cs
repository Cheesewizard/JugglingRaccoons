using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JugglingRaccoons.Core;
using JugglingRaccoons.Core.GameStates;
using UnityEngine;

namespace JugglingRaccoons.Gameplay.Juggling
{
	public class JugglingBehaviour : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private LocalPlayerBehaviour localPlayer;
		[SerializeField]
		private GameObject ballPrefab;

		[field: SerializeField]
		public Transform StartingHand { get; private set; }
		[field: SerializeField]
		public Transform PassingHand { get; private set; }

		[Header("Config")]
		[SerializeField]
		private int startingBallsCount = 3;
		[SerializeField]
		private int maxBallsCount = 10;

		[field: Header("Balls Config")]
		[field: SerializeField]
		public float JuggleHeight { get; private set; } = 2.2f;
		[field: SerializeField]
		public float JuggleTime { get; private set; } = 1.25f;
		[field: SerializeField]
		public float HandPassHeight { get; private set; } = 0.6f;
		[field: SerializeField]
		public float HandPassTime { get; private set; } = 0.6f;
		[field: SerializeField]
		public float ThrowTime { get; private set; } = 4f;
		[field: SerializeField]
		public float ThrowHeight { get; private set; } = 5f;

		public float TotalJuggleTime => JuggleTime + HandPassTime;
		
		private List<JuggleBall> jugglingBalls = new ();
		private List<JuggleBall> thrownBalls = new ();
		
		private int prevBallsCount;
		private int delayIndex;
		private bool isNewJuggle;
		private bool readyToReceiveBall;
		
		public event Action<JugglingBehaviour> OnReadyToReceiveBall;
		public event Action<JuggleBall> OnBallThrown;
		public event Action<JuggleBall> OnBallCatched;
		public event Action OnMaxBallsReached;
		
		private void Awake()
		{
			if (localPlayer)
			{
				localPlayer.ShootingBehaviour.OnTargetHit += ThrowBallAtOpponent;
			}

			Cleanup();
			Initialize();
			GameplayState.OnGameplayStateEntered += Initialize;
			GameplayState.OnGameplayStateExited += Cleanup;
		}

		private void Initialize()
		{
			for (int i = 0; i < startingBallsCount; i++)
			{
				if (i > maxBallsCount)
				{
					Debug.LogError("Too many ballz bro");
					return;
				}
				
				var ball = Instantiate(ballPrefab).GetComponent<JuggleBall>();
				jugglingBalls.Add(ball);
				var step = TotalJuggleTime / startingBallsCount;

				ball.Delay = step * i;
				ball.transform.position = StartingHand.position;
				ball.SetJugglingBehaviour(this);
				ball.OnNewJuggle += HandleNewJuggle;
			}
		}

		private void Cleanup()
		{
			foreach (var ball in jugglingBalls)
			{
				Destroy(ball.gameObject);
			}
			jugglingBalls.Clear();
			
			foreach (var ball in thrownBalls)
			{
				Destroy(ball.gameObject);
			}
			thrownBalls.Clear();
			
			delayIndex = 0;
			isNewJuggle = false;
			prevBallsCount = startingBallsCount;
			readyToReceiveBall = true;
		}
		
		public static Vector3 GetParabolicPoint(Vector3 start, Vector3 end, float height, float t)
		{
			var y = Mathf.Sin(Mathf.PI * t) * height;
			return Vector3.Lerp(start, end, t) + Vector3.up * y;
		}

		public async void ThrowBall(JugglingBehaviour target)
		{
			var ball = await AwaitForBallInStartingHand();
			RemoveBall(ball);
			thrownBalls.Add(ball);
			ball.Throw(target);
			ball.OnReachedTarget += BallOnOnReachedTarget;

			void BallOnOnReachedTarget(JuggleBall ball)
			{
				thrownBalls.Remove(ball);
				target.AddBall(ball);
				ball.OnReachedTarget -= BallOnOnReachedTarget;
			}
			
			OnBallThrown?.Invoke(ball);
		}

		private void ThrowBallAtOpponent()
		{
			if (!PlayerManager.Instance.TryGetOpponent(localPlayer, out var opponent))
			{
				Debug.LogError("NoOpponent");
				return;
			}
			
			ThrowBall(opponent.JugglingBehaviour);
		}
		
		private async void AddBall(JuggleBall ball)
		{
			if (jugglingBalls.Count == maxBallsCount)
			{
				Debug.LogError("MAX BALLS");
				OnMaxBallsReached?.Invoke();
			}

			readyToReceiveBall = false;
			prevBallsCount = jugglingBalls.Count;
			jugglingBalls.Add(ball);
			ball.SetJugglingBehaviour(this);
			ball.Delay = float.MaxValue;
			ball.OnNewJuggle += HandleNewJuggle;
			ball.transform.position = StartingHand.position;
			
			OnBallCatched?.Invoke(ball);
			
			// Await until the first ball reaches the starting hand
			await UniTask.WaitUntil(() => isNewJuggle);
			ball.Delay = 0;
		}
		
		private void RemoveBall(JuggleBall ball)
		{
			if (!jugglingBalls.Contains(ball))
			{
				Debug.LogError("Cannot remove ball! Not in collection");
				return;
			}

			prevBallsCount = jugglingBalls.Count;
			jugglingBalls.Remove(ball);
			ball.OnNewJuggle -= HandleNewJuggle;

			//TODO: readjust visual correctly
			readyToReceiveBall = false;
		}

		private void HandleNewJuggle(JuggleBall ball)
		{
			if (readyToReceiveBall)
			{
				delayIndex = 0;
				isNewJuggle = false;
				return;
			}

			// Wait!
			isNewJuggle = true;
			var delay = TotalJuggleTime / jugglingBalls.Count;
			var step = TotalJuggleTime / prevBallsCount - TotalJuggleTime / jugglingBalls.Count;

			ball.Delay = delay - step * delayIndex++;
			readyToReceiveBall = delayIndex >= jugglingBalls.Count - 1;

			if (readyToReceiveBall)
			{
				OnReadyToReceiveBall?.Invoke(this);
			}
		}

		// Absolute garbage
		private async UniTask<JuggleBall> AwaitForBallInStartingHand()
		{
			bool done = false;
			JuggleBall target = null;
			foreach (var ball in jugglingBalls)
			{
				ball.OnNewJuggle += BallOnOnNewJuggle;
			}
			
			void BallOnOnNewJuggle(JuggleBall ball)
			{
				done = true;
				target = ball;
				ball.OnNewJuggle -= BallOnOnNewJuggle;
			}
			
			await UniTask.WaitUntil(() => done);
			
			return target;
		}
	}
}