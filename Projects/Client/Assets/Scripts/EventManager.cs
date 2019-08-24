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



    public static class EventManager
    {
        public static event HealthUpdateEventHandler OnHealthUpdate;
        public static event HellfireUpdateEventHandler OnHellfireUpdate;
        public static event FuelUpdateEventHandler OnFuelUpdate;
        public static event LifeUpdateEventHandler OnLifeUpdate;
        public static event HydraUpdateEventHandler OnHydraUpdate;
        public static event GatlingGunUpdateEventHandler OnGatlingGunUpdate;
        public static event RefugeeUpdateEventHandler OnRefugeeUpdate;



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

        internal static void OnGatlingGunUpdateCall()
        {
            if (OnGatlingGunUpdate != null)
                OnGatlingGunUpdate();
        }

        internal static void OnRefugeeUpdateCall()
        {
            if (OnRefugeeUpdate != null)
                OnRefugeeUpdate();
        }
    }
}
