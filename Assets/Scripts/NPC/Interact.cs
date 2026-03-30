using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour, IInteractable
{

    [SerializeField] string textToSay;
    public string GetTextToSay() => textToSay;
    
}
