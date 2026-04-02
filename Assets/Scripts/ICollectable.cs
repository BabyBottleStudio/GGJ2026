using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    int GetValue { get; }
    GameObject GetOnCollectedVFX { get; }
    GameObject GetGeometry { get; }
    AudioClip GetOnCollectedSFX { get;  }



    

}
