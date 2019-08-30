//Rambo Team
using RamboTeam.Client.UI;
using System;
using System.Collections;
using UnityEngine;

namespace RamboTeam.Client
{
    public class Chopter : MonoBehaviorBase
    {
        [SerializeField]
        private GameObject smokeParticle;
        [SerializeField]
        private Animator chopterDeathAnimation;
        [SerializeField]
        private Animator chopterRotorBladeAnimation;


        private float nextFuelUpdateTime = 0.0F;
        public static Chopter Instance
        {
            get;
            private set;
        }

        public ChoppersPicker pickUp;
        //Weapons Count


        public uint HellfireCount;
        public uint HydraCount;
        public uint GatlingGunCount;

        /////////////////////////
        public float HP;
        public uint LifeCount;
        public uint FuelAmount;

        public uint currentHellfireCount { get; private set; } = 0;
        public uint currentHydraCount { get; private set; } = 0;
        public uint currentGatlingGunCount { get; private set; } = 0;
        public float currentHP { get; private set; } = 0;
        public uint currentLifeCount { get; private set; } = 0;
        public uint currentFuelAmount { get; private set; } = 0;
        public uint currentRefugeesCount { get; private set; } = 0;
        public bool IsDead { get; private set; } = false;
        public float FuelCostTime = 2.0f;

        private bool IsPilot
        {
            get { return NetworkLayer.Instance.IsPilot; }
        }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            currentHP = HP;
            currentFuelAmount = FuelAmount;
            currentLifeCount = LifeCount;
            currentRefugeesCount = currentRefugeesCount;
            currentGatlingGunCount = GatlingGunCount;
            currentHydraCount = HydraCount;
            currentHellfireCount = HellfireCount;

            smokeParticle.SetActive(false);
            nextFuelUpdateTime = Time.time + FuelCostTime;
        }

        protected override void Update()
        {
            base.Update();

            if (IsDead)
                return;

            if (!IsPilot)
                return;

            if (Time.time > nextFuelUpdateTime)
            {
                currentFuelAmount--;
                nextFuelUpdateTime = Time.time + FuelCostTime;
                EventManager.OnFuelUpdateCall();

                if (currentFuelAmount == 0)
                    OnChopterDeath();
            }
        }

        internal void TriggerHellfireShot()
        {
            currentHellfireCount--;
            EventManager.OnHellfireUpdateCall();
        }

        internal void TriggerGaltingfireShot()
        {
            currentGatlingGunCount--;
            EventManager.OnGatlingGunUpdateCall();
        }

        public void ApplyDamage(float Damage)
        {
            if (!IsPilot)
                return;

            if (IsDead)
                return;

            currentHP = Mathf.Clamp(currentHP - Damage, 0, HP);

            EventManager.OnHealthUpdateCall();


            if (currentHP == 0)
            {
                OnChopterDeath();
            }
        }

        private void OnChopterDeath()
        {
            Debug.Log("Dead");
            smokeParticle.SetActive(true);
            IsDead = true;
            currentLifeCount--;

            chopterRotorBladeAnimation.enabled = false;
            chopterDeathAnimation.enabled = true;
            chopterDeathAnimation.Play("ChopperDeath", -1, 0.0F);

            if (currentLifeCount == 0)
            {
                EventManager.OnLifeUpdateCall();
                StartCoroutine(DoGameOver());
                return;
            }

            StartCoroutine(ReviveChopter());
        }

        private IEnumerator DoGameOver()
        {
            yield return new WaitForSecondsRealtime(2.0F);

            GameOver.Instance.Open();
        }


        private IEnumerator ReviveChopter()
        {

            yield return new WaitForSecondsRealtime(chopterDeathAnimation.GetCurrentAnimatorClipInfo(0).Length + 0.5F);

            Debug.Log("Revive");
            chopterDeathAnimation.enabled = false;
            chopterRotorBladeAnimation.enabled = true;
            chopterDeathAnimation.transform.localPosition = Vector3.zero;
            currentFuelAmount = FuelAmount;
            currentHP = HP;
            currentRefugeesCount = 0;
            IsDead = false;
            smokeParticle.SetActive(false);

            EventManager.OnHealthUpdateCall();
            EventManager.OnLifeUpdateCall();
            EventManager.OnFuelUpdateCall();

            IsDead = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (IsDead)
                return;

            if (other.gameObject.tag == "Picker")
            {
                if (pickUp.pickedItem == null)
                    return;

                switch (pickUp.pickedItem.Type)
                {
                    case PickUpBehaviour.PickUpType.Refugee:
                        {
                            currentRefugeesCount += pickUp.pickedItem.Amount;
                            EventManager.OnRefugeeUpdateCall();
                        }
                        break;
                    case PickUpBehaviour.PickUpType.HellfireAmmo:
                        {
                            currentHellfireCount = (uint)Mathf.Min(currentHellfireCount + pickUp.pickedItem.Amount, HellfireCount);
                            EventManager.OnHellfireUpdateCall();
                        }
                        break;
                    case PickUpBehaviour.PickUpType.HydraAmmo:
                        {
                            currentHydraCount = (uint)Mathf.Min(currentHydraCount + pickUp.pickedItem.Amount, HydraCount);
                            EventManager.OnHydraUpdateCall();
                        }
                        break;
                    case PickUpBehaviour.PickUpType.GatlingGun:
                        currentGatlingGunCount = (uint)Mathf.Min(currentGatlingGunCount + pickUp.pickedItem.Amount, GatlingGunCount);
                        EventManager.OnGatlingGunUpdateCall();

                        break;
                    case PickUpBehaviour.PickUpType.Fuel:
                        currentFuelAmount = (uint)Mathf.Min(currentFuelAmount + pickUp.pickedItem.Amount, FuelAmount);
                        EventManager.OnFuelUpdateCall();
                        break;
                    case PickUpBehaviour.PickUpType.HealthPack:
                        currentHP = (uint)Mathf.Min(currentHP + pickUp.pickedItem.Amount, HP);
                        EventManager.OnHealthUpdateCall();
                        break;
                    default:
                        break;
                }
                pickUp.DestroyPickedItem();
            }
        }

        internal void TriggerHydraShot()
        {
            currentHydraCount--;
            EventManager.OnHydraUpdateCall();
        }
    }
}