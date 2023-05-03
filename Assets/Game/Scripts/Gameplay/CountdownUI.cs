using TMPro;
using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
	public class CountdownUI : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text countdownText;
		
		[SerializeField]
		private float startFontSize;
		
		[SerializeField]
		private float endFontSize;

		public void UpdateCountdown(int countdown, float t)
		{
			countdownText.fontSize = startFontSize + (endFontSize - startFontSize) * (1-t);
			countdownText.text = (countdown + 1).ToString();
		}
	}
}