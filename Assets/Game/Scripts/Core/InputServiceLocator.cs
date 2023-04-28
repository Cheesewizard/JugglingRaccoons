using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts.Core
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