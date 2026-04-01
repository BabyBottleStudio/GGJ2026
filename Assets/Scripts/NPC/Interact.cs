using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour, IInteractable
{
    [SerializeField] NonPlayableCharacter npcData;

    public NonPlayableCharacter OnPlayerEnter() => npcData;

    public void SetNPCData(NonPlayableCharacter npcData)
    {
        this.npcData = npcData;
    }
    
}
