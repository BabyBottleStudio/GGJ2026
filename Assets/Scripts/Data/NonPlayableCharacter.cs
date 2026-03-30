using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class NonPlayableCharacter : ScriptableObject
{
    public GameObject Geometry;

    public Sprite Icon;

    string textToSay;

    //public string GetTextToSay()
    //{
    //    if (textToSay is null)
    //        return String.Empty; 

    //    return textToSay;
    //}

}
