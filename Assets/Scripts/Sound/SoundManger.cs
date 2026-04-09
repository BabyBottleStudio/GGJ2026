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
    public Pickup basicCoin;

    public AudioClip openInteractionSound;
    public AudioClip buttonClick;

    public AudioSource audioSourceOne;
    public AudioSource audioSourceTwo;

    public AudioSource interactionDialogSound;



    private void OnEnable()
    {

        // suskrajbuj se na eventove
        EventRepository.OnPickupCollected += CollectablePicked;
        //EventRepository.OnKeyCollected += CollectablePicked;
        //EventRepository.OnActionKeyPressed += MaskSwap; // registrovan je dole u metodi
        EventRepository.OnThrowPressed += PlayThrowSound;
        //EventRepository.OnCoinHitFloor += PlayCoinHitFloor;
        EventRepository.OnInteractionStart += PlayInteractionOpen;
        EventRepository.OnInteractionMenuClose += PlayInteractionClose;
    }

    private void OnDisable()
    {
        // unsuscribe
        EventRepository.OnPickupCollected -= CollectablePicked;
        //EventRepository.OnKeyCollected -= CollectablePicked;
        EventRepository.OnActionKeyPressed -= MaskSwap;
        EventRepository.OnThrowPressed -= PlayThrowSound;
        //EventRepository.OnCoinHitFloor -= PlayCoinHitFloor;
        EventRepository.OnInteractionStart -= PlayInteractionOpen;
        EventRepository.OnInteractionMenuClose -= PlayInteractionClose;
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

    void PlayInteractionOpen(object sender, InteractionEventArgs e)
    {
        if (StateMachine.GetInteractionState() == Interaction.Done)
            interactionDialogSound.PlayOneShot(openInteractionSound);
    }

    void PlayInteractionClose()
    {
        interactionDialogSound.PlayOneShot(openInteractionSound);
    }

    void PlayThrowSound()
    {
        if (StateMachine.GetCoinsState() == AnyCoins.Yes)
        {
            if (basicCoin.onThrowSFX != null)
                audioSourceOne.PlayOneShot(basicCoin.onThrowSFX);
        }
        else
        {
            if (basicCoin.onEmptyThrowSFX != null)
                audioSourceOne.PlayOneShot(basicCoin.onEmptyThrowSFX);
        }
    }

    void PlayCoinHitFloor()
    {
        if (basicCoin.onGroundHitSFX != null && !audioSourceTwo.isPlaying)
            audioSourceTwo.PlayOneShot(basicCoin.onGroundHitSFX);

        // logika je prebacena na coin zarad testiranja laga na zvuku
    }

    public void PlayButtonSound()
    {
        interactionDialogSound.PlayOneShot(buttonClick);
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

        audioSourceTwo.PlayOneShot(playerData.maskSwapSound, 0.4f);
    }

    /*
    void MaskSwap(object sender, ActionPressedEventArgs e)
    {
        audioSourceTwo.PlayOneShot(playerData.maskSwapSound);
    }
    */

}
