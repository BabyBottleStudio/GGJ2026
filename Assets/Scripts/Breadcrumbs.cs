using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breadcrumbs : MonoBehaviour
{
    [SerializeField] Pickup pickup;

    BreadcrumbsRepository breadcrumbs;
    Transform objPoolRoot;
    //Transform VFXPoolRoot;
    float throwForce = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        breadcrumbs = new BreadcrumbsRepository();
        objPoolRoot = new GameObject("Breadcrumbs").transform;
        //VFXPoolRoot = new GameObject("Breadcrumbs_VFX").transform;
    }


    private void OnEnable()
    {
        EventRepository.OnPickupCollected += PickBreadcrumb;
        EventRepository.OnThrowPressed += ThrowBreadcrumb;
    }

    private void OnDisable()
    {
        EventRepository.OnPickupCollected -= PickBreadcrumb;
        EventRepository.OnThrowPressed -= ThrowBreadcrumb;
    }


    public void PickBreadcrumb(object sender, PickupCollectedEventArgs e)
    {

        var newBreadcrumb = sender as GameObject;

        if (newBreadcrumb == null)
        {
            Debug.Log("Casting unsucessfull");
            return;
        }

        


        //newBreadcrumb.SetActive(false);


        // ako je obican, samo ga enqueue a ako je onaj vredniji, onda instanciraj u obj pool odgovarajuci broj komada
        Debug.Log($"Picked up {e.Value} value coin!");

        if (e.Value <= 1)
        {
            // ako je vrednost jedan, taj ces kasnije koristiti kao kamencic
            breadcrumbs.AddToPool(newBreadcrumb);
            newBreadcrumb.transform.parent = objPoolRoot;
        }
        else
        {

            //Debug.Log($"Usao sam u deo gde je value veci od 1");
            // a ako je vrednost veca, razbij mu vrednost na manje kamencice koji vrede 1
            // treba da postoji neka mustra za instanciranje
            for (int i = 0; i < e.Value; i++)
            {
                // instanciraj i ubaci u queue

                //Debug.Log($"Usao sam u loop.Vrednost \"i\" je {i}");

                var obj = Instantiate(pickup.geometry);
                breadcrumbs.AddToPool(obj);
                obj.transform.parent = objPoolRoot;
                obj.SetActive(false);

            }
        }

        StateMachine.SetCoinsState(AnyCoins.Yes);
    }

    public void ThrowBreadcrumb()
    {
        //Debug.Log("Sucsessfully started throw breadcrumb");

        if (breadcrumbs.Count == 0)
            return;


        var currentBreadcrumb = breadcrumbs.RemoveFromPool();

        CoinValue coinValue = currentBreadcrumb.GetComponent<CoinValue>();


        // scale na 0
        var spawnPosition = transform.position + transform.up * 0.75f;

        spawnPosition += -transform.forward * 0.3f; // pomeranje da bude iza ledja

        currentBreadcrumb.transform.position = spawnPosition; // ovo ce da baguje jer ce odmah da ga instant pokupi
        // unhide
        coinValue.PrepareForPicking();

        Vector3 throwDir = (-transform.forward + Vector3.up).normalized * 1.2f;

        var rb = currentBreadcrumb.GetComponent<Rigidbody>();
        rb.AddForce(throwDir * throwForce, ForceMode.Impulse);

        if (breadcrumbs.Count == 0)
            StateMachine.SetCoinsState(AnyCoins.No);

    }
}


public class BreadcrumbsRepository
{
    public int Count
    {
        get => objPool.Count;
    }

    Queue<GameObject> objPool;
    //Queue<GameObject> VFXPool;


    // kada plejer pokupi coin, on bi trebalo da se stavi u obj pool i da se stavi u queue

    public BreadcrumbsRepository()
    {
        objPool = new Queue<GameObject>();
        //VFXPool = new Queue<GameObject>();

    }

    public void AddToPool(GameObject objToAdd)
    {
        //    if (objPool == null)
        //        objPool = new Queue<GameObject>();
        objPool.Enqueue(objToAdd);
    }

    public GameObject RemoveFromPool()
    {
        return objPool.Dequeue();
    }
}