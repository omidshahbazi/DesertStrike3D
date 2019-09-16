using RamboTeam.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadarBehaviour : MonoBehaviorBase
{
    public Transform RoatingObject;
    public float rotateSpeed;
    public bool isClockWWise = true;
    public bool LockX, LockY, LockZ;
    public bool isWWorldSpace = true;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        float MoveAmount = Time.smoothDeltaTime * rotateSpeed;
        RoatingObject.Rotate(LockX ? MoveAmount : 0.0F, LockY ? MoveAmount : 0.0F, LockZ ? MoveAmount : 0.0F, isWWorldSpace ? Space.World : Space.Self);
    }
}
