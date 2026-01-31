using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            EventRepository.InvokeOnPickupCollected(1, other.gameObject);
        }

        if (other.CompareTag("Key"))
        {
            EventRepository.InvokeOnKeyCollected(0, other.gameObject);
        }
    }




}
