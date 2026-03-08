using UnityEngine;
using UnityEngine.Playables;

public class PickupHandler : MonoBehaviour
{
    //public Pickup coinData;
    // public Pickup gemData;
    public Pickup keyData;
    public PlayableDirector timeline;

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

        GameObject onPickedVFX = collectableGameObj.GetComponent<ICollectable>().GetOnCollectedVFX;
        Instantiate(onPickedVFX, collectableGameObj.transform.position, Quaternion.identity);

        collectableGameObj.SetActive(false);
    }

    private void KeyPickedUp(object sender, PickupCollectedEventArgs e)
    {
        var collectableGameObj = sender as GameObject;
        if (collectableGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }

        Debug.Log("Cutscene Working");
        collectableGameObj.SetActive(false);

        timeline.gameObject.transform.position = collectableGameObj.transform.position;
        timeline.gameObject.SetActive(true);
        timeline.Play();
    }


}
