using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
	public class PillarBoi : MonoBehaviour
	{
		[SerializeField]
		private GameObject yay;
		[SerializeField]
		private GameObject hide;

		public void ChangeState(PillarBoiState state)
		{
			yay.SetActive(state == PillarBoiState.Yay);
			hide.SetActive(state == PillarBoiState.Hide);
		}
	}

	public enum PillarBoiState
	{
		Yay,
		Hide
	}
}