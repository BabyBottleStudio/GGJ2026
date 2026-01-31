using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public GameObject playerPrefab;
    public GameObject maskPrefab;
     
    public Sprite playerIcon;
    public Sprite playerIconWithMask;

    public Sprite emptyMask;
    public Sprite collectedMask;

    public AudioClip maskSwapSound;



}
