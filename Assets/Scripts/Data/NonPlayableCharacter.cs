using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[CreateAssetMenu]
public class NonPlayableCharacter : ScriptableObject
{
    public GameObject Geometry;
    public Sprite Icon;
    public LocalizedString dialogue;

}
