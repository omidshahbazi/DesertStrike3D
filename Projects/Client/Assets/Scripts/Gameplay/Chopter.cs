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
        public int RightArmDestructionHP = 150;
        public GameObject RightArmDestructionParticle;
        public int LeftArmDestructionHP = 75;
        public GameObject LeftArmDestructionParticle;
        private AudioSource rotorAudio;

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
        private float nextRefugeeReleaseTime;
        public float RefugessReleaseGapTime = 2.0f;
        public int HealthPerRefugeeAmount = 30;

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
            LeftArmDestructionParticle.SetActive(false);
            RightArmDestructionParticle.SetActive(false);
            rotorAudio = GetComponent<AudioSource>();
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

            if (currentRefugeesCount != 0)
            {
                if (Landing.Instance.state == Landing.State.Landed)
                {
                    if (nextRefugeeReleaseTime < Time.time)
                    {
                        nextRefugeeReleaseTime = Time.time + RefugessReleaseGapTime;
                        UpdateCurrentRefugee(-1);
                        UpdateCurrentHP(HealthPerRefugeeAmount);
                    }
                }
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

            CheckArmsDestructionState();
            if (currentHP == 0)
            {
                OnChopterDeath();
            }
        }

        private void CheckArmsDestructionState()
        {
            if (currentHP <= LeftArmDestructionHP && !LeftArmDestructionParticle.activeSelf)
                LeftArmDestructionParticle.SetActive(true);

            if (currentHP > LeftArmDestructionHP && LeftArmDestructionParticle.activeSelf)
                LeftArmDestructionParticle.SetActive(false);


            if (currentHP <= RightArmDestructionHP && !RightArmDestructionParticle.activeSelf)
                RightArmDestructionParticle.SetActive(true);
            if (currentHP > RightArmDestructionHP && RightArmDestructionParticle.activeSelf)
                RightArmDestructionParticle.SetActive(false);

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
            rotorAudio.Stop();

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

            yield return new WaitForSecondsRealtime(chopterDeathAnimation.GetCurrentAnimatorClipInfo(0).Length + 1.5F);

            Debug.Log("Revive");
            chopterDeathAnimation.enabled = false;
            chopterRotorBladeAnimation.enabled = true;
            chopterDeathAnimation.transform.localPosition = Vector3.zero;
            currentFuelAmount = FuelAmount;
            currentHP = HP;
            //currentRefugeesCount = 0;
            IsDead = false;
            smokeParticle.SetActive(false);
            rotorAudio.Play();

            EventManager.OnHealthUpdateCall();
            EventManager.OnLifeUpdateCall();
            EventManager.OnFuelUpdateCall();
            CheckArmsDestructionState();
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

                EventManager.OnPickUpCall(pickUp.pickedItem);

                switch (pickUp.pickedItem.Type)
                {
                    case PickUpBehaviour.PickUpType.Refugee:
                        UpdateCurrentRefugee((int)pickUp.pickedItem.Amount);
                        break;
                    case PickUpBehaviour.PickUpType.HellfireAmmo:
                        UpdateCurrentHellfire((int)pickUp.pickedItem.Amount);
                        break;
                    case PickUpBehaviour.PickUpType.HydraAmmo:
                        UpdateCurrentHydra((int)pickUp.pickedItem.Amount);
                        break;
                    case PickUpBehaviour.PickUpType.GatlingGun:
                        UpdateCurrentGatlingGun((int)pickUp.pickedItem.Amount);
                        break;
                    case PickUpBehaviour.PickUpType.Fuel:
                        UpdateCurrentFuel((int)pickUp.pickedItem.Amount);
                        break;
                    case PickUpBehaviour.PickUpType.HealthPack:
                        UpdateCurrentHP((int)pickUp.pickedItem.Amount);
                        break;
                    default:
                        break;
                }
                pickUp.DestroyPickedItem();
            }
        }

        private void UpdateCurrentRefugee(int Amount)
        {
            currentRefugeesCount = (uint)Mathf.Max(0, currentRefugeesCount + Amount);
            EventManager.OnRefugeeUpdateCall();
        }

        private void UpdateCurrentHellfire(int Amount)
        {

            currentHellfireCount = (uint)Mathf.Min(currentHellfireCount + Amount, HellfireCount);
            EventManager.OnHellfireUpdateCall();
        }

        private void UpdateCurrentHydra(int Amount)
        {
            currentHydraCount = (uint)Mathf.Min(currentHydraCount + Amount, HydraCount);
            EventManager.OnHydraUpdateCall();
        }

        private void UpdateCurrentGatlingGun(int Amount)
        {
            currentGatlingGunCount = (uint)Mathf.Min(currentGatlingGunCount + Amount, GatlingGunCount);
            EventManager.OnGatlingGunUpdateCall();
        }

        private void UpdateCurrentFuel(int Amount)
        {
            currentFuelAmount = (uint)Mathf.Min(currentFuelAmount + Amount, FuelAmount);
            EventManager.OnFuelUpdateCall();
        }

        private void UpdateCurrentHP(int Amount)
        {
            currentHP = (uint)Mathf.Min(currentHP + Amount, HP);
            EventManager.OnHealthUpdateCall();
            CheckArmsDestructionState();
        }

        internal void TriggerHydraShot()
        {
            currentHydraCount--;
            EventManager.OnHydraUpdateCall();
        }
    }
}