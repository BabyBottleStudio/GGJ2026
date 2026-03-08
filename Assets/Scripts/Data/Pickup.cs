using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEngine.Playables;

[CreateAssetMenu]
public class Pickup : ScriptableObject
{
    public GameObject geometry;
    public GameObject onPickedVFX;
    //public PlayableDirector timeline;

    public AudioClip onPickedSFX;
    public int value;

    
}
