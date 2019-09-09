//Rambo Team

using RamboTeam.Client;
using System;
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
        HealthPack,
        EngineBoost
    }

    public PickUpType Type;
    public uint Amount = 1;
    public float OnAreaRange = 10.0f;
    public bool isOnAreaAudioLoop = true;
    public AudioClip onAreaAudio;
    public AudioClip OnPickAudio;
    private float OnAreaRangesqr;
    private Chopter target;

    private AudioSource onAreaAudioSource = null;
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

        OnAreaRangesqr = OnAreaRange * OnAreaRange;
        target = Chopter.Instance;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (target == null)
        {
            target = Chopter.Instance;
            return;
        }

        if (target.IsDead)
            return;


        Vector3 diff = target.transform.position - transform.position;

        if (diff.sqrMagnitude > OnAreaRangesqr)
        {
            if (onAreaAudioSource != null && onAreaAudioSource.isPlaying)
                onAreaAudioSource.Stop();
            return;
        }

        if (onAreaAudioSource != null && onAreaAudioSource.isPlaying)
        {
            return;
        }

        onAreaAudioSource = AudioManager.Instance.PlayAudio(onAreaAudio, Chopter.Instance.transform.position, null, 0, 1, isOnAreaAudioLoop);
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

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireSphere(transform.position, OnAreaRange);
    }
    internal void Picked()
    {

        if (OnPickAudio != null)
        {
            AudioManager.Instance.PlayAudio(OnPickAudio, Chopter.Instance.transform.position, null);
        }
    }
}
