using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public bool isLocked = true;
    [SerializeField] GameObject lockedGeometry;
    [SerializeField] GameObject unlockedGeometry;

    private void OnEnable()
    {
        EventRepository.OnKeyCollected += UnlockTheGate;
    }

    private void OnDisable()
    {
        
        EventRepository.OnKeyCollected -= UnlockTheGate;
    }
   

    public void UnlockTheGate(object sender, PickupCollectedEventArgs e)
    {
        lockedGeometry.SetActive(false);
        unlockedGeometry.SetActive(true);
    }
}
