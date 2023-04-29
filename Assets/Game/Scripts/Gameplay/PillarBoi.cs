using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
	public class PillarBoi : MonoBehaviour
	{
		[SerializeField]
		private GameObject yay;
		[SerializeField]
		private GameObject hideLeft;
		[SerializeField]
		private GameObject hideRight;
		
		public void ChangeState(PillarBoiState state)
		{
			yay.SetActive(state == PillarBoiState.Yay);
			hideLeft.SetActive(state == PillarBoiState.HideLeft);
			hideRight.SetActive(state == PillarBoiState.HideRight);
		}
	}

	public enum PillarBoiState
	{
		Yay,
		HideLeft,
		HideRight
	}
}