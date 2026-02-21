using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector3 Movement { get; private set; }
    //public bool MaskPressed { get; private set; }
    bool maskOn;

    //private void Start()
    //{
    //    maskOn = false;
    //}
    // Start is called before the first frame update
    public void OnMove(InputAction.CallbackContext ctx)
    {
        var vektor = ctx.ReadValue<Vector2>();
        MoveInput = vektor;
        Movement = new Vector3 (vektor.x, 0.0f, vektor.y);
        //Debug.Log($"Javljam se iz nove skripte. Vektor2 je {Movement}");
    }

    public void OnAction(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        // MaskPressed = true;
        if (StateMachine.GetMaskAvailability() == Mask.Lost)
            return;

        maskOn = StateMachine.GetMaskState() == MaskUse.MaskOn ? false : true;

        EventRepository.InvokeOnActionKeyPressed(maskOn);
        //Debug.Log($"Javljam se iz nove skripte. OnMask: {maskOn}");
    }

    //public void ConsumeAction()
    //{
    //    MaskPressed = false;
    //    Debug.Log($"Javljam se iz nove skripte. ConsumeMask: {MaskPressed}");
    //}
}
