using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    AudioClip GetOnThrowSFX { get; }
    AudioClip GetOnGroundHitSFX { get; }
}
