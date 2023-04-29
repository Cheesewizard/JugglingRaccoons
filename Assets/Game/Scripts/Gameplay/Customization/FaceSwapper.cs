using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace JugglingRaccoons.Gameplay.Customization
{
	public class FaceSwapper : MonoBehaviour
	{
		[SerializeField]
		public SpriteResolver spriteResolver;

		private const string CATEGORY = "Head";

		[Button]
		public void UpdateFace(FaceStates faceState)
		{
			var label = string.Empty;
			switch (faceState)
			{
				case FaceStates.Default:
					label = "Default";
					break;
				case FaceStates.Danger:
					label = "Danger";
					break;
				case FaceStates.Focus:
					label = "Focus";
					break;
				case FaceStates.FocusBlink:
					label = "Focus Blink";
					break;
				case FaceStates.KO:
					label = "KOed";
					break;
				case FaceStates.Win:
					label = "Win";
					break;
				case FaceStates.Wobbling:
					label = "Wobbling";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(faceState), faceState, null);
			}

			if (string.IsNullOrEmpty(label))
			{
				Debug.LogError("Face state was not correctly defined when swapping the face");
				return;
			}

			spriteResolver.SetCategoryAndLabel(CATEGORY, label);
			spriteResolver.ResolveSpriteToSpriteRenderer();
		}
	}

	public enum FaceStates
	{
		Default,
		Danger,
		Focus,
		FocusBlink,
		KO,
		Win,
		Wobbling
	}
}