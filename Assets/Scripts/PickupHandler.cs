using UnityEngine;
using UnityEngine.Playables;

public class PickupHandler : MonoBehaviour
{
    //public Pickup coinData;
    // public Pickup gemData;
    public Pickup keyData;
    public PlayableDirector pickupMaskAnim;
    public PlayableDirector CutsceneManager;

    private void OnEnable()
    {
        // suskrajbuj se na eventove
        EventRepository.OnPickupCollected += CollectablePickedUp;
        EventRepository.OnKeyCollected += KeyPickedUp;
    }

    private void OnDisable()
    {
        // unsuscribe
        EventRepository.OnPickupCollected -= CollectablePickedUp;
        EventRepository.OnKeyCollected -= KeyPickedUp;
    }

    private void CollectablePickedUp(object sender, PickupCollectedEventArgs e)
    {
        var collectableGameObj = sender as GameObject;
        if (collectableGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }

        var coinValue = collectableGameObj.GetComponent<CoinValue>();
        GameObject onPickedVFX = coinValue.GetOnCollectedVFX;
        coinValue.OnCollect();
        //Instantiate(onPickedVFX, collectableGameObj.transform.position, Quaternion.identity);

        //collectableGameObj.SetActive(false);
    }

    private void KeyPickedUp(object sender, PickupCollectedEventArgs e)
    {
        var collectableGameObj = sender as GameObject;
        if (collectableGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }

        collectableGameObj.SetActive(false);

        pickupMaskAnim.gameObject.transform.position = collectableGameObj.transform.position;
        //pickupMaskAnim.gameObject.SetActive(true);
        //Debug.Log("Cutscene Working 1");
        CutsceneManager.Play();
    }


}
