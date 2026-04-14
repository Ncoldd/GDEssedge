using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private InputAction moveAction;

    private void Awake()
    {
        //movement input action using WASD/Arrows
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        // Only move if this is my own player, not other players on the network
        if (!IsOwner) return;

        Vector2 movement = moveAction.ReadValue<Vector2>();
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
