using RamboTeam.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviorBase
{
    Camera cam;
    protected override void Start()
    {
        if (!NetworkLayer.Instance.IsPilot)
        {
            cam = ChopterPilotController.Instance.transform.Find("CoPilotCamera").GetComponent<Camera>();
        }
        else
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.forward = cam.transform.forward;

    }

    public void SetPosition()
    {

    }
}
