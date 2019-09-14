using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RamboTeam.Client;
using UnityEngine.AI;


public enum PatrolMode
{
    Sequential,
    Random
}
[RequireComponent(typeof(NavMeshAgent))]

public class PatrolBehaviour : MonoBehaviorBase
{

    public List<Vector3> Points = new List<Vector3>();
    public PatrolMode patrolMode = PatrolMode.Sequential;
    public float DelayOnEachPoint = 5.0F;
    private int pointNumber = 0;
    private bool isProcessing = true;


    private NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(GoToNextPoint());
    }

    protected override void Update()
    {
        base.Update();

        if (!isProcessing && !agent.pathPending && agent.remainingDistance < 0.5F)
        {
            StartCoroutine(GoToNextPoint());
        }
    }

    private IEnumerator GoToNextPoint()
    {
        isProcessing = true;
        yield return new WaitForSeconds(DelayOnEachPoint);

        if (Points.Count == 0)
            yield return null;

        agent.destination = Points[pointNumber];

        if (patrolMode == PatrolMode.Sequential)
            pointNumber = (pointNumber + 1) % Points.Count;
        else
        {
            int currentPoint = pointNumber;

            while (pointNumber == currentPoint)
            {
                pointNumber = UnityEngine.Random.Range(0, Points.Count);
            }
        }

        isProcessing = false;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        for (int i = 0; i < Points.Count; ++i)
            Gizmos.DrawSphere(Points[i], 4);
    }


}
