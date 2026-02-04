using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskVisionEffect : MonoBehaviour
{
    Camera mainCamera;
    public LayerMask ghostLayer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.cullingMask &= ~ghostLayer;
    }

    private void OnEnable()
    {
        EventRepository.OnKeyCollected += SubscribeToEvent;
    }

    private void OnDisable()
    {
        EventRepository.OnActionKeyPressed -= ToggleGhostVision;
        
    }
    // Update is called once per frame


    void SubscribeToEvent(object sender, PickupCollectedEventArgs e)
    {
        EventRepository.OnActionKeyPressed += ToggleGhostVision;
        EventRepository.OnKeyCollected -= SubscribeToEvent;

    }


    void ToggleGhostVision(object sender, ActionPressedEventArgs e)
    {
        if (e.Value)
        {
            // Kada je maska UKLJUČENA:
            // Uključi "Ghosts" layer u Culling Mask-u kamere (OR operacija)
            mainCamera.cullingMask |= ghostLayer;
        }
        else
        {
            // Kada je maska ISKLJUČENA:
            // Isključi "Ghosts" layer iz Culling Mask-a kamere (AND NOT operacija)
            mainCamera.cullingMask &= ~ghostLayer;
        }
    }

}
