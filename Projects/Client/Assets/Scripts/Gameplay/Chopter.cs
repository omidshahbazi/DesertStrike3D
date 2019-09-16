//Rambo Team
using RamboTeam.Client.UI;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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
        public GameObject EngineBoostModel;
        public Transform EngineBoostParticle;

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
        public float currentFuelAmount { get; private set; } = 0;
        public uint currentRefugeesCount { get; private set; } = 0;
        public bool IsDead { get; private set; } = false;
        public float FuelCostPerSecond = 2.0f;
        private float nextRefugeeReleaseTime;
        public float RefugessReleaseGapTime = 2.0f;
        public int HealthPerRefugeeAmount = 30;
        public List<AudioClip> OnHitAudioClips = new List<AudioClip>();
        public AudioClip OnDeathAudio;
        public AudioClip OnLowFuelAudio;
        public int LowFuelAmount = 30;
        public bool islowFuelLoop = true;
        private float fuelElapsedUpdateTime = 0.0F;
        private AudioSource lowFuelAudioSource = null;
        public AudioSource engineBoostAudio;
        public ParticleSystem boostTrailsLeft;
        public ParticleSystem boostTrailsRight;

        public bool isEngineBoostEquipted
        {
            get;
            private set;
        }

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
            nextFuelUpdateTime = Time.time + FuelCostPerSecond;
        }

        protected override void Update()
        {
            base.Update();

            if (IsDead)
                return;

            if (!IsPilot)
                return;

            UpdateFuel();

            if (currentRefugeesCount != 0)
            {
                if (Landing.state == Landing.State.Landed)
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

        private float turnOnStartTime;
        private bool isEngineBoostOn = false;
        private float EngineBoostEffectTime = 1.0F;
        private float elapsedEngineActivationTime = 0.0F;
        private void UpdateFuel()
        {
            float cost;
            if (Input.GetKey(KeyCode.Space) && isEngineBoostEquipted && currentFuelAmount > LowFuelAmount)
            {
                TurnOnEngineBoostEffecs();
                cost = Time.deltaTime * FuelCostPerSecond * 10;
            }
            else
            {
                TurnOffEngineBoostEffecs();
                //EngineBoostParticle.localPosition = new Vector3(EngineBoostParticle.localPosition.x, 40, EngineBoostParticle.localPosition.z);
                cost = Time.deltaTime * FuelCostPerSecond;
            }

            UpdateCurrentFuel(-cost);
        }

        private void TurnOnEngineBoostEffecs()
        {
            if (!isEngineBoostOn)
            {
                isEngineBoostOn = true;
                turnOnStartTime = Time.time;
                elapsedEngineActivationTime = 0.0F;
                boostTrailsLeft.Play();
                boostTrailsRight.Play();
            }   

            if (elapsedEngineActivationTime / EngineBoostEffectTime <= 1.0F)
            {
                if (!engineBoostAudio.isPlaying)
                {
                    engineBoostAudio.Play();
                }
                engineBoostAudio.volume = elapsedEngineActivationTime / EngineBoostEffectTime;
                EngineBoostParticle.localPosition = Vector3.Lerp(EngineBoostParticle.localPosition, new Vector3(EngineBoostParticle.localPosition.x, -8, EngineBoostParticle.localPosition.z), elapsedEngineActivationTime / EngineBoostEffectTime);
                elapsedEngineActivationTime += Time.deltaTime;
            }
            else
            {
                engineBoostAudio.volume = 1;
            }
        }

        private void TurnOffEngineBoostEffecs()
        {
            if (isEngineBoostOn)
            {
                isEngineBoostOn = false;
                turnOnStartTime = Time.time;
                elapsedEngineActivationTime = 0.0F;
                boostTrailsLeft.Stop();
                boostTrailsRight.Stop();
            }
            if (elapsedEngineActivationTime / EngineBoostEffectTime <= 1.0F)
            {
                //if (!engineBoostAudio.isPlaying)
                //{
                //    engineBoostAudio.Play();
                //}
                engineBoostAudio.volume = 1.0F - (elapsedEngineActivationTime / EngineBoostEffectTime);
                EngineBoostParticle.localPosition = Vector3.Lerp(EngineBoostParticle.localPosition, new Vector3(EngineBoostParticle.localPosition.x, 50, EngineBoostParticle.localPosition.z), elapsedEngineActivationTime / EngineBoostEffectTime);
                elapsedEngineActivationTime += Time.deltaTime;
            }
            else
            {
                engineBoostAudio.volume = 0;

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
            else
            {
                if (OnHitAudioClips.Count != 0)
                {

                    AudioClip clip = OnHitAudioClips[UnityEngine.Random.Range(0, OnHitAudioClips.Count)];
                    if (clip != null)
                        AudioManager.Instance.PlayAudio(clip, transform.position, null);
                }
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
            if (OnDeathAudio != null)
                AudioManager.Instance.PlayAudio(OnDeathAudio, transform.position, null);

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
                    case PickUpBehaviour.PickUpType.EngineBoost:
                        isEngineBoostEquipted = true;
                        EngineBoostParticle.localPosition = new Vector3(EngineBoostParticle.localPosition.x, 40, EngineBoostParticle.localPosition.z);
                        EngineBoostModel.SetActive(true);
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

        private void UpdateCurrentFuel(float Amount)
        {
            currentFuelAmount = Mathf.Clamp(currentFuelAmount + Amount, 0.0F, FuelAmount);

            fuelElapsedUpdateTime += Math.Abs(Amount);

            if (fuelElapsedUpdateTime > 1.0F)
            {
                EventManager.OnFuelUpdateCall();
                fuelElapsedUpdateTime -= 1.0F;
            }

            if (currentFuelAmount > LowFuelAmount)
            {
                if (lowFuelAudioSource != null)
                    lowFuelAudioSource.Stop();
            }
            else
            {
                if (lowFuelAudioSource == null)
                {
                    lowFuelAudioSource = AudioManager.Instance.PlayAudio(OnLowFuelAudio, Vector3.zero, this.transform, 0.0F, 1.0F, islowFuelLoop);
                }
                else
                {
                    if (!lowFuelAudioSource.isPlaying)
                        lowFuelAudioSource.Play();
                }
            }

            if (currentFuelAmount == 0)
                OnChopterDeath();
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