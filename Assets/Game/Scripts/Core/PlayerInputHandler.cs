using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
	public PlayerInput PlayerInput { get; private set; }
	public InputAction BalanceAction { get; private set; }
	public InputAction ThrowAction { get; private set; }

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();
		BalanceAction = PlayerInput.currentActionMap.FindAction("Balance");
		ThrowAction = PlayerInput.currentActionMap.FindAction("Throw");
	}
}