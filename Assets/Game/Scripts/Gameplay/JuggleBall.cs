using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
	public class JuggleBall : MonoBehaviour
	{
		[SerializeField]
		private float juggleHeight = 3;
		[SerializeField]
		private float juggleTime = 1;
		[SerializeField]
		private float handPassHeight = 0.1f;
		[SerializeField]
		private float handPassTime = 0.3f;
		[SerializeField]
		private float rotateSpeed = 123f;
		
		private float totalJuggleTime => juggleTime + handPassTime;

		private float time;
		private bool isPassing;
		
		public void SetInitialJugglePosition(float t)
		{
			time = juggleTime * t;
			transform.Rotate(0,0, Random.Range(0, 360));
			Debug.Log($"Setting t to {t}");
		}

		public void Move(Vector3 start, Vector3 end)
		{
			time += Time.deltaTime;
			while (time > totalJuggleTime)
			{
				time -= totalJuggleTime;
			}

			var juggleT = time / juggleTime;
			var passT = (time - juggleTime) / handPassTime;
			isPassing = passT > 0;
			var height = (isPassing) ? handPassHeight : juggleHeight;
			var t = (isPassing) ? 1 - passT : juggleT;

			transform.position = JugglingManager.GetParabolicPoint(start, end, height, t);
			transform.Rotate(0,0, rotateSpeed * Time.deltaTime);
		}
	}
}