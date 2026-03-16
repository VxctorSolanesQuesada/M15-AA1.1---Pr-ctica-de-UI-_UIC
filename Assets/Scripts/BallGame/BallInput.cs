using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallController))]
public class BallInput : MonoBehaviour
{
    private BallGameInputSystem input;

    BallController controller;
    private void Start()
    {
        controller = GetComponent<BallController>();
        input = new BallGameInputSystem();
        input.Enable();
    }
    void Update()
    {
        if (input.Player.Jump.WasPressedThisFrame()) { 
            controller.Jump();
        }

        Vector2 move = input.Player.Move.ReadValue<Vector2>();

        if (move != Vector2.zero)
        {
            controller.Move(move);
        }
    }


    private void OnDisable()
    {
        input?.Disable();
    }
}
