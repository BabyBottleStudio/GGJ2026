using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHitDetection : MonoBehaviour
{

    bool isHit;
    AudioSource audioSource;
    Pickup pickupData;

    //AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //pickupData = GetComponent<CoinValue>().pickupData;
    }


    private void OnEnable()
    {
        isHit = false;

    }
   /*
    private void OnTriggerEnter(Collider other)
    {
        if (isHit)
            return;

        isHit = true;
        Debug.Log("Coin sucsessfully fell");
        EventRepository.InvokeOnCoinHitFloor();
    }
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (isHit)
            return;

        isHit = true;
        Debug.Log("Coin sucsessfully fell");
        //EventRepository.InvokeOnCoinHitFloor();
        //audioSource.clip = pickupData.onGroundHitSFX;
        audioSource.Play();

    }


}
