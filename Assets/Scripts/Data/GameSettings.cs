using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Camera Settings")]
    public float CameraTransitionDuration;
    public AnimationCurve CameraTransition;


}
