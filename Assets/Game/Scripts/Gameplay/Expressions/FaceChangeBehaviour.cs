using System;
using Cysharp.Threading.Tasks;
using JugglingRaccoons.Gameplay.BalancingArrow;
using JugglingRaccoons.Gameplay.Customization;
using UnityEngine;

namespace JugglingRaccoons.Gameplay.Expressions
{
	public class FaceChangeBehaviour : MonoBehaviour
	{
		[SerializeField]
		private FaceSwapper faceSwapper;

		[SerializeField]
		private Blinking eyeBlinking;

		[SerializeField]
		private LocalPlayerBehaviour localPlayerBehaviour;

		private BalancingArrowBehaviour balancingBehaviour;

		private bool isBadEmotion;
		private bool isBlinking;

		private void Start()
		{
			balancingBehaviour = localPlayerBehaviour.BalancingArrowBehaviour;

			balancingBehaviour.OnDangerZoneEnter += HandleDangerZoneEnter;
			balancingBehaviour.OnDangerZoneExit += HandleDangerZoneExit;
			balancingBehaviour.OnBalanceLost += HandleBalanceLost;

			eyeBlinking.OnBlink += HandleEyeBlink;
		}

		private async void HandleEyeBlink(float blinkInterval)
		{
			if (isBadEmotion || isBlinking) return;

			isBlinking = true;
			faceSwapper.UpdateFace(FaceStates.FocusBlink);
			await UniTask.Delay(TimeSpan.FromSeconds(blinkInterval));
			faceSwapper.UpdateFace(FaceStates.Focus);
			isBlinking = false;
		}

		private void HandleDangerZoneExit(int obj)
		{
			isBadEmotion = false;
			faceSwapper.UpdateFace(FaceStates.Focus);
		}

		private void HandleBalanceLost(int obj)
		{
			isBadEmotion = true;
			faceSwapper.UpdateFace(FaceStates.KO);
		}

		private void HandleDangerZoneEnter(int obj)
		{
			isBadEmotion = true;
			faceSwapper.UpdateFace(FaceStates.Danger);
		}

		private void OnDestroy()
		{
			balancingBehaviour.OnDangerZoneEnter -= HandleDangerZoneEnter;
			balancingBehaviour.OnDangerZoneExit -= HandleDangerZoneExit;
			balancingBehaviour.OnBalanceLost -= HandleBalanceLost;

			eyeBlinking.OnBlink -= HandleEyeBlink;
		}
	}
}