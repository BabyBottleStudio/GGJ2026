using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 Movement { get; private set; }
    public bool MaskPressed { get; private set; }

    // Start is called before the first frame update
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Movement = ctx.ReadValue<Vector2>();
        Debug.Log($"Javljam se iz nove skripte. Vektor2 je {Movement}");
    }

    public void OnMask(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        MaskPressed = true;
        Debug.Log($"Javljam se iz nove skripte. OnMask: {MaskPressed}");

    }

    public void ConsumeMask()
    {
        MaskPressed = false;
        Debug.Log($"Javljam se iz nove skripte. ConsumeMask: {MaskPressed}");
    }
}
