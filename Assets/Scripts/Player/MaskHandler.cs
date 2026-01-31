using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskHandler : MonoBehaviour
{

    public GameObject maskGeometry;

    public bool isMaskOn;
   

    private void OnEnable()
    {
        isMaskOn = false;
        EventRepository.OnKeyCollected += SubscribeToTheEvent;

       // event 
    }

    private void OnDisable()
    {
        EventRepository.OnActionKeyPressed -= ShowHideMask;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0))
        {
            isMaskOn = !isMaskOn;
            EventRepository.InvokeOnActionKeyPressed(isMaskOn);
        }
    }


    void ShowHideMask(object sender, ActionPressedEventArgs e)
    {
        
        maskGeometry.SetActive(isMaskOn);
    }

    void SubscribeToTheEvent(object sender, PickupCollectedEventArgs e)
    {
        EventRepository.OnActionKeyPressed += ShowHideMask;
        Debug.Log("Sucsessfully registered Show Hide mask");
        EventRepository.OnKeyCollected -= SubscribeToTheEvent; // sebe unsabskrajbuj
        Debug.Log("Sucsessfully unregistered Subscribe to the event");
    }
}
