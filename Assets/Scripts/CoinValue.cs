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
    BoxCollider boxCollider;
    CapsuleCollider capsuleCollider;

    public int GetValue => pickupData.value;



    private void Awake()
    {
        SetReferences();
        
    }

    private void Start()
    {
        PrepareForPicking();

    }



    //private void OnEnable()
    //{
    //    SetReferences();
    //    Debug.Log("Setting up references from OnEnable");
    //}

    public void SetReferences()
    {
        //Debug.Log(string.Format("------------------- Javljam se iz {0} metode", flag));
        geometry = transform.Find("Geometry").gameObject;
        OnCollectedVFX = transform.Find("VFX").gameObject;
        rb = GetComponent<Rigidbody>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void PrepareForPicking()
    {
        geometry.SetActive(true);
        OnCollectedVFX.SetActive(false);
        
        capsuleCollider.enabled = true;
        boxCollider.enabled = true;

        rb.useGravity = true;
    }

    public Rigidbody GetCoinRB() => rb;


    public GameObject GetGeometry => geometry;

    public GameObject GetOnCollectedVFX => OnCollectedVFX;

    public AudioClip GetOnCollectedSFX => pickupData.onPickedSFX;

    public void OnCollect()
    {
        geometry.SetActive(false);
        OnCollectedVFX.SetActive(true);
        
        capsuleCollider.enabled = false;
        boxCollider.enabled = false;

        rb.useGravity = false;

        if (rb.isKinematic)
            rb.isKinematic = false;
    }


    //public AudioClip GetOnThrowSFX => pickupData.onThrowSFX;

    //public AudioClip GetOnGroundHitSFX => pickupData.onGroundHitSFX;

}
