using RamboTeam.Client;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mission
{
    [Tooltip("Text Show In Mission # Accomplished in HUD, It will Replaced WWith #")]
    public string Title;
    [Tooltip("Text To See in Mission Window")]
    public string Description;
    [Tooltip("Pick Type of Task")]
    public MissionManager.TaskTypes TaskType;
    [Tooltip("Set the pick up type, work only if task type is set to pickup")]
    public PickUpBehaviour.PickUpType PickUpType;
    [Tooltip("Set the enemy type to destroy, work only if task type is set to destroy")]
    public EnemyType EnemyType;
    [Tooltip("Set the count of this task")]
    public int Count = 1;
    [Tooltip("Set the sound to play after mission gets completed")]
    public AudioClip OnCompleteSound;

    private int currentCount = 0;

    public bool IsDone
    {
        get { return currentCount >= Count; }
    }

    public void IncreaseCurrentCount(int Count)
    {
        currentCount += Count;
    }

    public void Reset()
    {
        currentCount = 0;
    }

    public int GetCurrentCount()
    {
        return currentCount;
    }

    //private void OnPickUpItem(PickUpBehaviour PickedItem)
    //{
    //    if (PickedItem == null)
    //    {
    //        Debug.Log("A Null Item Picked UP!!!");
    //        return;
    //    }

    //    if (TaskType != MissionManager.TaskTypes.PickUp)
    //        return;

    //    if (PickUpType != PickedItem.Type)
    //        return;

    //    currentCount++;
    //    //EventManager.OnMissionUpdatedCall(this);
    //    Debug.Log("Task Updated --- " + TaskType + " " + PickedItem.Type + "-" + currentCount + "/" + Count);


    //    if (IsDone)
    //    {
    //        //EventManager.OnMissionCompletedCall(this);
    //        Debug.Log("Task Completed --- " + TaskType + " " + PickedItem.Type + "-" + currentCount + "/" + Count);
    //    }
    //}

    //private void OnEnemyDeath(Enemy DeadEnemy)
    //{
    //    if (DeadEnemy == null)
    //    {
    //        Debug.Log("A Null Enemy Destroyed!!!");
    //        return;
    //    }

    //    //if (TaskType != TaskTypes.Destroy)
    //    //    return;

    //    if (EnemyType != DeadEnemy.Type)
    //        return;

    //    currentCount++;
    //   // EventManager.OnMissionUpdatedCall(this);
    //    Debug.Log("Task Updated --- " + TaskType + " " + DeadEnemy.Type + "-" + currentCount + "/" + Count);

    //    if (IsDone)
    //    {
    //        //EventManager.OnMissionCompletedCall(this);
    //        Debug.Log("Task Completed --- " + TaskType + " " + DeadEnemy.Type + "-" + currentCount + "/" + Count);
    //    }
    //}
}
