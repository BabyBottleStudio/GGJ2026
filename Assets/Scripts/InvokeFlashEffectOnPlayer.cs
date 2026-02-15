using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeFlashEffectOnPlayer : MonoBehaviour
{
   public void FlashTrigger()
    {
        Debug.Log("Invoking");
        EventRepository.InvokeOnMaskPickupAnimFinish();
    }
}
