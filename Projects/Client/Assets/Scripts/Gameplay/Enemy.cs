//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
    public class Enemy : MonoBehaviorBase
    {
        private Transform target = null;

        private float sqrRange = 0;
        private float rateOfShot = 0;
        private float nextShotTime = 0;

        public float Range = 10;
        public float ShotPerSecond = 1;

        [SerializeField]
        private GameObject BulletPrefab = null;

        protected bool IsPilot
        {
            get;
            private set;
        }

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

            NetworkCommands.OnPilot += OnPilot;
            NetworkCommands.OnCommando += OnCommando;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NetworkCommands.OnPilot -= OnPilot;
            NetworkCommands.OnCommando -= OnCommando;
        }

        private void OnPilot()
        {
            IsPilot = true;
        }

        private void OnCommando()
        {
            IsPilot = false;
        }

        protected override void Update()
        {
            base.Update();

            if (!IsPilot || target == null || Chopter.Instance.IsDead)
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

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}