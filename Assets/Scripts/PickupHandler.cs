using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    //public Pickup coinData;
    // public Pickup gemData;
    public Pickup keyData;

    private void OnEnable()
    {
        // suskrajbuj se na eventove
        EventRepository.OnPickupCollected += CollectablePickedUp;
        EventRepository.OnKeyCollected += CollectablePickedUp;
    }

    private void OnDisable()
    {
        // unsuscribe
        EventRepository.OnPickupCollected -= CollectablePickedUp;
        EventRepository.OnKeyCollected -= CollectablePickedUp;
    }

    private void CollectablePickedUp(object sender, PickupCollectedEventArgs e)
    {
        var collectableGameObj = sender as GameObject;
        if (collectableGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }

        GameObject onPickedVFX = collectableGameObj.GetComponent<ICollectable>().GetOnCollectedVFX;
        Instantiate(onPickedVFX, collectableGameObj.transform.position, Quaternion.identity);

        collectableGameObj.SetActive(false);
    }

  
}
