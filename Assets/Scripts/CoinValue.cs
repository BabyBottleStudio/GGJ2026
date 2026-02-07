using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValue : MonoBehaviour, ICollectable
{
    public Pickup pickupData;
    //[SerializeField] int value;

    public int GetValue => pickupData.value;

    public GameObject GetOnCollectedVFX => pickupData.onPickedVFX;
    public AudioClip GetOnCollectedSFX => pickupData.onPickedSFX;

   
}
