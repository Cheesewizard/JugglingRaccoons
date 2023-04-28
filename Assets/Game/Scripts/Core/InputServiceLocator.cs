using UnityEngine;

namespace JugglingRaccoons.Core
{
	namespace JugglingRaccoons.Core
	{
		public class InputServiceLocator : MonoBehaviour
		{
			private static InputManager playerInput;

			public static InputManager GetPlayerInput()
			{
				return playerInput ??= new InputManager();
			}
		}
	}
}