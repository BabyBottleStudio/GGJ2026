using UnityEngine;


public class MaskHandler : MonoBehaviour
{
    public GameObject maskGeometry;
    //public bool isMaskOn;

    private void Awake()
    {
        //isMaskOn = false;
        StateMachine.SetMaskState(MaskUse.MaskOff);
        StateMachine.SetMask(Mask.Lost);
    }

    private void OnEnable()
    {
        //StateMachine.SetState(State.MaskOff);
        EventRepository.OnKeyCollected += SubscribeToTheEvent;
    }

    private void OnDisable()
    {
        EventRepository.OnActionKeyPressed -= ShowHideMask;
    }

    void ShowHideMask(bool maskPressed)
    {
        //isMaskOn = !isMaskOn;
        StateMachine.SetMaskState(maskPressed ? MaskUse.MaskOn : MaskUse.MaskOff);

        maskGeometry.SetActive(maskPressed);
    }
  
    void SubscribeToTheEvent(object sender, PickupCollectedEventArgs e)
    {
        //isMaskOn = false;
        StateMachine.SetMask(Mask.Found);
        EventRepository.OnActionKeyPressed += ShowHideMask;
        //Debug.Log("Sucsessfully registered Show Hide mask");
        EventRepository.OnKeyCollected -= SubscribeToTheEvent; // sebe unsabskrajbuj
        //Debug.Log("Sucsessfully unregistered Subscribe to the event");
    }
}
