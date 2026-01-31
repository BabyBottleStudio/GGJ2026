using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public Pickup coinData;
    public Pickup keyData;

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
        var particle = Instantiate(coinData.onPickedVFX, coinGameObj.transform.position, Quaternion.identity);
        //particle.Emit(50);
        coinGameObj.SetActive(false);
    }

    private void KeyPickedUp(object sender, PickupCollectedEventArgs e)
    {
        var keyGameObj = sender as GameObject;
        if (keyGameObj == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }
        var particle = Instantiate(keyData.onPickedVFX, keyGameObj.transform.position, Quaternion.identity);
        //particle.Emit(50);
        keyGameObj.SetActive(false);
    }
}
