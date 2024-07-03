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
        // Move�A�N�V���������͂��ꂽ�ꍇ
        if (context.performed) _moveControl.Move(context.ReadValue<Vector2>());

        // Move�A�N�V�����������[�X���ꂽ�ꍇ
        else if (context.canceled) _moveControl.Move(Vector2.zero);
    }
}
