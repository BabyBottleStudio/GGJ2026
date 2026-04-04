using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour, IInteractable
{
    [SerializeField] InteractiveObject npcData;

    public InteractiveObject OnPlayerEnter() => npcData;

    public void SetNPCData(InteractiveObject npcData)
    {
        this.npcData = npcData;
    }
    
}
