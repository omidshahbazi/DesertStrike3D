using RamboTeam.Client;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


/*We need following features for missions:
1- An input for adding missions
2- Identifying which game objects should the chopper destroy.It could be more than one game object.
3- text which will appear in the mission panel inside the game. The texts explains what the mission is.
4- Mission Accomplish logic
5- A text on UI which says Mission #  Accomplished
6- A music theme will be played when a mission is done
7- When a mission is done it should be checked in the Mission Panel
8- When all missions are done the level is fished and a text on UI should say All the Missions Accomplished. Return to Base.
- Missions don't have any order and priority. The player is free to do every mission he likes to.
- Missions don't have any dependency to each other.*/

[RequireComponent(typeof(AudioSource))]

public class MissionManager : MonoBehaviorBase
{
    //[Serializable]
    //public class Mission// : MonoBehaviour
    //{
    //    public string Name;
    //    public TaskTypes TaskType;
    //    public PickUpBehaviour.PickUpType PickUpType;
    //    public EnemyType EnemyType;
    //    public int Count = 1;
    //    public AudioClip OnCompleteSound;

    //    private int currentCount = 0;

    //    public bool IsDone
    //    {
    //        get { return currentCount >= Count; }
    //    }

    //    //protected override void Awake()
    //    //{
    //    //    base.Awake();

    //    //    EventManager.OnEnemyDeath += OnEnemyDeath;
    //    //    EventManager.OnPickUp += OnPickUpItem;
    //    //}

    //    private void OnPickUpItem(PickUpBehaviour PickedItem)
    //    {
    //        if (PickedItem == null)
    //        {
    //            Debug.Log("A Null Item Picked UP!!!");
    //            return;
    //        }

    //        if (TaskType != TaskTypes.PickUp)
    //            return;

    //        if (PickUpType != PickedItem.Type)
    //            return;

    //        currentCount++;
    //        EventManager.OnMissionUpdatedCall(this);
    //        Debug.Log("Task Updated --- " + TaskType + " " + PickedItem.Type + "-" + currentCount + "/" + Count);


    //        if (IsDone)
    //        {
    //            EventManager.OnMissionCompletedCall(this);
    //            Debug.Log("Task Completed --- " + TaskType + " " + PickedItem.Type + "-" + currentCount + "/" + Count);
    //        }
    //    }

    //    private void OnEnemyDeath(Enemy DeadEnemy)
    //    {
    //        if (DeadEnemy == null)
    //        {
    //            Debug.Log("A Null Enemy Destroyed!!!");
    //            return;
    //        }

    //        if (TaskType != TaskTypes.Destroy)
    //            return;

    //        if (EnemyType != DeadEnemy.Type)
    //            return;

    //        currentCount++;
    //        EventManager.OnMissionUpdatedCall(this);
    //        Debug.Log("Task Updated --- " + TaskType + " " + DeadEnemy.Type + "-" + currentCount + "/" + Count);

    //        if (IsDone)
    //        {
    //            EventManager.OnMissionCompletedCall(this);
    //            Debug.Log("Task Completed --- " + TaskType + " " + DeadEnemy.Type + "-" + currentCount + "/" + Count);
    //        }
    //    }
    //}

    private static MissionManager instance = null;

    public static MissionManager Instance
    {
        get
        {
            return instance;
            //if (instance == null)
            //{
            //    GameObject Manager = GameObject.Instantiate(new GameObject("MissionManager"));
            //    Manager.AddComponent<MissionManager>();
            //}
        }
    }
    public List<Mission> Missions = new List<Mission>();
    public string Name;
    private AudioSource audioSource;

    public enum TaskTypes
    {
        PickUp = 0,
        Destroy
    }

    public bool AreAllTasksDone
    {
        get;
        private set;
    }

    protected override void Awake()
    {
        base.Awake();

        instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.Stop();
        if (Missions == null || Missions.Count == 0)
        {
            Debug.Log("No Mission is Set");
            return;
        }

        EventManager.OnEnemyDeath += OnEnemyDeath;
        EventManager.OnPickUp += OnPickUpItem;
        EventManager.OnMissionUpdate += OnMissionCompleted;
    }

    private void OnMissionCompleted(Mission Mission)
    {
        bool allCompleted = true;

        for (int i = 0; i < Missions.Count; ++i)
        {
            allCompleted &= Missions[i].IsDone;
        }

        if (allCompleted)
        {
            AreAllTasksDone = true;
            EventManager.OnAllMissionsCompletedCall();
            Debug.Log("Congratulations, All Missions Completed");
        }
    }

    private void OnPickUpItem(PickUpBehaviour PickedItem)
    {
        if (PickedItem == null)
        {
            Debug.Log("A Null Item Picked UP!!!");
            return;
        }

        for (int i = 0; i < Missions.Count; ++i)
        {
            Mission currentMission = Missions[i];

            if (currentMission.IsDone)
                continue;

            if (currentMission.TaskType != MissionManager.TaskTypes.PickUp)
                continue;

            if (currentMission.PickUpType != PickedItem.Type)
                continue;

            currentMission.IncreaseCurrentCount(1);
            EventManager.OnMissionUpdatedCall(currentMission);
            Debug.Log("Task Updated --- " + currentMission.TaskType + " " + PickedItem.Type + "-" + currentMission.GetCurrentCount() + "/" + currentMission.Count);


            if (currentMission.IsDone)
            {
                EventManager.OnMissionCompletedCall(currentMission);
                if (currentMission.OnCompleteSound != null)
                {
                    audioSource.clip = currentMission.OnCompleteSound;
                    audioSource.Play();
                }
                Debug.Log("Task Completed --- " + currentMission.TaskType + " " + PickedItem.Type + "-" + currentMission.GetCurrentCount() + "/" + currentMission.Count);
            }
        }

    }

    private void OnEnemyDeath(Enemy DeadEnemy)
    {
        if (DeadEnemy == null)
        {
            Debug.Log("A Null Enemy Destroyed!!!");
            return;
        }

        for (int i = 0; i < Missions.Count; ++i)
        {
            Mission currentMission = Missions[i];

            if (currentMission.IsDone)
                continue;

            if (currentMission.TaskType != TaskTypes.Destroy)
                continue;

            if (currentMission.EnemyType != DeadEnemy.Type)
                continue;

            currentMission.IncreaseCurrentCount(1);
            EventManager.OnMissionUpdatedCall(currentMission);
            Debug.Log("Task Updated --- " + currentMission.TaskType + " " + DeadEnemy.Type + "-" + currentMission.GetCurrentCount() + "/" + currentMission.Count);

            if (currentMission.IsDone)
            {
                EventManager.OnMissionCompletedCall(currentMission);
                if (currentMission.OnCompleteSound != null)
                {
                    audioSource.clip = currentMission.OnCompleteSound;
                    audioSource.Play();
                }
                Debug.Log("Task Completed --- " + currentMission.TaskType + " " + DeadEnemy.Type + "-" + currentMission.GetCurrentCount() + "/" + currentMission.Count);
            }
        }

    }
}
