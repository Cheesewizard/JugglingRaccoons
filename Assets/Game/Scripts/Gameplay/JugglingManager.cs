using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
	public class JugglingManager : MonoBehaviour
	{
		[SerializeField]
		private JuggleBall[] balls;
		[SerializeField]
		private Transform leftHand;
		[SerializeField]
		private Transform rightHand;

		private void Start()
		{
			var step = 1f / (balls.Length);
			var halfStep = step / 2;
			for (int i = 0; i < balls.Length; i++)
			{
				balls[i].SetInitialJugglePosition(i * step + halfStep);
			}
		}

		private void Update()
		{
			foreach (var ball in balls)
			{
				ball.Move(leftHand.position, rightHand.position);
			}
		}

		public static Vector3 GetParabolicPoint(Vector3 start, Vector3 end, float height, float t)
		{
			var y = Mathf.Sin(Mathf.PI * t) * height;
			return Vector3.Lerp(start, end, t) + Vector3.up * y;
		} 
	}
}