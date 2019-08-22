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

        public float currentHP { get; private set; } = 0;
        public uint currentLifeCount { get; private set; } = 0;
        public uint currentFuelAmount { get; private set; } = 0;
        public uint currentRefugeesCount { get; private set; } = 0;

        public bool IsDead { get; private set; } = false;


        private bool isPilot = false;

        public float HP = 100;
        public uint LifeCount { get; private set; } = 3;
        public uint FuelAmount { get; private set; } = 120;

        public float FuelCostTime = 2.0f;

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            currentHP = HP;
            currentFuelAmount = FuelAmount;
            currentLifeCount = LifeCount;
            currentRefugeesCount = currentRefugeesCount;

            smokeParticle.SetActive(false);
            nextFuelUpdateTime = Time.time + FuelCostTime;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            NetworkCommands.OnPilot += OnPilot;
            NetworkCommands.OnCommando += OnCommando;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NetworkCommands.OnPilot -= OnPilot;
            NetworkCommands.OnCommando -= OnCommando;
        }

        protected override void Update()
        {
            base.Update();

            if (IsDead)
                return;

            if (!isPilot)
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

        private void OnPilot()
        {
            isPilot = true;
        }

        private void OnCommando()
        {
            isPilot = false;
        }

        public void ApplyDamage(float Damage)
        {
            if (!isPilot)
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
    }
}