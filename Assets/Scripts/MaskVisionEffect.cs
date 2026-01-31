using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskVisionEffect : MonoBehaviour
{
    Camera camera;
    public LayerMask ghostLayer;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        camera.cullingMask &= ~ghostLayer;
    }

    private void OnEnable()
    {
        EventRepository.OnActionKeyPressed += ToggleGhostVision; // ili subskrajbovati kasnije
    }

    private void OnDisable()
    {
        EventRepository.OnActionKeyPressed += ToggleGhostVision;
        
    }
    // Update is called once per frame


    void ToggleGhostVision(object sender, ActionPressedEventArgs e)
    {
        if (e.Value)
        {
            // Kada je maska UKLJUČENA:
            // Uključi "Ghosts" layer u Culling Mask-u kamere (OR operacija)
            camera.cullingMask |= ghostLayer;
        }
        else
        {
            // Kada je maska ISKLJUČENA:
            // Isključi "Ghosts" layer iz Culling Mask-a kamere (AND NOT operacija)
            camera.cullingMask &= ~ghostLayer;
        }
    }

}
