//Rambo Team
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RamboTeam.Client
{
    public delegate void EnemyDead(Enemy Enemy);
    public enum EnemyPriority
    {
        //Highest Number Means Higest priorities
        none = 0x00,
        unarmed = 0x01,
        armed = 0x02,
        exotic = 0x04
    }

    public enum EnemyType
    {
        None,
        AntiAircraft,
        M3VDA,
        MissileLauncher,
        RifleMan,
        RPGMan,
        SamRadar,
        PowerStation,
        Hangar,
        WatchTower,
        Tent,
        Factory,
        House
    }

    public class Enemy : MonoBehaviorBase
    {
        [SerializeField]
        public EnemyPriority EnemyType;
        [SerializeField]
        public EnemyType Type;
        public static EnemyDead OnEnemyDead;
        private Transform target = null;

        private float sqrRange = 0;
        private float rateOfShot = 0;
        private float nextShotTime = 0;

        public float Range = 10;
        public float ShotPerSecond = 1;
        public float delayShotPerSecond = 0;

        [SerializeField]
        private GameObject BulletPrefab = null;

        public float HP = 100;

        public float currentHP { get; private set; }

        public bool isAttacker = true;

        protected bool IsPilot
        {
            get { return NetworkLayer.Instance.IsPilot; }
        }


        private bool IsDead { get; set; } = false;

        public List<Transform> TransformsForTargetRotation = new List<Transform>();
        public float RotationToTargetSpeed = 150;
        public Transform ShotStartPosition;
        private bool isRotatingTowardTarget = false;
        public List<AudioClip> OnDeathAudio;
        private int perSecondCounter;
        protected override void Start()
        {
            base.Start();

            if (!isAttacker)
                return;


            sqrRange = Range * Range;
            rateOfShot = 1 / ShotPerSecond;
            perSecondCounter = (int)Math.Round(ShotPerSecond);

            if (isAttacker)
                target = ChopterPilotController.Instance.transform;
        }



        protected override void OnEnable()
        {
            base.OnEnable();

            IsDead = false;
            currentHP = HP;
        }

        protected override void Update()
        {
            base.Update();

            if (!IsPilot || target == null || Chopter.Instance.IsDead || IsDead)
                return;


            Vector3 diff = target.position - transform.position;

            if (diff.sqrMagnitude > sqrRange)
                return;

            if (Time.time < nextShotTime)
                return;


            if (isAnyTransformAlignedToTarget())
            {
                if (perSecondCounter == 0)
                {
                    nextShotTime = Time.time + rateOfShot + delayShotPerSecond;
                    perSecondCounter = (int)Math.Round(ShotPerSecond);
                }
                else
                {
                    nextShotTime = Time.time + rateOfShot;
                }

                Shot(ShotStartPosition.position, diff.normalized);
                perSecondCounter--;
            }
            else // Rotate To Target First
            {
                isRotatingTowardTarget = true;
                for (int i = 0; i < TransformsForTargetRotation.Count; ++i)
                {
                    if (TransformsForTargetRotation[i] != null)
                        StartCoroutine(RotateToTarget(TransformsForTargetRotation[i]));
                    else
                    {
                        Debug.LogError("a trasform is set to get rotated to target but it's reference is missig!");
                    }
                }
            }

        }

        private bool isAnyTransformAlignedToTarget()
        {
            if (TransformsForTargetRotation == null || TransformsForTargetRotation.Count == 0)
                return true;

            if (GetRotationTowardTarget(TransformsForTargetRotation[0]) == TransformsForTargetRotation[0].rotation)
                return true;

            return false;
        }

        private IEnumerator RotateToTarget(Transform Trans)
        {
            var lookRot = GetRotationTowardTarget(Trans);

            if (lookRot == Trans.rotation)
            {
                isRotatingTowardTarget = false;
                yield return null;
            }

            Trans.rotation = Quaternion.RotateTowards(Trans.rotation, lookRot, RotationToTargetSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        private Quaternion GetRotationTowardTarget(Transform trans)
        {
            Vector3 dir = trans.position - target.position;


            dir = new Vector3(dir.x, 0.0f, dir.z);

            var lookRot = Quaternion.LookRotation(dir);

            return lookRot;
        }

        protected virtual void Shot(Vector3 Position, Vector3 Direction)
        {
            ShotInternal(Position, Direction);
        }

        protected void ShotInternal(Vector3 Position, Vector3 Direction)
        {
            GameObject newObject = GameObject.Instantiate(BulletPrefab);
            newObject.transform.position = Position;

            Bullet bullet = newObject.GetComponent<Bullet>();

            bullet.SetParamaeters(Direction);
        }

        public void ApplyDamage(float Damage)
        {

            if (IsDead)
                return;

            currentHP = Mathf.Clamp(currentHP - Damage, 0, HP);

            EventManager.OnHealthUpdateCall();


            if (currentHP == 0)
            {
                OnEnemyDeath();
            }
        }

        private void OnEnemyDeath()
        {
            EventManager.OnEnemyDeathCall(this);
            PilotCameraController.Instance.SetCameraShake();
            if (OnDeathAudio.Count != 0)
            {
                AudioClip clip = OnDeathAudio[UnityEngine.Random.Range(0, OnDeathAudio.Count)];
                if (clip != null)
                    AudioManager.Instance.PlayAudio(clip, transform.position, null);
            }

            IsDead = true;
            gameObject.SetActive(false);
            OnEnemyDead?.Invoke(this);
        }



        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}