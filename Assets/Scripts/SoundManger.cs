using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManger : MonoBehaviour
{
    public Pickup coinData;
    public Pickup keyData;

    AudioSource audioSourceOne;

    private void Start()
    {
        audioSourceOne = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        
        // suskrajbuj se na eventove
        EventRepository.OnPickupCollected += CoinPickedUp;
        EventRepository.OnKeyCollected += KeyPickedUp;
    }

    private void OnDisable()
    {
        // unsuscribe
        EventRepository.OnPickupCollected -= CoinPickedUp;
        EventRepository.OnKeyCollected -= KeyPickedUp;
    }

    private void CoinPickedUp(object sender, PickupCollectedEventArgs e)
    {
        var coinGameObj = sender as GameObject;
        if (coinGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }
        // odsviraj zvuk
        audioSourceOne.PlayOneShot(coinData.onPickedSFX);
       
    }

    private void KeyPickedUp(object sender, PickupCollectedEventArgs e)
    {
        var coinGameObj = sender as GameObject;
        if (coinGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }
        // odsviraj zvuk
        audioSourceOne.PlayOneShot(keyData.onPickedSFX);

    }
}
