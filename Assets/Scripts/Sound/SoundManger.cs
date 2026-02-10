using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManger : MonoBehaviour
{
    //public Pickup coinData;
    //public Pickup gemData;
    //public Pickup keyData;
    public PlayerData playerData;

    public AudioSource audioSourceOne;
    public AudioSource audioSourceTwo;



    private void OnEnable()
    {

        // suskrajbuj se na eventove
        EventRepository.OnPickupCollected += CollectablePicked;
        EventRepository.OnKeyCollected += CollectablePicked;
        //EventRepository.OnActionKeyPressed += MaskSwap; // registrovan je dole u metodi

    }

    private void OnDisable()
    {
        // unsuscribe
        EventRepository.OnPickupCollected -= CollectablePicked;
        EventRepository.OnKeyCollected -= CollectablePicked;
        EventRepository.OnActionKeyPressed -= MaskSwap;
    }

    private void CollectablePicked(object sender, PickupCollectedEventArgs e)
    {
        var coinGameObj = sender as GameObject;
        if (coinGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }
        // odsviraj zvuk
        AudioClip onPickedSFX = coinGameObj.GetComponent<ICollectable>().GetOnCollectedSFX;
        audioSourceOne.PlayOneShot(onPickedSFX);
        EventRepository.OnActionKeyPressed += MaskSwap;
    }

    void MaskSwap(bool maskOn)
    {
      
        if (audioSourceTwo == null)
        {
            Debug.Log("audio source is null");
            return;
        }

        if (playerData.maskSwapSound == null)
        {
            Debug.Log("Sound is null");
            return;
        }

        audioSourceTwo.PlayOneShot(playerData.maskSwapSound);
    }

    /*
    void MaskSwap(object sender, ActionPressedEventArgs e)
    {
        audioSourceTwo.PlayOneShot(playerData.maskSwapSound);
    }
    */

}
