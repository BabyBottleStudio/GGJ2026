using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValue : MonoBehaviour, ICollectable
{
    public Pickup pickupData;
    //[SerializeField] int value;

    private GameObject geometry;
    private GameObject OnCollectedVFX;

    Rigidbody rb;

    public int GetValue => pickupData.value;

    private void Start()
    {
        geometry = transform.Find("Geometry").gameObject;
        OnCollectedVFX = transform.Find("VFX").gameObject;
        rb = GetComponent<Rigidbody>();
        PrepareForPicking();
    }

    public void PrepareForPicking()
    {
        geometry.SetActive(true);
        OnCollectedVFX.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        rb.useGravity = true;
    }

    public GameObject GetGeometry => geometry;

    public GameObject GetOnCollectedVFX => OnCollectedVFX;

    public AudioClip GetOnCollectedSFX => pickupData.onPickedSFX;

    public void OnCollect()
    {
        geometry.SetActive(false);
        OnCollectedVFX.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        rb.useGravity = false;
        if (rb.isKinematic)
            rb.isKinematic = false;
    }
    //public AudioClip GetOnThrowSFX => pickupData.onThrowSFX;

    //public AudioClip GetOnGroundHitSFX => pickupData.onGroundHitSFX;
}
