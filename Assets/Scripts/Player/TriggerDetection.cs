using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        int value = other.GetComponent<ICollectable>() != null ? other.GetComponent<ICollectable>().GetValue : 0;

        if (other.CompareTag("Collectable"))
        {
            EventRepository.InvokeOnPickupCollected(value, other.gameObject);
        }

        if (other.CompareTag("Key"))
        {
            StateMachine.SetMask(Mask.Found); // ovo bi trebalo da se prebaci negde drugde
            EventRepository.InvokeOnKeyCollected(value, other.gameObject);
        }

        if (other.CompareTag("Exit"))
        {
            Debug.Log("Level sucsess");
            EventRepository.InvokeOnLevelFinished();
        }

        if (other.CompareTag("CameraTrigger"))
        {
            StateMachine.SetTile(Tile.Special);
            EventRepository.InvokeOnEnterTile(other, other.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CameraTrigger"))
        {
            StateMachine.SetTile(Tile.Regular);
            EventRepository.InvokeOnExitTile(other, other.gameObject.transform.position);
        }
    }

  



}
