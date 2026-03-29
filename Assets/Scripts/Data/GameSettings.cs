using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Mask Transition Camera")]
    public float CameraTransitionDuration;
    public AnimationCurve CameraTransition;

    [Header("Intro Transition Camera")]
    public float CameraIntroTransitionDuration;
    public AnimationCurve CameraIntroToGamePosTransition;
    public AnimationCurve CameraIntroToGameRotationTransition;

    public AnimationCurve CameraIntroFOVTransition;
    public float CameraFOVTransitionDuration;
    public float IntroFOV;
    public float GameFOV;



    public float PostProcessWeightTransitionDuration;
    public AnimationCurve PostProcesWeightTransition;


}
