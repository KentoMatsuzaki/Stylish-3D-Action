using UnityEngine;
using Unity.TinyCharacterController.Control;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private MoveControl _moveControl;
    
    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションが入力された場合
        if (context.performed) _moveControl.Move(context.ReadValue<Vector2>());

        // Moveアクションがリリースされた場合
        else if (context.canceled) _moveControl.Move(Vector2.zero);
    }
}
