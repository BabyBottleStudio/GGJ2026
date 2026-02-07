using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    int GetValue { get; }
    GameObject GetOnCollectedVFX { get; }
    AudioClip GetOnCollectedSFX { get;  }

}
