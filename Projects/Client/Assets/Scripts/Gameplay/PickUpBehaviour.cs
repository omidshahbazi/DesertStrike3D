//Rambo Team

using RamboTeam.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class PickUpBehaviour : MonoBehaviorBase
{
    public enum PickUpType
    {
        Refugee = 0,
        HellfireAmmo,
        HydraAmmo,
        GatlingGun,
        Fuel,
        HealthPack
    }

    public PickUpType Type;
    public uint Amount = 1;
    //public float PickupRange = 30;

    //private BoxCollider minRangeCollider;
    //private Transform target;

    protected override void Awake()
    {
        base.Awake();
       // minRangeCollider = this.GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //if (target == null)
        //    return;

        //Vector3 offset = target.position - transform.position;
        //float sqrLen = offset.sqrMagnitude;

        //// square the distance we compare with
        //if (sqrLen < PickupRange * PickupRange)
        //{
        //    print("inside pickup area!");
        //};
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        //if (other.gameObject.tag == "Player")
        //{
        //    target = other.transform;
        //    Debug.Log("Chopper is Inside");
        //}
    }

    protected override void OnTriggerExit(Collider Collision)
    {
        base.OnTriggerExit(Collision);

       // target = null;
    }

    private void OnGUI()
    {

    }
}
