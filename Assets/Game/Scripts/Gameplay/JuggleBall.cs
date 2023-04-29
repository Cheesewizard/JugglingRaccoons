using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JugglingRaccoons.Gameplay
{
	public class JuggleBall : MonoBehaviour
	{
		[SerializeField]
		private float rotateSpeed = 123f;

		public JuggleState State { get; private set; }
		public float Delay { get; set; }
		
		private JugglingBehaviour jugglingBehaviour;
		private Transform startTransform;
		private Transform endTransform;
		private float travelTime;

		public event Action<JuggleBall> OnNewJuggle;
		public event Action<JuggleBall> OnReachedTarget;

		private void Awake()
		{
			transform.Rotate(0,0, Random.Range(0, 360));
		}

		public void SetJugglingBehaviour(JugglingBehaviour jugglingBehaviour)
		{
			this.jugglingBehaviour = jugglingBehaviour;
			startTransform = this.jugglingBehaviour.StartingHand;
			endTransform = this.jugglingBehaviour.PassingHand;
		}

		public void Throw(JugglingBehaviour target)
		{
			State = JuggleState.Throwing;
			endTransform = target.StartingHand;
		}
		
		private void Update()
		{
			if(!jugglingBehaviour) return;

			if (Delay > 0)
			{
				// Keep following the hand
				transform.position = startTransform.position;
				Delay -= Time.deltaTime;
				return;
			}
			
			travelTime += Time.deltaTime;
			transform.Rotate(0,0, rotateSpeed * Time.deltaTime);

			if (State == JuggleState.Throwing)
			{
				var throwTime = jugglingBehaviour.ThrowTime;
				var throwHeight = jugglingBehaviour.ThrowHeight;

				if (travelTime > throwTime)
				{
					travelTime -= throwTime;
					SetJugglingBehaviour(jugglingBehaviour);
					State = JuggleState.Juggling;
					OnReachedTarget?.Invoke(this);
					return;
				}

				var throwT = travelTime / throwTime;
				transform.position = JugglingBehaviour.GetParabolicPoint(startTransform.position, endTransform.position, throwHeight, throwT);
				return;
			}
			
			var totalJuggleTime = jugglingBehaviour.TotalJuggleTime;
			var juggleTime = jugglingBehaviour.JuggleTime;
			var handPassTime = jugglingBehaviour.HandPassTime;
			var juggleHeight = jugglingBehaviour.JuggleHeight;
			var handPassHeight = jugglingBehaviour.HandPassHeight;
			
			while (travelTime > jugglingBehaviour.TotalJuggleTime)
			{
				// New juggle
				travelTime -= totalJuggleTime;
				OnNewJuggle?.Invoke(this);
			}

			var juggleT = travelTime / juggleTime;
			var passT = (travelTime - juggleTime) / handPassTime;
			var isPassing = passT > 0;
			var height = (isPassing) ? handPassHeight : juggleHeight;
			var t = (isPassing) ? 1 - passT : juggleT;
			if (isPassing)
			{
				State = JuggleState.Passing;
			}
			
			transform.position = JugglingBehaviour.GetParabolicPoint(startTransform.position, endTransform.position, height, t);
		}
	}

	public enum JuggleState
	{
		Juggling,
		Passing,
		Throwing,
		Falling
	}
}