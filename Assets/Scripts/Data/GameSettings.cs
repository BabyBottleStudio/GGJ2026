using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Camera Settings")]
    public float CameraTransitionDuration;
    public AnimationCurve CameraTransition;

    [Header("Camera Intro Settings")]
    public float CameraIntroDuration;
    public AnimationCurve CameraIntroToGameTransition;

    public float PostProcessWeightTransitionDuration;
    public AnimationCurve PostProcesWeightTransition;


}
