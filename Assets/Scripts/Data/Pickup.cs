using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class Pickup : ScriptableObject
{
    public GameObject geometry;
    public GameObject onPickedVFX;
    public AudioClip onPickedSFX;
    public int value;

    
}
