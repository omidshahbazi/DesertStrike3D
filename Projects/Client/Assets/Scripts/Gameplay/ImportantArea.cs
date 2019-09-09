using System.Collections;
using System.Collections.Generic;
using RamboTeam.Client;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class ImportantArea : MonoBehaviorBase {
    public float Range;

    private Chopter target;
    private AudioSource audioSource;
    private float sqrRange;
    protected override void Awake () {
        base.Awake ();

        target = Chopter.Instance;
        audioSource = GetComponent<AudioSource> ();
        sqrRange = Range * Range;
    }

    protected override void Update () {
        base.Update ();

        if (target == null) {
            target = Chopter.Instance;
            return;
        }

        if (target.IsDead)
            return;

        if (audioSource.clip == null)
            return;

        Vector3 diff = target.transform.position - transform.position;

        if (diff.sqrMagnitude > sqrRange) {
            if (audioSource.isPlaying)
                audioSource.Stop ();
            return;
        }

        if (audioSource.isPlaying) {
            return;
        }

        audioSource.Play ();
    }

    protected override void OnDrawGizmosSelected () {
        base.OnDrawGizmosSelected ();

        Gizmos.DrawWireSphere (transform.position, Range);
    }

}