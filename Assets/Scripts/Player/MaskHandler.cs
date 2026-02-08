using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskHandler : MonoBehaviour
{
    private InputAction actionKey;
    public GameObject maskGeometry;

    public bool isMaskOn;

    private void Awake()
    {

        isMaskOn = false;
        StateMachine.SetState(State.MaskOff);
        StateMachine.SetMask(Mask.Lost);
        //actionKey = new InputAction(
        //    "Action",
        //    InputActionType.Button,
        //    "<Keyboard>/e"
        //);

        actionKey = new InputAction("Action", InputActionType.Button);

        // Tastatura
        actionKey.AddBinding("<Keyboard>/e");

        // Miš (levi klik)
        actionKey.AddBinding("<Mouse>/leftButton");

        // Kontroler (primer: X dugme na Xbox kontroleru)
        actionKey.AddBinding("<Gamepad>/buttonWest");
    }
    private void OnEnable()
    {
        //isMaskOn = true; // ovo je opasno ostaviti ovako, ali je bagovala prva upotreba maske
        StateMachine.SetState(State.MaskOff);
        EventRepository.OnKeyCollected += SubscribeToTheEvent;

        actionKey.Enable();
        actionKey.performed += OnAction;

    }

    private void OnDisable()
    {
        EventRepository.OnActionKeyPressed -= ShowHideMask;
        actionKey.performed -= OnAction;
        actionKey.Disable();
    }



    void ShowHideMask(object sender, ActionPressedEventArgs e)
    {
        isMaskOn = e.isMaskOn;

        if (isMaskOn)
        {
            StateMachine.SetState(State.MaskOn);
        }
        else
        {
            StateMachine.SetState(State.MaskOff);

        }

        maskGeometry.SetActive(isMaskOn);
    }

    void SubscribeToTheEvent(object sender, PickupCollectedEventArgs e)
    {
        isMaskOn = false;
        EventRepository.OnActionKeyPressed += ShowHideMask;
        Debug.Log("Sucsessfully registered Show Hide mask");
        EventRepository.OnKeyCollected -= SubscribeToTheEvent; // sebe unsabskrajbuj
        Debug.Log("Sucsessfully unregistered Subscribe to the event");
    }

    private void OnAction(InputAction.CallbackContext ctx)
    {
        if (StateMachine.GetPlayerInputState() == PlayerControlls.Off)
            return;
        
        isMaskOn = !isMaskOn;
        EventRepository.InvokeOnActionKeyPressed(isMaskOn);
    }
}
