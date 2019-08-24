//Rambo Team
using System;
using UnityEngine;

namespace RamboTeam.Client
{
    public delegate void EnemyDead(Enemy Enemy);
    public class Enemy : MonoBehaviorBase
    {
        public static EnemyDead OnEnemyDead;
        private Transform target = null;

        private float sqrRange = 0;
        private float rateOfShot = 0;
        private float nextShotTime = 0;

        public float Range = 10;
        public float ShotPerSecond = 1;

        [SerializeField]
        private GameObject BulletPrefab = null;

        public float HP = 100;
        [SerializeField]
        private float currentHP;

        protected bool IsPilot
        {
			get { return NetworkLayer.Instance.IsPilot; }
        }


        private bool IsDead { get; set; } = false;


        protected override void Start()
        {
            base.Start();

            sqrRange = Range * Range;
            rateOfShot = 1 / ShotPerSecond;

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

            nextShotTime = Time.time + rateOfShot;

            Shot(transform.position, diff.normalized);
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
            IsDead = true;
            OnEnemyDead?.Invoke(this);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}