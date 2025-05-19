using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public InputActionReference MoveInput;
    public Rigidbody Body;
    public float Speed = 3f;

    private Vector3 _moveDir;

    private void OnEnable()
    {
        MoveInput.action.performed += ReadMoveInput;
        MoveInput.action.canceled += ReadMoveInput;
    }

    private void OnDisable()
    {
        MoveInput.action.performed -= ReadMoveInput;
        MoveInput.action.canceled -= ReadMoveInput;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadMoveInput(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        _moveDir = new Vector3(rawInput.x, 0f, rawInput.y);
    }

    private void Move()
    {
        if (_moveDir.sqrMagnitude <= 0f) return;
        
        Body.AddForce(_moveDir * (Speed * Time.fixedTime));
    }
}
