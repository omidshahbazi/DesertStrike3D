//Rambo Team
using System;
using UnityEngine;

namespace RamboTeam.Client
{

    public delegate void HealthUpdateEventHandler();
    public delegate void HellfireUpdateEventHandler();
    public delegate void FuelUpdateEventHandler();
    public delegate void LifeUpdateEventHandler();
    public delegate void HydraUpdateEventHandler();
    public delegate void GatlingGunUpdateEventHandler();
    public delegate void RefugeeUpdateEventHandler();
    //public delegate void OnEnemyDeathEventHandler(Enemy DeadEnemy);
    public delegate void OnPickUpEventHandler(PickUpBehaviour PickedItem);
    //public delegate void OnMissionUpdateEventHandler(Mission Mission);
    //public delegate void OnMissionCompleteEventHandler(Mission Mission);
    //public delegate void OnAllMissonsCompleteEventHandler();





    public static class EventManager
    {
        public static event HealthUpdateEventHandler OnHealthUpdate;
        public static event HellfireUpdateEventHandler OnHellfireUpdate;
        public static event FuelUpdateEventHandler OnFuelUpdate;
        public static event LifeUpdateEventHandler OnLifeUpdate;
        public static event HydraUpdateEventHandler OnHydraUpdate;
        public static event GatlingGunUpdateEventHandler OnGatlingGunUpdate;
        public static event RefugeeUpdateEventHandler OnRefugeeUpdate;
//        public static event OnEnemyDeathEventHandler OnEnemyDeath;
        public static event OnPickUpEventHandler OnPickUp;
        //public static event OnMissionUpdateEventHandler OnMissionUpdate;
        //public static event OnMissionCompleteEventHandler OnMissionComplete;
        //public static event OnAllMissonsCompleteEventHandler OnAllMissionsComplete;

        public static void OnHealthUpdateCall()
        {
            if (OnHealthUpdate != null)
                OnHealthUpdate();
        }


        public static void OnHellfireUpdateCall()
        {
            if (OnHellfireUpdate != null)
                OnHellfireUpdate();
        }

        public static void OnFuelUpdateCall()
        {
            if (OnFuelUpdate != null)
                OnFuelUpdate();
        }

        public static void OnLifeUpdateCall()
        {
            if (OnLifeUpdate != null)
                OnLifeUpdate();
        }

        public static void OnHydraUpdateCall()
        {
            if (OnHydraUpdate != null)
                OnHydraUpdate();
        }

        public static void OnGatlingGunUpdateCall()
        {
            if (OnGatlingGunUpdate != null)
                OnGatlingGunUpdate();
        }

        internal static void OnMissionCompletedCall(Mission Mission)
        {
            //if (OnMissionComplete != null)
            //    OnMissionComplete(Mission);
        }

        public static void OnMissionUpdatedCall(Mission Mission)
        {
            //if (OnMissionUpdate != null)
            //    OnMissionUpdate(Mission);
        }

        public static void OnRefugeeUpdateCall()
        {
            if (OnRefugeeUpdate != null)
                OnRefugeeUpdate();
        }

        public static void OnEnemyDeathCall(Enemy Enemy)
        {
            //if (OnEnemyDeath != null)
            //    OnEnemyDeath(Enemy);
        }

        public static void OnPickUpCall(PickUpBehaviour pickedItem)
        {
            if (OnPickUp != null)
                OnPickUp(pickedItem);
        }

        internal static void OnAllMissionsCompletedCall()
        {
            //if (OnAllMissionsComplete != null)
            //    OnAllMissionsComplete();
        }
    }
}
